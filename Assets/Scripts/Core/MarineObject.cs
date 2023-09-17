using UnityEngine;
using UnityEngine.AI;

public class MarineObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Marine scirptableObject;
    [SerializeField] private NavMeshAgent agent;

    [Header("Stats")]
    [SerializeField] private float health;
    [SerializeField] private TypeMarine typeMarine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        typeMarine = scirptableObject.TypeMarine;
        health = scirptableObject.Health;
    }

    private void Update()
    {
        //agent.destination = 
    }
}
