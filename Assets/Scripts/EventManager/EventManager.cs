using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    private Dictionary<GenericEvents, Action<Hashtable>> eventsGenericDictionary;

    private static EventManager eventManager;

    public static EventManager Instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
                if (eventManager)
                    DontDestroyOnLoad(eventManager.gameObject);

                if (!eventManager)
                {
                    //Debug.LogError("You need EventManger.cs in a GameObject on the scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        eventsGenericDictionary ??= new Dictionary<GenericEvents, Action<Hashtable>>();
    }


    public static void StartListening(GenericEvents eventName, Action<Hashtable> listener)
    {
        if (Instance.eventsGenericDictionary.ContainsKey(eventName))
            Instance.eventsGenericDictionary[eventName] += listener;
        else
            Instance.eventsGenericDictionary.Add(eventName, listener);
    }

    public static void StopListening(GenericEvents eventName, Action<Hashtable> listener)
    {
        if (Instance && Instance.eventsGenericDictionary.ContainsKey(eventName))
        {
            Instance.eventsGenericDictionary[eventName] -= listener;
        }
    }

    public static void TriggerEvent(GenericEvents eventName, Hashtable eventParams = default(Hashtable))
    {
        Action<Hashtable> thisEvent = null;
        if (Instance.eventsGenericDictionary.TryGetValue(eventName, out thisEvent))
        {
            if (thisEvent != null)
                thisEvent.Invoke(eventParams);
        }
    }
}



