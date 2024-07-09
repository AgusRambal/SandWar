using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DockItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float maxScale = 1.5f;
    public float minScale = 1.0f;
    public float animationDuration = 0.3f;

    public Vector3 originalScale;
    private DockSystem dockSystem;

    void Start()
    {
        originalScale = transform.localScale;
        dockSystem = GetComponentInParent<DockSystem>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        dockSystem.ScaleItems(transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dockSystem.ResetScales();
    }
}
