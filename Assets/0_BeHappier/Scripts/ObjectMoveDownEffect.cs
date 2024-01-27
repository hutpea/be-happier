using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObjectMoveDownEffect : MonoBehaviour
{
    public float moveDownValue = -3F;
    public float moveTime = 3F;
    public float delay = 0F;
    private void Start()
    {
        transform.DOMoveY(moveDownValue, moveTime).SetDelay(delay);
    }
}
