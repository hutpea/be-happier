using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeliverContentNode_", menuName = "Node/DeliverContent Node")]
public class DeliverContentNode : GameNode
{
    public CardObjectName cardObjectName;
    public List<DialogueItem> dialogueItemList;
}