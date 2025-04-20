using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static Action<Shop> sendUpdatedShopInfo;

    [SerializeField] DataBaseManager dataBaseManager;

    //�������� ��������
    private Shop shop;

    private void Awake()
    {
        DataBaseManager.updateShopData += UpdateShopData;
    }


    #region ������ ��������� ���������� �� ���� ������
    //���������� ������ ��������
    private void UpdateShopData(Shop shop)
    {
        this.shop = shop;
        sendUpdatedShopInfo?.Invoke(shop);
    }
    #endregion

    #region ������ ���������� ���������� � ���� ������
    public void AddCash(float amount)
    {
        dataBaseManager.AddCash(amount);
    }

    public void IncreaseDay()
    {
        dataBaseManager.IncreaseDay();
    }

    public List<WorkDays> GetStats()
    {
        return dataBaseManager.GetStats();
    }
    #endregion


    #region ������ �������� � ��������� ������ ��������
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
