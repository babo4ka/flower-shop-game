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

    private void Start()
    {
        currentPos = "CameraInitPos";
        OpenShowCases();
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
            move?.Invoke(positions.Pop());
    }

    private void OpenShowCases()
    {
        var maxCases = shopManager.OpenedShowCases();
        if (maxCases > 1)
        {
            var cases = GameObject.FindGameObjectsWithTag("boxDefault");
            cases = cases.OrderBy(c => int.Parse(c.name.Split(" ")[1])).ToArray();
            for (int i = 1; i < maxCases; i++)
            {
                Destroy(cases[i].transform.Find("unavailable").gameObject);
            }
        }
        
    }
}
