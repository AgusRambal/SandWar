using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get 
        {
            if (instance is null)
                Debug.Log("null");

            return instance;
        }
    }

    public float oilAmmount { get; private set; }
    [SerializeField] private float timeToReset;
    [SerializeField] private float oilAmmountToAdd;

    private float timer = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToReset)
        {
            oilAmmount += oilAmmountToAdd;
            timer = 0;
        }
    }
}
