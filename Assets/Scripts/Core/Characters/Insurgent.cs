using UnityEngine;

public class Insurgent : MonoBehaviour
{
    //Datos dummy para testear el sistema de disparo
    [SerializeField] private Character insrugent;
    public float health;

    private void Awake()
    {
        health = insrugent.Health;
    }
}
