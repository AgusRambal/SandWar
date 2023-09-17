using UnityEngine;
using UnityEngine.EventSystems;

public class IsOverUiChecker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIMethods.isOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIMethods.isOverUI = false;
    }
}
