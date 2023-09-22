using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    //[SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private GameObject options;

    [Header("Tabs")]
    [SerializeField] private GameObject graphicsTab;
    [SerializeField] private GameObject soundTab;
    [SerializeField] private GameObject gameplayTab;

    [Header("Resolution Settings")]
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private int qualityLevel;
    private bool isFullscreen;
    private float time = 0;
    private bool changeScene = false;
    private GameObject openTab;
    private TabType openTabType;
    private bool optionsOpened = false;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            options.transform.DOScale(0f, .2f);
            optionsOpened = false;
        }

        if (changeScene)
        {
            time += Time.deltaTime;
            //CanvasGroup.alpha = time;

            if (time >= 1)
            {
                SceneManager.LoadScene("Gameplay");
                changeScene = false;
            }
        }
    }

    public void ChangeMenuMaterial(Transform other, Texture texture)
    {
        if (optionsOpened)
            return;

        for (int i = 0; i < other.childCount; i++)
        {
            other.GetChild(i).GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", texture);
        }
    }

    public void ChangeScene()
    {
        changeScene = true;
    }

    public void Options()
    {
        options.transform.DOScale(1f, .2f);
        optionsOpened = true;
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
}

public enum TabType
{
    Graphics,
    Sound,
    Gameplay
}