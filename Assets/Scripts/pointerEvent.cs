using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class pointerEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool selected;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        selected = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        selected = false;
    }
}
