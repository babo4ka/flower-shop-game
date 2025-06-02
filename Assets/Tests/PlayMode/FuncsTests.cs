using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class FuncsTests
{

    [UnityTest]
    public IEnumerator TestBuyFlower()
    {
        var load = SceneManager.LoadSceneAsync("Scenes/New Scene");

        while (!load.isDone)
        {
            yield return null;
        }

        var gameManager = GameObject.FindGameObjectWithTag("GameManager");
        var flowersManager = gameManager.GetComponent<FlowersManager>();
        var db = gameManager.GetComponent<DataBaseManager>();

        flowersManager.BuyFlower("Хризантема", 3, 250f);

        var sf = flowersManager.GetShopFlowers();
        var s = sf.Where(f => f.flower_name == "Хризантема").First();

        Assert.AreEqual(27, s.count_in_stock);
    }


    [UnityTest]
    public IEnumerator TestHireWorker() {
        var load = SceneManager.LoadSceneAsync("Scenes/New Scene");

        while (!load.isDone)
        {
            yield return null;
        }

        var gameManager = GameObject.FindGameObjectWithTag("GameManager");
        var workersManager = gameManager.GetComponent<WorkersManager>();

        Workers w = new() { name="Имя", motivation=100, minimal_shop_rating=1, minimal_hour_salary=240 };

        var currWorkersCount = workersManager.GetHiredWorkers().Count;

        workersManager.HireWorker(w);

        Assert.AreEqual(currWorkersCount+1, workersManager.GetHiredWorkers().Count);
    }


    [UnityTest]
    public IEnumerator TestPutFlowerToSale()
    {
        var load = SceneManager.LoadSceneAsync("Scenes/New Scene");

        while (!load.isDone)
        {
            yield return null;
        }

        var gameManager = GameObject.FindGameObjectWithTag("GameManager");
        var flowersManager = gameManager.GetComponent<FlowersManager>();

        flowersManager.ToggleSaleFlowers("Хризантема", 1, DataBaseManager.ToggleSaleAction.PUT);

        var flower = flowersManager.GetShopFlowers().Where(f => f.flower_name == "Хризантема").First();

        Assert.AreEqual(26, flower.count_in_stock);
        Assert.AreEqual(1, flower.count_on_sale);
    }


    [UnityTest]
    public IEnumerator TestWrongMotivationForWorker()
    {
        var load = SceneManager.LoadSceneAsync("Scenes/New Scene");

        while (!load.isDone)
        {
            yield return null;
        }

        var gameManager = GameObject.FindGameObjectWithTag("GameManager");
        var workersManager = gameManager.GetComponent<WorkersManager>();

        Workers w = new() { name = "Имя", motivation = 15, minimal_shop_rating = 1, minimal_hour_salary = 240 };
        var (_, status) = workersManager.SendWorkerToShift(w, 240);

        Assert.AreEqual("у работника не хватает мотивации!", status);
    }
}
