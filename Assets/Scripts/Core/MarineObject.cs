using UnityEngine;
using UnityEngine.AI;

public class MarineObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Marine scirptableObject;
    [SerializeField] private NavMeshAgent agent;

    [Header("Stats")]
    [SerializeField] private int id;
    [SerializeField] private float health;
    [SerializeField] private TypeMarine typeMarine;
    [SerializeField] private Weapon weapon;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        typeMarine = scirptableObject.TypeMarine;
        health = scirptableObject.Health;
        id = scirptableObject.Id;
        weapon = scirptableObject.Weapon;
    }

    private void Update()
    {
        //agent.destination = 
    }
}
