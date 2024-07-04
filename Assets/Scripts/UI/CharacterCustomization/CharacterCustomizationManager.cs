using System.Collections.Generic;
using UnityEngine;

//Apply in this script the save & Load system
public class CharacterCustomizationManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> allMarines = new List<GameObject>();

    //Tengo que usar probablemente el scriptable object
    public List<int> partsID = new List<int>();
    
    //Data to save
    /*public int HelmetID { get; private set; }
    public int HairID { get; private set; }
    public int FaceID { get; private set; }
    public int PatchID { get; private set; }*/

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
        //Al cambiar de marine, reiniciar los objects y aplicarlselos de 0
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
        
        marineSelected = allMarines[marineID];
        EventManager.TriggerEvent(GenericEvents.RestartCustoms);
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
