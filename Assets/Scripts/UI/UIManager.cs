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
    #region внутренние переменные
    //список точек популярности
    private List<GameObject> chartDots;
    #endregion

    #region переменные из редактора
    //активная панель
    [SerializeField] GameObject activePanel;

    //панель с отображением денег
    [SerializeField] TMP_Text cashTxt;
    [SerializeField] TMP_Text daysTxt;
    [SerializeField] TMP_Text ratingTxt;

    //основные панели для взаимодействия с цветами
    [SerializeField] GameObject flowersSettingsPanel;
    //панели внутри панели с цветами
    [SerializeField] GameObject shopFlowersPanel;
    [SerializeField] GameObject marketFlowersPanel;
    [SerializeField] TMP_Text flowersPanelName;

    //панель для данных работников
    [SerializeField] GameObject workersPanel;
    //панели внутри панели персонала
    [SerializeField] GameObject workersToHirePanel;
    [SerializeField] GameObject hiredWorkersPanel;
    [SerializeField] TMP_Text workersPanelName;

    //панели для начала рабочего дня
    [SerializeField] GameObject shiftWorkersPanel;
    [SerializeField] GameObject shiftWorkersListPanel;


    //менеджер базы данных
    [SerializeField] DataBaseManager dataBaseManager;
    //менеджер для работы с цветами
    [SerializeField] FlowersManager flowersManager;
    //менеджер для работы с работниками
    [SerializeField] WorkersManager workersManager;
    //менеджер для работы с магазином
    [SerializeField] ShopManager shopManager;

    //панели для рынка с цветами
    [SerializeField] GameObject flowersListOnMarketContent;
    [SerializeField] GameObject flowersListOnMarketObject;
    List<GameObject> flowersOnMarketCards;

    //панели для рынка с работниками
    [SerializeField] GameObject workersListOnMarketContent;
    [SerializeField] GameObject workersListOnMarketObject;
    List<GameObject> workersOnMarketCards;

    //панели для цветов в магазине
    [SerializeField] GameObject flowersListOnShopContent;
    [SerializeField] GameObject flowersListOnShopObject;
    List<GameObject> flowersOnShopCards;

    //панели для нанятых работников
    [SerializeField] GameObject hiredWorkersListContent;
    [SerializeField] GameObject hiredWorkersListObject;
    List<GameObject> hiredWorkersCards;

    //панели для настроек магазина
    [SerializeField] GameObject shopSettingsPanel;
    [SerializeField] GameObject maxShowCasesPanel;
    [SerializeField] GameObject maxWorkersPanel;
    [SerializeField] TMP_Text shopPageNameTxt;
    [SerializeField] Button buyShowCaseBtn;
    [SerializeField] Button buyWorkPlaceBtn;


    //панели для работников на смену
    [SerializeField] GameObject shiftWorkersListContent;
    [SerializeField] GameObject shiftWorkersListObject;
    List<GameObject> shiftWorkersCards;
    //панель с интерфейсом вывода работника на смену
    [SerializeField] GameObject salaryForWorkerSettingPanel;

    //график популярности
    //панель для графика
    [SerializeField] GameObject popularityChartPanel;
    //график популярности
    [SerializeField] GameObject popularityChart;
    //название цветка на графике
    [SerializeField] TMP_Text flowerNameOnChart;
    //префаб точки для графика
    [SerializeField] GameObject chartDot;
    

    //панель покупки цветов
    //цена цветка
    [SerializeField] GameObject flowerPriceText;
    //кнопка для открытия панели для покупки цветов
    [SerializeField] GameObject openBuyFlowerPanelBtn;
    //панель с интерфейсом для покупки цветов
    [SerializeField] GameObject buyFlowerPanel;

    //панель цветка в магазине
    //кнопки для изменения данных цветка
    [SerializeField] Button changeFlowerPriceBtn;
    [SerializeField] Button deleteFlowerFromSaleBtn;
    [SerializeField] Button putFlowerOnSaleBtn;

    //отображение информации о цветках
    [SerializeField] TMP_Text flowerNameOnShopTxt;
    [SerializeField] TMP_Text flowerOnShopPriceTxt;
    [SerializeField] TMP_Text countFlowersOnSaleTxt;
    [SerializeField] TMP_Text countFlowersInStockTxt;
    //поля для внесения изменений в инфомрацию о цветах
    [SerializeField] TMP_InputField changeFlowerPriceInput;
    [SerializeField] TMP_InputField toggleFlowersOnSaleInput;

    //статистика
    [SerializeField] GameObject statsAfterShiftPanel;
    [SerializeField] TMP_Text[] textForStats;

    //статистика
    [SerializeField] GameObject fullStatsPanel;
    [SerializeField] TMP_Text[] textForFullStats;

    //баннер для отображения информации
    [SerializeField] GameObject messageBanner;
    #endregion

    #region включение отключение элементов интерфейса
    //метод для переключения панели цветов
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
            flowersPanelName.text = "рынок";
        }
        else
        {
            shopFlowersPanel.SetActive(true);
            marketFlowersPanel.SetActive(false);
            flowersPanelName.text = "склад";
        }
    }

    //метод для переключения панели персонала
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
            workersPanelName.text = "рабочие";
        }else{
            workersToHirePanel.SetActive(true);
            hiredWorkersPanel.SetActive(false);
            workersPanelName.text = "биржа";
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
            shopPageNameTxt.text = "рабочие места";
            buyWorkPlaceBtn.onClick.RemoveAllListeners();
            buyWorkPlaceBtn.onClick.AddListener(() =>
            {
                bool bought = shopManager.IncreaseMaxWorkers();
                if (!bought)
                    ShowMessage("недостаточно средств", maxWorkersPanel);
            });
        }
        else
        {
            maxWorkersPanel.SetActive(false);
            maxShowCasesPanel.SetActive(true);
            shopPageNameTxt.text = "витрины";

            buyShowCaseBtn.onClick.RemoveAllListeners();
            buyShowCaseBtn.onClick.AddListener(() =>
            {
                bool bought = shopManager.IncreaseMaxShowCases();
                if (!bought)
                    ShowMessage("недостаточно средств", maxShowCasesPanel);
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

    #region методы для отображения и изменения информации о цветах
    //метод для отображения списка цветков на рынке
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
                flowerPriceText.GetComponent<TMP_Text>().text = $"Цена: {price}";
                openBuyFlowerPanelBtn.GetComponent<Button>().onClick.RemoveAllListeners();
                openBuyFlowerPanelBtn.GetComponent<Button>().onClick.AddListener(() => OpenBuyFlowerPanel(story.flower_name, price));
            });
            
        });

    }

    

    //метод для отображения панели интерфейса для покупки цветов и привязки к кнопкам метода покупки цветов
    private void OpenBuyFlowerPanel(string flowerName, float price)
    {
        buyFlowerPanel.SetActive(true);
        buyFlowerPanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(() => { buyFlowerPanel.SetActive(false); });

        buyFlowerPanel.transform.Find("FlowerNameTxt").GetComponent<TMP_Text>().text = flowerName;
        buyFlowerPanel.transform.Find("PriceTxt").GetComponent<TMP_Text>().text = $"Цена: {price}";

        var countInput = buyFlowerPanel.transform.Find("CountInput").GetComponent<TMP_InputField>();
        var buyBtn = buyFlowerPanel.transform.Find("BuyBtn").GetComponent<Button>();

        countInput.onValueChanged.RemoveAllListeners();
        countInput.text = "";
        

        countInput.onValueChanged.AddListener((input) => {
            int count = int.Parse(input);
            float sum = (float)Math.Round(price * count, 2);
            buyFlowerPanel.transform.Find("SumTxt").GetComponent<TMP_Text>().text = $"Сумма: {sum}";
            buyBtn.onClick.RemoveAllListeners();
            //привязка к кнопке покупки цветов
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

    //метод для отображения информации о цветке и его графика популярности
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
                countFlowersOnSaleTxt.text = $"Количество в продаже: {flower.count_on_sale}";
                countFlowersInStockTxt.text = $"Количество на складе: {flower.count_in_stock}";

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



    #region методы для отображения и изменения информации о работниках
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
            workerCard.transform.Find("WorkerDataTxt").GetComponent<TMP_Text>().text = $@"Минимальный рейтинг магазина: {worker.minimal_shop_rating}
                                                                                        Минимальная ставка в час: {worker.minimal_hour_salary}";

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
            workerCard.transform.Find("WorkerDataTxt").GetComponent<TMP_Text>().text = $@"Мотивация сотрудника: {worker.motivation}
                                                                                        Минимальная ставка в час: {worker.minimal_hour_salary}";

            workerCard.transform.Find("FireBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                workersManager.FireWorker(worker);
                ReloadPanel(hiredWorkersPanel);
            });
        });
    }
    #endregion

    #region методы для отображения и изменения информации о рабочем дне
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
            workerCard.transform.Find("WorkerDataTxt").GetComponent<TMP_Text>().text = $"Мотивация сотрудника: {worker.motivation}";

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
                    ShowMessage("Недостаточная часовая ставка для этого сотрудника!", shiftWorkersPanel);
                }
            });
        });
    }
    #endregion

    #region методы для отображения информации статистики

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

    #region методы для отображения информации магазина
    private void GetUpdatedShopInfo(Shop shop)
    {
        cashTxt.text = $"{shop.cash} $";
        daysTxt.text = $"Дней прошло: {shop.daysGone}";
        ratingTxt.text = $"{shop.rating}";
    }
    #endregion

}
