using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region ���������� ����������
    //������ ����� ������������
    private List<GameObject> chartDots;
    private List<GameObject> chartLines;
    #endregion

    #region ���������� �� ���������
    [Header("��������")]
    //canvas
    [SerializeField] GameObject canvas;
    //�������� ������
    [SerializeField] GameObject activePanel;

    //������� ������
    [SerializeField] GameObject mainBtnsPanel;

    //������ ��� ����������� �������� �������� ���
    [Header("������ ��� ����������� �������� �������� ���")]
    [SerializeField] GameObject workImiatationPanel;

    [Header("������ �������")]
    [SerializeField] GameObject eventPanel;
    [SerializeField] TMP_Text eventName;
    [SerializeField] TMP_Text eventPrice;
    [SerializeField] Button solveEventBtn;

    //����� ������ ����� ��������� �� �������
    [Header("����� ������ ����� ��������� �� �������")]
    [SerializeField] GameObject flowersToChoosePanel;
    [SerializeField] GameObject flowersToChooseContent;
    List<GameObject> flowersToChooseCards;
    [SerializeField] GameObject flowerToChooseObject;
    [SerializeField] GameObject flowerToChooseImage;

    //������ � ������������ �����
    [Header("������ � ������������ �����")]
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


    [Header("���������")]
    //�������� ���� ������
    [SerializeField] DataBaseManager dataBaseManager;
    //�������� ��� ������ � �������
    [SerializeField] FlowersManager flowersManager;
    //�������� ��� ������ � �����������
    [SerializeField] WorkersManager workersManager;
    //�������� ��� ������ � ���������
    [SerializeField] ShopManager shopManager;

    //������ ��� ����� � �������
    [Header("������ ��� ����� � �������")]
    [SerializeField] GameObject flowersListOnMarketContent;
    [SerializeField] GameObject flowersListOnMarketObject;
    List<GameObject> flowersOnMarketCards;

    //������ ��� ����� � �����������
    [Header("������ ��� ����� � �����������")]
    [SerializeField] GameObject workersListOnMarketContent;
    [SerializeField] GameObject workersListOnMarketObject;
    List<GameObject> workersOnMarketCards;

    //������ ��� ������ � ��������
    [Header("������ ��� ������ � ��������")]
    [SerializeField] GameObject flowersListOnShopContent;
    [SerializeField] GameObject flowersListOnShopObject;
    List<GameObject> flowersOnShopCards;

    //������ ��� ������� ����������
    [Header("������ ��� ������� ����������")]
    [SerializeField] GameObject hiredWorkersListContent;
    [SerializeField] GameObject hiredWorkersListObject;
    List<GameObject> hiredWorkersCards;

    //������ ��� �������� ��������
    [Header("������ ��� �������� ��������")]
    [SerializeField] GameObject shopSettingsPanel;
    [SerializeField] GameObject maxShowCasesPanel;
    [SerializeField] GameObject maxWorkersPanel;
    //�������� ��������
    [SerializeField] TMP_Text shopPageNameTxt;
    //������
    [SerializeField] Button buyShowCaseBtn;
    [SerializeField] Button buyWorkPlaceBtn;
    //����������
    [SerializeField] TMP_Text maxShowCasesTxt;
    [SerializeField] TMP_Text showCasePriceTxt;
    [SerializeField] TMP_Text maxWorkersTxt;
    [SerializeField] TMP_Text workplacePriceTxt;


    //������ ��� ���������� �� �����
    [Header("������ ��� ���������� �� �����")]
    [SerializeField] TMP_Text shiftWorkersCountTxt;
    [SerializeField] GameObject shiftWorkersListContent;
    [SerializeField] GameObject shiftWorkersListObject;
    List<GameObject> shiftWorkersCards;
    //������ � ����������� ������ ��������� �� �����
    [SerializeField] GameObject salaryForWorkerSettingPanel;

    //������ ������������
    [Header("������ ������������")]
    //������ ��� �������
    [SerializeField] GameObject popularityChartPanel;
    //������ ������������
    [SerializeField] GameObject popularityChart;
    //�������� ������ �� �������
    [SerializeField] TMP_Text flowerNameOnChart;
    //������ ����� ��� �������
    [SerializeField] GameObject chartDot;


    //������ ������� ������
    [Header("������ ������� ������")]
    //���� ������
    [SerializeField] GameObject flowerPriceText;
    //������ ��� �������� ������ ��� ������� ������
    [SerializeField] GameObject openBuyFlowerPanelBtn;
    //������ � ����������� ��� ������� ������
    [SerializeField] GameObject buyFlowerPanel;

    //������ ������ � ��������
    [Header("������ ������ � ��������")]
    //������ ��� ��������� ������ ������
    [SerializeField] Button changeFlowerPriceBtn;
    [SerializeField] Button deleteFlowerFromSaleBtn;
    [SerializeField] Button putFlowerOnSaleBtn;

    //����������� ���������� � �������
    [Header("����������� ���������� � ������")]
    [SerializeField] TMP_Text flowerNameOnShopTxt;
    [SerializeField] TMP_Text flowerOnShopPriceTxt;
    [SerializeField] TMP_Text countFlowersOnSaleTxt;
    [SerializeField] TMP_Text countFlowersInStockTxt;
    //���� ��� �������� ��������� � ���������� � ������
    [SerializeField] TMP_InputField changeFlowerPriceInput;
    [SerializeField] TMP_InputField toggleFlowersOnSaleInput;

    //����������
    [Header("����������")]
    [SerializeField] GameObject statsAfterShiftPanel;
    [SerializeField] TMP_Text[] textForStats;

    //����������
    [SerializeField] GameObject fullStatsPanel;
    [SerializeField] TMP_Text[] textForFullStats;

    //������ ��� ����������� ����������
    [SerializeField] GameObject messageBanner;

    public static Action dayContinue;
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
        }
        else
        {
            maxWorkersPanel.SetActive(false);
            maxShowCasesPanel.SetActive(true);
            shopPageNameTxt.text = "�������";
        }
    }


    public void ToggleMainButtons(string action)
    {
        Debug.Log(action);
        if (action == "show")
            mainBtnsPanel.GetComponent<Animator>().SetBool("hide", false);
        else
            mainBtnsPanel.GetComponent<Animator>().SetBool("hide", true);

        ToggleFlowersToChoosePanel(action);
    }

    private void ToggleFlowersToChoosePanel(string action)
    {
        if (action == "show")
        {
            RemoveCards(flowersToChooseCards);
            flowersToChoosePanel.SetActive(false);
            flowerToChooseImage.SetActive(false);
        }
        else
        {
            flowersToChooseCards = new();
            flowersToChoosePanel.SetActive(true);
            var shopFlowers = flowersManager.GetShopFlowers();
            shopFlowers.ForEach(sf =>
            {
                GameObject card = Instantiate(flowerToChooseObject, flowersToChooseContent.transform);
                card.transform.Find("FlowerNameTxt").GetComponent<TMP_Text>().text = sf.flower_name;
                flowersToChooseCards.Add(card);
                card.GetComponent<Button>().onClick.RemoveAllListeners();
                card.GetComponent<Button>().onClick.AddListener(() =>
                {
                    flowerToChooseImage.GetComponent<DragNDrop>().FlowerName = sf.flower_name;
                });
                flowerToChooseImage.SetActive(true);
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
        maxWorkersPanel.GetComponent<OnEnableEvent>().enabled += WorkPlacesInfoShow;
        maxShowCasesPanel.GetComponent<OnEnableEvent>().enabled += ShowCaseInfoShow;
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
        chartLines = new();


        RectTransform chartRectTransform = popularityChart.GetComponent<RectTransform>();

        float xPos = chartRectTransform.rect.xMin + 5f;
        float step = chartRectTransform.rect.width / popularityStory.Count;
        float startY = chartRectTransform.rect.yMin;
        float height = chartRectTransform.rect.height;

        Vector2 lastDotPos = Vector2.zero;
        popularityStory.ForEach(story =>
        {
            GameObject dot = Instantiate(chartDot, chartRectTransform);
            float yPos = startY + (story.popularity_level / 100 * height);
            dot.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
            xPos += step;

            chartDots.Add(dot);

            if(lastDotPos != Vector2.zero)
            {
                GameObject conn = new("dotConn", typeof(Image));
                conn.layer = 5;
                conn.GetComponent<Image>().color = Color.black;
                conn.transform.SetParent(chartRectTransform, false);
                RectTransform rt = conn.GetComponent<RectTransform>();
                Vector2 dir = (dot.GetComponent<RectTransform>().anchoredPosition - lastDotPos).normalized;
                float distance = Vector2.Distance(lastDotPos, dot.GetComponent<RectTransform>().anchoredPosition);
                rt.anchorMin = new Vector2(.5f, .5f);
                rt.anchorMax = new Vector2(.5f, .5f);
                rt.sizeDelta = new Vector2(distance, 2f);
                rt.anchoredPosition = lastDotPos + dir * distance * .5f;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                rt.localEulerAngles = new Vector3(0, 0, angle);

                chartLines.Add(conn);

            }
            lastDotPos = dot.GetComponent<RectTransform>().anchoredPosition;
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

            if(chartLines is not null)
            {
                chartLines.ForEach(line =>
                {
                    Destroy(line);
                });
                chartLines.Clear();
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
        shiftWorkersCountTxt.text = $"�������� �� ������ {shopManager.MaxWorkers()} ���������� �� �����";
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
                    var (isShifted, status) = workersManager.SendWorkerToShift(worker, float.Parse(input));
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

    public void OpenWorkImitationPanel()
    {
        Instantiate(workImiatationPanel, canvas.transform);
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

    #region ������ ��� ����������� ���������� � ��������
    private void ShowCaseInfoShow()
    {
        maxShowCasesTxt.text = $"������ �������: {shopManager.OpenedShowCases()}";
        showCasePriceTxt.text = $"���� ������� {shopManager.ShowCasePrice}";

        buyShowCaseBtn.onClick.RemoveAllListeners();
        buyShowCaseBtn.onClick.AddListener(() =>
        {
            bool bought = shopManager.IncreaseMaxShowCases();
            if (!bought)
                ShowMessage("������������ �������", maxShowCasesPanel);
            else
                ReloadPanel(maxShowCasesPanel);
        });
    }

    private void WorkPlacesInfoShow()
    {
        maxWorkersTxt.text = $"������� ����: {shopManager.MaxWorkers()}";
        workplacePriceTxt.text = $"���� �������� �����: {shopManager.WorkPlacePrice}";

        buyWorkPlaceBtn.onClick.RemoveAllListeners();
        buyWorkPlaceBtn.onClick.AddListener(() =>
        {
            bool bought = shopManager.IncreaseMaxWorkers();
            if (!bought)
                ShowMessage("������������ �������", maxWorkersPanel);
            else
                ReloadPanel(maxWorkersPanel);
        });
    }
    #endregion

    #region ������ ��� ������ � ���������
    public void ToggleEventPanel(string name, float price)
    {
        eventPanel.SetActive(true);

        eventName.text = name;
        eventPrice.text = price.ToString();

        solveEventBtn.onClick.RemoveAllListeners();
        solveEventBtn.onClick.AddListener(() =>
        {
            Debug.Log($"solve {name} for {price}");
            shopManager.SpendCash(price);
            eventPanel.SetActive(false);
            dayContinue?.Invoke();
        });
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

    public void ShowMessage(string message, GameObject panel)
    {
        messageBanner.transform.Find("MessageTxt").GetComponent<TMP_Text>().text = message;
        Instantiate(messageBanner, panel==null?canvas.transform:panel.transform);
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
