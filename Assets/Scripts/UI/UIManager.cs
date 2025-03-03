using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject shopSettingsPanel;
    [SerializeField] DataBaseManager dataBaseManager;

    [SerializeField] GameObject flowersListContent;
    [SerializeField] GameObject flowersListObject;

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

    private void Awake()
    {
       GetFlowersPrices();
    }


    private void GetFlowersPrices()
    {
        Instantiate(flowersListObject, flowersListContent.transform);
    }
}
