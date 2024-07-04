using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CustomizationMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EditMenu edit;
    [SerializeField] private GameObject customizationTabs;
    [SerializeField] private List<GameObject> goBack = new List<GameObject>();

    [Header("Textures")]
    [SerializeField] private Texture selectedTexturel;
    [SerializeField] private Texture idleTexture;

    [Header("Settings")]
    [SerializeField] private WindowType windowType;

    [Header("DollyPath")]
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineSmoothPath newPath;

    private CinemachineTrackedDolly trackedDolly;

    private void Start()
    {
        trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    private void Update()
    {
        if (!customizationTabs.activeInHierarchy)
            return;

        if (MainMenu.Instance.windowTypeSelected != WindowType.GoToMarines && MainMenu.Instance.windowTypeSelected != WindowType.GoToWeapon)
        {
            MainMenu.Instance.windowTypeSelected = WindowType.None;
        }

        if (MainMenu.Instance.windowTypeSelected == WindowType.None)
        {
            MainMenu.Instance.ChangeMenuMaterial(transform, idleTexture);

            for (int i = 0; i < edit.transform.childCount; i++)
            {
                edit.transform.GetChild(i).gameObject.SetActive(true);
            }

            edit.GetComponent<BoxCollider>().enabled = true;
            customizationTabs.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        trackedDolly.m_Path = newPath;

        MainMenu.Instance.ChangeCamera($"{windowType}");

        EventManager.TriggerEvent(GenericEvents.ButtonSound);
        Invoke(nameof(GoBackTimer), 2.5f);
    }

    private void GoBackTimer()
    {
        goBack.ForEach(x => x.SetActive(true));
    }

    private void OnMouseEnter()
    {
        MainMenu.Instance.windowTypeSelected = windowType;
        MainMenu.Instance.ChangeMenuMaterial(transform, selectedTexturel);
    }

    private void OnMouseExit()
    {
        MainMenu.Instance.ChangeMenuMaterial(transform, idleTexture);
        MainMenu.Instance.windowTypeSelected = WindowType.None;

    }
}
