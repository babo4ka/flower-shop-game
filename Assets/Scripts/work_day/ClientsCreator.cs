using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ClientsCreator
{
    //менеджер работы с данными магазина
    readonly ShopManager shopManager;
    //менеджер для работы с данными цветов
    readonly FlowersManager flowersManager;

    const float up = 3f;


    private string[] flowerNames = {"Ландыш", "Пион" };

    public ClientsCreator(ShopManager shopManager, FlowersManager flowersManager)
    {
        this.shopManager = shopManager;
        this.flowersManager = flowersManager;   
    }


    public List<Client> GetClients()
    {
        var clients = new List<Client>();    
        int clientsCount = Random.Range(1, 4);

        for (int i = 0; i < clientsCount; i++)
        {
            string flowerWants = flowerNames[Random.Range(0, flowerNames.Length)];
            float flowerPrice = flowersManager.GetFlowerPriceByName(flowerWants);
            clients.Add(new Client(flowerWants, flowerPrice + (flowerPrice * (up * shopManager.CurrentRating()/ 100)), flowerPrice));
        }

        return clients;
    }

}
