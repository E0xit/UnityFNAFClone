using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClicker : MonoBehaviour, IPointerUpHandler,IPointerDownHandler
{
    public bool isClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        isClick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClick = false;
    }
}
