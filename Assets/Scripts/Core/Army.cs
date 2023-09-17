using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour, IEventListener
{
    [SerializeField] private List<Marine> allMarines = new List<Marine>();
    [SerializeField] private List<Marine> myMarines = new List<Marine>();

    private void Awake()
    {
        OnEnableEventListenerSubscriptions();
    }

    public void RecruitMarine(Hashtable hashtable)
    {
        int ID = (int)hashtable[GameplayEventHashtableParams.MarineID.ToString()];

        if (myMarines.Count >= 10)
            return;

        for (int i = 0; i < allMarines.Count; i++) 
        {
            if (allMarines[i].Id == ID)
            {
                myMarines.Add(allMarines[i]); 
            }
        }
    }

    private void OnDestroy()
    {
        CancelEventListenerSubscriptions();
    }

    private void OnDisable()
    {
        CancelEventListenerSubscriptions();
    }

    public void OnEnableEventListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.RecruitMarine, RecruitMarine);
    }

    public void CancelEventListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.RecruitMarine, RecruitMarine);
    }
}
