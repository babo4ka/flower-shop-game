using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropTarget : MonoBehaviour
{
    private int flowersCount = 0;
    [SerializeField] GameObject flower;
    [SerializeField] private bool available;
    [SerializeField] UIManager uiManager;
    [SerializeField] FlowersManager flowersManager;

    private List<GameObject> flowers;

    private void Awake()
    {
        WorkDayManager.dayFinish += ClearFlowers;
        flowers = new();
    }

    public bool Available
    {
        get => available; 
        set => available = value;
    }

    private void ClearFlowers()
    {
        flowers.ForEach(f =>
        {
            Destroy(f);
        });
        flowers.Clear();
    }

    public void OnDropObject(GameObject droppedObject, string flowerName)
    {
        //Debug.Log($"Object {droppedObject.name} dropped on {gameObject.name}");
        if(flowersCount < 5)
        {
            if (available)
            {
                //Debug.Log(GetComponentsInChildren<Transform>().First().gameObject.name);
                var place = GetComponentsInChildren<Transform>().First().Find($"FlowerPlace {flowersCount+1}");
                var obj = Instantiate(flower, place);
                var pos = obj.transform.position;
                pos.y += 5;
                obj.transform.position = pos;
                flowersCount++;
                Debug.Log($"toggle flower {flowerName}");
                flowersManager.ToggleSaleFlowers(flowerName, 1, DataBaseManager.ToggleSaleAction.PUT);
                flowers.Add(obj);
            }
            else uiManager.ShowMessage("Витрина недоступна", null);
        }
        else uiManager.ShowMessage("Максимальное количество цветов на витрине", null);
    }
}
