using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Texture selectedTexturel;
    [SerializeField] private Texture idleTexture;

    private void OnMouseDown()
    {
        MainMenu.Instance.Options();
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
