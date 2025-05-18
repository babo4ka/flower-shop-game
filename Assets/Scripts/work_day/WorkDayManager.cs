using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkDayManager : MonoBehaviour
{
    //менеджер работы с данными магазина
    [SerializeField] ShopManager shopManager;
    //менеджер для работы с данными цветов
    [SerializeField] FlowersManager flowersManager;
    //менеджер для работы с данынми работников
    [SerializeField] WorkersManager workersManager;
    //менеджер для работы с базой данных
    [SerializeField] DataBaseManager dataBaseManager;

    public static Action<float> startDay;

    public static Action startAnnotherDay;

    public static Action<StatisticsManager> statisticsShow;

    private ClientsCreator clientCreator;
    private Queue<Client> clientsQueue;

    //работники на смене
    private List<Workers> workersOnShift;

    //деньги, заработанные на смене
    private float moneyEarned = 0f;


    private const float workDayTime = 30f;

    private float dayStartTime = 0f;

    private bool dayStarted = false;

    private readonly Dictionary<Workers, ServeClient> workersCoroutines = new();

    private StatisticsManager statsManager;
    
    private Workers GetFreeWorker()
    {
        foreach (var worker in workersOnShift)
        {
            if (!workersCoroutines[worker].IsBuzy)
            {
                return worker;
            }
        }

        return null;
    }

    private void Awake()
    {
        clientCreator = new ClientsCreator(shopManager, flowersManager);
        GameManager.startAnotherDay += StartAnotherDay;
        WorkersManager.replaceWorkerOnShift += ReplaceWorkerOnShift;
    }

    private void Update()
    {
        if(dayStarted)
        {
            if(Time.time - dayStartTime >= workDayTime) {
                FinishWorkDay();
            }

            var freeWorker = GetFreeWorker();
            if(freeWorker != null && clientsQueue.Count != 0)
            {
                
                var client = clientsQueue.Dequeue();
                if (!flowersManager.HasFlower(client.FlowerWants))
                {
                    client.Satisfaction = 0;
                    if (freeWorker.motivation < 5) freeWorker.motivation = 0;
                    else freeWorker.motivation -= 5f;
                }
                else
                {
                    var wc = workersCoroutines[freeWorker];
                    wc.IsBuzy = true;
                    wc.Client = client;
                    wc.FlowerPrice = flowersManager.GetShopFlowerPriceByName(client.FlowerWants);
                    StartCoroutine(wc);
                }
            }
        }
    }

    private void SpendFlower(string flowerName)
    {
        flowersManager.SpendFlower(flowerName);
    }

    private void AddCash(float amount)
    {
        moneyEarned += amount;
    }

    private void CountStats(float satisfaction)
    {
        statsManager.PlusFlower();
        statsManager.AddClient(satisfaction);
    }

    private void StartAnotherDay()
    {
        startAnnotherDay?.Invoke();
    }

    public void StartWorkDay()
    {
        statsManager = new StatisticsManager(dataBaseManager);
        Debug.Log("Day started!");
        clientsQueue = new Queue<Client>();
        InvokeRepeating(nameof(GetClients), 0, 5);
        dayStartTime = Time.time;
        dayStarted = true;
        startDay?.Invoke(workDayTime);
    }

    private void FinishWorkDay()
    {
        CancelInvoke();
        statsManager.MoneyEarned = moneyEarned;
        
        statisticsShow?.Invoke(statsManager);
        statsManager.CountInfo();

        dayStartTime = -1f;
        dayStarted = false;

        clientsQueue.Clear();

        workersOnShift.ForEach(w => {
            w.isOnShift = false;
            workersManager.UpdateWorker(w);
        });
        dataBaseManager.PaySalary(workersOnShift);
        workersOnShift.Clear();
        workersCoroutines.Clear();
        shopManager.AddCash(moneyEarned);
        moneyEarned = 0f;
        shopManager.IncreaseDay();
        flowersManager.UpdateFlwoersPopularityStory();
        flowersManager.ClearFlowers();
        Debug.Log("Day finished!");
    }


    private void GetClients()
    {
        if (Time.time - dayStartTime <= 25)
        {
            var clients = clientCreator.GetClients();
            Debug.Log($"generated {clients.Count}");
            clients.ForEach(c =>
            {
                clientsQueue.Enqueue(c);
            });
        }
    }


    private (bool, string) ReplaceWorkerOnShift(Workers worker, string action)
    {
        switch (action)
        {
            case "to":
                if (workersOnShift != null)
                {
                    if (workersOnShift.Count >= shopManager.MaxWorkers()) return (false, "количество работников на смене максимально");
                }
                workersOnShift ??= new();
                workersOnShift.Add(worker);
                workersCoroutines.Add(worker, new ServeClient(flowersManager, worker, AddCash, SpendFlower, CountStats));
                return (true, "");

            case "from":
                workersOnShift?.Remove(worker);
                workersCoroutines.Remove(worker);
                return (true, "");

            default:
                return (false, "нет таких параметров");
        }
    }


    
    class ServeClient : IEnumerator
    {
        //длительность обслуживания клиента работником
        private float duration;
        //занят ли работник
        private bool isBuzy;
        //цена цветка
        private float flowerPrice;
        //клиент, которого обслуживают
        private Client client;
        //работник
        private readonly Workers worker;

        //базовое время обслуживания клиентов
        private const float baseServiceTime = .5f;

        private readonly Action<float> addCash;
        private readonly Action<string> spendFlower;
        private readonly Action<float> countStats;

        public bool IsBuzy
        {
            get => isBuzy;
            set { isBuzy = value; }
        }

        public Client Client
        {
            get => client;
            set { client = value; }
        }

        public float FlowerPrice
        {
            get => flowerPrice;
            set { flowerPrice = value; }
        }

        private const float maxSatisfaction = 150;

        public object Current => new WaitForSeconds(duration);

        public bool MoveNext()
        {
            isBuzy = false;
            var partOfSatisfaction = worker.motivation * client.GetPriceSatisfaction(flowerPrice);
            var percent = duration / baseServiceTime * 10;
            var satisfaction = partOfSatisfaction - (partOfSatisfaction * (percent / 100));
            client.Satisfaction = satisfaction;

            if(satisfaction >= 0)
            {
                spendFlower(client.FlowerWants);
                addCash(flowerPrice);

                var satisfactionPercent = satisfaction / maxSatisfaction;

                if (satisfactionPercent < 35)
                {
                    if (worker.motivation < 5) worker.motivation = 0;
                    else worker.motivation -= 5f;
                }
                else if (satisfactionPercent > 75)
                {
                    if (worker.motivation > 95) worker.motivation = 100;
                    else worker.motivation += 5f;
                }
            }
            else
            {
                if (worker.motivation < 5) worker.motivation = 0;
                else worker.motivation -= 5f;
            }
            
            duration = baseServiceTime * ((100 - worker.motivation) / 10);
            countStats(satisfaction);
            return false;
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public ServeClient(FlowersManager fm, Workers w, Action<float> addCash, Action<string> spendFlower, Action<float> countStats)
        {
            duration = baseServiceTime * ((100 - w.motivation) / 10);
            isBuzy = false;
            worker = w;
            this.addCash = addCash;
            this.spendFlower = spendFlower;
            this.countStats = countStats;   
        }
    }
}
