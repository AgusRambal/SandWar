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
    [SerializeField] private float damage;
    [SerializeField] private int bulletsOnMagazine;
    [SerializeField] private float fireRate;
    [SerializeField] private float accuracy;
    [SerializeField] private float reloadTime;

    //AGREGAR UN MODIFICADOR EN BASE A LA DISTANCIA

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
}

public enum WeaponType
{
    Unarmed,
    Pistol,
    Sniper,
    Rifle,
    MachineGun
}
