using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action startAnotherDay;

    private void Awake()
    {
        StartCoroutine(WaitForDbReady());
        Debug.Log(float.Parse("14.33"));
    }

    void StartAnotherDay()
    {
        startAnotherDay?.Invoke();
    }


    private IEnumerator WaitForDbReady()
    {
        while (!DataBaseManager.loaded) yield return null;

        StartAnotherDay();
    }
}
