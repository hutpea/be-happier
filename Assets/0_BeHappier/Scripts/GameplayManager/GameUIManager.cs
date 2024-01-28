using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public GameObject handCanvasObject;
    public Transform handChoiceLayout;
    public List<Transform> handLayoutSlotTransformList;
    public TextEffectManager narrativeTextEffectManager;
    public List<CardView> currentChoiceCardViews = new List<CardView>();

    public TextMeshProUGUI checkConditionNodeDebugTxt;
    
    public void SetupChoice(List<ChoiceItemData> choiceItemDataList)
    {
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
    }

    private void DeleteAllFromLayout()
    {
        foreach (var slot in handLayoutSlotTransformList)
        {
            while (slot.childCount > 0)
            {
                DestroyImmediate(slot.GetChild(0));
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
        handChoiceLayout.GetComponent<RectTransform>().DOAnchorPosX(-18, 1F).SetEase(Ease.InBounce).OnComplete(() =>
        {
            handCanvasObject.SetActive(false);
            GameManager.Instance.nodeManager.endExecuteNoteStartEvent.Invoke();
        });
    }
}
