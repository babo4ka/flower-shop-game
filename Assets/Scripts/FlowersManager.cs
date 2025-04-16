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


    #region методы получения информации от базы данных
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
    //метод получения истории популярности цветка
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
    public (bool, string) ToggleSaleFlowers(string flowerName, int count, DataBaseManager.ToggleSaleAction action)
    {

        switch (action)
        {
            case DataBaseManager.ToggleSaleAction.PUT:
                if (GetShopFlowerByName(flowerName).count_in_stock < count) return (false, "Недостаточно цветов на складе");
                break;

            case DataBaseManager.ToggleSaleAction.REMOVE:
                if (GetShopFlowerByName(flowerName).count_on_sale < count) return (false, "Недостаточно цветов на витрине");
                break;
        }

        dataBaseManager.ToggleSaleFlowers(flowerName, count, action);
        return (true, "");
    }

    //покупка цветка
    public (bool, string) BuyFlower(string flowerName, int count, float price)
    {
        if (shopManager.IsCashEnough(count*price))
        {
            dataBaseManager.BuyFlower(flowerName, count, price);
            return (true, "");
        }
        else
        {
            return (false, "Недостаточно средств");
        }
    }

    public void SpendFlower(string flowerName)
    {
        dataBaseManager.SpendFlower(flowerName);
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

    public float GetFlowerPriceByName(string flowerName)
    {
        foreach(var flower in flowersPrices){
            if(flower.flower_name == flowerName)
            {
                return flower.GetFlowerPrice();
            }
        }

        return -1f;
    }

    public float GetShopFlowerPriceByName(string flowerName)
    {
        foreach (var flower in shopflowers)
        {
            if (flower.flower_name == flowerName)
            {
                return flower.price;
            }
        }

        return -1f;
    }


    public void UpdateFlwoersPopularityStory()
    {
        dataBaseManager.UpdateFlowersPopulartityStory();
    }
    #endregion

}
