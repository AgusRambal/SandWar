using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get 
        {
            if (instance == null)
                Debug.Log("null");

            return instance;
        }
    }

    [Header("Settings")]
    public List<Marine> allMarines = new List<Marine>();
    public float timerReset = 10f;

    public float oilAmmount { get; private set; }
    public float dollarsAmmount { get; private set; }
    public bool badMarketStatus { get; private set; }
    [field: SerializeField] public float goodOilSellValue { get; private set; }
    [field: SerializeField] public float badOilSellValue { get; private set; }
    [field: SerializeField] public float badDollarToReceive { get; private set; }
    [field: SerializeField] public float goodDollarToReceive { get; private set; }

    [Range(0, 100)][SerializeField] private float marketChance = 50f;
    [SerializeField] private float timeToReset;
    [SerializeField] private float oilAmmountToAdd;

    private float timer = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        badMarketStatus = false;
        StartCoroutine(SetOilValue());
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

    public IEnumerator SetOilValue()
    {
        //Cada vez que sucede el cambio de estado, tirar otra chance de que suba un 10%
        //Pero cada vez que compro, la chance baja un 10%
        yield return new WaitForSeconds(timerReset);

        float randomValue = Random.Range(0, 100);

        if (randomValue > marketChance)
        {
            badMarketStatus = true;
        }

        else
        {
            badMarketStatus = false;
        }

        StartCoroutine(SetOilValue());
    }

    public void SellOil()
    {
        if (!badMarketStatus)
        {
            oilAmmount -= goodOilSellValue;
            dollarsAmmount += goodDollarToReceive;
        }

        else
        {
            oilAmmount -= badOilSellValue;
            dollarsAmmount += badDollarToReceive;
        }
    }

    public void SubstractDollars(int id)
    {
        dollarsAmmount -= allMarines[id - 1].MarineValue;
    }
}
