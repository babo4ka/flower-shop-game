using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class MovnigTest
{
    [UnityTest]
    public IEnumerator MoveToShowcaseTest()
    {
        var load = SceneManager.LoadSceneAsync("Scenes/New Scene");

        while (!load.isDone)
        {
            yield return null;
        }

        var showCase = GameObject.FindGameObjectsWithTag("boxDefault").Where(box => box.name == "default 1").First();

        var camera = Camera.main;
        var initialCamearPos = camera.transform.position;
        camera.transform.LookAt(showCase.transform);

        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));

        bool hitSth = Physics.Raycast(ray, out RaycastHit hit);

        hit.collider.gameObject.SendMessage("OnMouseDown");

        yield return new WaitForSeconds(1f);
        
        Assert.AreNotEqual(initialCamearPos, camera.transform.position);
    }
}
