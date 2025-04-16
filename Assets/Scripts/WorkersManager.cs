using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkersManager : MonoBehaviour
{
    #region ���������
    readonly string[] names = new string[] { "�������", "�����", "����", "�����", "�������", "���������",
       "������", "����", "������", "�������", "�������", "�����", "�����", "�������", "������" };

    readonly float[] multipliers = new float[] { 1f, 1.18f, 1.35f, 1.51f, 1.68f };

    const float base_hourly_salary = 174.35f;
    #endregion

    public static Func<Workers, string, (bool, string)> replaceWorkerOnShift;

    //�������� ������ � ������� ��������
    [SerializeField] ShopManager shopManager;
    //�������� ������ � ����� ������
    [SerializeField] DataBaseManager databaseManager;

    //������ ����������, ��������� ��� �����
    private List<Workers> availableWorkers;
    //������ �����������, ������� � �������
    private List<Workers> hiredWorkers;

    private void Awake()
    {
        GameManager.startAnotherDay += GenerateWorkers;
        DataBaseManager.updateWorkersData += UpdateWorkersData;
    }

    #region ������ ��� ��������� ����������
    //����� ��� ��������� ������ �����������, ��������� ��� �����
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
            return (false, $"��������� ����������� ������� ��������: {worker.minimal_shop_rating}");
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

    #region ������ ��� ��������� ������ �� ������� � ��������� �� ������ � ����������
    //����� ��� ��������� �����������, ������� � �������
    public List<Workers> GetHiredWorkers()
    {
        return hiredWorkers;
    }

    //����� ��� ��������� ������ �����������, ��������� ��� �����
    public List<Workers> GetAvailableWorkers()
    {
        return availableWorkers;
    }

    public (bool, string) SendWorkerToShift(Workers worker)
    {
        if (worker.motivation <= 20) return (false, "� ��������� �� ������� ���������!");
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
