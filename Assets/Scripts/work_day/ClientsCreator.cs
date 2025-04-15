using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClientsCreator
{
    //менеджер работы с данными магазина
    readonly ShopManager shopManager;
    //менеджер для работы с данными цветов
    readonly FlowersManager flowersManager;

    private List<FlowersPrice> flowersPrices;

    private Dictionary<FlowersPrice, (float, float)> flowersChanceRanges;

    const float up = 3f;


    private string[] flowerNames = {"Ландыш", "Пион" };

    public ClientsCreator(ShopManager shopManager, FlowersManager flowersManager)
    {
        this.shopManager = shopManager;
        this.flowersManager = flowersManager;   
    }

    private void UpdateInfo()
    {
        flowersPrices = flowersManager.GetFlowersPrice();
        flowersChanceRanges ??= new Dictionary<FlowersPrice, (float, float)>();

        flowersChanceRanges.Clear();

        float flowersPopularityLevelSum = flowersPrices.Sum(fp => fp.popularity_level);

        float currentMin = 0;

        flowersPrices.ForEach(fp =>
        {
            float percent = fp.popularity_level / flowersPopularityLevelSum;

            flowersChanceRanges.Add(fp, (currentMin, currentMin + percent));
            currentMin += percent;
        });

    }

    private (string, float) GetFlower()
    {
        float dot = Random.Range(0f, 1f);

        foreach (var key in flowersChanceRanges.Keys)
        {
            var fp = flowersChanceRanges[key];
            if(dot >= fp.Item1 && dot <= fp.Item2)
            {
                return (key.flower_name, key.GetFlowerPrice());
            }
        }

        return ("", -1);
    }

    public List<Client> GetClients()
    {
        var clients = new List<Client>();    
        int clientsCount = Random.Range(1, 4);

        for (int i = 0; i < clientsCount; i++)
        {
            var (flowerWants, flowerPrice) = GetFlower();
           /* string flowerWants = flowerNames[Random.Range(0, flowerNames.Length)];
            float flowerPrice = flowersManager.GetFlowerPriceByName(flowerWants);*/
            clients.Add(new Client(flowerWants, flowerPrice + (flowerPrice * (up * shopManager.CurrentRating()/ 100)), flowerPrice));
        }

        return clients;
    }

}
