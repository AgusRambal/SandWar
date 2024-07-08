using UnityEngine;
using TMPro;

namespace ModelShark
{
    public class TextMeshProLinkButtons : MonoBehaviour
    {
        public TextMeshProUGUI dynamicTextField;

        public void ButtonPizzaClick()
        {
            dynamicTextField.text = "Good choice! Notice how <link=\"pizza|MetroSimple (TMP)\"><color=\"yellow\">this tooltip</color></link> is using the MetroSimple style. You can specify which tooltip template to use by putting the template name after the link name, separated by a pipe, as so:\n\n˂link=\"pizza|MetroSimple (TMP)\"˃this tooltip˂/link˃";
        }

        public void ButtonWafflesClick()
        {
            dynamicTextField.text = "Another great option! Notice how <link=\"waffles|CopperFiligreeSimple (TMP)\"><color=\"yellow\">this tooltip</color></link> is using the Copper Filigree style. You can specify which tooltip template to use by putting the template name after the link name, separated by a pipe, as so:\n\n˂link=\"waffles|CopperFiligreeSimple (TMP)\"˃this tooltip˂/link˃";
        }
    }
}