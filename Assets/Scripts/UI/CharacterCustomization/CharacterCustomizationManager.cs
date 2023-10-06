using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizationManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> allMarines = new List<GameObject>();
    [SerializeField] private GameObject marineSelected;
    
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
            if (marineID == 11)
                return;
            
            allMarines.ForEach(x=>x.SetActive(false));

            marineID++;
            allMarines[marineID].SetActive(true);
        }

        else
        {
            if (marineID == 0)
                return;
            
            allMarines.ForEach(x=>x.SetActive(false));

            marineID--;
            allMarines[marineID].SetActive(true);
        }
        
        marineSelected = allMarines[marineID];
    }

    public void InstantiateObjectOnSelectedMarine(GameObject obj)
    {
        //Destruir el objeto anterior si hubiese
        Debug.Log($"Voy a instanciar el objeto {obj.name} en el marine {marineSelected.name}");
    }
}
