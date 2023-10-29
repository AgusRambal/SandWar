using UnityEngine;

namespace VehicleSystem.Transports
{
    [CreateAssetMenu(fileName = "New Transport", menuName = "Transport")]
    public class Transports : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string vehicleName;
        [SerializeField] private Sprite icon;
        [SerializeField] private TypeVehicle typeVehicle;
        [SerializeField] private int maxCapacityOfUnit;
       
        public int Id => id;
        public string VehicleNameName => vehicleName;
        public Sprite Icon => icon;
        public TypeVehicle TypeVehicle => typeVehicle;
        public int MaxCapacityOfUnit
        {
            get => maxCapacityOfUnit;
            set => maxCapacityOfUnit = value;
        }
    }
    
    public enum TypeVehicle
    {
        Ground,
        Air
    }
}
