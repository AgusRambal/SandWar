using UnityEngine;

namespace Interfaces
{
    public interface ITransport 
    {
        IUnit CurrentDriver { get; set; }
        void CanStartDrive();
        void Deselect();
    }
}
