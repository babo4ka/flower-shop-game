using NUnit.Framework;
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

    [SerializeField] ShopManager shopManager;

    private List<Workers> availableWorkers;

    private void Awake()
    {
        GameManager.startAnotherDay += GenerateWorkers;
    }

    private void GenerateWorkers()
    {
        if(availableWorkers is not null) availableWorkers.Clear();
        availableWorkers = new();


        int count = Random.Range(3, 12);

        for (int i=0;i<count;i++)
        {
            availableWorkers.Add(new Workers() { name = names[Random.Range(0, names.Length)], 
                minimal_shop_rating = shopManager.CurrentRating(),
                motivation = 100f, minimal_hour_salary = base_hourly_salary * multipliers[shopManager.CurrentRating()]
            });
        }


        availableWorkers.ForEach(w => { Debug.Log(w); });
    }


    public List<Workers> GetAvailableWorkers()
    {
        return availableWorkers;
    }
}
