using DG.Tweening;
using UnityEngine;

public class DockSystem : MonoBehaviour
{
    [SerializeField] private UIMethods uiManager;
    [SerializeField] private RectTransform[] dockImages;
    [SerializeField] private float maxScale;
    [SerializeField] private float animationDuration;
    [SerializeField] private float spacing;
    [SerializeField] private float parentPadding;

    private RectTransform parentImage;
    private Vector3[] originalPositions;

    private void Start()
    {
        parentImage = GetComponent<RectTransform>();
        originalPositions = new Vector3[dockImages.Length];

        for (int i = 0; i < dockImages.Length; i++)
        {
            originalPositions[i] = dockImages[i].localPosition;
        }
    }

    private void Update()
    {
        if (!uiManager.gameStarted)
            return;

        Vector2 mousePosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, mousePosition, null, out Vector2 localMousePosition);

        if (RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, mousePosition))
        {
            float[] scales = new float[dockImages.Length];

            for (int i = 0; i < dockImages.Length; i++)
            {
                float distanceFromMouse = Mathf.Abs(localMousePosition.x - dockImages[i].localPosition.x);
                float t = Mathf.Clamp01(distanceFromMouse / (dockImages[i].rect.width * 2));
                scales[i] = Mathf.Lerp(maxScale, 1.0f, t);
            }

            for (int i = 0; i < dockImages.Length; i++)
            {
                float targetScale = 1f;

                if (i > 0)
                {
                    targetScale = Mathf.Max(targetScale, Mathf.Lerp(scales[i], scales[i - 1], 0.5f));
                }

                if (i < dockImages.Length - 1)
                {
                    targetScale = Mathf.Max(targetScale, Mathf.Lerp(scales[i], scales[i + 1], 0.5f));
                }

                if (i > 1)
                {
                    targetScale = Mathf.Max(targetScale, Mathf.Lerp(scales[i], scales[i - 2], 0.25f));
                }

                if (i < dockImages.Length - 2)
                {
                    targetScale = Mathf.Max(targetScale, Mathf.Lerp(scales[i], scales[i + 2], 0.25f));
                }

                dockImages[i].DOScale(Vector3.one * targetScale, animationDuration);
            }
        }
        else
        {
            for (int i = 0; i < dockImages.Length; i++)
            {
                dockImages[i].DOScale(Vector3.one, animationDuration);
            }
        }

        AdjustParentSize();
        AdjustSpacing();
    }

    private void AdjustSpacing()
    {
        float totalWidth = parentPadding * 2 + (dockImages.Length - 1) * spacing;

        for (int i = 0; i < dockImages.Length; i++)
        {
            float halfWidth = dockImages[i].rect.width * dockImages[i].localScale.x / 2;
            totalWidth += halfWidth * 2;
        }

        float startX = -totalWidth / 2 + parentPadding;

        for (int i = 0; i < dockImages.Length; i++)
        {
            RectTransform rectTransform = dockImages[i];
            float halfWidth = rectTransform.rect.width * rectTransform.localScale.x / 2;
            float originalY = originalPositions[i].y;
            float heightDifference = (rectTransform.rect.height * (rectTransform.localScale.x - 1)) / 2;

            rectTransform.localPosition = new Vector3(startX + halfWidth, originalY + heightDifference, rectTransform.localPosition.z);
            startX += halfWidth * 2 + spacing;
        }
    }

    private void AdjustParentSize()
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;

        for (int i = 0; i < dockImages.Length; i++)
        {
            RectTransform rectTransform = dockImages[i];
            float halfWidth = rectTransform.rect.width * rectTransform.localScale.x / 2;

            minX = Mathf.Min(minX, rectTransform.localPosition.x - halfWidth);
            maxX = Mathf.Max(maxX, rectTransform.localPosition.x + halfWidth);
        }

        float totalWidth = maxX - minX + 2 * parentPadding;

        parentImage.sizeDelta = new Vector2(totalWidth, parentImage.sizeDelta.y);
    }
}