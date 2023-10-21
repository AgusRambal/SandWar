using System.Collections.Generic;
using Core.Characters;
using UnityEngine;

public class SelectionManager
{
    private static SelectionManager instance;

    public static SelectionManager Instance
    {
        get 
        {
            if (instance == null)
            { 
                instance = new SelectionManager();
            }

            return instance;
        }

        private set 
        { 
            instance = value; 
        }
    }

    public HashSet<Marine> SelectedMarines = new HashSet<Marine>();
    public List<Marine> AvailableMarines = new List<Marine>();

    private SelectionManager() { }

    public void Select(Marine marine)
    {
        marine.OnSelected();
        SelectedMarines.Add(marine);
    }

    public void Deselect(Marine marine)
    {
        marine.OnDeselected();
        SelectedMarines.Remove(marine);
    }

    public void DeselectAll()
    {
        foreach (var marine in SelectedMarines) 
        {
            marine.OnDeselected();
        }

        SelectedMarines.Clear();
    }

    public bool IsSelected(Marine marine) 
    {
        return SelectedMarines.Contains(marine);
    }
}
