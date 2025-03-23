using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    //панель с отображением денег
    [SerializeField] TMP_Text cashTxt;
    [SerializeField] TMP_Text daysTxt;

    //основные панели для взаимодействия с магазином
    [SerializeField] GameObject shopSettingsPanel;

    //панель рынка
    [SerializeField] GameObject marketPanel;
    //панели внутри панели рынка
    [SerializeField] GameObject activePanelOnMarket;


    //менеджер базы данных
    [SerializeField] DataBaseManager dataBaseManager;
    //менеджер для работы с цветами
    [SerializeField] FlowersManager flowersManager;
    //менеджер для работы с работниками
    [SerializeField] WorkersManager workersManager;

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
    #endregion

    #region включение отключение элементов интерфейса
    //метод для переключения панели настроек магазина
    public void ToggleShopSettingsPanel()
    {
        
        if(shopSettingsPanel.activeInHierarchy)
        {
            shopSettingsPanel.SetActive(false);
        }
        else
        {
            shopSettingsPanel.SetActive(true);
        }
    }

    //метод для переключения панели рынка
    public void ToggleMarketPanel()
    {
        if(marketPanel.activeInHierarchy)
        {
            marketPanel.SetActive(false);
        }
        else
        {
            marketPanel.SetActive(true);
        }
    }

    public void TogglePanelsInsideMarket(GameObject panel)
    {
        if (activePanelOnMarket != null) activePanelOnMarket.SetActive(false);
        activePanelOnMarket = panel;
        panel.SetActive(true);
    }
    #endregion

    private void Awake()
    {
        ShopManager.sendUpdatedShopInfo += GetUpdatedShopInfo;
        flowersListOnMarketContent.GetComponent<OnEnableEvent>().enabled += GetFlowersPrices;
        flowersListOnShopContent.GetComponent<OnEnableEvent>().enabled += GetShopsFlowersList;
        workersListOnMarketContent.GetComponent<OnEnableEvent>().enabled += GetWorkersOnMarketList;
    }

    #region методы для отображения и изменения информации о цветах
    //метод для отображения списка цветков на рынке
    private void GetFlowersPrices()
    {
        List<FlowersPrice> stories = flowersManager.GetFlowersPrice();
        RemoveFlowerOnMarketCards();
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


        void RemoveFlowerOnMarketCards()
        {
            if(flowersOnMarketCards is not null)
            {
                flowersOnMarketCards.ForEach(card =>
                {
                    Destroy(card);
                });

                flowersOnMarketCards.Clear();
            }
        }
    }

    

    //метод для отображения панели интерфейса для покупки цветов и привязки к кнопкам метода покупки цветов
    private void OpenBuyFlowerPanel(string flowerName, float price)
    {
        buyFlowerPanel.SetActive(true);
        buyFlowerPanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(() => { buyFlowerPanel.SetActive(false); });

        buyFlowerPanel.transform.Find("FlowerNameTxt").GetComponent<TMP_Text>().text = flowerName;
        buyFlowerPanel.transform.Find("PriceTxt").GetComponent<TMP_Text>().text = $"Цена: {price}";

        buyFlowerPanel.transform.Find("CountInput").GetComponent<TMP_InputField>().onValueChanged.RemoveAllListeners();
        buyFlowerPanel.transform.Find("BuyBtn").GetComponent<Button>().onClick.RemoveAllListeners();

        buyFlowerPanel.transform.Find("CountInput").GetComponent<TMP_InputField>().onValueChanged.AddListener((input) => {
            int count = int.Parse(input);
            float sum = (float)Math.Round(price * count, 2);
            buyFlowerPanel.transform.Find("SumTxt").GetComponent<TMP_Text>().text = $"Сумма: {sum}";

            //привязка к кнопке покупки цветов
            buyFlowerPanel.transform.Find("BuyBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                bool bought = flowersManager.BuyFlower(flowerName, count, price);
                if (bought)
                {
                }
                else
                {
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
        RemoveShopFlowerCards();
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
                        flowersManager.ToggleSaleFlowers(flower.flower_name, int.Parse(value), DataBaseManager.ToggleSaleAction.REMOVE);
                    });

                    putFlowerOnSaleBtn.onClick.AddListener(() =>
                    {
                        flowersManager.ToggleSaleFlowers(flower.flower_name, int.Parse(value), DataBaseManager.ToggleSaleAction.PUT);
                    });
                });
            });

        });

        void RemoveShopFlowerCards()
        {
            if(flowersOnShopCards is not null)
            {
                flowersOnShopCards.ForEach(card =>
                {
                    Destroy(card);
                });

                flowersOnShopCards.Clear();
            }
        }
    }


    #endregion


    #region методы для отображения и изменения информации о работниках
    private void GetWorkersOnMarketList()
    {
        var workers = workersManager.GetAvailableWorkers();
        RemoveWorkersCards();
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

            });
        });


        void RemoveWorkersCards()
        {
            if(workersOnMarketCards is not null)
            {
                workersOnMarketCards.ForEach(card => { Destroy(card); });
                workersOnMarketCards.Clear();
            }
        }
    }
    #endregion

    #region методы для отображения информации магазина
    private void GetUpdatedShopInfo(Shop shop)
    {
        cashTxt.text = $"{shop.cash} $";
        daysTxt.text = $"Дней прошло: {shop.daysGone}";
    }
    #endregion

}
