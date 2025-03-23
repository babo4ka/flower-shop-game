using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action startAnotherDay;

    private void Awake()
    {
        StartCoroutine(WaitForDbReady());
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
