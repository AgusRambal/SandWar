using System.Collections;
using UnityEngine;

public class CharacterCustomizationArrow : MonoBehaviour
{       
    [Header("References")] 
    [SerializeField] private CharacterCustomizationManager manager;
    
    [Header("Textures")]
    [SerializeField] private Texture selectedTexture;
    [SerializeField] private Texture idleTexture;
    
    [SerializeField] private bool leftOrRight;
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

    public void OnMouseDown()
    {
        manager.ChangeMarine(leftOrRight);
    }
    
    private void OnMouseEnter()
    {        
        GetComponent<MeshRenderer>().material.SetTexture(BaseMap, selectedTexture);
    }

    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material.SetTexture(BaseMap, idleTexture);
    }
}
