using System.Collections;
using System.Collections.Generic;
using Core.Characters;
using UnityEngine;
using SandWar.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Refernces")]
    [SerializeField] private UIMethods UIMethods;

    [Header("Settings")]
    [Range(0, 100)] public float marketChance = 50f;
    public float timerReset = 10f;
    [SerializeField] private float timeToReset;
    [SerializeField] private float maxOilAmountToAdd;
    [field: SerializeField] public float goodOilSellValue { get; private set; }
    [field: SerializeField] public float badOilSellValue { get; private set; }
    [field: SerializeField] public float badDollarToReceive { get; private set; }
    [field: SerializeField] public float goodDollarToReceive { get; private set; }
    public bool test = false;

    [Header("Marines")]
    public List<Character> allMarines = new List<Character>();
    [SerializeField] private List<Transform> marinesSpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> marinesTarget = new List<Transform>();

    [Header("My data")]
    [SerializeField] private List<Character> myMarines = new List<Character>();
    public float oilAmount { get; private set; }
    public float dollarsAmount { get; private set; }

    //Flags
    public bool badMarketStatus { get; private set; }
    public float actualMarketPrice { get; private set; }

    private float marketChanceDecrease = 50f;
    private float timer = 0;
    private Marine lastMarineCreated;

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
        actualMarketPrice = goodOilSellValue;
        StartCoroutine(SetOilValue());

        if (test)
        {
            dollarsAmount = SerializationManager.TryLoadData("Dollar", out float _dollar) ? _dollar : 8000;
            oilAmount = SerializationManager.TryLoadData("Oil", out float _oil) ? _oil : 8000;
        }
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
            UIMethods.ShowNotification("Max Army capacity", "Your army has reached it's maximum capacity");
            return;
        }

        for (int i = 0; i < allMarines.Count; i++)
        {
            if (allMarines[i].Id == ID)
            {
                myMarines.Add(allMarines[i]);
                CreateMarine(allMarines[i].GetRandomMarine());
                UIMethods.CreateSelectableIcon(ID, lastMarineCreated);
            }
        }
    }

    private void CreateMarine(GameObject marine)
    {
        GameObject marineInstantiated = Instantiate(marine, marinesSpawnPoints[Random.Range(0, marinesSpawnPoints.Count)].position, Quaternion.identity);
        marineInstantiated.GetComponent<Marine>().MoveTo(marinesTarget[myMarines.Count - 1].position);
        lastMarineCreated = marineInstantiated.GetComponent<Marine>();
    }

    private IEnumerator SetOilValue()
    {
        yield return new WaitForSeconds(timerReset);

        float randomValue = Random.Range(0, 100);
        float randomValue2 = Random.Range(0, 100);

        if (randomValue > marketChance)
        {
            badMarketStatus = true;
            actualMarketPrice = badOilSellValue;
        }

        else
        {
            badMarketStatus = false; 
            actualMarketPrice = goodOilSellValue;
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

    private void OnApplicationQuit()
    {
        SerializationManager.SaveData("Dollar", dollarsAmount);
        SerializationManager.SaveData("oil", oilAmount);
    }
}
