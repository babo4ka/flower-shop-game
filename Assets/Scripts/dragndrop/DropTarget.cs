using System.Linq;
using UnityEngine;

public class DropTarget : MonoBehaviour
{
    private int flowersCount = 0;
    [SerializeField] GameObject flower;

    public void OnDropObject(GameObject droppedObject)
    {
        Debug.Log($"Object {droppedObject.name} dropped on {gameObject.name}");
        if(flowersCount <= 5)
        {
            Debug.Log(GetComponentsInChildren<Transform>().First().gameObject.name);
            Instantiate(flower, GetComponentsInChildren<Transform>().First());
            flowersCount++;
        }
    }
}
