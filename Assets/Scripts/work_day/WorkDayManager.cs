using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkDayManager : MonoBehaviour
{
    //менеджер работы с данными магазина
    [SerializeField] ShopManager shopManager;
    //менеджер для работы с данными цветов
    [SerializeField] FlowersManager flowersManager;

    private ClientsCreator clientCreator;
    private Queue<Client> clientsQueue;

    private List<Workers> workersOnShift;


    private const float workDayTime = 60f;

    private float dayStartTime = 0f;

    private bool dayStarted = false;

    private Dictionary<Workers, ServeClient> workersCoroutines = new Dictionary<Workers, ServeClient>();
    
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
        clientsQueue = new Queue<Client>();
        InvokeRepeating("GetClients", 0, 15);
        dayStartTime = Time.time;
        dayStarted = true;
    }

    


    private void GetCleints()
    {
        var clients = clientCreator.GetClients();
        clients.ForEach(c =>
        {
            clientsQueue.Enqueue(c);
        });
    }


    protected void ReplaceWorkerOnShift(Workers worker, string action)
    {
        switch(action)
        {
            case "to":
                workersOnShift ??= new();
                workersOnShift.Add(worker);
                workersCoroutines.Add(worker, new ServeClient(worker.motivation));
                break;

            case "from":
                workersOnShift?.Remove(worker);
                workersCoroutines.Remove(worker);
                break;
        }
    }


    
    class ServeClient : IEnumerator
    {
        private float duration;
        private float workerMotivation;
        private bool isBuzy;
        private float flowerPrice;
        private Client client;

        private const float baseServiceTime = 5f;

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

        public object Current => new WaitForSeconds(duration);

        public bool MoveNext()
        {
            isBuzy = false;
            var partOfSatisfaction = workerMotivation * client.GetPriceSatisfaction(flowerPrice);
            var percent = duration / baseServiceTime * 10;
            client.Satisfaction = partOfSatisfaction - (partOfSatisfaction * (percent/100));
            return false;
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public ServeClient(float workerMotivation)
        {
            this.duration = baseServiceTime * ((100 - workerMotivation) / 10);
            isBuzy = false;
            this.workerMotivation = workerMotivation;
        }
    }
}
