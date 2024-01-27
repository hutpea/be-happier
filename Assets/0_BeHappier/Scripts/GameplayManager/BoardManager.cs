using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class BoardManager : SerializedMonoBehaviour
{
    public List<CardView> cardViews;

    public Transform cardBeginPos;
    public Dictionary<int, Transform> cardSlots;
    
    public void AddNewCardToSlot(CardObjectName cardObjectName, string startState, string initContent, int slotID)
    {
        GameObject cardObj = Instantiate(GameManager.Instance.prefabManager.normalCardGameObject);
        CardView cardView = cardObj.GetComponent<CardView>();
        cardView.SetupCard(cardObjectName, startState, initContent);
        cardViews.Add(cardView);
        
        Transform targetSlot = cardSlots[slotID];
        Transform cardTransform = cardObj.transform;
        StartCoroutine(CardMoveCoroutine(cardTransform, targetSlot));
    }

    IEnumerator CardMoveCoroutine(Transform cardTransform, Transform targetSlot)
    {
        cardTransform.position = cardBeginPos.position;
        cardTransform.rotation = cardBeginPos.rotation;
        yield return new WaitForSeconds(0.5F);
        cardTransform.DORotate(targetSlot.rotation.eulerAngles, 0.5F);
        cardTransform.DOMove(targetSlot.position, 1F).OnComplete(() =>
        {
            cardTransform.SetParent(targetSlot, worldPositionStays: true);
        });
    }
}
