using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChoiceNode_", menuName = "Node/Choice Node")]
public class ChoiceNode : GameNode
{
    public List<ChoiceItemData> choiceItemDataList;
}

[System.Serializable]
public class ChoiceItemData
{
    public Sprite choiceSprite;
    public string choiceContent;
}
