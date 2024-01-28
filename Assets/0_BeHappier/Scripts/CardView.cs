using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public CardObjectName currentCardObjectName;
    public string currentState;
    public CardObjectData currentCardObjectData;
    public CardViewType cardViewType;
    public List<DialogueItem> currentDialogueItemList = new List<DialogueItem>();
    public int currentDialogueIndex = 0;
    [ShowIf("cardViewType",CardViewType.ChoiceCard)]
    public int choiceID;
    
    public Image cardImage;
    public TextEffectManager textEffectManager;
    public Button continueButton;
    
    private RectTransform cardRect;
    public bool canBeSelect;
    private bool isActiveDialogue = false;
    
    private void Awake()
    {
        cardRect = GetComponent<RectTransform>();
        continueButton.gameObject.SetActive(false);
    }

    public void SetupCard(CardViewType cardViewType, CardObjectName cardObjectName, string startState, string initContent = null)
    {
        this.cardViewType = cardViewType;
        this.currentCardObjectName = cardObjectName;
        this.currentState = startState;
        this.currentCardObjectData = GameManager.Instance.liveDataManager.GetCardObjectDataOf(cardObjectName);
        gameObject.name = cardViewType + "_" + cardObjectName;
        SetCardViewByType();
        SetCardState();
        if (initContent != null)
        {
            textEffectManager.Display(initContent);
        }
        else
        {
            textEffectManager.Display("");
        }
    }

    public void SetupCardSimpleSprite(CardViewType cardViewType, Sprite sprite, string content)
    {
        this.cardViewType = cardViewType;
        cardImage.sprite = sprite;
        SetCardViewByType();
        textEffectManager.Display(content);
    }

    public void SetCardViewByType()
    {
        switch (cardViewType)
        {
            case CardViewType.ObjectCard:
            {
                canBeSelect = false;
                break;
            }
            case CardViewType.ChoiceCard:
            {
                canBeSelect = true;
                break;
            }
        }
    }

    public void SetCardState()
    {
        Sprite stateSprite = currentCardObjectData.GetSpriteOfState(this.currentState);
        cardImage.sprite = stateSprite;
    }

    public void SetDialogueContentList(List<DialogueItem> dialogueItems)
    {
        currentDialogueItemList.Clear();
        foreach (var dialogueItem in dialogueItems)
        {
            currentDialogueItemList.Add(dialogueItem);
        }
        currentDialogueIndex = 0;
        continueButton.gameObject.SetActive(true);
    }

    public void BeginDeliverDialogue()
    {
        isActiveDialogue = true;
        continueButton.gameObject.SetActive(true);
        ExecuteDialogue();
        ScaleUpContinueButton();
    }

    private void ExecuteDialogue()
    {
        this.currentState = currentDialogueItemList[GetCurrentIndex()].cardState;
        SetCardState();
        string typeContent = currentDialogueItemList[GetCurrentIndex()].dialogueContent;
        textEffectManager.Typing(typeContent);
    }

    public void ToggleInteraction(bool value)
    {
        if (cardViewType == CardViewType.ObjectCard)
        {
            if (value)
            {
                if(isActiveDialogue) continueButton.gameObject.SetActive(true);
            }
            else
            {
                if(isActiveDialogue) continueButton.gameObject.SetActive(false);
            }
        }
    }

    public void OnContinueButtonClicked()
    {
        currentDialogueIndex++;
        Debug.Log("Continue Button clicked. currentIndex = " + currentDialogueIndex);
        if (currentDialogueIndex <= currentDialogueItemList.Count - 1)
        {
            Debug.Log(currentDialogueIndex + " " + (currentDialogueItemList.Count - 1));
            ExecuteDialogue();
        }
        else
        {
            isActiveDialogue = false;
            continueButton.gameObject.SetActive(false);
            GameManager.Instance.nodeManager.endExecuteNoteStartEvent.Invoke();
        }
    }

    public int GetCurrentIndex()
    {
        return Mathf.Min(currentDialogueIndex, currentDialogueItemList.Count - 1);
    }

    public void OnMouseClickedEvent()
    {
        if (!canBeSelect) return;
        SoundManager.Instance.Play(GameAudioName.Select1);
        GameManager.Instance.gameUIManager.SelectChoice(choiceID);
    }
    
    public void OnMouseEnterEvent()
    {
        if (!canBeSelect) return;
        SoundManager.Instance.Play(GameAudioName.FingerTap1);
        cardRect.DOAnchorPosY(0.2F, 0.15F).SetEase(Ease.Linear).OnStart(() =>
        {
            canBeSelect = false;
        }).OnComplete(() =>
        {
            canBeSelect = true;
        });
    }
    
    public void OnMouseExitEvent()
    {
        if (!canBeSelect) return;
        cardRect.DOAnchorPosY(0F, 0.15F).SetEase(Ease.Linear).OnStart(() =>
        {
            canBeSelect = false;
        }).OnComplete(() =>
        {
            canBeSelect = true;
        });
    }

    public void CardMoveOutOfChoice()
    {
        
    }
    
    public void CardMoveInOfChoice()
    {
        
    }
    
    private void ScaleUpContinueButton()
    {
        RectTransform btnRect = continueButton.GetComponent<RectTransform>();
        btnRect.DOScale(1.1F, 0.5F).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            btnRect.DOScale(1F, 0.4F);
        });
    }

    [Button]
    public void GoToZeroPos()
    {
        cardRect.anchoredPosition = Vector2.zero;
        var cardPos = this.transform.position;
        cardPos.z = 0f;
        this.transform.position = cardPos;
    }
}

public enum CardViewType{
    ObjectCard,
    ChoiceCard
}