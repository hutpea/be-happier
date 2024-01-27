using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public Transform handChoiceLayout;

    public void SetupChoice(List<ChoiceItemData> choiceItemDataList)
    {
        DeleteAllFromLayout();
        foreach (var choiceItem in choiceItemDataList)
        {
            GameObject cardObj = Instantiate(GameManager.Instance.prefabManager.normalCardGameObject, handChoiceLayout);
            CardView cardView = cardObj.GetComponent<CardView>();
            cardView.SetupCard(choiceItem.cardObjectName, choiceItem.cardState, choiceItem.cardContent);
        }
    }

    private void DeleteAllFromLayout()
    {
        while (handChoiceLayout.childCount > 0)
        {
            Destroy(handChoiceLayout.GetChild(0));
        }
    }
}
