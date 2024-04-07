using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour, IEventListener
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("AudioSources")]
    [SerializeField] private AudioSource UI_FX_Source;

    [Header("Sliders")]
    /*[SerializeField] private Slider generalSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider UIFXSlider;
    [SerializeField] private Slider GameplaySlider;*/

    [Header("Prepared sounds")]
    [SerializeField] private AudioClip buttonSound;

    public const string GENERAL = "GeneralVolume";
    public const string MUSIC = "MusicVolume";
    public const string UI_SFX = "UISFXVolume";
    public const string GAMEPLAY = "GameplayVolume";

    private void Awake()
    {
        OnEnableEventListenerSubscriptions();
       /* generalSlider.onValueChanged.AddListener(SetGeneralVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        UIFXSlider.onValueChanged.AddListener(SetUISFXVolume);
        GameplaySlider.onValueChanged.AddListener(SetGameplayVolume);*/
    }

    private void Start()
    {
        /*generalSlider.value = PlayerPrefs.GetFloat(AudioManager.General_Key, 1f);
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.Music_Key, 1f);
        UIFXSlider.value = PlayerPrefs.GetFloat(AudioManager.UI_SFX_Key, 1f);
        GameplaySlider.value = PlayerPrefs.GetFloat(AudioManager.Gameplay_Key, 1f);*/
    }

    public void PlaySound(Hashtable hashtable)
    {
        AudioClip clip = (AudioClip)hashtable[GameplayEventHashtableParams.AudioClip.ToString()];
        AudioSource source = (AudioSource)hashtable[GameplayEventHashtableParams.AudioSource.ToString()];

        source.PlayOneShot(clip);
    }

    public void ButtonSound(Hashtable hashtable)
    {
        UI_FX_Source.PlayOneShot(buttonSound);
    }

    public void SetGeneralVolume(float value)
    {
        mixer.SetFloat(GENERAL, Mathf.Log10(value) * 20);
    } 

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat(MUSIC, Mathf.Log10(value) * 20);
    }    

    public void SetUISFXVolume(float value)
    {
        mixer.SetFloat(UI_SFX, Mathf.Log10(value) * 20);
    }    
    
    public void SetGameplayVolume(float value)
    {
        mixer.SetFloat(GAMEPLAY, Mathf.Log10(value) * 20);
    }

    public void OnEnableEventListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.PlaySound, PlaySound);
        EventManager.StartListening(GenericEvents.ButtonSound, ButtonSound);
    }

    public void CancelEventListenerSubscriptions()
    {
        EventManager.StopListening(GenericEvents.PlaySound, PlaySound);
        EventManager.StopListening(GenericEvents.ButtonSound, ButtonSound);
    }

    private void OnDisable()
    {
        /*PlayerPrefs.SetFloat(AudioManager.General_Key, generalSlider.value);
        PlayerPrefs.SetFloat(AudioManager.Music_Key, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.UI_SFX_Key, UIFXSlider.value);
        PlayerPrefs.SetFloat(AudioManager.Gameplay_Key, GameplaySlider.value);*/

        CancelEventListenerSubscriptions();
    }

    void OnDestroy()
    {
        CancelEventListenerSubscriptions();
    }
}
