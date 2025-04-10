using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkDayManager : MonoBehaviour
{
    //менеджер работы с данными магазина
    [SerializeField] ShopManager shopManager;
    //менеджер дл€ работы с данными цветов
    [SerializeField] FlowersManager flowersManager;
    //менеджер дл€ работы с данынми работников
    [SerializeField] WorkersManager workersManager;

    private ClientsCreator clientCreator;
    private Queue<Client> clientsQueue;

    private List<Workers> workersOnShift;


    private const float workDayTime = 60f;

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
                CancelInvoke();
                dayStartTime = -1f;
                dayStarted = false;

                clientsQueue.Clear();
                workersOnShift.Clear();
                workersCoroutines.Clear();
            }

            var freeWorker = GetFreeWorker();
            if(freeWorker != null)
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

    private void StartAnotherDay()
    {
        
    }

    private void StartWorkDay()
    {
        clientsQueue = new Queue<Client>();
        InvokeRepeating("GetClients", 0, 5);
        dayStartTime = Time.time;
        dayStarted = true;
    }


    private void GetCleints()
    {
        if(Time.time - dayStartTime >= 10)
        {
            var clients = clientCreator.GetClients();
            clients.ForEach(c =>
            {
                clientsQueue.Enqueue(c);
            });
        }
    }


    protected void ReplaceWorkerOnShift(Workers worker, string action)
    {
        switch(action)
        {
            case "to":
                workersOnShift ??= new();
                workersOnShift.Add(worker);
                workersCoroutines.Add(worker, new ServeClient(worker.motivation, shopManager, flowersManager, workersManager, worker));
                Debug.Log($"added worker {worker.name}");
                break;

            case "from":
                workersOnShift?.Remove(worker);
                workersCoroutines.Remove(worker);
                break;
        }
    }


    
    class ServeClient : IEnumerator
    {
        //длительность обслуживани€ клиента работником
        private float duration;
        //мотиваци€ работника
        private float workerMotivation;
        //зан€т ли работник
        private bool isBuzy;
        //цена цветка
        private float flowerPrice;
        //клиент, которого обслуживают
        private Client client;
        //работник
        private Workers worker;

        //базовое врем€ обслуживани€ клиентов
        private const float baseServiceTime = .5f;

        private ShopManager shopManager;
        private FlowersManager flowersManager;
        private WorkersManager workersManager;

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

            shopManager.AddCash(flowerPrice);
            flowersManager.SpendFlower(client.FlowerWants);

            var satisfactionPercent = (satisfaction - minSatisfaction) / (maxSatisfaction - minSatisfaction);

            if(satisfactionPercent < 25)
            {
                workersManager.DecreaseWorkerMotivation(worker, 10f);
            }else if(satisfactionPercent > 75)
            {
                workersManager.IncreaseWorkerMotivation(worker, 10f);
            }
            return false;
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public ServeClient(float workerMotivation, ShopManager sm, FlowersManager fm, WorkersManager wm, Workers w)
        {
            duration = baseServiceTime * ((100 - workerMotivation) / 10);
            isBuzy = false;
            this.workerMotivation = workerMotivation;

            shopManager = sm;
            flowersManager = fm;
            workersManager = wm;
            worker = w;
        }
    }
}
