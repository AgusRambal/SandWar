using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public const string General_Key = "generalVolume";
    public const string Music_Key = "musicVolume";
    public const string UI_SFX_Key = "uisfxVolume";
    public const string Gameplay_Key = "GameplayVolume";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadVolume();
    }

    private void LoadVolume()
    { 
        float generalVoume = PlayerPrefs.GetFloat(General_Key, 1f);
        float musicVolume = PlayerPrefs.GetFloat(Music_Key, 1f);
        float uisfxVolume = PlayerPrefs.GetFloat(UI_SFX_Key, 1f);
        float gameplayVolume = PlayerPrefs.GetFloat(Gameplay_Key, 1f);

        audioMixer.SetFloat(SoundManager.GENERAL, Mathf.Log10(generalVoume) * 20);
        audioMixer.SetFloat(SoundManager.MUSIC, Mathf.Log10(musicVolume) * 20);
        audioMixer.SetFloat(SoundManager.UI_SFX, Mathf.Log10(uisfxVolume) * 20);
        audioMixer.SetFloat(SoundManager.GAMEPLAY, Mathf.Log10(gameplayVolume) * 20);
    }
}
