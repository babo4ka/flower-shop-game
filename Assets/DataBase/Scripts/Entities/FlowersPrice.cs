using System;
using UnityEngine;

public class FlowersPrice
{   
    public string flower_name { get; set; }

    private float _popularity_level;

    public float popularity_level
    {
        get { return _popularity_level; }
        set { _popularity_level = (float)Math.Round(value, 2);}
    }

    private float _popularity_coefficient;

    public float popularity_coefficient
    {
        get { return _popularity_coefficient; }
        set { _popularity_coefficient = (float)Math.Round(value, 2); }
    }

    private float _market_price;

    public float market_price
    {
        get { return _market_price; }
        set { _market_price = (float)Math.Round(value, 2); }
    }

    public float GetFlowerPrice()
    {
        return (float)Math.Round(market_price * popularity_level * popularity_coefficient / 10, 2);
    }
}

