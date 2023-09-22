using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<Marine> allMarines = new List<Marine>();
    [SerializeField] private GameObject bombsuitMarine;
    [SerializeField] private GameObject ghillieMarine;
    [SerializeField] private GameObject spyMarine;
    [SerializeField] private List<GameObject> pilotMarines = new List<GameObject>();
    [SerializeField] private List<GameObject> navySEALS = new List<GameObject>();
    [SerializeField] private List<GameObject> regularMarines = new List<GameObject>();
    [SerializeField] private List<Transform> marinesSpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> marinesTarget = new List<Transform>();

    [Header("My data")]
    [SerializeField] private List<Marine> myMarines = new List<Marine>();
    public float oilAmount { get; private set; }
    public float dollarsAmount { get; private set; }

    //Flags
    public bool badMarketStatus { get; private set; }
    public float actualMarketPrice { get; private set; }

    private float marketChanceDecrease = 50f;
    private float timer = 0;
    private MarineObject lastMarineCreated;

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
            oilAmount = 500;
            dollarsAmount = 8000;
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
            UIMethods.ShowNotification("Max Army capacity", "Your army has reached it's maximum capacity at the moment");
            return;
        }

        for (int i = 0; i < allMarines.Count; i++)
        {
            if (allMarines[i].Id == ID)
            {
                myMarines.Add(allMarines[i]);
                CreateMarine(allMarines[i].TypeMarine);
                UIMethods.CreateSelectableIcon(ID, lastMarineCreated);
            }
        }
    }

    private void CreateMarine(TypeMarine type)
    {
        switch (type)
        {
            case TypeMarine.Defuser:
                GameObject bombsuitInstantiated = Instantiate(bombsuitMarine, marinesSpawnPoints[Random.Range(0, marinesSpawnPoints.Count)].position, Quaternion.identity);
                bombsuitInstantiated.GetComponent<MarineObject>().MoveTo(marinesTarget[myMarines.Count - 1].position);
                lastMarineCreated = bombsuitInstantiated.GetComponent<MarineObject>();
                break;

            case TypeMarine.Marine:
                GameObject marineInstantiated = Instantiate(regularMarines[Random.Range(0, regularMarines.Count)], marinesSpawnPoints[Random.Range(0, marinesSpawnPoints.Count)].position, Quaternion.identity);
                marineInstantiated.GetComponent<MarineObject>().MoveTo(marinesTarget[myMarines.Count - 1].position);
                lastMarineCreated = marineInstantiated.GetComponent<MarineObject>();
                break;

            case TypeMarine.Driver:
                GameObject pilotInstantiated = Instantiate(pilotMarines[Random.Range(0, pilotMarines.Count)], marinesSpawnPoints[Random.Range(0, marinesSpawnPoints.Count)].position, Quaternion.identity);
                pilotInstantiated.GetComponent<MarineObject>().MoveTo(marinesTarget[myMarines.Count - 1].position);
                lastMarineCreated = pilotInstantiated.GetComponent<MarineObject>();
                break;

            case TypeMarine.NavySEAL:
                GameObject sealInstantiated = Instantiate(navySEALS[Random.Range(0, navySEALS.Count)], marinesSpawnPoints[Random.Range(0, marinesSpawnPoints.Count)].position, Quaternion.identity);
                sealInstantiated.GetComponent<MarineObject>().MoveTo(marinesTarget[myMarines.Count - 1].position);
                lastMarineCreated = sealInstantiated.GetComponent<MarineObject>();
                break;

            case TypeMarine.Sniper:
                GameObject sniperInstantiated = Instantiate(ghillieMarine, marinesSpawnPoints[Random.Range(0, marinesSpawnPoints.Count)].position, Quaternion.identity);
                sniperInstantiated.GetComponent<MarineObject>().MoveTo(marinesTarget[myMarines.Count - 1].position);
                lastMarineCreated = sniperInstantiated.GetComponent<MarineObject>();
                break;

            case TypeMarine.Spy:
                GameObject spyInstantiated = Instantiate(spyMarine, marinesSpawnPoints[Random.Range(0, marinesSpawnPoints.Count)].position, Quaternion.identity);
                spyInstantiated.GetComponent<MarineObject>().MoveTo(marinesTarget[myMarines.Count - 1].position);
                lastMarineCreated = spyInstantiated.GetComponent<MarineObject>();
                break;
        }
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
}
