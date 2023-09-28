using UnityEngine;

public class CustomizationMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EditMenu edit;
    [SerializeField] private GameObject customizationTabs;
    [SerializeField] private GameObject goBack;

    [Header("Textures")]
    [SerializeField] private Texture selectedTexturel;
    [SerializeField] private Texture idleTexture;

    [Header("Settings")]
    [SerializeField] private WindowType windowType;
    [SerializeField] private int cameraID;
    
    private void Update()
    {
        if (!customizationTabs.activeInHierarchy)
            return;

        if (MainMenu.Instance.windowTypeSelected != WindowType.Marine && MainMenu.Instance.windowTypeSelected != WindowType.Weapon)
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
        MainMenu.Instance.ChangeCamera(cameraID);
        EventManager.TriggerEvent(GenericEvents.ButtonSound);
        Invoke("GoBackTimer", 2f);
    }

    private void GoBackTimer()
    {
        goBack.SetActive(true);
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
