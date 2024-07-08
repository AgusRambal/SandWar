using UnityEngine;

namespace ModelShark
{
    /// <summary>
    /// Put this script on your Dropdown control. Then, you can add an Option Tooltip text for each of the items in your dropdown.
    /// Note: this only works with a simple list added through the editor, not a dynamic list of dropdown items added through code.
    /// It also assumes you are using a simple tooltip that uses the %BodyText% parameter field. You'll have to change the code a
    /// little if you're using a more complex tooltip.
    /// </summary>
    public class DropDownTooltip : MonoBehaviour
    {
        public string[] optionTooltips;

        private TooltipTrigger parentTooltipTrigger;

        public void Start()
        {
            parentTooltipTrigger = GetComponent<TooltipTrigger>();
        }

        public void Update()
        {
            if (transform.childCount > 3) // dropdown list is open
            {
                // Disable the parent tooltip (if any) while the dropdown list is open.
                // This prevents a flickering tooltip caused by the gap between the dropdown control (containing the
                // dropdown tooltip) and the dropdown list (containing tooltips for each dropdown option item)
                if (parentTooltipTrigger != null && parentTooltipTrigger.enabled)
                    parentTooltipTrigger.enabled = false;

                // Find the TooltipTriggers on the Option Items in the DropDownList and assign their tooltip text accordingly.
                TooltipTrigger[] tooltipTriggers = GetComponentsInChildren<TooltipTrigger>();
                int currOption = 0;
                foreach (TooltipTrigger tooltipTrigger in tooltipTriggers)
                {
                    if (tooltipTrigger == parentTooltipTrigger) continue; // don't change the parent tooltip (if any).
                    tooltipTrigger.SetText("BodyText", optionTooltips[currOption]);
                    currOption++;
                }
            }
            else
            {
                // Re-enable the parent tooltip trigger once the dropdown list is closed.
                if (parentTooltipTrigger != null && !parentTooltipTrigger.enabled)
                    parentTooltipTrigger.enabled = true;
            }
        }
    }
}
