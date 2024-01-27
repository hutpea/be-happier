using UnityEngine;
using UnityEngine.Events;

public class CardBody : MonoBehaviour
{
    public UnityEvent onMouseEnterEvent;
    public UnityEvent onMouseExitEvent;
    
    private void OnMouseEnter()
    {
        onMouseEnterEvent?.Invoke();
    }
    
    private void OnMouseExit()
    {
        onMouseExitEvent?.Invoke();
    }
}
