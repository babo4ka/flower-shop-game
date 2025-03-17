using System.Diagnostics;
using UnityEngine;

public class FlowersManager : MonoBehaviour
{
    private Shop shop;

    [SerializeField] DataBaseManager dataBaseManager;

    private void Awake()
    {
        dataBaseManager.usi += UpdateShopInfo;
    }

    public void ChangeFlowerPrice(string flowerName, float price)
    {
        dataBaseManager.ChangeFlowerPrice(flowerName, price);
    }


    public bool BuyFlower(string flowerName, int count, float sum)
    {
        if(shop.cash >= sum)
        {
            dataBaseManager.BuyFlower(flowerName, count, sum);
            return true;
        }
        else
        {
            return false;
        }
    }


    private void UpdateShopInfo(Shop s)
    {
        this.shop = s;
    }
}
