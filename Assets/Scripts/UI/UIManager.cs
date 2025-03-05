using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //�������� ������
    [SerializeField] GameObject shopSettingsPanel;
    [SerializeField] GameObject marketPanel;

    //���� ������
    [SerializeField] DataBaseManager dataBaseManager;

    //������ ��� ����� � �������
    [SerializeField] GameObject flowersListContent;
    [SerializeField] GameObject flowersListObject;

    [SerializeField] GameObject popularityChart;
    [SerializeField] TMP_Text flowerNameOnChart;

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
        flowersListContent.GetComponent<OnEnableEvent>().enabled += GetFlowersPrices;
    }

    //����� ��� ����������� ������ ������� �� �����
    private void GetFlowersPrices()
    {
        List<PopularityStoryWithFlower> stories = dataBaseManager.GetFlowersPrice();

        stories.ForEach(story =>
        {
            GameObject flowerCard = Instantiate(flowersListObject, flowersListContent.transform);
            TMP_Text flowerNameTxt = flowerCard.transform.Find("FlowerNameTxt").GetComponent<TMP_Text>();
            TMP_Text flowerPriceTxt = flowerCard.transform.Find("FlowerPriceTxt").GetComponent<TMP_Text>();

            flowerNameTxt.text = story.name;
            flowerPriceTxt.text = $"{(float)Math.Round(story.market_price * story.popularity_level * story.popularity_coefficient / 10, 2)}";

            List<PopularityStory> flowerPopularityStory = dataBaseManager.GetFlowerPopularityStory(story.name);


            flowerCard.GetComponent<Button>().onClick.AddListener(()=> ShowFlowerInfo(story.name, flowerPopularityStory));
        });
        
    }


    private void ShowFlowerInfo(string flowerName, List<PopularityStory> popularityStory)
    {
        if (!popularityChart.activeInHierarchy) popularityChart.SetActive(true);

        flowerNameOnChart.text = flowerName;
    }
}
