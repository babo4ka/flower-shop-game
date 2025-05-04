using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
    //�������� ������
    [SerializeField] GameObject activePanel;

    //������ � ������������ �����
    [SerializeField] TMP_Text cashTxt;
    [SerializeField] TMP_Text daysTxt;
    [SerializeField] TMP_Text ratingTxt;

    //�������� ������ ��� �������������� � �������
    [SerializeField] GameObject flowersSettingsPanel;
    //������ ������ ������ � �������
    [SerializeField] GameObject shopFlowersPanel;
    [SerializeField] GameObject marketFlowersPanel;
    [SerializeField] TMP_Text flowersPanelName;

    //������ ��� ������ ����������
    [SerializeField] GameObject workersPanel;
    //������ ������ ������ ���������
    [SerializeField] GameObject workersToHirePanel;
    [SerializeField] GameObject hiredWorkersPanel;
    [SerializeField] TMP_Text workersPanelName;

    //������ ��� ������ �������� ���
    [SerializeField] GameObject shiftWorkersPanel;
    [SerializeField] GameObject shiftWorkersListPanel;


    //�������� ���� ������
    [SerializeField] DataBaseManager dataBaseManager;
    //�������� ��� ������ � �������
    [SerializeField] FlowersManager flowersManager;
    //�������� ��� ������ � �����������
    [SerializeField] WorkersManager workersManager;
    //�������� ��� ������ � ���������
    [SerializeField] ShopManager shopManager;

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

    //������ ��� �������� ��������
    [SerializeField] GameObject shopSettingsPanel;
    [SerializeField] GameObject maxShowCasesPanel;
    [SerializeField] GameObject maxWorkersPanel;
    [SerializeField] TMP_Text shopPageNameTxt;
    [SerializeField] Button buyShowCaseBtn;
    [SerializeField] Button buyWorkPlaceBtn;


    //������ ��� ���������� �� �����
    [SerializeField] GameObject shiftWorkersListContent;
    [SerializeField] GameObject shiftWorkersListObject;
    List<GameObject> shiftWorkersCards;
    //������ � ����������� ������ ��������� �� �����
    [SerializeField] GameObject salaryForWorkerSettingPanel;

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

    //����������
    [SerializeField] GameObject statsAfterShiftPanel;
    [SerializeField] TMP_Text[] textForStats;

    //����������
    [SerializeField] GameObject fullStatsPanel;
    [SerializeField] TMP_Text[] textForFullStats;

    //������ ��� ����������� ����������
    [SerializeField] GameObject messageBanner;
    #endregion

    #region ��������� ���������� ��������� ����������
    //����� ��� ������������ ������ ������
    public void ToggleFlowersSettingsPanel()
    {
        if(flowersSettingsPanel.activeInHierarchy)
        {
            flowersSettingsPanel.SetActive(false);
        }
        else
        {
            if(activePanel != null) activePanel.SetActive(false);
            flowersSettingsPanel.SetActive(true);
            activePanel = flowersSettingsPanel;
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

    //����� ��� ������������ ������ ���������
    public void ToggleWorkersPanel()
    {
        if(workersPanel.activeInHierarchy)
        {
            workersPanel.SetActive(false);
        }
        else
        {
            if (activePanel != null) activePanel.SetActive(false);
            workersPanel.SetActive(true);
            activePanel = workersPanel;
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

    public void ToggleStartShiftPanel()
    {
        if (shiftWorkersPanel.activeInHierarchy)
        {
            shiftWorkersPanel.SetActive(false);
        }
        else
        {
            if (activePanel != null) activePanel.SetActive(false);
            shiftWorkersPanel.SetActive(true);
            activePanel = shiftWorkersPanel;
        }
    }

    public void ToggleStatsAfterShiftPanel()
    {
        if(statsAfterShiftPanel.activeInHierarchy) statsAfterShiftPanel.SetActive(false);
    }

    public void ToggleFullStatsPanel()
    {
        if (fullStatsPanel.activeInHierarchy)
        {
            fullStatsPanel.SetActive(false);
        }
        else
        {
            if (activePanel != null) activePanel.SetActive(false);
            fullStatsPanel.SetActive(true);
            activePanel = fullStatsPanel;
        }
    }

    public void ToggleShopSettingsPanel()
    {
        if (shopSettingsPanel.activeInHierarchy)
        {
            shopSettingsPanel.SetActive(false);
        }
        else
        {
            if (activePanel != null) activePanel.SetActive(false);
            shopSettingsPanel.SetActive(true);
            activePanel = shopSettingsPanel;
            TogglePanelAboutShop();
        }
    }

    public void TogglePanelAboutShop()
    {
        if (maxShowCasesPanel.activeInHierarchy)
        {
            maxShowCasesPanel.SetActive(false);
            maxWorkersPanel.SetActive(true);
            shopPageNameTxt.text = "������� �����";
            buyWorkPlaceBtn.onClick.RemoveAllListeners();
            buyWorkPlaceBtn.onClick.AddListener(() =>
            {
                bool bought = shopManager.IncreaseMaxWorkers();
                if (!bought)
                    ShowMessage("������������ �������", maxWorkersPanel);
            });
        }
        else
        {
            maxWorkersPanel.SetActive(false);
            maxShowCasesPanel.SetActive(true);
            shopPageNameTxt.text = "�������";

            buyShowCaseBtn.onClick.RemoveAllListeners();
            buyShowCaseBtn.onClick.AddListener(() =>
            {
                bool bought = shopManager.IncreaseMaxShowCases();
                if (!bought)
                    ShowMessage("������������ �������", maxShowCasesPanel);
            });
        }
    }
    #endregion

    private void Awake()
    {
        ShopManager.sendUpdatedShopInfo += GetUpdatedShopInfo;
        flowersListOnMarketContent.GetComponent<OnEnableEvent>().enabled += GetFlowersPrices;
        flowersListOnShopContent.GetComponent<OnEnableEvent>().enabled += GetShopsFlowersList;
        workersListOnMarketContent.GetComponent<OnEnableEvent>().enabled += GetWorkersOnMarketList;
        hiredWorkersListContent.GetComponent<OnEnableEvent>().enabled += GetHiredWorkersList;
        shiftWorkersListContent.GetComponent<OnEnableEvent>().enabled += GetShiftWorkersList;
        WorkDayManager.statisticsShow += ShowStatisticsAfterShift;
        fullStatsPanel.GetComponent<OnEnableEvent>().enabled += ShowFullStats;
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

        var countInput = buyFlowerPanel.transform.Find("CountInput").GetComponent<TMP_InputField>();
        var buyBtn = buyFlowerPanel.transform.Find("BuyBtn").GetComponent<Button>();

        countInput.onValueChanged.RemoveAllListeners();
        countInput.text = "";
        

        countInput.onValueChanged.AddListener((input) => {
            int count = int.Parse(input);
            float sum = (float)Math.Round(price * count, 2);
            buyFlowerPanel.transform.Find("SumTxt").GetComponent<TMP_Text>().text = $"�����: {sum}";
            buyBtn.onClick.RemoveAllListeners();
            //�������� � ������ ������� ������
            buyBtn.onClick.AddListener(() =>
            {
                var (bought, status) = flowersManager.BuyFlower(flowerName, count, price);
                if (bought)
                {
                }
                else
                {
                    ShowMessage(status, flowersSettingsPanel);
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

                changeFlowerPriceInput.text = "";
                changeFlowerPriceInput.onValueChanged.RemoveAllListeners();
                changeFlowerPriceInput.onValueChanged.AddListener(value =>
                {
                    changeFlowerPriceBtn.onClick.RemoveAllListeners();
                    changeFlowerPriceBtn.onClick.AddListener(() =>
                    {
                        flowersManager.ChangeFlowerPrice(flower.flower_name, float.Parse(value));
                    });
                });

                toggleFlowersOnSaleInput.text = "";

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

    #region ������ ��� ����������� � ��������� ���������� � ������� ���
    private void GetShiftWorkersList()
    {
        var workers = workersManager.GetHiredWorkers();
        RemoveCards(shiftWorkersCards);
        shiftWorkersCards = new();


        workers.FindAll(w => !w.isOnShift).ForEach(worker =>
        {
            GameObject workerCard = Instantiate(shiftWorkersListObject, shiftWorkersListContent.transform);
            shiftWorkersCards.Add(workerCard);

            workerCard.transform.Find("WorkerNameTxt").GetComponent<TMP_Text>().text = worker.name;
            workerCard.transform.Find("WorkerDataTxt").GetComponent<TMP_Text>().text = $"��������� ����������: {worker.motivation}";

            workerCard.transform.Find("ShiftBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                OpenSalaryForWorkerSettingPanel(worker);
            });
        });
    }

    private void OpenSalaryForWorkerSettingPanel(Workers worker)
    {
        salaryForWorkerSettingPanel.SetActive(true);
        salaryForWorkerSettingPanel.transform.Find("CloseBtn").GetComponent<Button>()
            .onClick.AddListener(() => { salaryForWorkerSettingPanel.SetActive(false); });

        salaryForWorkerSettingPanel.transform.Find("NameTxt").GetComponent<TMP_Text>().text = worker.name;

        var salaryInput = salaryForWorkerSettingPanel.transform.Find("HourlySalaryInput").GetComponent<TMP_InputField>();
        var toShiftBtn = salaryForWorkerSettingPanel.transform.Find("ToShiftBtn").GetComponent<Button>();

        salaryInput.text = "";

        salaryInput.onValueChanged.RemoveAllListeners();

        salaryInput.onValueChanged.AddListener((input) =>
        {
            toShiftBtn.onClick.RemoveAllListeners();
            toShiftBtn.onClick.AddListener(() =>
            {
                if(float.Parse(input) >= worker.minimal_hour_salary)
                {
                    var (isShifted, status) = workersManager.SendWorkerToShift(worker);
                    salaryForWorkerSettingPanel.SetActive(false);
                    if (isShifted)
                    {
                        ReloadPanel(shiftWorkersListPanel);
                    }
                    else
                    {
                        ShowMessage(status, shiftWorkersListPanel);
                    }
                    
                }
                else
                {
                    ShowMessage("������������� ������� ������ ��� ����� ����������!", shiftWorkersPanel);
                }
            });
        });
    }
    #endregion

    #region ������ ��� ����������� ���������� ����������

    public void ShowStatisticsAfterShift(StatisticsManager stats)
    {
        textForStats[0].text = stats.FlowersSold.ToString();
        textForStats[1].text = stats.ClientsCount.ToString();
        textForStats[2].text = stats.AverageSatisfaction.ToString();
        textForStats[3].text = stats.MoneyEarned.ToString();

        statsAfterShiftPanel.SetActive(true);
    }

    private void ShowFullStats()
    {
        var stats = shopManager.GetStats();
        var avgSatiscation = stats.Sum(s => s.average_clients_motivation) / stats.Sum(s => s.clients_count);


        textForFullStats[0].text = stats.Sum(s => s.flowers_sold).ToString();
        textForFullStats[1].text = stats.Sum(s => s.clients_count).ToString();
        textForFullStats[2].text = avgSatiscation.ToString();
        textForFullStats[3].text = stats.Sum(s=>s.money_earned).ToString();
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

    private void ShowMessage(string message, GameObject panel)
    {
        messageBanner.transform.Find("MessageTxt").GetComponent<TMP_Text>().text = message;
        Instantiate(messageBanner, panel.transform);
        Debug.Log(message);
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
        ratingTxt.text = $"{shop.rating}";
    }
    #endregion

}
