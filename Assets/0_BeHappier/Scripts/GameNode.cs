using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameNode
{
    public UnityAction OnBeginAction;
    public UnityAction OnEndAction;
}
