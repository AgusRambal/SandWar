using DG.Tweening;
using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMethods : MonoBehaviour, IEventListener
{
    [Header("References")]
    [SerializeField] private GameObject recruitWindow;
    [SerializeField] private GameObject sellOilWindow;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private Transform progressBarTransform;

    [Header("SelectWindow")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color deselectedColor;
    [SerializeField] private List<Image> marinesBorder = new List<Image>();
    [SerializeField] private List<GameObject> marinesModels = new List<GameObject>();
    [SerializeField] private TMP_Text marineName;
    [SerializeField] private TMP_Text marineclass;
    [SerializeField] private TMP_Text marineDescription;
    [SerializeField] private TMP_Text marineHealth;
    [SerializeField] private TMP_Text marineWeapon;

    [Header("Resources")]
    [SerializeField] private TMP_Text oilAmmount;
    [SerializeField] private TMP_Text dollarsAmmount;
    [SerializeField] private GameObject marketGood;
    [SerializeField] private GameObject marketBad;
    [SerializeField] private TMP_Text oilValue;
    [SerializeField] private TMP_Text dollarsValueToRecibie;

    public static bool isOverUI = false;

    private int selectedID = 1;
    private float sellTimer = 0;
    private bool sellTimerState = false;

    private void Awake()
    {    
        OnEnableEventListenerSubscriptions();
        DOTween.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllWindows();
        }

        SetSellTimerStatus();
        ShowResources();
        SetMarketStatus();
    }

    public void OpenRecruitWindow(Hashtable hashtable)
    {
        if (isOverUI)
            return;

        recruitWindow.transform.DOScale(1f, .2f);
    }

    public void OpenSellOilWindow(Hashtable hashtable)
    {
        if (isOverUI)
            return;

        sellOilWindow.transform.DOScale(1f, .2f);
    }

    public void CloseAllWindows()
    {
        recruitWindow.transform.DOScale(0f, .2f);
        sellOilWindow.transform.DOScale(0f, .2f);
    }

    public void SelectMarine(int id)
    {
        marinesBorder.ForEach(x => x.color = deselectedColor);
        marinesModels.ForEach(x => x.SetActive(false));

        var allMarines = GameManager.Instance.allMarines;

        for (int i = 0; i < allMarines.Count; i++)
        {
            if (allMarines[i].Id == id)
            {
                marinesBorder[i].color = selectedColor;
                marinesModels[i].SetActive(true);
                selectedID = id;
                marineName.text = $"{allMarines[i].MarineName}";
                marineclass.text = $"{allMarines[i].TypeMarine}";
                marineDescription.text = $"{allMarines[i].Description}";
                marineHealth.text = $"Health: {allMarines[i].MarineName}";
                marineWeapon.text = $"Weapon: {allMarines[i].Weapon}";
            }
        }
    }

    //Button
    public void RecruitMarine()
    {
        var allMarines = GameManager.Instance.allMarines;

        for (int i = 0; i < allMarines.Count; i++)
        {
            if (allMarines[i].Id == selectedID)
            {
                if (GameManager.Instance.dollarsAmmount < allMarines[i].MarineValue)
                {
                    //Mandar popUp de que no te alcanza
                    return;
                }

                GameManager.Instance.SubstractDollars(selectedID);
                GameObject progressBarInstantiated = Instantiate(progressBar, progressBarTransform);
                progressBarInstantiated.GetComponent<ProgressBarTimer>().creationTime = allMarines[i].CreationTime;
                progressBarInstantiated.GetComponent<ProgressBarTimer>().marineID = selectedID;

                int newCreationTime = 100 / (int)allMarines[i].CreationTime;

                progressBarInstantiated.GetComponent<ProgressBar>().speed = newCreationTime;
                progressBarInstantiated.GetComponent<ProgressBar>().customName = $" {allMarines[i].MarineName}";
            }
        }
    }

    public void SetMarketStatus()
    {
        if (!GameManager.Instance.badMarketStatus)
        {
            marketBad.SetActive(false);
            marketGood.SetActive(true);
            oilValue.text = $"{GameManager.Instance.goodOilSellValue}";
            dollarsValueToRecibie.text = $"{GameManager.Instance.goodDollarToReceive}";
        }

        else
        {
            marketBad.SetActive(true);
            marketGood.SetActive(false);
            oilValue.text = $"{GameManager.Instance.badOilSellValue}";
            dollarsValueToRecibie.text = $"{GameManager.Instance.badDollarToReceive}";
        }
    }

    public void SellOilButton()
    {
        if (sellTimerState)
            return;

        GameManager.Instance.SellOil();
        sellTimerState = true;
    }

    private void SetSellTimerStatus()
    {
        //Al apretar el boton, hacerlo desaparecer e instanciar un progress circular

        if (!sellTimerState)
            return;

        sellTimer += Time.deltaTime;

        if (sellTimer >= GameManager.Instance.timerReset)
        {
            sellTimer = 0;
            sellTimerState = false;
        }
    }

    public void ShowResources()
    {
        oilAmmount.text = $"{GameManager.Instance.oilAmmount} lts";
        dollarsAmmount.text = $"{GameManager.Instance.dollarsAmmount} USD";
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
        EventManager.StartListening(GenericEvents.OpenSellOilWindow, OpenSellOilWindow);
    }

    public void CancelEventListenerSubscriptions()
    {
        EventManager.StopListening(GenericEvents.OpenRecruitWindow, OpenRecruitWindow);
        EventManager.StopListening(GenericEvents.OpenSellOilWindow, OpenSellOilWindow);
    }
}
