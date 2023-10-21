using UnityEngine;

namespace Interfaces
{
    public interface IUnit
    {
        void HandleInteraction(IInteractable interactable);
        void AssignVehicle(ITransport vehicle);
        object GetUnitName();
    }
}
