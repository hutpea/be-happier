using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddCardNode_", menuName = "Node/AddCard Node")]
public class AddCardNode : GameNode
{
    public CardObjectName cardObjectName;
    public string cardInitState;
    public int slotID;
    public string initCardContent;
}
