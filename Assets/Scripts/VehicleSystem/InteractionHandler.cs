using System;
using Interfaces;
using UnityEngine;

namespace Selector
{
    public class InteractionHandler : MonoBehaviour
    {
        private Selector<IUnit> unitsSelector = new Selector<IUnit>();
        private Selector<IInteractable> interactableSelector = new Selector<IInteractable>();

        public IUnit CurrentlySelectedUnit { get; private set; }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (unitsSelector.TrySelect(ray))
                {
                    CurrentlySelectedUnit = unitsSelector.SelectedObject;
                    //Debug.Log("Unidad seleccionada: " + CurrentlySelectedUnit.GetUnitName());
                }
            }
            
            if (Input.GetMouseButtonDown(1) && CurrentlySelectedUnit != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (interactableSelector.TrySelect(ray))
                {
                    var interactable = interactableSelector.SelectedObject;
                    CurrentlySelectedUnit.HandleInteraction(interactable);
                    
                    ITransport vehicle = interactable as ITransport;
                    if (vehicle != null)
                    {
                        CurrentlySelectedUnit.AssignVehicle(vehicle);
                    }
                }
            }
        }
        
    }
}
