using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public GameObject handCanvasObject;
    public Transform handChoiceLayout;
    public List<Transform> handLayoutSlotTransformList;
    public TextEffectManager narrativeTextEffectManager;
    public Button continueNarrativeButton;
    public List<string> currentContentList = new List<string>();
    public int currentNarrativeIndex = 0;
    
    public CanvasGroup darkCanvasGroup;
    public List<CardView> currentChoiceCardViews = new List<CardView>();

    public TextMeshProUGUI checkConditionNodeDebugTxt;

    private void Awake()
    {
        continueNarrativeButton.gameObject.SetActive(false);
    }

    public void SetupChoice(List<ChoiceItemData> choiceItemDataList)
    {
        Debug.Log("Setup Choice");
        currentChoiceCardViews.Clear();
        DeleteAllFromLayout();
        var originalHandLayoutPos = handChoiceLayout.transform.position;
        originalHandLayoutPos.x = 13;
        handChoiceLayout.transform.position = originalHandLayoutPos;
        
        handCanvasObject.SetActive(true);
        for (var index = 0; index < choiceItemDataList.Count; index++)
        {
            var choiceItem = choiceItemDataList[index];
            GameObject cardObj = Instantiate(GameManager.Instance.prefabManager.normalCardGameObject, handLayoutSlotTransformList[index].transform);
            RectTransform cardRect = cardObj.GetComponent<RectTransform>();
            cardRect.anchoredPosition = Vector2.zero;
            cardRect.localRotation = Quaternion.identity;
            var cardPos = cardObj.transform.position;
            cardPos.z = 0f;
            cardObj.transform.position = cardPos;
            CardView cardView = cardObj.GetComponent<CardView>();
            cardView.SetupCardSimpleSprite(CardViewType.ChoiceCard, choiceItem.choiceSprite, choiceItem.choiceContent);
            cardView.choiceID = index;
            currentChoiceCardViews.Add(cardView);
            handLayoutSlotTransformList[index].gameObject.SetActive(true);
        }

        for (int i = choiceItemDataList.Count; i < handLayoutSlotTransformList.Count; i++)
        {
            handLayoutSlotTransformList[i].gameObject.SetActive(false);
        }

        handChoiceLayout.GetComponent<RectTransform>().DOAnchorPosX(0, 1F).SetEase(Ease.OutBounce);
        SoundManager.Instance.Play(GameAudioName.QuickTransition1);
        Debug.Log("Setup Choice end");
    }

    private void DeleteAllFromLayout()
    {
        foreach (var slot in handLayoutSlotTransformList)
        {
            //Debug.Log(slot.gameObject.name + " delete all children!");
            //slot.GetComponentsInChildren<Canvas>().ForEach(c => Destroy(c));
            for(int i = 0; i < slot.childCount; i++)
            {
                Destroy(slot.GetChild(i).gameObject);
            }
        }
    }

    public void SelectChoice(int choiceID)
    {
        GameData.SetIntData("choice_id", choiceID);

        foreach (var cardView in currentChoiceCardViews)
        {
            cardView.canBeSelect = false;
        }
        
        SoundManager.Instance.Play(GameAudioName.QuickTransition1);
        handChoiceLayout.GetComponent<RectTransform>().DOAnchorPosX(-18, 1F).OnComplete(() =>
        {
            handCanvasObject.SetActive(false);
            GameManager.Instance.nodeManager.endExecuteNoteStartEvent.Invoke();
        });
    }

    public void SetDarkBackground(float duration = 1F)
    {
        StartCoroutine(DarkBackgroundIEnumerator(duration));
    }

    private IEnumerator DarkBackgroundIEnumerator(float duration)
    {
        darkCanvasGroup.DOFade(1F, duration / 3F);
        yield return new WaitForSeconds(duration / 3F);
        darkCanvasGroup.DOFade(0F, duration / 3F);
    }
    
    public void SetNarrativeContentList(List<string> contentList)
    {
        currentContentList.Clear();
        foreach (var content in contentList)
        {
            currentContentList.Add(content);
        }
        currentNarrativeIndex = 0;
        
        narrativeTextEffectManager.gameObject.SetActive(true);
        continueNarrativeButton.gameObject.SetActive(true);

        BeginDeliverNarrative();
    }
    
    public void BeginDeliverNarrative()
    {
        continueNarrativeButton.gameObject.SetActive(true);
        ExecuteNarativeContent();
    }
    
    private void ExecuteNarativeContent()
    {
        string typeContent = currentContentList[GetCurrentNarrativeContentIndex()];
        narrativeTextEffectManager.Typing(typeContent);
    }
    
    public void OnContinueNarativeButtonClicked()
    {
        currentNarrativeIndex++;
        Debug.Log("Continue Button clicked. currentIndex = " + currentNarrativeIndex);
        if (currentNarrativeIndex <= currentContentList.Count - 1)
        {
            //Debug.Log(currentNarrativeIndex + " " + (currentContentList.Count - 1));
            ExecuteNarativeContent();
        }
        else
        {
            narrativeTextEffectManager.gameObject.SetActive(false);
            continueNarrativeButton.gameObject.SetActive(false);
            GameManager.Instance.nodeManager.endExecuteNoteStartEvent.Invoke();
        }
    }
    
    public int GetCurrentNarrativeContentIndex()
    {
        return Mathf.Min(currentNarrativeIndex, currentContentList.Count - 1);
    }

    private void ScaleUpContinueButton()
    {
        RectTransform btnRect = continueNarrativeButton.GetComponent<RectTransform>();
        btnRect.DOScale(1.1F, 0.5F).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            btnRect.DOScale(1F, 0.4F);
        });
    }
}
