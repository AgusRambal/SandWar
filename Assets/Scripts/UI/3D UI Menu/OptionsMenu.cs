using Cinemachine;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [Header("Textures")]
    [SerializeField] private Texture selectedTexturel;
    [SerializeField] private Texture idleTexture;

    [Header("Settings")]
    [SerializeField] private WindowType windowType;
    [SerializeField] private GameObject goBack;

    [Header("DollyPath")]
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineSmoothPath newPath;

    private CinemachineTrackedDolly trackedDolly;

    private void Start()
    {
        trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    private void OnMouseDown()
    {
        trackedDolly.m_Path = newPath;
        EventManager.TriggerEvent(GenericEvents.ButtonSound);
        MainMenu.Instance.ChangeCamera($"{windowType}");
        Invoke(nameof(GoBackTimer), 2.5f);
    }

    private void GoBackTimer()
    {
        goBack.SetActive(true);
    }

    private void OnMouseEnter()
    {
        MainMenu.Instance.ChangeMenuMaterial(transform, selectedTexturel);
    }

    private void OnMouseExit()
    {
        MainMenu.Instance.ChangeMenuMaterial(transform, idleTexture);
    }

}
