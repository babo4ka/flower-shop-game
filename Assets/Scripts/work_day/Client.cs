using Unity.VisualScripting;
using UnityEngine;

public class Client
{
    private float _satisfaction;

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

    private float[] _priceRange;
    public float[] PriceWants
    {
        get { return _priceRange; }
        set { _priceRange = value; }
    }


    public Client(string flowerWants, float[] priceRange)
    {
        Satisfaction = 100f;
        FlowerWants = flowerWants;
        PriceWants = priceRange;
    }
}
