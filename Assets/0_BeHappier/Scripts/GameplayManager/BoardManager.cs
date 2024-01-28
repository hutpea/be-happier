using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class BoardManager : SerializedMonoBehaviour
{
    public Dictionary<CardObjectName, CardView> cardViews;

    public Transform cardBeginPos;
    public Dictionary<int, Transform> cardSlots;

    private void Awake()
    {
        cardViews = new Dictionary<CardObjectName, CardView>();
        foreach (CardObjectName cardObjectName in Enum.GetValues(typeof(CardObjectName)))
        {
            cardViews.Add(cardObjectName, null);
        }
    }

    public void AddNewCardToSlot(CardObjectName cardObjectName, string startState, string initContent, int slotID)
    {
        GameObject cardObj = Instantiate(GameManager.Instance.prefabManager.normalCardGameObject);
        CardView cardView = cardObj.GetComponent<CardView>();
        cardView.SetupCard(CardViewType.ObjectCard, cardObjectName, startState, initContent);
        
        cardViews[cardObjectName] = cardView;
        
        Transform targetSlot = cardSlots[slotID];
        Transform cardTransform = cardObj.transform;
        StartCoroutine(CardMoveCoroutine(cardTransform, targetSlot));
    }

    IEnumerator CardMoveCoroutine(Transform cardTransform, Transform targetSlot)
    {
        cardTransform.position = cardBeginPos.position;
        cardTransform.rotation = cardBeginPos.rotation;
        SoundManager.Instance.Play(GameAudioName.Slide1);
        yield return new WaitForSeconds(0.5F);
        cardTransform.DORotate(targetSlot.rotation.eulerAngles, 0.5F);
        cardTransform.DOMove(targetSlot.position, 1F).OnComplete(() =>
        {
            cardTransform.SetParent(targetSlot, worldPositionStays: true);
            GameManager.Instance.nodeManager.endExecuteNoteStartEvent.Invoke();
        });
    }

    public void ToggleInteractionAllCardViews(bool value)
    {
        foreach (var cardViewKeyPair in cardViews)
        {
            if (cardViewKeyPair.Value != null)
            {
                cardViewKeyPair.Value.ToggleInteraction(value);
            }
        }
    }

    public void DeliverContentToCardObject(CardObjectName cardObjectName, List<DialogueItem> dialogueItems)
    {
        CardView deliveredCardView = GetBoardCardView(cardObjectName);
        deliveredCardView.SetDialogueContentList(dialogueItems);
        deliveredCardView.BeginDeliverDialogue();
    }

    private CardView GetBoardCardView(CardObjectName name)
    {
        if (cardViews[name] == null)
        {
            Debug.LogError($"You're trying to get an empty cardView of {name}");
            return null;
        }
        else
        {
            return cardViews[name];
        }
    }
}
