using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [Header("Textures")]
    [SerializeField] private Texture selectedTexturel;
    [SerializeField] private Texture idleTexture;

    private void OnMouseDown()
    {
        MainMenu.Instance.Options();
        EventManager.TriggerEvent(GenericEvents.ButtonSound);
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
