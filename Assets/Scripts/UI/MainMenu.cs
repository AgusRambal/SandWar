using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Michsky.LSS;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject options;
    [SerializeField] private Animator dollyCamAnimator;
    [SerializeField] private LSS_Manager lssManager;

    [Header("Tabs")]
    [SerializeField] private GameObject graphicsTab;
    [SerializeField] private GameObject soundTab;
    [SerializeField] private GameObject gameplayTab;

    [Header("Objects")]
    [SerializeField] private CanvasGroup canvasGroup;

    public WindowType windowTypeSelected;

    [Header("Resolution Settings")]
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private int qualityLevel;
    private bool isFullscreen;
    private float time = 0;
    private bool changeScene = false;
    private GameObject openTab;
    private TabType openTabType;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }

        DOTween.Init();
        openTab = graphicsTab;
    }
    
    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution()
    { 
        Resolution resolution = resolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ChangeMenuMaterial(Transform other, Texture texture)
    {
        for (int i = 0; i < other.childCount; i++)
        {
            other.GetChild(i).GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", texture);
        }
    }

    public IEnumerator ChangeScene()
    {
        canvasGroup.DOFade(1, .5f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(.5f);
        lssManager.LoadScene("Gameplay");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SelectTab(TabType tab)
    {
        if (openTabType == tab)
            return;

        switch (tab)
        {
            case TabType.Graphics:
                openTab.transform.DOScale(0f, .2f).OnComplete(() =>
                {
                    graphicsTab.transform.DOScale(1f, .2f);
                });
                openTab = graphicsTab;
                openTabType = TabType.Graphics;
                break;

            case TabType.Sound:
                openTab.transform.DOScale(0f, .2f).OnComplete(() =>
                {
                    soundTab.transform.DOScale(1f, .2f);
                });
                openTab = soundTab;
                openTabType = TabType.Sound;
                break;

            case TabType.Gameplay:
                openTab.transform.DOScale(0f, .2f).OnComplete(() =>
                {
                    gameplayTab.transform.DOScale(1f, .2f);
                });
                openTab = gameplayTab;
                openTabType = TabType.Gameplay;
                break;
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        this.isFullscreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    { 
        qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetInt("masterQuality", qualityLevel);
        QualitySettings.SetQualityLevel(qualityLevel);

        PlayerPrefs.SetInt("masterFullscreen", isFullscreen ? 1 : 0);
        Screen.fullScreen = isFullscreen;
    }

    public void ChangeCamera(string trigger)
    {
        dollyCamAnimator.SetTrigger(trigger);
    }
}

public enum TabType
{
    Graphics,
    Sound,
    Gameplay
}

//For customizationMenu
public enum WindowType
{
    None,
    GoToMarines,
    GoToWeapon,
    BackFromWeapon,
    BackFromMarines,
    GoToSettings,
    BackFromSettings
}