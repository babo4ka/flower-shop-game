using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkersManager : MonoBehaviour
{
    #region константы
    readonly string[] names = new string[] { "Алексей", "Мария", "Иван", "Ольга", "Дмитрий", "Екатерина",
       "Сергей", "Анна", "Андрей", "Татьяна", "Николай", "Елена", "Павел", "Наталья", "Виктор" };

    readonly float[] multipliers = new float[] { 1f, 1.18f, 1.35f, 1.51f, 1.68f };

    const float base_hourly_salary = 174.35f;
    #endregion

    public static Func<Workers, string, (bool, string)> replaceWorkerOnShift;

    //менеджер работы с данными магазина
    [SerializeField] ShopManager shopManager;
    //менеджер работы с базой данных
    [SerializeField] DataBaseManager databaseManager;

    //список работников, доступных для найма
    private List<Workers> availableWorkers;
    //список сотрудников, нанятых в магазин
    private List<Workers> hiredWorkers;

    private void Awake()
    {
        GameManager.startAnotherDay += GenerateWorkers;
        DataBaseManager.updateWorkersData += UpdateWorkersData;
    }

    #region методы для изменения информации
    //метод для генерации списка сотрудников, доступных для найма
    private void GenerateWorkers()
    {
        availableWorkers?.Clear();
        availableWorkers = new();


        int count = UnityEngine.Random.Range(3, 12);

        for (int i=0;i<count;i++)
        {
            availableWorkers.Add(new Workers() { name = names[UnityEngine.Random.Range(0, names.Length)], 
                minimal_shop_rating = shopManager.CurrentRating(),
                motivation = 100f, minimal_hour_salary = base_hourly_salary * multipliers[shopManager.CurrentRating()]
            });
        }
    }


    public (bool, string) HireWorker(Workers worker)
    {
        if(shopManager.CurrentRating() < worker.minimal_shop_rating)
        {
            return (false, $"Необходим минимальный рейтинг магазина: {worker.minimal_shop_rating}");
        }

        databaseManager.HireWorker(worker);
        availableWorkers.Remove(worker);
        return (true, "");
    }

    public void FireWorker(Workers worker)
    {
        databaseManager.FireWorker(worker);
    }

    public void DecreaseWorkerMotivation(Workers worker, float amount)
    {
        databaseManager.DecreaseWorkerMotivation(worker, amount);
    }

    public void IncreaseWorkerMotivation(Workers worker, float amount)
    {
        databaseManager.IncreaseWorkerMotivation(worker, amount);
    }

    public void UpdateWorker(Workers worker)
    {
        databaseManager.UpdateWorker(worker);
    }
    #endregion

    private void UpdateWorkersData(List<Workers> workers)
    {
        hiredWorkers = workers;
    }

    #region методы для получения данных по запросу к менеджеру по работе с персоналом
    //метод для получения сотрудников, нанятых в магазин
    public List<Workers> GetHiredWorkers()
    {
        return hiredWorkers;
    }

    //метод для получения списка сотрудников, доступных для найма
    public List<Workers> GetAvailableWorkers()
    {
        return availableWorkers;
    }

    public (bool, string) SendWorkerToShift(Workers worker)
    {
        if (worker.motivation <= 20) return (false, "у работника не хватает мотивации!");
        worker.isOnShift = true;
        return ((bool, string))(replaceWorkerOnShift?.Invoke(worker, "to"));
    }

    public void ReturnWorkerFromShift(Workers worker)
    {
        worker.isOnShift = false;
        replaceWorkerOnShift?.Invoke(worker, "from");
    }
    #endregion
}
