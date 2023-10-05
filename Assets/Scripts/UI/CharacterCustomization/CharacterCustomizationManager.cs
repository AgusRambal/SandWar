using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizationManager : MonoBehaviour, IEventListener
{
    [SerializeField] private List<GameObject> allMarines = new List<GameObject>();

    public int marineID;

    private void Awake()
    {
        OnEnableEventListenerSubscriptions();
    }

    private void Start()
    {
        allMarines.ForEach(x=>x.SetActive(false));
        marineID = 0;
        allMarines[marineID].SetActive(true);
    }

    private void ChangeMarine(Hashtable hashtable)
    {
        bool leftOrRight = (bool)hashtable[GameplayEventHashtableParams.LeftOrRight.ToString()];
        
        if (leftOrRight)
        {            
            if (marineID == 11)
                return;
            
            allMarines.ForEach(x=>x.SetActive(false));

            marineID++;
            allMarines[marineID].SetActive(true);
        }

        else
        {
            if (marineID == 0)
                return;
            
            allMarines.ForEach(x=>x.SetActive(false));

            marineID--;
            allMarines[marineID].SetActive(true);
        }
    }


    public void OnEnableEventListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.ChangeMarine, ChangeMarine);
    }

    public void CancelEventListenerSubscriptions()
    {
        EventManager.StopListening(GenericEvents.ChangeMarine, ChangeMarine);
    }
    
    private void OnDisable()
    {
        CancelEventListenerSubscriptions();
    }

    private void OnDestroy()
    {
        CancelEventListenerSubscriptions();
    }
}
