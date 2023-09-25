using UnityEngine;

public class Insurgent : MonoBehaviour
{
    [SerializeField] private Marine insrugent;
    public float health;

    private void Awake()
    {
        health = insrugent.Health;
    }
}
