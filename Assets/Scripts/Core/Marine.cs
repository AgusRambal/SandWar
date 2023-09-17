using UnityEngine;

[CreateAssetMenu(fileName = "NewMarine", menuName = "ScriptableObjects/Marines")]

public class Marine : ScriptableObject
{
    [Header("Variables")]
    [SerializeField] private int id;
    [SerializeField] private new string name;
    [SerializeField] private Sprite icon;
    [TextArea][SerializeField] private string description;
    [SerializeField] private TypeMarine typeMarine;

    [Header("Stats")]
    [SerializeField] private float health;

    //Faltan todas las specs tecnicas de vida y demas

    public int Id => id;
    public string Name => name;
    public Sprite Icon => icon;
    public string Descriptions => description;
    public TypeMarine TypeMarine => typeMarine;
    public float Health => health;
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
