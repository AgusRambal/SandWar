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

    [Header("Refernces")]
    [SerializeField] private UIMethods UIMethods;

    [Header("Settings")]
    public List<Marine> allMarines = new List<Marine>();
    public float timerReset = 10f;

    public float oilAmount { get; private set; }
    public float dollarsAmount { get; private set; }
    public bool badMarketStatus { get; private set; }
    [field: SerializeField] public float goodOilSellValue { get; private set; }
    [field: SerializeField] public float badOilSellValue { get; private set; }
    [field: SerializeField] public float badDollarToReceive { get; private set; }
    [field: SerializeField] public float goodDollarToReceive { get; private set; }

    [Range(0, 100)] public float marketChance = 50f;
    [SerializeField] private float timeToReset;
    [SerializeField] private float maxOilAmountToAdd;

    private float marketChanceDecrease = 50f;
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
            var adition = Random.Range(0.2f, maxOilAmountToAdd);
            oilAmount += adition;
            UIMethods.FeedbackArrowGreen(0);
            timer = 0;
        }
    }

    public IEnumerator SetOilValue()
    {
        yield return new WaitForSeconds(timerReset);

        float randomValue = Random.Range(0, 100);
        float randomValue2 = Random.Range(0, 100);

        if (randomValue > marketChance)
        {
            badMarketStatus = true;
        }

        else
        {
            badMarketStatus = false;
        }

        if (randomValue2 > marketChanceDecrease)
        {
            marketChance += 10f;
        }

        StartCoroutine(SetOilValue());
    }

    public void SellOil()
    {
        if (!badMarketStatus)
        {
            oilAmount -= goodOilSellValue;
            dollarsAmount += goodDollarToReceive;
        }

        else
        {
            oilAmount -= badOilSellValue;
            dollarsAmount += badDollarToReceive;
        }
    }

    public void SubstractDollars(int id)
    {
        dollarsAmount -= allMarines[id - 1].MarineValue;
        marketChance -= 10f;
    }
}
