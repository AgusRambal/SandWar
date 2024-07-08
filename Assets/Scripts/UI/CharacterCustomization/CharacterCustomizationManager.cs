using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizationManager : MonoBehaviour
{
    [Header("Scriptable objects")]
    [SerializeField] private Character characterScriptableObject;
    [SerializeField] private List<Character> characterScriptableObjects = new List<Character>();

    [Header("Marines")]
    [SerializeField] private List<GameObject> allMarines = new List<GameObject>();
    [SerializeField] private List<CharacterCustomPart> allParts = new List<CharacterCustomPart>();

    [Header("IDs")]
    public List<int> partsID = new List<int>();

    [Header("Objects")]
    public List<GameObject> blockParts = new List<GameObject>();

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
        ResetMarine();
    }

    public void ChangeMarine(bool leftOrRight)
    {
        if (leftOrRight)
        {
            if (marineID >= 11)
                return;

            allMarines.ForEach(x => x.SetActive(false));

            marineID++;
            allMarines[marineID].SetActive(true);
        }

        else
        {
            if (marineID <= 0)
                return;

            allMarines.ForEach(x => x.SetActive(false));

            marineID--;
            allMarines[marineID].SetActive(true);
        }

        allParts.ForEach(x => x.Reset());
        SetCustomAvailability();

        marineSelected = allMarines[marineID];
        characterScriptableObject = characterScriptableObjects[marineID];
        selectedMarineID = marineSelected.GetComponentInChildren<MarineRotation>().ID;

        ResetMarine();
    }

    private void SetCustomAvailability()
    {
        blockParts.ForEach(x => x.SetActive(false));
        allParts.ForEach(x => x.GetComponent<BoxCollider>().enabled = true);

        for (int i = 0; i < marineSelected.GetComponentInChildren<MarineRotation>().canCustomize.Count; i++)
        {
            if (marineSelected.GetComponentInChildren<MarineRotation>().canCustomize[i] == false)
            {
                allParts[i].GetComponent<BoxCollider>().enabled = false;
                blockParts[i].SetActive(true);
            }
        }
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

    public void ResetMarine()
    {
        partsID.ForEach(x => x = 0);

        for (int i = 0; i < marineSelected.GetComponentInChildren<MarineRotation>().transformsList.Count; i++)
        {
            if (marineSelected.GetComponentInChildren<MarineRotation>().transformsList[i].childCount > 0)
            {
                Destroy(marineSelected.GetComponentInChildren<MarineRotation>().transformsList[i].GetChild(0).gameObject);
            }

            var obj = characterScriptableObject.allParts[i].parts[characterScriptableObject.customizationSelected[selectedMarineID].parts[i]];

            GameObject customPart = Instantiate(obj, marineSelected.GetComponentInChildren<MarineRotation>().transformsList[i]);
            customPart.transform.localScale = Vector3.one;
            customPart.transform.localPosition = Vector3.zero;
            customPart.transform.localRotation = Quaternion.identity;
        }
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
