using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingManager : MonoBehaviour
{
    private Stack<string> positions = new();
    [SerializeField] GameObject camera;

    private string currentPos;

    public static Action<string> move;

    [SerializeField] ShopManager shopManager;

    private void Awake()
    {
        currentPos = "CameraInitPos";
        ShopManager.sendUpdatedShopInfo += OpenShowCases;
    }

    public void AddPosition(string position)
    {
        positions.Push(currentPos);
        currentPos = position;
    }

    public void GetBack()
    {
        Debug.Log(positions.Count);
        Debug.Log(currentPos);
        if(positions.Count != 0)
        {
            var pos = positions.Pop();
            move?.Invoke(pos);
            currentPos = pos;
        }
            
    }

    private void OpenShowCases(Shop shop)
    {
        var maxCases = shop.maxShowCases;
        if (maxCases > 1)
        {
            var cases = GameObject.FindGameObjectsWithTag("boxDefault");
            cases = cases.OrderBy(c => int.Parse(c.name.Split(" ")[1])).ToArray();
            for (int i = 1; i < maxCases; i++)
            {
                cases[i].GetComponent<DropTarget>().Available = true;
                Transform krest;
                if((krest = cases[i].transform.Find("unavailable")) != null)
                    Destroy(krest.gameObject);
            }
        }
        
    }
}
