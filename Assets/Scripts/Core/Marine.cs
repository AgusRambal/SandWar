using UnityEngine;

[CreateAssetMenu(fileName = "NewMarine", menuName = "ScriptableObjects/Marines")]

public class Marine : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private new string name;
    [SerializeField] private Sprite icon;
    [TextArea][SerializeField] private string description;
    [SerializeField] private TypeMarine typeMarine;
    [SerializeField] private GameObject prefab;

    //Faltan todas las specs tecnicas de vida y demas

    public int Id => id;
    public string Name => name;
    public Sprite Icon => icon;
    public string Descriptions => description;
    public TypeMarine TypeTech => typeMarine;
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
