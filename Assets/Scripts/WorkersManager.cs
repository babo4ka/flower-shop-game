using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WorkersManager : MonoBehaviour
{
    [SerializeField] ShopManager shopManager;

    readonly string[] names = new string[] { "Алексей", "Мария", "Иван", "Ольга", "Дмитрий", "Екатерина", 
       "Сергей", "Анна", "Андрей", "Татьяна", "Николай", "Елена", "Павел", "Наталья", "Виктор" };

    readonly float [] multipliers = new float[] { 1f, 1.18f, 1.35f, 1.51f, 1.68f };

    const float base_hourly_salary = 174.35f;

    public List<Workers> GenerateWorkers()
    {
        int count = Random.Range(3, 12);
        List<Workers> workers = new();

        for (int i=0;i<count;i++)
        {
            workers.Add(new Workers() { name = names[Random.Range(0, names.Length)], 
                minimal_shop_rating = shopManager.CurrentRating(),
                motivation = 100f, minimal_hour_salary = base_hourly_salary * multipliers[shopManager.CurrentRating()]
            });
        }

        return workers;
    }
}
