using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Variables")]
    [SerializeField] private int id;
    [SerializeField] private string weaponName;
    [SerializeField] private Sprite icon;
    [TextArea][SerializeField] private string description;
    [SerializeField] private WeaponType type;

    [Header("Stats")]
    [SerializeField] private float damage; //Lo controla el arma
    [SerializeField] private int bulletsOnMagazine; //Lo deberia controlar el magazine
    [SerializeField] private float fireRate; //Lo controla el arma
    [SerializeField] private float accuracy; //Lo controla la mira
    [SerializeField] private float reloadTime; //Lo controla el magazine
    [SerializeField] private float maximumRangeAccuracy; //Lo controla el arma

    public int Id => id;
    public string WeaponName => weaponName;
    public Sprite Icon => icon;
    public string Description => description;
    public WeaponType WeaponType => type;
    public float Damage => damage;
    public float FireRate => fireRate;
    public float Accuracy => accuracy;
    public float ReloadTime => reloadTime;
    public int BulletsOnMagazine => bulletsOnMagazine;
    public float MaximumRangeAccuracy => maximumRangeAccuracy;
}

public enum WeaponType
{
    Unarmed,
    Pistol,
    Sniper,
    Rifle,
    MachineGun,
    Shotgun,
    Knife
}
