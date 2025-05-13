using System.Linq;
using UnityEngine;

public class DropTarget : MonoBehaviour
{
    private int flowersCount = 0;
    [SerializeField] GameObject flower;
    [SerializeField]private bool available;
    [SerializeField] UIManager uiManager;

    public bool Available
    {
        get => available; 
        set => available = value;
    }

    public void OnDropObject(GameObject droppedObject)
    {
        Debug.Log($"Object {droppedObject.name} dropped on {gameObject.name}");
        if(flowersCount < 5)
        {
            if (available)
            {
                Debug.Log(GetComponentsInChildren<Transform>().First().gameObject.name);
                var obj = Instantiate(flower, GetComponentsInChildren<Transform>().First());
                var pos = obj.transform.position;
                pos.y += 5;
                obj.transform.position = pos;
                flowersCount++;
            }
            else uiManager.ShowMessage("Витрина недоступна", null);
        }
        else uiManager.ShowMessage("Максимальное количество цветов на витрине", null);
    }
}
