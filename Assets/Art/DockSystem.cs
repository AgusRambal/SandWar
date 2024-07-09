using DG.Tweening;
using UnityEngine;

public class DockSystem : MonoBehaviour
{
    public DockItem[] dockItems;
    public float maxScale = 1.5f;
    public float midScale = 1.25f; // Escala para los elementos adyacentes
    public float minScale = 1.0f;
    public float animationDuration = 0.3f;

    void Start()
    {
        foreach (DockItem item in dockItems)
        {
            item.minScale = minScale;
            item.maxScale = maxScale;
            item.animationDuration = animationDuration;
        }
    }

    public void ScaleItems(Transform centerItem)
    {
        int centerIndex = -1;
        for (int i = 0; i < dockItems.Length; i++)
        {
            if (dockItems[i].transform == centerItem)
            {
                centerIndex = i;
                break;
            }
        }

        for (int i = 0; i < dockItems.Length; i++)
        {
            if (i == centerIndex)
            {
                dockItems[i].transform.DOScale(dockItems[i].originalScale * maxScale, animationDuration).SetEase(Ease.OutQuad);
            }
            else if (i == centerIndex - 1 || i == centerIndex + 1)
            {
                dockItems[i].transform.DOScale(dockItems[i].originalScale * midScale, animationDuration).SetEase(Ease.OutQuad);
            }
            else if (i == centerIndex - 2 || i == centerIndex + 2)
            {
                dockItems[i].transform.DOScale(dockItems[i].originalScale * (minScale + (midScale - minScale) / 2), animationDuration).SetEase(Ease.OutQuad);
            }
            else
            {
                dockItems[i].transform.DOScale(dockItems[i].originalScale * minScale, animationDuration).SetEase(Ease.InBack);
            }
        }
    }

    public void ResetScales()
    {
        foreach (DockItem item in dockItems)
        {
            item.transform.DOScale(item.originalScale * minScale, animationDuration).SetEase(Ease.InBack);
        }
    }
}
