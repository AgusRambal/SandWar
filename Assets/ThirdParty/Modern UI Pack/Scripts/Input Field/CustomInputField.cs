using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
#if !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

namespace Michsky.MUIP
{
    [RequireComponent(typeof(TMP_InputField))]
    [RequireComponent(typeof(Animator))]
    public class CustomInputField : MonoBehaviour
    {
        [Header("Resources")]
        public TMP_InputField inputText;
        public Animator inputFieldAnimator;

        [Header("Settings")]
        public bool processSubmit = false;
        public bool clearOnSubmit = true;

        [Header("Events")]
        public UnityEvent onSubmit;

        // Hidden variables
        private string inAnim = "In";
        private string outAnim = "Out";
        private string instaInAnim = "Instant In";
        private string instaOutAnim = "Instant Out";

        void Awake()
        {
            if (inputText == null) { inputText = gameObject.GetComponent<TMP_InputField>(); }
            if (inputFieldAnimator == null) { inputFieldAnimator = gameObject.GetComponent<Animator>(); }

            inputText.onSelect.AddListener(delegate { AnimateIn(); });
            inputText.onEndEdit.AddListener(delegate { AnimateOut(); });
            UpdateStateInstant();
        }

        void OnEnable()
        {
            if (inputText == null) { return; }
            if (gameObject.activeInHierarchy == true) { StartCoroutine("DisableAnimator"); }

            inputText.ForceLabelUpdate();
            UpdateStateInstant();
        }

        void Update()
        {
            if (processSubmit == false ||
                string.IsNullOrEmpty(inputText.text) == true ||
                EventSystem.current.currentSelectedGameObject != inputText.gameObject)
            { return; }

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.Return)) { onSubmit.Invoke(); if (clearOnSubmit == true) { inputText.text = ""; } }
#elif ENABLE_INPUT_SYSTEM
            if (Keyboard.current.enterKey.wasPressedThisFrame) { onSubmit.Invoke(); if (clearOnSubmit == true) { inputText.text = ""; } }
#endif
        }

        public void AnimateIn() 
        {      
            if (inputFieldAnimator.gameObject.activeInHierarchy == true && !inputFieldAnimator.GetCurrentAnimatorStateInfo(0).IsName("Instant In")) 
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");

                inputFieldAnimator.enabled = true;
                inputFieldAnimator.Play(inAnim);
            }
        }

        public void AnimateOut()
        {
            StopCoroutine("DisableAnimator");

            if (inputFieldAnimator.gameObject.activeInHierarchy == true)
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");

                inputFieldAnimator.enabled = true;
                if (inputText.text.Length == 0) { inputFieldAnimator.Play(outAnim); }     
            }
        }

        public void UpdateState()
        {
            if (inputText.text.Length == 0) { AnimateOut(); }
            else { AnimateIn(); }
        }

        public void UpdateStateInstant()
        {
            inputFieldAnimator.enabled = true;

            StopCoroutine("DisableAnimator");
            StartCoroutine("DisableAnimator");

            if (inputText.text.Length == 0) { inputFieldAnimator.Play(instaOutAnim); }
            else { inputFieldAnimator.Play(instaInAnim); }
        }

        IEnumerator DisableAnimator()
        {
            yield return new WaitForSecondsRealtime(MUIPInternalTools.GetAnimatorClipLength(inputFieldAnimator, "Standard In") + 0.1f);
            inputFieldAnimator.enabled = false;
        }
    }
}