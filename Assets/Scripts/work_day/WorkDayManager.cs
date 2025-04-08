using UnityEngine;

public class WorkDayManager : MonoBehaviour
{
    //�������� ������ � ������� ��������
    [SerializeField] ShopManager shopManager;
    //�������� ��� ������ � ������� ������
    [SerializeField] FlowersManager flowersManager;

    private ClientsCreator clientCreator;


    private void Awake()
    {
        clientCreator = new ClientsCreator(shopManager, flowersManager);
        GameManager.startAnotherDay += StartAnotherDay;
    }


    private void StartAnotherDay()
    {
        clientCreator.SetCleintsForDay();
    }
}
