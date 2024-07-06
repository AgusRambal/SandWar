using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMarineCustom : MonoBehaviour
{
    [Header("Textures")]
    [SerializeField] private Texture selectedTexturel;
    [SerializeField] private Texture idleTexture;

    private void OnMouseDown()
    {
        EventManager.TriggerEvent(GenericEvents.SaveMarineCustomization);
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
