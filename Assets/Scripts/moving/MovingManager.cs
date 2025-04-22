using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingManager : MonoBehaviour
{
    private Stack<string> positions = new();
    [SerializeField] GameObject camera;

    private string currentPos;

    public static Action<string> move;

    private void Start()
    {
        currentPos = "CameraInitPos";
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
}
