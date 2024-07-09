using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonScaleSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float scaleMultiplier = 1.2f;
    public float duration = 0.2f;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * scaleMultiplier, duration).SetEase(Ease.InOutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, duration).SetEase(Ease.InOutQuad);
    }
}
