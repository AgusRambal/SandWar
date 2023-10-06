using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomPart : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private CharacterCustomizationManager manager;
    
    [Header("Textures")]
    [SerializeField] private Texture selectedTexture;
    [SerializeField] private Texture idleTexture;

    [Header("List")] 
    [SerializeField] private List<GameObject> partList = new List<GameObject>();

    private int index;

    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

    private void Start()
    {
        partList.ForEach(x=>x.SetActive(false));
        index = 0;
        partList[index].SetActive(true);
        manager.InstantiateObjectOnSelectedMarine(partList[index]);
    }

    private void OnMouseDown()
    {
        ChangeMarineCustomization();
    }
    
    private void ChangeMarineCustomization()
    {
        partList.ForEach(x=>x.SetActive(false));

        if (index == partList.Count - 1)
        {
            index = -1;
        }
        
        index++;
        
        partList[index].SetActive(true);
        manager.InstantiateObjectOnSelectedMarine(partList[index]);
    }
    
    private void OnMouseEnter()
    {
        GetComponent<MeshRenderer>().materials[1].SetTexture(BaseMap, selectedTexture);
    }

    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().materials[1].SetTexture(BaseMap, idleTexture);
    }
}
