using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour, IEventListener
{
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

        for (int i = 0; i < GameManager.Instance.allMarines.Count; i++) 
        {
            if (GameManager.Instance.allMarines[i].Id == ID)
            {
                myMarines.Add(GameManager.Instance.allMarines[i]); 
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
