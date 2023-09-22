using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private GameObject options;

    [Header("Tabs")]
    [SerializeField] private GameObject graphicsTab;
    [SerializeField] private GameObject soundTab;
    [SerializeField] private GameObject gameplayTab;

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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            options.transform.DOScale(0f, .2f);
        }

        if (changeScene)
        {
            time += Time.deltaTime;
            CanvasGroup.alpha = time;

            if (time >= 1)
            {
                SceneManager.LoadScene("Gameplay");
                changeScene = false;
            }
        }
    }

    public void ChangeMenuMaterial(Transform other, Texture texture)
    {
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
}

public enum TabType
{
    Graphics,
    Sound,
    Gameplay
}