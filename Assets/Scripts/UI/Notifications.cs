using DG.Tweening;
using TMPro;
using UnityEngine;

public class Notifications : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text contentText;

    public string title;
    public string content;

    private void Start()
    {
        DOTween.Init();
        titleText.text = title;
        contentText.text = content;
        StartFading();
    }

    private void StartFading()
    {
        gameObject.transform.DOScale(1f, .2f);
        Invoke("EndNotification", 3f);
    }

    private void EndNotification()
    {
        gameObject.transform.DOScale(0f, .2f);
        Destroy(gameObject, .21f);
    }
}
