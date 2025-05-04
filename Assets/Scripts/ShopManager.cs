using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static Action<Shop> sendUpdatedShopInfo;

    [SerializeField] DataBaseManager dataBaseManager;

    //�������� ��������
    private Shop shop;

    private const float workPlacePrice = 50;
    public float WorkPlacePrice
    {
        get => workPlacePrice;
    }
    private const float showCasePrice = 50;
    public float ShowCasePrice
    {
        get => showCasePrice;
    }

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

    public bool IncreaseMaxWorkers()
    {
        if(shop.cash >= workPlacePrice)
        {
            dataBaseManager.IncreaseMaxWorkers(workPlacePrice);
            return true;
        }

        return false;
    }

    public bool IncreaseMaxShowCases()
    {
        if(shop.cash >= showCasePrice)
        {
            dataBaseManager.IncreaseMaxShowCases(showCasePrice);
            return true;
        }

        return false;
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

    public int OpenedShowCases()
    {
        return shop.maxShowCases;
    }

    public int MaxWorkers()
    {
        return shop.maxWorkers;
    }

    public List<WorkDays> GetStats()
    {
        return dataBaseManager.GetStats();
    }
    #endregion
}
