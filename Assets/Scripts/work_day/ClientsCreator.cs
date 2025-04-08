using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ClientsCreator
{
    //менеджер работы с данными магазина
    readonly ShopManager shopManager;
    //менеджер для работы с данными цветов
    readonly FlowersManager flowersManager;

    const float down = 2f;
    const float up = 3f;

    private List<Client> clientsForDay;

    private string[] flowerNames;

    public ClientsCreator(ShopManager shopManager, FlowersManager flowersManager)
    {
        this.shopManager = shopManager;
        this.flowersManager = flowersManager;   
    }


    public void SetCleintsForDay()
    {
        clientsForDay??= new();
        clientsForDay.Clear();

        int clientsCount = Random.Range(15, 40);

        for(int i=0;i<clientsCount; i++)
        {
            string flowerWants = flowerNames[Random.Range(0, flowerNames.Length)];
            float flowerPrice = flowersManager.GetFlowerPriceByName(flowerWants);
            clientsForDay.Add(new Client(flowerWants, 
                new float[] { (flowerPrice - (flowerPrice * (down * shopManager.CurrentRating() / 100))), 
                    (flowerPrice + (flowerPrice * (up * shopManager.CurrentRating()/ 100))) }));
        }

    }
}
