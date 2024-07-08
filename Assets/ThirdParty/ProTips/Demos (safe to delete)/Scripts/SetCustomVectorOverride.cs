using TMPro;
using UnityEngine;

namespace ModelShark
{
    [RequireComponent(typeof(TooltipTrigger))]
    public class SetCustomVectorOverride : MonoBehaviour
    {
        public Vector3 positionOverride;
        public TMP_InputField xInputField;
        public TMP_InputField yInputField;
        public TMP_InputField zInputField;
        private TooltipTrigger tooltipTrigger;

        public void Start()
        {
            positionOverride = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            tooltipTrigger = gameObject.GetComponent<TooltipTrigger>();

            xInputField.text = positionOverride.x.ToString();
            yInputField.text = positionOverride.y.ToString();
            zInputField.text = positionOverride.z.ToString();

            SetVectorFromInput();
        }

        public void SetVectorFromInput()
        {
            bool changed = false;
            if (float.TryParse(xInputField.text, out var xOut))
            {
                positionOverride.x = xOut;
                changed = true;
            }
            if (float.TryParse(yInputField.text, out var yOut))
            {
                positionOverride.y = yOut;
                changed = true;
            }
            if (float.TryParse(zInputField.text, out var zOut))
            {
                positionOverride.z = zOut;
                changed = true;
            }

            if (changed)
                tooltipTrigger.overridePositionVector = positionOverride;
        }

        public void SetVectorForCanvasRenderMode(RenderMode renderMode)
        {
            if (renderMode == RenderMode.ScreenSpaceOverlay)
            {
                Vector3 pos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
                xInputField.text = pos.x.ToString();
                yInputField.text = pos.y.ToString();
                zInputField.text = pos.z.ToString();
            }
            else
            {
                Vector3 pos = new Vector3(-0.4f, 3.4f, -4.2f);
                xInputField.text = pos.x.ToString();
                yInputField.text = pos.y.ToString();
                zInputField.text = pos.z.ToString();
            }

            SetVectorFromInput();
        }
    }
}