using DG.Tweening;
using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using Core.Characters;
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
    [SerializeField] private TMP_Text marineClass;
    [SerializeField] private TMP_Text marineDescription;
    [SerializeField] private TMP_Text marineHealth;
    [SerializeField] private TMP_Text marineWeapon;
    [SerializeField] private TMP_Text marineCost;
    [SerializeField] private TMP_Text marineTime;

    [Header("Resources")]
    [SerializeField] private TMP_Text oilAmount;
    [SerializeField] private TMP_Text dollarsAmount;
    [SerializeField] private GameObject marketGood;
    [SerializeField] private GameObject marketBad;
    [SerializeField] private GameObject sellButton;
    [SerializeField] private GameObject progressForSellingAgain;
    [SerializeField] private TMP_Text oilValue;
    [SerializeField] private TMP_Text dollarsValueToReceive;
    [SerializeField] private GameObject greenArrow;
    [SerializeField] private GameObject redArrow;
    [SerializeField] private Transform oilArrowParent;
    [SerializeField] private Transform dollarArrowParent;

    [Header("Notifications")]
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private Transform notificationParent;

    [Header("SelectableCharacters")]
    public List<SelectableCharacter> selectableCharacterPrefabs = new List<SelectableCharacter>();
    [SerializeField] private GameObject selectableCharacterPrefab;
    [SerializeField] private Transform selectableCharacterParent;

    public static bool isOverUI = false;

    private int selectedID = 1;
    private float sellTimer;
    private bool sellTimerState;
    private bool notificationActive;

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

    private void OpenRecruitWindow(Hashtable hashtable)
    {
        if (isOverUI)
            return;

        recruitWindow.transform.DOScale(1f, .2f);
    }

    private void OpenSellOilWindow(Hashtable hashtable)
    {
        if (isOverUI)
            return;

        sellOilWindow.transform.DOScale(1f, .2f);
    }

    private void CloseAllWindows()
    {
        recruitWindow.transform.DOScale(0f, .2f);
        sellOilWindow.transform.DOScale(0f, .2f);
    }

    public void CloseRecruitWindows()
    {
        recruitWindow.transform.DOScale(0f, .2f);
    }

    public void CloseSellOilWindows()
    {
        sellOilWindow.transform.DOScale(0f, .2f);
    }

    public void SelectMarine(int id)
    {
        marinesBorder.ForEach(x => x.color = deselectedColor);
        marinesModels.ForEach(x => x.SetActive(false));

        var allMarines = GameManager.Instance.allMarines;

        for (int i = 0; i < allMarines.Count; i++)
        {
            if (allMarines[i].Id != id) 
                continue;
            
            marinesBorder[i].color = selectedColor;
            marinesModels[i].SetActive(true);
            selectedID = id;
            marineName.text = $"{allMarines[i].MarineName}";
            marineClass.text = $"{allMarines[i].SubType}";
            marineDescription.text = $"{allMarines[i].Description}";
            marineHealth.text = $"Health: {allMarines[i].MaxHealth}";
            marineWeapon.text = $"Weapon: {allMarines[i].Weapon.WeaponName}";
            marineCost.text = $"Cost: {allMarines[i].MarineValue} USD";
            marineTime.text = $"Recruiting time: {allMarines[i].CreationTime} secs";
        }
    }

    //Button
    public void RecruitMarine()
    {
        if (notificationParent.childCount == 0)
        {
            notificationActive = false;
        }

        var allMarines = GameManager.Instance.allMarines;

        for (int i = 0; i < allMarines.Count; i++)
        {
            if (allMarines[i].Id != selectedID) 
                continue;
            
            if (GameManager.Instance.dollarsAmount < allMarines[i].MarineValue)
            {
                if (notificationActive) 
                    return;
                    
                ShowNotification("Not enough cash", "You can't recruit this marine because you doesn't have the right amount of cash");
                notificationActive = true;

                return;
            }

            GameManager.Instance.SubstractDollars(selectedID);
            FeedbackArrowRed(1);
            GameObject progressBarInstantiated = Instantiate(progressBar, progressBarTransform);
            progressBarInstantiated.GetComponent<ProgressBarTimer>().creationTime = allMarines[i].CreationTime;
            progressBarInstantiated.GetComponent<ProgressBarTimer>().marineID = selectedID;

            var newCreationTime = 100 / (int)allMarines[i].CreationTime;

            progressBarInstantiated.GetComponent<ProgressBar>().speed = newCreationTime;
            progressBarInstantiated.GetComponent<ProgressBar>().customName = $" {allMarines[i].MarineName}";
        }
    }

    private void SetMarketStatus()
    {
        if (!GameManager.Instance.badMarketStatus)
        {
            marketBad.SetActive(false);
            marketGood.SetActive(true);
            oilValue.text = $"{GameManager.Instance.goodOilSellValue}";
            dollarsValueToReceive.text = $"{GameManager.Instance.goodDollarToReceive}";
        }

        else
        {
            marketBad.SetActive(true);
            marketGood.SetActive(false);
            oilValue.text = $"{GameManager.Instance.badOilSellValue}";
            dollarsValueToReceive.text = $"{GameManager.Instance.badDollarToReceive}";
        }
    }

    public void SellOilButton()
    {
        if (sellTimerState)
            return;

        if (GameManager.Instance.oilAmount < GameManager.Instance.actualMarketPrice)
            return;

        FeedbackArrowRed(0);
        FeedbackArrowGreen(1);

        GameManager.Instance.SellOil();
        sellTimerState = true;
    }

    private void FeedbackArrowRed(int id)
    {
        Instantiate(redArrow, id == 0 ? oilArrowParent : dollarArrowParent);
    }

    public void FeedbackArrowGreen(int id)
    {
        Instantiate(greenArrow, id == 0 ? oilArrowParent : dollarArrowParent);
    }

    private void SetSellTimerStatus()
    {
        if (!sellTimerState)
            return;

        sellButton.SetActive(false);
        progressForSellingAgain.SetActive(true);

        sellTimer += Time.deltaTime;

        if (sellTimer >= GameManager.Instance.timerReset)
        {
            sellTimer = 0;
            sellTimerState = false;
            sellButton.SetActive(true);
            progressForSellingAgain.SetActive(false);
            progressForSellingAgain.GetComponent<ProgressBar>().currentPercent = 0;
        }
    }

    private void ShowResources()
    {
        oilAmount.text = $"{GameManager.Instance.oilAmount:F2} lts";
        dollarsAmount.text = $"{GameManager.Instance.dollarsAmount} USD";
    }

    public void ShowNotification(string title, string content)
    {
        GameObject notification = Instantiate(notificationPrefab, notificationParent);
        notification.GetComponent<Notifications>().title = title;
        notification.GetComponent<Notifications>().content = content;
    }

    public void CreateSelectableIcon(int id, Marine marine)
    {
        GameObject icon = Instantiate(selectableCharacterPrefab, selectableCharacterParent);
        selectableCharacterPrefabs.Add(icon.GetComponent<SelectableCharacter>());
        icon.GetComponent<SelectableCharacter>().Uimethods = this;
        icon.GetComponent<SelectableCharacter>().marineObject = marine;
        marine.characterSelectionImage = icon.GetComponent<SelectableCharacter>();
        icon.GetComponent<SelectableCharacter>().marineImage.sprite = GameManager.Instance.allMarines[id - 1].Icon;
        icon.transform.DOScale(1f, .2f);
    }

    public void OnSelectIcon(SelectableCharacter icon, Marine marine)
    {
        DeselectAll();
        icon.selectedSprite.SetActive(true);
        SelectionManager.Instance.Select(marine);
    }

    public void DeselectAll()
    {
        for (int i = 0; i < selectableCharacterPrefabs.Count; i++)
        {
            selectableCharacterPrefabs[i].selectedSprite.SetActive(false);
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
        EventManager.StartListening(GenericEvents.OpenSellOilWindow, OpenSellOilWindow);
    }

    public void CancelEventListenerSubscriptions()
    {
        EventManager.StopListening(GenericEvents.OpenRecruitWindow, OpenRecruitWindow);
        EventManager.StopListening(GenericEvents.OpenSellOilWindow, OpenSellOilWindow);
    }
}
