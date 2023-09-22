using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickSFX : MonoBehaviour
{
    [SerializeField] private AudioClip SFX;
    [SerializeField] private List<AudioClip> SFXs = new List<AudioClip>();
    [SerializeField] private AudioSource AudioSource;

    public bool playAtStart = false;
    public bool useRandomList = false;

    private void Start()
    {
        if (playAtStart)
        {
            if (useRandomList)
            {
                EventManager.TriggerEvent(GenericEvents.PlaySound, new Hashtable() {
                {GameplayEventHashtableParams.AudioClip.ToString(), SFXs[Random.Range(0, SFXs.Count)]},
                {GameplayEventHashtableParams.AudioSource.ToString(), AudioSource}
                });
            }

            else
            {
                EventManager.TriggerEvent(GenericEvents.PlaySound, new Hashtable() {
                {GameplayEventHashtableParams.AudioClip.ToString(), SFX},
                {GameplayEventHashtableParams.AudioSource.ToString(), AudioSource}
                });
            }
        }
    }

    //Button only
    public void PlaySoundFX()
    {
        EventManager.TriggerEvent(GenericEvents.PlaySound, new Hashtable() {
        {GameplayEventHashtableParams.AudioClip.ToString(), SFX},
        {GameplayEventHashtableParams.AudioSource.ToString(), AudioSource}
        });
    }
}
