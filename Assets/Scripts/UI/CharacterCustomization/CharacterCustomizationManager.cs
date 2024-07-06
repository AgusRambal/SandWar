using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizationManager : MonoBehaviour
{
    [SerializeField] private Character characterScriptableObject;
    [SerializeField] private List<Character> characterScriptableObjects = new List<Character>();
    [SerializeField] private List<GameObject> allMarines = new List<GameObject>();
    [SerializeField] private List<CharacterCustomPart> allParts = new List<CharacterCustomPart>();
    public List<int> partsID = new List<int>();

    private GameObject marineSelected;
    private int marineID;
    private int selectedMarineID;

    private void Start()
    {
        OnEnableEventListenerSubscriptions();
        allMarines.ForEach(x=>x.SetActive(false));
        marineID = 0;
        selectedMarineID = 0;
        allMarines[marineID].SetActive(true);
        marineSelected = allMarines[marineID];
        characterScriptableObject = characterScriptableObjects[marineID];
    }

    public void ChangeMarine(bool leftOrRight)
    {
        if (leftOrRight)
        {            
            if (marineID >= 11)
                return;
            
            allMarines.ForEach(x=>x.SetActive(false));

            marineID++;
            allMarines[marineID].SetActive(true);
        }

        else
        {
            if (marineID <= 0)
                return;
            
            allMarines.ForEach(x=>x.SetActive(false));

            marineID--;
            allMarines[marineID].SetActive(true);
        }
        
        for (int i = 0; i < allParts.Count; i++)
        {
            allParts[i].Reset();
        }

        marineSelected = allMarines[marineID];
        characterScriptableObject = characterScriptableObjects[marineID];
        selectedMarineID = marineSelected.GetComponentInChildren<MarineRotation>().ID;
    }

    public void InstantiateObjectOnSelectedMarine(GameObject obj, int typeOfObject)
    {
        if (marineSelected.GetComponentInChildren<MarineRotation>().transformsList[typeOfObject].childCount > 0)
        {
            Destroy(marineSelected.GetComponentInChildren<MarineRotation>().transformsList[typeOfObject].GetChild(0).gameObject);
        }

        GameObject customPart = Instantiate(obj, marineSelected.GetComponentInChildren<MarineRotation>().transformsList[typeOfObject]);
        customPart.transform.localScale = Vector3.one;
        customPart.transform.localPosition = Vector3.zero;
        customPart.transform.localRotation  = Quaternion.identity;
    }

    public void SaveMarineCustomization(Hashtable hashtable)
    {
        for (int z = 0; z < characterScriptableObject.customizationSelected[selectedMarineID].parts.Count; z++)
        {
            characterScriptableObject.customizationSelected[selectedMarineID].parts[z] = partsID[z];
        }
    }

    public void OnEnableEventListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.SaveMarineCustomization, SaveMarineCustomization);
    }

    public void CancelEventListenerSubscriptions()
    {
        EventManager.StopListening(GenericEvents.SaveMarineCustomization, SaveMarineCustomization);
    }

    private void OnDestroy()
    {
        CancelEventListenerSubscriptions();
    }

    private void OnDisable()
    {
        CancelEventListenerSubscriptions();
    }
}
