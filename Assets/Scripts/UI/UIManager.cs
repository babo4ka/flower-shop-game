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
    //сущность магазина
    private Shop shop;
    //список точек попул€рности
    private List<GameObject> chartDots;
    #endregion

    #region переменные из редактора
    //панель с отображением денег
    [SerializeField] TMP_Text cashTxt;

    //основные панели дл€ взаимодействи€ с магазином
    [SerializeField] GameObject shopSettingsPanel;
    [SerializeField] GameObject marketPanel;

    //менеджер базы данных
    [SerializeField] DataBaseManager dataBaseManager;

    //панели дл€ рынка с цветами
    [SerializeField] GameObject flowersListContent;
    [SerializeField] GameObject flowersListObject;

    //график попул€рности
    //панель дл€ графика
    [SerializeField] GameObject popularityChartPanel;
    //график попул€рности
    [SerializeField] GameObject popularityChart;
    //название цветка на графике
    [SerializeField] TMP_Text flowerNameOnChart;
    //префаб точки дл€ графика
    [SerializeField] GameObject chartDot;
    

    //панель покупки цветов
    //название цветка
    [SerializeField] GameObject flowerPriceText;
    //кнопка дл€ открыти€ панели дл€ покупки цветов
    [SerializeField] GameObject openBuyFlowerPanelBtn;
    //панель с интерфейсом дл€ покупки цветов
    [SerializeField] GameObject buyFlowerPanel;
    #endregion

    #region включение отключение элементов интерфейса
    //метод дл€ переключени€ панели настроек магазина
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

    //метод дл€ переключени€ панели рынка
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

    #endregion

    private void Awake()
    {
        dataBaseManager.usi += UpdateShopData;
        flowersListContent.GetComponent<OnEnableEvent>().enabled += GetFlowersPrices;
    }

    #region методы дл€ отображени€ и изменени€ информации о цветах
    //метод дл€ отображени€ списка цветков на рынке
    private void GetFlowersPrices()
    {
        List<PopularityStoryWithFlower> stories = dataBaseManager.GetFlowersPrice();

        stories.ForEach(story =>
        {
            GameObject flowerCard = Instantiate(flowersListObject, flowersListContent.transform);
            TMP_Text flowerNameTxt = flowerCard.transform.Find("FlowerNameTxt").GetComponent<TMP_Text>();
            TMP_Text flowerPriceTxt = flowerCard.transform.Find("FlowerPriceTxt").GetComponent<TMP_Text>();

            flowerNameTxt.text = story.flower_name;
            flowerPriceTxt.text = $"{(float)Math.Round(story.market_price * story.popularity_level * story.popularity_coefficient / 10, 2)}";

            List<PopularityStory> flowerPopularityStory = dataBaseManager.GetFlowerPopularityStory(story.flower_name);


            flowerCard.GetComponent<Button>().onClick.AddListener(() => {
                ShowFlowerInfo(story.flower_name, flowerPopularityStory);
                if (!flowerPriceText.activeInHierarchy) flowerPriceText.SetActive(true);
                if (!openBuyFlowerPanelBtn.activeInHierarchy) openBuyFlowerPanelBtn.SetActive(true);

                float price = (float)Math.Round(story.market_price * (story.popularity_level * story.popularity_coefficient / 10), 2);
                flowerPriceText.GetComponent<TMP_Text>().text = $"÷ена: {price}";
                openBuyFlowerPanelBtn.GetComponent<Button>().onClick.RemoveAllListeners();
                openBuyFlowerPanelBtn.GetComponent<Button>().onClick.AddListener(() => OpenBuyFlowerPanel(story.flower_name, price));
            });
            
        });
        
    }

    //метод дл€ отображени€ панели интерфейса д€л покупки цветов и прив€зки к кнопкам метода покупки цветов
    private void OpenBuyFlowerPanel(string flowerName, float price)
    {
        buyFlowerPanel.SetActive(true);
        buyFlowerPanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(() => { buyFlowerPanel.SetActive(false); });

        buyFlowerPanel.transform.Find("FlowerNameTxt").GetComponent<TMP_Text>().text = flowerName;
        buyFlowerPanel.transform.Find("PriceTxt").GetComponent<TMP_Text>().text = $"÷ена: {price}";

        buyFlowerPanel.transform.Find("CountInput").GetComponent<TMP_InputField>().onValueChanged.RemoveAllListeners();
        buyFlowerPanel.transform.Find("BuyBtn").GetComponent<Button>().onClick.RemoveAllListeners();

        buyFlowerPanel.transform.Find("CountInput").GetComponent<TMP_InputField>().onValueChanged.AddListener((input) => {
            int count = int.Parse(input);
            float sum = (float)Math.Round(price * count, 2);
            buyFlowerPanel.transform.Find("SumTxt").GetComponent<TMP_Text>().text = $"—умма: {sum}";

            buyFlowerPanel.transform.Find("BuyBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log($"Here {flowerName}");
                //сделать проверку на наличие денег
                bool bought = dataBaseManager.BuyFlower(flowerName, count, price);
                Debug.Log(bought);
            });
        });
    }

    //метод дл€ отображени€ информации о цветке и его графика попул€рности
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
    #endregion

    private void UpdateShopData(Shop shop)
    {
        this.shop = shop;
        cashTxt.text = $"{shop.cash} $";
    }

}
