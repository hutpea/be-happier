using System;
using UnityEngine;
using UnityEngine.Events;

public class CardBody : MonoBehaviour
{
    public UnityEvent onMouseEnterEvent;
    public UnityEvent onMouseExitEvent;
    public UnityEvent onMouseClickEvent;
    
    private void OnMouseEnter()
    {
        onMouseEnterEvent?.Invoke();
    }
    
    private void OnMouseExit()
    {
        onMouseExitEvent?.Invoke();
    }

    private void OnMouseUpAsButton()
    {
        onMouseClickEvent?.Invoke();
    }
}
