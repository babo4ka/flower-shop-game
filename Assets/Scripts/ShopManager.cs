using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static Action<Shop> sendUpdatedShopInfo;

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
