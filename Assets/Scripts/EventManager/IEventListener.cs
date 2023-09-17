using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IEventListener
{
    void OnEnableEventListenerSubscriptions();


    void CancelEventListenerSubscriptions();

}


public static class EventTriggers
{
    public static void TriggerEvent<T>(GenericEvents eventName, string parameterName, T parameter)
    {
        EventManager.TriggerEvent(eventName, new Hashtable() { { parameterName, parameter } });
    }


    public static void TriggerEvent(GenericEvents eventName)
    {
        EventManager.TriggerEvent(eventName, null);
    }
}
