using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomPart : MonoBehaviour
{
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
    }

    private void OnMouseDown()
    {
        EventManager.TriggerEvent(GenericEvents.ChangeMarineCustomization, new Hashtable() {
            {GameplayEventHashtableParams.MarineSkinList.ToString(), partList},
            {GameplayEventHashtableParams.ListIndex.ToString(), index}
        });
    }

    private void OnMouseEnter()
    {
        //GetComponent<MeshRenderer>().material.SetTexture(BaseMap, selectedTexture);
    }

    private void OnMouseExit()
    {
        //GetComponent<MeshRenderer>().material.SetTexture(BaseMap, selectedTexture);
    }
}
