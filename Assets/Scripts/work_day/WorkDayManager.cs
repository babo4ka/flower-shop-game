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

    public static Action startAnnotherDay;

    private ClientsCreator clientCreator;
    private Queue<Client> clientsQueue;

    //работники на смене
    private List<Workers> workersOnShift;

    //деньги, заработанные на смене
    private float moneyEarned = 0f;


    private const float workDayTime = 10f;

    private float dayStartTime = 0f;

    private bool dayStarted = false;

    private Dictionary<Workers, ServeClient> workersCoroutines = new();
    
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
                var wc = workersCoroutines[freeWorker];
                wc.IsBuzy = true;
                var client = clientsQueue.Dequeue();
                wc.Client = client;
                wc.FlowerPrice = flowersManager.GetShopFlowerPriceByName(client.FlowerWants);
                StartCoroutine(wc);
            }
        }
    }

    private void AddCash(float amount)
    {
        moneyEarned += amount;
    }

    private void StartAnotherDay()
    {
        startAnnotherDay?.Invoke();
    }

    public void StartWorkDay()
    {
        Debug.Log("Day started!");
        clientsQueue = new Queue<Client>();
        InvokeRepeating("GetClients", 0, 5);
        dayStartTime = Time.time;
        dayStarted = true;
    }

    private void FinishWorkDay()
    {
        CancelInvoke();
        dayStartTime = -1f;
        dayStarted = false;

        clientsQueue.Clear();

        workersOnShift.ForEach(w => {
            w.isOnShift = false;
            workersManager.UpdateWorker(w);
        });
        workersOnShift.Clear();
        workersCoroutines.Clear();
        shopManager.AddCash(moneyEarned);
        moneyEarned = 0f;
        shopManager.IncreaseDay();
        flowersManager.UpdateFlwoersPopularityStory();
        Debug.Log("Day finished!");
    }


    private void GetClients()
    {
        //if (Time.time - dayStartTime >= 10)
        //{
            var clients = clientCreator.GetClients();
            Debug.Log($"generated {clients.Count} clients");
            clients.ForEach(c =>
            {
                clientsQueue.Enqueue(c);
            });
        //}
    }


    private (bool, string) ReplaceWorkerOnShift(Workers worker, string action)
    {
        switch (action)
        {
            case "to":
                if (workersOnShift != null)
                {
                    if (workersOnShift.Count >= 2) return (false, "количество работников на смене максимально");
                }
                workersOnShift ??= new();
                workersOnShift.Add(worker);
                workersCoroutines.Add(worker, new ServeClient(worker.motivation, shopManager, flowersManager, workersManager, worker, AddCash));
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
        //мотивация работника
        private float workerMotivation;
        //занят ли работник
        private bool isBuzy;
        //цена цветка
        private float flowerPrice;
        //клиент, которого обслуживают
        private Client client;
        //работник
        private Workers worker;

        //базовое время обслуживания клиентов
        private const float baseServiceTime = .5f;

        private ShopManager shopManager;
        private FlowersManager flowersManager;
        private WorkersManager workersManager;

        private Action<float> addCash;

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
        private const float minSatisfaction = 12.5f;

        public object Current => new WaitForSeconds(duration);

        public bool MoveNext()
        {
            isBuzy = false;
            var partOfSatisfaction = workerMotivation * client.GetPriceSatisfaction(flowerPrice);
            var percent = duration / baseServiceTime * 10;
            var satisfaction = partOfSatisfaction - (partOfSatisfaction * (percent / 100));
            client.Satisfaction = satisfaction;

            flowersManager.SpendFlower(client.FlowerWants);
            addCash(flowerPrice);

            var satisfactionPercent = (satisfaction - minSatisfaction) / (maxSatisfaction - minSatisfaction);

            if(satisfactionPercent < 25)
            {
                worker.motivation -= 5f;
            }else if(satisfactionPercent > 75)
            {
                worker.motivation += 5f;
            }

            Debug.Log(satisfaction);
            return false;
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public ServeClient(float workerMotivation, ShopManager sm, FlowersManager fm, WorkersManager wm, Workers w, Action<float> addCash)
        {
            duration = baseServiceTime * ((100 - workerMotivation) / 10);
            isBuzy = false;
            this.workerMotivation = workerMotivation;

            shopManager = sm;
            flowersManager = fm;
            workersManager = wm;
            worker = w;
            this.addCash = addCash;
        }
    }
}
