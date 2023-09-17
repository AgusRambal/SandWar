using DG.Tweening;
using System.Collections;
using UnityEngine;

public class UIMethods : MonoBehaviour, IEventListener
{
    [SerializeField] private GameObject recruitWindow;

    private bool recruitWindowState = false;

    private void Awake()
    {    
        OnEnableEventListenerSubscriptions();
        DOTween.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventManager.TriggerEvent(GenericEvents.HandleRecruitWindow);
        }
    }

    public void HandleRecruitWindow(Hashtable hastable)
    {
        recruitWindowState = !recruitWindowState;

        if (recruitWindowState) 
        {
            recruitWindow.transform.DOScale(1f, .2f);
        }

        else
        {
            recruitWindow.transform.DOScale(0f, .2f);
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
        EventManager.StartListening(GenericEvents.HandleRecruitWindow, HandleRecruitWindow);
    }

    public void CancelEventListenerSubscriptions()
    {
        EventManager.StopListening(GenericEvents.HandleRecruitWindow, HandleRecruitWindow);
    }
}
