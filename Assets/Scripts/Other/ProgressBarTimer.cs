using System.Collections;
using UnityEngine;

public class ProgressBarTimer : MonoBehaviour
{
    public float creationTime;
    public int marineID;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= creationTime)
        {
            EventManager.TriggerEvent(GenericEvents.RecruitMarine, new Hashtable() {
            {GameplayEventHashtableParams.MarineID.ToString(), marineID}
            });

            Destroy(gameObject);
        }
    }
}
