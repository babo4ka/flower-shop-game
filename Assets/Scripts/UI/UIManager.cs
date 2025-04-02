using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region ���������� ����������
    //������ ����� ������������
    private List<GameObject> chartDots;
    #endregion

    #region ���������� �� ���������
    //������ � ������������ �����
    [SerializeField] TMP_Text cashTxt;
    [SerializeField] TMP_Text daysTxt;

    //�������� ������ ��� �������������� � �������
    [SerializeField] GameObject flowersSettingsPanel;
    //������ ������ ������ � �������
    [SerializeField] GameObject shopFlowersPanel;
    [SerializeField] GameObject marketFlowersPanel;
    [SerializeField] TMP_Text flowersPanelName;

    //������ �����
    [SerializeField] GameObject workersPanel;
    //������ ������ ������ ���������
    [SerializeField] GameObject workersToHirePanel;
    [SerializeField] GameObject hiredWorkersPanel;
    [SerializeField] TMP_Text workersPanelName;


    //�������� ���� ������
    [SerializeField] DataBaseManager dataBaseManager;
    //�������� ��� ������ � �������
    [SerializeField] FlowersManager flowersManager;
    //�������� ��� ������ � �����������
    [SerializeField] WorkersManager workersManager;

    //������ ��� ����� � �������
    [SerializeField] GameObject flowersListOnMarketContent;
    [SerializeField] GameObject flowersListOnMarketObject;
    List<GameObject> flowersOnMarketCards;

    //������ ��� ����� � �����������
    [SerializeField] GameObject workersListOnMarketContent;
    [SerializeField] GameObject workersListOnMarketObject;
    List<GameObject> workersOnMarketCards;

    //������ ��� ������ � ��������
    [SerializeField] GameObject flowersListOnShopContent;
    [SerializeField] GameObject flowersListOnShopObject;
    List<GameObject> flowersOnShopCards;

    //������ ��� ������� ����������
    [SerializeField] GameObject hiredWorkersListContent;
    [SerializeField] GameObject hiredWorkersListObject;
    List<GameObject> hiredWorkersCards;

    //������ ������������
    //������ ��� �������
    [SerializeField] GameObject popularityChartPanel;
    //������ ������������
    [SerializeField] GameObject popularityChart;
    //�������� ������ �� �������
    [SerializeField] TMP_Text flowerNameOnChart;
    //������ ����� ��� �������
    [SerializeField] GameObject chartDot;
    

    //������ ������� ������
    //���� ������
    [SerializeField] GameObject flowerPriceText;
    //������ ��� �������� ������ ��� ������� ������
    [SerializeField] GameObject openBuyFlowerPanelBtn;
    //������ � ����������� ��� ������� ������
    [SerializeField] GameObject buyFlowerPanel;

    //������ ������ � ��������
    //������ ��� ��������� ������ ������
    [SerializeField] Button changeFlowerPriceBtn;
    [SerializeField] Button deleteFlowerFromSaleBtn;
    [SerializeField] Button putFlowerOnSaleBtn;

    //����������� ���������� � �������
    [SerializeField] TMP_Text flowerNameOnShopTxt;
    [SerializeField] TMP_Text flowerOnShopPriceTxt;
    [SerializeField] TMP_Text countFlowersOnSaleTxt;
    [SerializeField] TMP_Text countFlowersInStockTxt;
    //���� ��� �������� ��������� � ���������� � ������
    [SerializeField] TMP_InputField changeFlowerPriceInput;
    [SerializeField] TMP_InputField toggleFlowersOnSaleInput;

    //������ ��� ����������� ����������
    [SerializeField] GameObject messageBanner;
    #endregion

    #region ��������� ���������� ��������� ����������
    //����� ��� ������������ ������ �������� ��������
    public void ToggleFlowersSettingsPanel()
    {
        
        if(flowersSettingsPanel.activeInHierarchy)
        {
            flowersSettingsPanel.SetActive(false);
        }
        else
        {
            flowersSettingsPanel.SetActive(true);
        }
    }

    public void TogglePanelsAboutFlowers()
    {
        if (shopFlowersPanel.activeInHierarchy)
        {
            shopFlowersPanel.SetActive(false);
            marketFlowersPanel.SetActive(true);
            flowersPanelName.text = "�����";
        }
        else
        {
            shopFlowersPanel.SetActive(true);
            marketFlowersPanel.SetActive(false);
            flowersPanelName.text = "�����";
        }
    }

    //����� ��� ������������ ������ �����
    public void ToggleWorkersPanel()
    {
        if(workersPanel.activeInHierarchy)
        {
            workersPanel.SetActive(false);
        }
        else
        {
            workersPanel.SetActive(true);
        }
    }

    public void TogglePanelAboutWorkers()
    {
        if (workersToHirePanel.activeInHierarchy)
        {
            workersToHirePanel.SetActive(false);
            hiredWorkersPanel.SetActive(true);
            workersPanelName.text = "�������";
        }else{
            workersToHirePanel.SetActive(true);
            hiredWorkersPanel.SetActive(false);
            workersPanelName.text = "�����";
        }
    }

    public void TogglePanelsInsideShopSettings(GameObject panel)
    {
        if (shopFlowersPanel != null) shopFlowersPanel.SetActive(false);
        shopFlowersPanel = panel;
        panel.SetActive(true);
    }
    #endregion

    private void Awake()
    {
        ShopManager.sendUpdatedShopInfo += GetUpdatedShopInfo;
        flowersListOnMarketContent.GetComponent<OnEnableEvent>().enabled += GetFlowersPrices;
        flowersListOnShopContent.GetComponent<OnEnableEvent>().enabled += GetShopsFlowersList;
        workersListOnMarketContent.GetComponent<OnEnableEvent>().enabled += GetWorkersOnMarketList;
        hiredWorkersListContent.GetComponent<OnEnableEvent>().enabled += GetHiredWorkersList;
    }

    #region ������ ��� ����������� � ��������� ���������� � ������
    //����� ��� ����������� ������ ������� �� �����
    private void GetFlowersPrices()
    {
        List<FlowersPrice> stories = flowersManager.GetFlowersPrice();
        RemoveCards(flowersOnMarketCards);
        flowersOnMarketCards = new();

        stories.ForEach(story =>
        {
            GameObject flowerCard = Instantiate(flowersListOnMarketObject, flowersListOnMarketContent.transform);
            flowersOnMarketCards.Add(flowerCard);
            TMP_Text flowerNameTxt = flowerCard.transform.Find("FlowerNameTxt").GetComponent<TMP_Text>();
            TMP_Text flowerPriceTxt = flowerCard.transform.Find("FlowerPriceTxt").GetComponent<TMP_Text>();

            flowerNameTxt.text = story.flower_name;
            flowerPriceTxt.text = $"{(float)Math.Round(story.market_price * story.popularity_level * story.popularity_coefficient / 10, 2)}";

            List<PopularityStory> flowerPopularityStory = flowersManager.GetFlowerPopularityStory(story.flower_name);


            flowerCard.GetComponent<Button>().onClick.AddListener(() => {
                ShowFlowerInfo(story.flower_name, flowerPopularityStory);
                if (!flowerPriceText.activeInHierarchy) flowerPriceText.SetActive(true);
                if (!openBuyFlowerPanelBtn.activeInHierarchy) openBuyFlowerPanelBtn.SetActive(true);

                float price = (float)Math.Round(story.market_price * (story.popularity_level * story.popularity_coefficient / 10), 2);
                flowerPriceText.GetComponent<TMP_Text>().text = $"����: {price}";
                openBuyFlowerPanelBtn.GetComponent<Button>().onClick.RemoveAllListeners();
                openBuyFlowerPanelBtn.GetComponent<Button>().onClick.AddListener(() => OpenBuyFlowerPanel(story.flower_name, price));
            });
            
        });

    }

    

    //����� ��� ����������� ������ ���������� ��� ������� ������ � �������� � ������� ������ ������� ������
    private void OpenBuyFlowerPanel(string flowerName, float price)
    {
        buyFlowerPanel.SetActive(true);
        buyFlowerPanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(() => { buyFlowerPanel.SetActive(false); });

        buyFlowerPanel.transform.Find("FlowerNameTxt").GetComponent<TMP_Text>().text = flowerName;
        buyFlowerPanel.transform.Find("PriceTxt").GetComponent<TMP_Text>().text = $"����: {price}";

        buyFlowerPanel.transform.Find("CountInput").GetComponent<TMP_InputField>().onValueChanged.RemoveAllListeners();
        buyFlowerPanel.transform.Find("BuyBtn").GetComponent<Button>().onClick.RemoveAllListeners();

        buyFlowerPanel.transform.Find("CountInput").GetComponent<TMP_InputField>().onValueChanged.AddListener((input) => {
            int count = int.Parse(input);
            float sum = (float)Math.Round(price * count, 2);
            buyFlowerPanel.transform.Find("SumTxt").GetComponent<TMP_Text>().text = $"�����: {sum}";

            //�������� � ������ ������� ������
            buyFlowerPanel.transform.Find("BuyBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                var (bought, status) = flowersManager.BuyFlower(flowerName, count, price);
                if (bought)
                {
                }
                else
                {
                    Debug.Log(status);
                    ShowMessage(status);
                }
                
            });
        });
    }

    //����� ��� ����������� ���������� � ������ � ��� ������� ������������
    private void ShowFlowerInfo(string flowerName, List<PopularityStory> popularityStory)
    {
        if (!popularityChartPanel.activeInHierarchy) popularityChartPanel.SetActive(true);

        ClearDots();
        chartDots = new();


        RectTransform chartRectTransform = popularityChart.GetComponent<RectTransform>();

        float xPos = chartRectTransform.rect.xMin + 5f;
        float step = chartRectTransform.rect.width / popularityStory.Count;
        float startY = chartRectTransform.rect.yMin;
        float height = chartRectTransform.rect.height;

        popularityStory.ForEach(story =>
        {
            GameObject dot = Instantiate(chartDot, popularityChart.transform);
            float yPos = startY + (story.popularity_level / 100 * height);
            dot.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
            xPos += step;

            chartDots.Add(dot);
        });


        flowerNameOnChart.text = flowerName;


        void ClearDots()
        {
            if(chartDots is not  null)
            {
                chartDots.ForEach(dot =>
                {
                    Destroy(dot);
                });
                chartDots.Clear();
            }  
        }
    }



    private void GetShopsFlowersList()
    {
        List<ShopFlowers> flowers = flowersManager.GetShopFlowers();
        RemoveCards(flowersOnShopCards);
        flowersOnShopCards = new();
        flowers.ForEach(flower =>
        {
            GameObject flowerCard = Instantiate(flowersListOnShopObject, flowersListOnShopContent.transform);
            flowersOnShopCards.Add(flowerCard);
            TMP_Text flowerNameTxt = flowerCard.transform.Find("FlowerNameTxt").GetComponent<TMP_Text>();
            TMP_Text flowerPriceTxt = flowerCard.transform.Find("FlowerPriceTxt").GetComponent<TMP_Text>();

            flowerNameTxt.text = flower.flower_name;
            flowerPriceTxt.text = $"{flower.price}";

            List<PopularityStory> flowerPopularityStory = flowersManager.GetFlowerPopularityStory(flower.flower_name);


            flowerCard.GetComponent<Button>().onClick.AddListener(() => {
                flowerNameOnShopTxt.text = flower.flower_name;
                countFlowersOnSaleTxt.text = $"���������� � �������: {flower.count_on_sale}";
                countFlowersInStockTxt.text = $"���������� �� ������: {flower.count_in_stock}";

                changeFlowerPriceInput.onValueChanged.RemoveAllListeners();
                changeFlowerPriceInput.onValueChanged.AddListener(value =>
                {
                    changeFlowerPriceBtn.onClick.RemoveAllListeners();
                    changeFlowerPriceBtn.onClick.AddListener(() =>
                    {
                        flowersManager.ChangeFlowerPrice(flower.flower_name, float.Parse(value));
                    });
                });

                toggleFlowersOnSaleInput.onValueChanged.RemoveAllListeners();
                toggleFlowersOnSaleInput.onValueChanged.AddListener(value =>
                {
                    deleteFlowerFromSaleBtn.onClick.RemoveAllListeners();
                    putFlowerOnSaleBtn.onClick.RemoveAllListeners();

                    deleteFlowerFromSaleBtn.onClick.AddListener(() =>
                    {
                        var (toggled, status) = flowersManager.ToggleSaleFlowers(flower.flower_name, int.Parse(value), DataBaseManager.ToggleSaleAction.REMOVE);
                        if (!toggled)
                        {
                            Debug.Log(status);
                        }
                    });

                    putFlowerOnSaleBtn.onClick.AddListener(() =>
                    {
                        var (toggled, status) = flowersManager.ToggleSaleFlowers(flower.flower_name, int.Parse(value), DataBaseManager.ToggleSaleAction.PUT);
                        if (!toggled)
                        {
                            Debug.Log(status);
                        }
                    });
                });
            });

        });
    }


    #endregion



    #region ������ ��� ����������� � ��������� ���������� � ����������
    private void GetWorkersOnMarketList()
    {
        var workers = workersManager.GetAvailableWorkers();
        RemoveCards(workersOnMarketCards);
        workersOnMarketCards = new();

        workers.ForEach(worker =>
        {
            GameObject workerCard = Instantiate(workersListOnMarketObject, workersListOnMarketContent.transform);
            workersOnMarketCards.Add(workerCard);

            workerCard.transform.Find("WorkerNameTxt").GetComponent<TMP_Text>().text = worker.name;
            workerCard.transform.Find("WorkerDataTxt").GetComponent<TMP_Text>().text = $@"����������� ������� ��������: {worker.minimal_shop_rating}
                                                                                        ����������� ������ � ���: {worker.minimal_hour_salary}";

            workerCard.transform.Find("HireBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                var (isHired, status) = workersManager.HireWorker(worker);

                if (isHired)
                {
                    ReloadPanel(workersToHirePanel);
                }
                else
                {
                    Debug.Log(status);
                }
            });
        });
    }


    private void GetHiredWorkersList()
    {
        var workers = workersManager.GetHiredWorkers();
        RemoveCards(hiredWorkersCards);
        hiredWorkersCards = new();

        workers.ForEach(worker =>
        {
            GameObject workerCard = Instantiate(hiredWorkersListObject, hiredWorkersListContent.transform);
            hiredWorkersCards.Add(workerCard);

            workerCard.transform.Find("WorkerNameTxt").GetComponent<TMP_Text>().text = worker.name;
            workerCard.transform.Find("WorkerDataTxt").GetComponent<TMP_Text>().text = $@"��������� ����������: {worker.motivation}
                                                                                        ����������� ������ � ���: {worker.minimal_hour_salary}";

            workerCard.transform.Find("FireBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                workersManager.FireWorker(worker);
                ReloadPanel(hiredWorkersPanel);
            });
        });
    }
    #endregion


    private void RemoveCards(List<GameObject> cards)
    {
        if (cards is not null)
        {
            cards.ForEach(card => { Destroy(card); });
            cards.Clear();
        }
    }

    private void ShowMessage(string message)
    {
        messageBanner.transform.Find("MessageTxt").GetComponent<TMP_Text>().text = message;
        Instantiate(messageBanner, workersPanel.transform);
    }

    private void ReloadPanel(GameObject panel)
    {
        if (panel.activeInHierarchy)
        {
            panel.SetActive(false);
            panel.SetActive(true);
        }
    }

    #region ������ ��� ����������� ���������� ��������
    private void GetUpdatedShopInfo(Shop shop)
    {
        cashTxt.text = $"{shop.cash} $";
        daysTxt.text = $"���� ������: {shop.daysGone}";
    }
    #endregion

}
