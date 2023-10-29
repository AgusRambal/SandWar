using Interfaces;
using JetBrains.Annotations;
using Selector;
using UnityEngine;

namespace VehicleSystem
{
    public class InteractionHandler : MonoBehaviour
    {
        private readonly Selector<IUnit> unitsSelector = new Selector<IUnit>();
        private readonly Selector<ITransport> transportSelector = new Selector<ITransport>();
        private readonly Selector<IInteractable> interactableSelector = new Selector<IInteractable>();

        private IUnit CurrentlySelectedUnit { get; set; }
        private ITransport CurrentlySelectedTransport { get; set; }

        private bool canUseCar;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (unitsSelector.TrySelect(ray))
                {
                    CurrentlySelectedUnit = unitsSelector.SelectedObject;
                }

                if (transportSelector.TrySelect(ray))
                {
                   
                    if (CurrentlySelectedTransport != null)
                    {
                        DeselectTransport(CurrentlySelectedTransport);
                    }

                    if (canUseCar)
                    {
                        CurrentlySelectedTransport = transportSelector.SelectedObject;
                        CurrentlySelectedTransport.CanStartDrive();
                    }
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
                    if (vehicle != null && CurrentlySelectedTransport != vehicle)
                    {
                        if (CurrentlySelectedTransport != null)
                        {
                            DeselectTransport(CurrentlySelectedTransport);
                        }

                        CurrentlySelectedUnit.AssignVehicle(vehicle);
                        CurrentlySelectedTransport = vehicle;
                        canUseCar = true;
                    }
                }
            }
        }

        private void DeselectTransport(ITransport transport)
        {
            transport.Deselect();
        }
    }
}
