using UnityEngine;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] private Texture selectedTexturel;
    [SerializeField] private Texture idleTexture;

    private void OnMouseDown()
    {
        MainMenu.Instance.ChangeScene();
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
