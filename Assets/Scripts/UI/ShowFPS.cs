using TMPro;
using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsText;

    private float fps;
    private float updateTimer;
    private bool show = true;

    private void Update()
    {
        UpdateFPS();
    }

    private void UpdateFPS()
    { 
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            show = !show;
            fpsText.enabled = show;
        }

        updateTimer -= Time.deltaTime;
        if (updateTimer < 0)
        {
            fps = 1f / Time.unscaledDeltaTime;
            fpsText.text = $"FPS: {Mathf.Round(fps)}";
            updateTimer = 0.5f;
        }
    }
}
