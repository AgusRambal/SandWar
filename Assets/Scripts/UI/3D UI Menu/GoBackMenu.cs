using UnityEngine;

public class GoBackMenu : MonoBehaviour
{
    [Header("Textures")]
    [SerializeField] private Texture selectedTexturel;
    [SerializeField] private Texture idleTexture;

    [Header("Settings")]
    [SerializeField] private WindowType windowType;

    private void OnMouseDown()
    {
        MainMenu.Instance.ChangeCamera($"{windowType}");
        EventManager.TriggerEvent(GenericEvents.ButtonSound);
        transform.GetChild(0).gameObject.SetActive(false);
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
