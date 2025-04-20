using UnityEngine;

public class StatisticsManager
{

    private readonly DataBaseManager databaseManager;

    private int flowersSold = 0;

    public float FlowersSold
    {
        get => flowersSold;
    }

    public void PlusFlower()
    {
        flowersSold++;
    }

    private float moneyEarned;
    public float MoneyEarned
    {
        get => moneyEarned;
        set => moneyEarned = value;
    }

    private int clientsCount = 0;

    public int ClientsCount
    {
        get => clientsCount;
    }

    private float allClientsSatisfaction = 0;

    public float AverageSatisfaction
    {
        get => allClientsSatisfaction / clientsCount;
    }

    public void AddClient(float satisfaction)
    {
        clientsCount++;
        allClientsSatisfaction += satisfaction;
    }


    public StatisticsManager(DataBaseManager databaseManager)
    {
        this.databaseManager = databaseManager;
    }

    public void CountInfo()
    {
        databaseManager.CountStats(flowersSold, moneyEarned, clientsCount, allClientsSatisfaction/clientsCount);
    }

}
