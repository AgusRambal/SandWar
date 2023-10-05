using System.Collections;
using UnityEngine;

public class CharacterCustomizationArrow : MonoBehaviour
{    
    [Header("Textures")]
    [SerializeField] private Texture selectedTexture;
    [SerializeField] private Texture idleTexture;
    
    [SerializeField] private bool leftOrRight;
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

    public void OnMouseDown()
    {
        EventManager.TriggerEvent(GenericEvents.ChangeMarine, new Hashtable() {
            {GameplayEventHashtableParams.LeftOrRight.ToString(), leftOrRight}
        });
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
