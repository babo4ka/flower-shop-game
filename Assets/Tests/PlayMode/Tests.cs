using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Tests
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestsSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestsWithEnumeratorPasses()
    {
        var load = SceneManager.LoadSceneAsync("Scenes/New Scene");

        while (!load.isDone)
        {
            yield return null;
        }


        var gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }
}
