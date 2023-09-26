using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "ScriptableObjects/Character")]

public class Character : ScriptableObject
{
    [Header("Variables")]
    [SerializeField] private int id;
    [SerializeField] private string marineName;
    [SerializeField] private Sprite icon;
    [TextArea][SerializeField] private string description;
    [SerializeField] private Type type;
    [SerializeField] private SubType subType;
    [SerializeField] private Weapon weapon;
    [SerializeField] private List<GameObject> marinePrefabs = new List<GameObject>();

    [Header("Stats")]
    [SerializeField] private float health;
    [SerializeField] private float creationTime;
    [SerializeField] private float marineValue;
    [SerializeField] private float accuracy;
    [SerializeField] private int magazines;

    public int Id => id;
    public string MarineName => marineName;
    public Sprite Icon => icon;
    public string Description => description;
    public Type Type => type;
    public SubType SubType => subType;
    public float Health => health;
    public float CreationTime => creationTime;
    public Weapon Weapon => weapon;
    public float MarineValue => marineValue;
    public float Accuracy => accuracy;
    public int Magazines => magazines;

    public GameObject GetRandomMarine()
    {
        return marinePrefabs[Random.Range(0, marinePrefabs.Count)];
    }
}

public enum Type
{
    Marine,
    Civilian,
    Insurgent
}

public enum SubType
{
    Defuser,
    Sniper,
    Regular,
    NavySEAL,
    Driver,
    Spy,
}
