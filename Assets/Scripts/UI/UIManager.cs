using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //основные панели
    [SerializeField] GameObject shopSettingsPanel;
    [SerializeField] GameObject marketPanel;

    //база данных
    [SerializeField] DataBaseManager dataBaseManager;

    //панели дл€ рынка с цветами
    [SerializeField] GameObject flowersListContent;
    [SerializeField] GameObject flowersListObject;

    //график попул€рности
    [SerializeField] GameObject popularityChartPanel;
    [SerializeField] GameObject popularityChart;
    [SerializeField] TMP_Text flowerNameOnChart;
    [SerializeField] GameObject chartDot;
    private List<GameObject> chartDots;

    //панель покупки цветов
    [SerializeField] GameObject flowerPriceText;
    [SerializeField] GameObject openBuyFlowerPanelBtn;
    [SerializeField] GameObject buyFlowerPanel;

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
        flowersListContent.GetComponent<OnEnableEvent>().enabled += GetFlowersPrices;
    }

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


}
