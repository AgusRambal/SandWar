using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizationManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> allMarines = new List<GameObject>();
    [SerializeField] private List<CharacterCustomPart> allParts = new List<CharacterCustomPart>();

    public List<int> partsID = new List<int>();

    private GameObject marineSelected;
    private int marineID;

    private void Start()
    {
        allMarines.ForEach(x=>x.SetActive(false));
        marineID = 0;
        allMarines[marineID].SetActive(true);
        marineSelected = allMarines[marineID];
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
    }

    public void SetAllMarinesToDefault(GameObject obj, int typeOfObject)
    {
        for (int i = 0; i < allMarines.Count; i++)
        {
            GameObject customPart = Instantiate(obj, allMarines[i].GetComponentInChildren<MarineRotation>().transformsList[typeOfObject]);
            customPart.transform.localScale = Vector3.one;
            customPart.transform.localPosition = Vector3.zero;
            customPart.transform.localRotation  = Quaternion.identity;
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
}
