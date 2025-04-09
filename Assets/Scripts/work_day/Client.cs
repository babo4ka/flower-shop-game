using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Client
{
    private float _satisfaction = 0;

    public float Satisfaction
    {
        get { return _satisfaction; }
        set { _satisfaction = value;}
    }

    private string _flowerWants;

    public string FlowerWants
    {
        get { return _flowerWants; }
        set { _flowerWants = value; }
    }

    private float maxPrice;
    public float MaxPrice
    {
        get => maxPrice;
        set { maxPrice = value; }
    }

    private float baseFlowerPrice;

    public float BaseFlowerPrice
    {
        get => baseFlowerPrice;
        set { baseFlowerPrice = value; }
    }

    public float GetPriceSatisfaction(float price)
    {
        if (price > maxPrice) return 0;
        if (price == baseFlowerPrice) return 1;

        float step = (maxPrice - baseFlowerPrice) / 2;

        if (price <= maxPrice && price > maxPrice - step)return .5f;
        else if (price <= maxPrice - step && price > baseFlowerPrice) return .75f;
        else if (price < baseFlowerPrice && price > baseFlowerPrice - step) return 1.25f;
        else return 1.5f;
    }


    public Client(string flowerWants, float maxPrice, float baseFlowerPrice)
    {
        FlowerWants = flowerWants;
        MaxPrice = maxPrice;
        BaseFlowerPrice = baseFlowerPrice;
    }
}
