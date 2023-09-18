using UnityEngine;

[CreateAssetMenu(fileName = "NewMarine", menuName = "ScriptableObjects/Marines")]

public class Marine : ScriptableObject
{
    [Header("Variables")]
    [SerializeField] private int id;
    [SerializeField] private string marineName;
    [SerializeField] private Sprite icon;
    [TextArea][SerializeField] private string description;
    [SerializeField] private TypeMarine typeMarine;
    [SerializeField] private Weapon weapon;

    [Header("Stats")]
    [SerializeField] private float health;
    [SerializeField] private float creationTime;
    [SerializeField] private float marineValue;


    public int Id => id;
    public string MarineName => marineName;
    public Sprite Icon => icon;
    public string Description => description;
    public TypeMarine TypeMarine => typeMarine;
    public float Health => health;
    public float CreationTime => creationTime;
    public Weapon Weapon => weapon;
    public float MarineValue => marineValue;
}

public enum TypeMarine
{
    Defuser,
    Sniper,
    Marine,
    NavySEAL,
    Driver,
    Spy,
    Civilian,
    Insurgent
}

public enum Weapon
{
    Unarmed,
    Pistol,
    Sniper,
    Rifle,
    MachineGun
}
