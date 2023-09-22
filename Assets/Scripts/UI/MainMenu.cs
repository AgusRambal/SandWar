using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private GameObject options;

    private float time = 0;
    private bool changeScene = false;

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
}
