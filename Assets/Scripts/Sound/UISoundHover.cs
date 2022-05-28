using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _onHoverEnterSound = "beep";
    [SerializeField] private string _onHoverExitSound = "beep";

    public void OnPointerEnter(PointerEventData eventData)
    {
        Manager.instance.audioManager.PlaySfx(_onHoverEnterSound, true, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Manager.instance.audioManager.PlaySfx(_onHoverExitSound, true, true);
    }
}
