using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static Action<Shop> sendUpdatedShopInfo;

    [SerializeField] DataBaseManager dataBaseManager;

    //сущность магазина
    private Shop shop;

    private void Awake()
    {
        DataBaseManager.updateShopData += UpdateShopData;
    }


    #region методы получени€ информации от базы данных
    //обновление данных магазина
    private void UpdateShopData(Shop shop)
    {
        this.shop = shop;
        sendUpdatedShopInfo?.Invoke(shop);
    }
    #endregion

    #region методы обновлени€ информации в базе данных
    public void AddCash(float amount)
    {
        dataBaseManager.AddCash(amount);
    }

    #endregion


    #region методы запросов к менеджеру данных магазина
    public bool IsCashEnough(float sum)
    {
        return sum <= shop.cash;
    }

    public int CurrentRating()
    {
        return shop.rating;
    }
    #endregion
}
