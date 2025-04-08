using UnityEngine;

public class WorkDayManager : MonoBehaviour
{
    //менеджер работы с данными магазина
    [SerializeField] ShopManager shopManager;
    //менеджер для работы с данными цветов
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
