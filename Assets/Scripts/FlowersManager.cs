using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FlowersManager : MonoBehaviour
{

    //база данных
    [SerializeField] DataBaseManager dataBaseManager;
    //менеджер данных магазмина
    [SerializeField] ShopManager shopManager;

    //список цветов с ценами
    private List<FlowersPrice> flowersPrices;
    //список цветов в магазине
    private List<ShopFlowers> shopflowers;

    private ShopFlowers GetShopFlowerByName(string name)
    {
        foreach(var flower in shopflowers)
        {
            if(flower.flower_name ==  name)
            {
                return flower;
            }
        }

        return null;
    }



    private void Awake()
    {
        DataBaseManager.updateFlowersPricesData += UpdateFlowerPricesData;
        DataBaseManager.updateShopFlowersData += UpdateShopFlowersData;
    }


    #region методы получени€ информации от базы данных
    //получение цены цветов
    private void UpdateFlowerPricesData(List<FlowersPrice> prices)
    {
        flowersPrices = prices;
    }

    private void UpdateShopFlowersData(List<ShopFlowers> shopFlowers)
    {
        this.shopflowers = shopFlowers;
    }
    #endregion


    #region полуечние данных по запросу к базе данных
    //метод получени€ истории попул€рности цветка
    public List<PopularityStory> GetFlowerPopularityStory(string flowerName)
    {
        return dataBaseManager.GetFlowerPopularityStory(flowerName);
    }
    #endregion

    #region методы запросов к менеджеру данных цветов
    //изменение цены цветка
    public void ChangeFlowerPrice(string flowerName, float price)
    {
        dataBaseManager.ChangeFlowerPrice(flowerName, price);
    }

    //выставление цветка на продажу
    public bool ToggleSaleFlowers(string flowerName, int count, DataBaseManager.ToggleSaleAction action)
    {
        if(GetShopFlowerByName(flowerName).count_in_stock >= count)
        {
            dataBaseManager.ToggleSaleFlowers(flowerName, count, action);
            return true;
        }

        return false;
    }

    //покупка цветка
    public bool BuyFlower(string flowerName, int count, float price)
    {
        if (shopManager.IsCashEnough(count*price))
        {
            dataBaseManager.BuyFlower(flowerName, count, price);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    //получение цветов в магазине
    public List<ShopFlowers> GetShopFlowers()
    {
        return shopflowers;
    }

    //получение цен на цветы
    public List<FlowersPrice> GetFlowersPrice()
    {
        return flowersPrices;
    }
    #endregion

}
