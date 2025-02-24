using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject shopSettingsPanel;

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
}
