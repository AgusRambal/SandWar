using DG.Tweening;
using UnityEngine;

public class DockSystem : MonoBehaviour
{
    public RectTransform[] dockImages;
    public float maxScale = 1.6f;
    public float adjacentScale1 = 1.4f;
    public float adjacentScale2 = 1.2f;
    public float animationDuration = 0.2f;
    public float spacing = 100f; // Espacio deseado entre las imágenes
    private Vector3[] originalPositions;

    private void Start()
    {
        // Guardar las posiciones originales
        originalPositions = new Vector3[dockImages.Length];
        for (int i = 0; i < dockImages.Length; i++)
        {
            originalPositions[i] = dockImages[i].localPosition;
        }

        AdjustSpacing();
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, mousePosition, null, out Vector2 localMousePosition);

        // Verificar si el ratón está dentro del dock
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

                dockImages[i].DOScale(Vector3.one * targetScale, animationDuration);
            }
        }
        else
        {
            // Resetear las escalas si el ratón no está en el dock
            for (int i = 0; i < dockImages.Length; i++)
            {
                dockImages[i].DOScale(Vector3.one, animationDuration);
            }
        }

        AdjustSpacing();
    }

    private void AdjustSpacing()
    {
        float currentPosition = originalPositions[0].x;

        for (int i = 0; i < dockImages.Length; i++)
        {
            RectTransform rectTransform = dockImages[i];
            float scaleFactor = rectTransform.localScale.x;
            float halfWidth = rectTransform.rect.width * scaleFactor / 2;

            if (i > 0)
            {
                float previousHalfWidth = dockImages[i - 1].rect.width * dockImages[i - 1].localScale.x / 2;
                currentPosition += previousHalfWidth + spacing + halfWidth;
            }

            // Ajustar la posición y para escalar hacia arriba
            float originalY = originalPositions[i].y;
            float heightDifference = (rectTransform.rect.height * (scaleFactor - 1)) / 2;
            rectTransform.localPosition = new Vector3(currentPosition, originalY + heightDifference, rectTransform.localPosition.z);
            currentPosition += halfWidth;
        }
    }
}