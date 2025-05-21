using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FlowersManager : MonoBehaviour
{

    //���� ������
    [SerializeField] DataBaseManager dataBaseManager;
    //�������� ������ ���������
    [SerializeField] ShopManager shopManager;

    //������ ������ � ������
    private List<FlowersPrice> flowersPrices;
    //������ ������ � ��������
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


    #region ������ ��������� ���������� �� ���� ������
    //��������� ���� ������
    private void UpdateFlowerPricesData(List<FlowersPrice> prices)
    {
        flowersPrices = prices;
    }

    private void UpdateShopFlowersData(List<ShopFlowers> shopFlowers)
    {
        this.shopflowers = shopFlowers;
    }
    #endregion


    #region ��������� ������ �� ������� � ���� ������
    //����� ��������� ������� ������������ ������
    public List<PopularityStory> GetFlowerPopularityStory(string flowerName)
    {
        return dataBaseManager.GetFlowerPopularityStory(flowerName);
    }
    #endregion

    #region ������ �������� � ��������� ������ ������
    //��������� ���� ������
    public void ChangeFlowerPrice(string flowerName, float price)
    {
        dataBaseManager.ChangeFlowerPrice(flowerName, price);
    }

    //����������� ������ �� �������
    public (bool, string) ToggleSaleFlowers(string flowerName, int count, DataBaseManager.ToggleSaleAction action)
    {
        var flower = GetShopFlowerByName(flowerName);
        switch (action)
        {
            case DataBaseManager.ToggleSaleAction.PUT:
                if (flower.count_in_stock < count) return (false, "������������ ������ �� ������");
                else
                {
                    flower.count_on_sale += count;
                    flower.count_in_stock -= count;
                }
                break;

            case DataBaseManager.ToggleSaleAction.REMOVE:
                if (flower.count_on_sale < count) return (false, "������������ ������ �� �������");
                else
                {
                    flower.count_on_sale -= count;
                    flower.count_in_stock += count;
                }
                break;
        }

        

        //dataBaseManager.ToggleSaleFlowers(flowerName, count, action);
        return (true, "");
    }

    //������� ������
    public (bool, string) BuyFlower(string flowerName, int count, float price)
    {
        if (shopManager.IsCashEnough(count*price))
        {
            dataBaseManager.BuyFlower(flowerName, count, price);
            return (true, "");
        }
        else
        {
            return (false, "������������ �������");
        }
    }

    public void ClearFlowers()
    {
        shopflowers.ForEach(f =>
        {
            var c = f.count_on_sale;
            if (c > 0)
            {
                f.count_in_stock += c;
                f.count_in_stock -= c;
            }
        });
    }

    public void SpendFlower(string flowerName)
    {
        dataBaseManager.SpendFlower(flowerName);
    }
    
    //��������� ������ � ��������
    public List<ShopFlowers> GetShopFlowers()
    {
        return shopflowers;
    }

    //��������� ��� �� �����
    public List<FlowersPrice> GetFlowersPrice()
    {
        return flowersPrices;
    }

    public bool HasFlower(string flowerName)
    {
        return GetShopFlowerByName(flowerName).count_on_sale > 0;
        //return dataBaseManager.HasFlower(flowerName);
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
