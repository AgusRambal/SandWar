using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Refernces")]
    [SerializeField] private UIMethods UIMethods;

    [Header("Settings")]
    public List<Marine> allMarines = new List<Marine>();
    [SerializeField] private List<Marine> myMarines = new List<Marine>();
    public float timerReset = 10f;

    [field: SerializeField] public float goodOilSellValue { get; private set; }
    [field: SerializeField] public float badOilSellValue { get; private set; }
    [field: SerializeField] public float badDollarToReceive { get; private set; }
    [field: SerializeField] public float goodDollarToReceive { get; private set; }

    public float oilAmount { get; private set; }
    public float dollarsAmount { get; private set; }
    public bool badMarketStatus { get; private set; }

    [Range(0, 100)] public float marketChance = 50f;
    [SerializeField] private float timeToReset;
    [SerializeField] private float maxOilAmountToAdd;

    private float marketChanceDecrease = 50f;
    private float timer = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
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

    public void RecruitMarine(int ID)
    {
        if (myMarines.Count >= 10)
        {
            UIMethods.ShowNotification("Max Army capacity", "Your army has reached it's maximum capacity at the moment");
            return;
        }

        for (int i = 0; i < allMarines.Count; i++)
        {
            if (allMarines[i].Id == ID)
            {
                myMarines.Add(allMarines[i]);
            }
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
