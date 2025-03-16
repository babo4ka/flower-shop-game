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
    #region ���������� ����������
    //�������� ��������
    private Shop shop;
    //������ ����� ������������
    private List<GameObject> chartDots;
    #endregion

    #region ���������� �� ���������
    //������ � ������������ �����
    [SerializeField] TMP_Text cashTxt;

    //�������� ������ ��� �������������� � ���������
    [SerializeField] GameObject shopSettingsPanel;
    [SerializeField] GameObject marketPanel;

    //�������� ���� ������
    [SerializeField] DataBaseManager dataBaseManager;

    //������ ��� ����� � �������
    [SerializeField] GameObject flowersListContent;
    [SerializeField] GameObject flowersListObject;

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
    //�������� ������
    [SerializeField] GameObject flowerPriceText;
    //������ ��� �������� ������ ��� ������� ������
    [SerializeField] GameObject openBuyFlowerPanelBtn;
    //������ � ����������� ��� ������� ������
    [SerializeField] GameObject buyFlowerPanel;
    #endregion

    #region ��������� ���������� ��������� ����������
    //����� ��� ������������ ������ �������� ��������
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

    //����� ��� ������������ ������ �����
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

    #region ������ ��� ����������� � ��������� ���������� � ������
    //����� ��� ����������� ������ ������� �� �����
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

            buyFlowerPanel.transform.Find("BuyBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log($"Here {flowerName}");
                //������� �������� �� ������� �����
                bool bought = dataBaseManager.BuyFlower(flowerName, count, price);
                Debug.Log(bought);
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
    #endregion

    private void UpdateShopData(Shop shop)
    {
        this.shop = shop;
        cashTxt.text = $"{shop.cash} $";
    }

}
