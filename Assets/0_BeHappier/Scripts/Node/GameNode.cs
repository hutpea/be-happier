using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameNode : ScriptableObject
{
    public int nodeID;

    public List<GameAction> nodeStartActionList;
    public List<GameAction> nodeEndActionList;

    public virtual void BeginEvent()
    {
        
    }
}
