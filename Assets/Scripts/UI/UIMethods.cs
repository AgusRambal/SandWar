using DG.Tweening;
using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMethods : MonoBehaviour, IEventListener
{
    [Header("Settings")]
    [SerializeField] private List<Marine> allMarines = new List<Marine>();

    [Header("References")]
    [SerializeField] private GameObject recruitWindow;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private Transform progressBarTransform;

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

        for (int i = 0; i < allMarines.Count; i++)
        {
            if (allMarines[i].Id == id)
            {
                GameObject progressBarInstantiated = Instantiate(progressBar, progressBarTransform);
                progressBarInstantiated.GetComponent<ProgressBarTimer>().creationTime = allMarines[i].CreationTime;
                progressBarInstantiated.GetComponent<ProgressBarTimer>().marineID = id;

                int newCreationTime = 100 / (int)allMarines[i].CreationTime;

                progressBarInstantiated.GetComponent<ProgressBar>().speed = newCreationTime;
                progressBarInstantiated.GetComponent<ProgressBar>().customName = $" {allMarines[i].MarineName}";
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
        EventManager.StartListening(GenericEvents.OpenRecruitWindow, OpenRecruitWindow);
    }

    public void CancelEventListenerSubscriptions()
    {
        EventManager.StopListening(GenericEvents.OpenRecruitWindow, OpenRecruitWindow);
    }
}
