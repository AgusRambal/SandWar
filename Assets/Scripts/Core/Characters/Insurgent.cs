using UnityEngine;

public class Insurgent : MonoBehaviour
{
    [SerializeField] private Character insrugent;
    public float health;

    private void Awake()
    {
        health = insrugent.Health;
    }
}
