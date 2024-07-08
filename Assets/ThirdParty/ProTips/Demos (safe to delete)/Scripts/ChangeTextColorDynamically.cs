using System.Text.RegularExpressions;
using UnityEngine;

namespace ModelShark
{
    /// <summary>Put this script on a TooltipTrigger. It will give you access to the ChangeTootipTextColor() method you can call from anywhere.</summary>
    [RequireComponent(typeof(TooltipTrigger))]
    public class ChangeTextColorDynamically : MonoBehaviour
    {
        public string textField = "BodyText";
        public Color32 newColor = Color.red;

        private TooltipTrigger tooltipTrigger;

        void Start()
        {
            // This is an example of changing the text on Start() to whatever was selected as the newColor.
            // But you could call the ChangeTooltipTextColor method from anywhere, as shown in the commented code below:
            // myGameObject.GetComponent<ChangeTextColorDynamically>().ChangeTooltipTextColor("BodyText", Color.green);
            ChangeTooltipTextColor(textField, newColor);
        }

        /// <summary>Changes the tooltip text color.</summary>
        /// <param name="field">The dynamic text field to change the color of.</param>
        /// <param name="color">The color to change the tooltip text to.</param>
        public void ChangeTooltipTextColor(string txtField, Color32 color)
        {
            // Get the existing text on the tooltip trigger
            tooltipTrigger = GetComponent<TooltipTrigger>();
            string existingText = tooltipTrigger.GetText(txtField);

            // If there is any text on the tooltip trigger
            if (existingText != null)
            {
                // Check if the text already begins with a color tag
                Match match = Regex.Match(existingText, "^<color[^>]*>");
                string newText;
                if (match.Success)
                {
                    // If so, modify the hex color in the color tag to our new color and replace the text on the tooltip trigger
                    Debug.Log("Replacing tooltip text color with the new color...");
                    newText = Regex.Replace(existingText, "^<color[^>]*>", $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>");
                }
                else
                {
                    // If no color tag at the beginning of the tooltip text was found, wrap the text with the new color's tag
                    Debug.Log("Adding color tags to tooltip to change the color...");
                    newText = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{existingText}</color>";
                }

                // Set the tooltip trigger to have the modified text
                tooltipTrigger.SetText(txtField, newText);
            }
        }
    }
}