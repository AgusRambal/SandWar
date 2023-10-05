using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizationManager : MonoBehaviour, IEventListener
{
    [SerializeField] private List<GameObject> allMarines = new List<GameObject>();
    
    [SerializeField] private List<GameObject> helmets = new List<GameObject>();
    [SerializeField] private List<GameObject> hairs = new List<GameObject>();

    private int marineID;

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

    private void ChangeMarineCustomization(Hashtable hashtable)
    {
        List<GameObject> marineSkinPart = (List<GameObject>)hashtable[GameplayEventHashtableParams.MarineSkinList.ToString()];
        int listIndex = (int)hashtable[GameplayEventHashtableParams.ListIndex.ToString()];
        
        marineSkinPart.ForEach(x=>x.SetActive(false));

        if (listIndex == marineSkinPart.Count - 1)
        {
            listIndex = -1;
        }
        
        listIndex++;
        marineSkinPart[listIndex].SetActive(true);
    }

    public void OnEnableEventListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.ChangeMarine, ChangeMarine);
        EventManager.StartListening(GenericEvents.ChangeMarineCustomization, ChangeMarineCustomization);
    }

    public void CancelEventListenerSubscriptions()
    {
        EventManager.StopListening(GenericEvents.ChangeMarine, ChangeMarine);
        EventManager.StopListening(GenericEvents.ChangeMarineCustomization, ChangeMarineCustomization);
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
