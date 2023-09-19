using System.Collections.Generic;
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

    public HashSet<MarineObject> SelectedMarines = new HashSet<MarineObject>();
    public List<MarineObject> AvailableMarines = new List<MarineObject>();

    private SelectionManager() { }

    public void Select(MarineObject marine)
    {
        marine.OnSelected();
        SelectedMarines.Add(marine);
    }

    public void Deselect(MarineObject marine)
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

    public bool IsSelected(MarineObject marine) 
    {
        return SelectedMarines.Contains(marine);
    }
}
