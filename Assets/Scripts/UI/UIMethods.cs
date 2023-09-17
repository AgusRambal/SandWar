using DG.Tweening;
using System.Collections;
using UnityEngine;

public class UIMethods : MonoBehaviour, IEventListener
{
    [SerializeField] private GameObject recruitWindow;

    public static bool isOverUI = false;

    private void Awake()
    {    
        OnEnableEventListenerSubscriptions();
        DOTween.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseRecruitWindow();
        }
    }

    public void OpenRecruitWindow(Hashtable hastable)
    {
        if (isOverUI)
            return;

        recruitWindow.transform.DOScale(1f, .2f);
    }

    public void CloseRecruitWindow()
    {
        recruitWindow.transform.DOScale(0f, .2f);
    }

    //Button
    public void RecruitMarine(int id)
    {
        EventManager.TriggerEvent(GenericEvents.RecruitMarine, new Hashtable() {
        {GameplayEventHashtableParams.MarineID.ToString(), id}       
        });
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
        EventManager.StartListening(GenericEvents.OpenRecruitWindow, OpenRecruitWindow);
    }

    public void CancelEventListenerSubscriptions()
    {
        EventManager.StopListening(GenericEvents.OpenRecruitWindow, OpenRecruitWindow);
    }
}
