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
    [SerializeField] private int typeOfPart;
    
    private int index;

    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

    private void Start()
    {
        index = 0;
        partList.ForEach(x=>x.SetActive(false));
        partList[index].SetActive(true);
        manager.partsID[typeOfPart] = index;
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
        manager.InstantiateObjectOnSelectedMarine(partList[index], typeOfPart);
        manager.partsID[typeOfPart] = index;
    }

    public void Reset()
    {
        index = 0;
        partList.ForEach(x => x.SetActive(false));
        manager.partsID.ForEach(x => x = index);
        partList[index].SetActive(true);
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
