using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SoundMaster sound;
    public bool IsHover { get; set; }
    public void OnPointerEnter(PointerEventData eventData)
    {
        sound.PlayMove();
        IsHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsHover = false;
    }
}