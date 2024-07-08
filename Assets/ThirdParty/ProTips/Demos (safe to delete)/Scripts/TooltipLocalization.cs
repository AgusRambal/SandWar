using System.Collections.Generic;
using UnityEngine;

namespace ModelShark
{
    public class TooltipLocalization : MonoBehaviour
    {
        public string selectedLanguage = "en";
        public string phraseKey = "hat";

        // Setup your localization dictionaries
        private Dictionary<string, string> englishText = new Dictionary<string, string>();
        private Dictionary<string, string> spanishText = new Dictionary<string, string>();

        private void Awake()
        {
            AddEntriesToLanguageDictionary();
        }

        private void Start()
        { 
            // Get the TooltipTrigger component on the current gameobject.
            TooltipTrigger tooltipTrigger = gameObject.GetComponent<TooltipTrigger>();

            // Set the tooltip text, depending on the language chosen.
            switch (selectedLanguage)
            {
                case "en":
                    tooltipTrigger.SetText("BodyText", englishText[phraseKey]);
                    break;
                case "es":
                    tooltipTrigger.SetText("BodyText", spanishText[phraseKey]);
                    break;
            }
        }

        private void AddEntriesToLanguageDictionary()
        {
            // Add phrases for each dictionary here
            englishText.Add("greeting", "Hello!");
            englishText.Add("hat", "I like your hat.");

            spanishText.Add("greeting", "Hola!");
            spanishText.Add("hat", "Me gusta tu sombrero.");
        }
    }
}
