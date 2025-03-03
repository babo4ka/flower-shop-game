using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject shopSettingsPanel;
    [SerializeField] GameObject marketPanel;
    [SerializeField] DataBaseManager dataBaseManager;

    [SerializeField] GameObject flowersListContent;
    [SerializeField] GameObject flowersListObject;

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

    #endregion

    private void Awake()
    {
        flowersListContent.GetComponent<OnEnableEvent>().enabled += GetFlowersPrices;
    }

    //метод для отображения списка цветков не рынке
    private void GetFlowersPrices()
    {
        List<PopularityStoryWithFlower> stories = dataBaseManager.GetFlowersPrice();

        stories.ForEach(story =>
        {
            GameObject flowerCard = Instantiate(flowersListObject, flowersListContent.transform);
            TMP_Text flowerNameTxt = flowerCard.transform.Find("FlowerNameTxt").GetComponent<TMP_Text>();
            TMP_Text flowerPriceTxt = flowerCard.transform.Find("FlowerPriceTxt").GetComponent<TMP_Text>();

            flowerNameTxt.text = story.name;
            flowerPriceTxt.text = $"{story.market_price * story.popularity_level * story.popularity_coefficient / 10}";
        });
        
    }
}
