using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class FuncsTests
{

    AsyncOperation load;

    [OneTimeSetUp]
    public void SetUp()
    {
        load = SceneManager.LoadSceneAsync("Scenes/New Scene");
    }

    [UnityTest]
    public IEnumerator TestBuyFlower()
    {
        while (!load.isDone)
        {
            yield return null;
        }

        var gameManager = GameObject.FindGameObjectWithTag("GameManager");
        var flowersManager = gameManager.GetComponent<FlowersManager>();
        var db = gameManager.GetComponent<DataBaseManager>();

        flowersManager.BuyFlower("����������", 3, 250f);

        var sf = flowersManager.GetShopFlowers();
        var s = sf.Where(f => f.flower_name == "����������").First();

        Assert.AreEqual(30, s.count_in_stock);
    }


    [UnityTest]
    public IEnumerator TestHireWorker() {
        while (!load.isDone)
        {
            yield return null;
        }

        var gameManager = GameObject.FindGameObjectWithTag("GameManager");
        var workersManager = gameManager.GetComponent<WorkersManager>();

        Workers w = new() { name="���", motivation=100, minimal_shop_rating=1, minimal_hour_salary=240 };

        var currWorkersCount = workersManager.GetHiredWorkers().Count;

        workersManager.HireWorker(w);

        Assert.AreEqual(currWorkersCount+1, workersManager.GetHiredWorkers().Count);
    }


    [UnityTest]
    public IEnumerator TestPutFlowerToSale()
    {
        while (!load.isDone)
        {
            yield return null;
        }

        var flowersManager = GameObject.FindGameObjectWithTag("GameManager")
            .GetComponent<FlowersManager>();

        flowersManager.ToggleSaleFlowers("����������", 1, 
            DataBaseManager.ToggleSaleAction.PUT);

        var flower = flowersManager.GetShopFlowers()
            .Where(f => f.flower_name == "����������").First();

        Assert.AreEqual(29, flower.count_in_stock);
        Assert.AreEqual(1, flower.count_on_sale);
    }


    [UnityTest]
    public IEnumerator TestWrongMotivationForWorker()
    {
        while (!load.isDone)
        {
            yield return null;
        }

        var gameManager = GameObject.FindGameObjectWithTag("GameManager");
        var workersManager = gameManager.GetComponent<WorkersManager>();

        Workers w = new() { name = "���", motivation = 15, minimal_shop_rating = 1, minimal_hour_salary = 240 };
        var (_, status) = workersManager.SendWorkerToShift(w, 240);

        Assert.AreEqual("� ��������� �� ������� ���������!", status);
    }
}
