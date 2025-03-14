using UnityEngine;
using SQLite;
using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

public class DataBaseManager: MonoBehaviour
{
    private SQLiteConnection _dbConnection;

    void Start()
    {
        // ���� � ���� ������
        string databasePath = Application.dataPath + "/DataBase/DB/gameDatabase.db";

        // �������� ����������� � ���� ������
        _dbConnection = new SQLiteConnection(databasePath);

        CreateDataBase();
        //GetFlowersDataToCheck();
        //CreateInitialPopularityPatterns();
        //CreateInitialFlowers();
        //GetFlowersPriceToCheck();
    }


    private void CreateDataBase()
    {
        //�������� ������ � ��
        _dbConnection.CreateTable<Shop>();
        _dbConnection.CreateTable<Credits>();
        _dbConnection.CreateTable<Workers>();
        _dbConnection.CreateTable<PopularityPatterns>();
        _dbConnection.CreateTable<Flowers>();
        _dbConnection.CreateTable<ShopFlowers>();
        _dbConnection.CreateTable<EventTypes>();
        _dbConnection.CreateTable<WorkDays>();
        _dbConnection.CreateTable<EventsHappen>();
        _dbConnection.CreateTable<PopularityStory>();
    }


    #region ��������� ������ �������� ������
    //��������� ����� ��� ��������� ������ ������ � �����������
    private void GetFlowersDataToCheck()
    {
        var flowersList2 = _dbConnection.Query<FlowersWithPattern>(
            @"select flowers.id as flower_id, flowers.name, popularity_patterns.pattern as popularity_pattern, flowers.popularity_coefficient, flowers.market_price, flowers.noise 
            from flowers join popularity_patterns on flowers.popularity_pattern = popularity_patterns.Id"
            );
        
        flowersList2.ForEach(f =>
        {
            Debug.Log($"{f.name} {f.popularity_pattern} {f.popularity_coefficient} {f.market_price} {f.noise}");

            List<float> storyPatternAsFloat = new();
            var story = f.popularity_pattern.Split("-").ToList();
            story.ForEach(it => { storyPatternAsFloat.Add(float.Parse(it, CultureInfo.InvariantCulture.NumberFormat)); });

            storyPatternAsFloat.ForEach(s =>
            {
                var sInT = new PopularityStory { flower_id = f.flower_id, popularity_level = s };
                _dbConnection.Insert(sInT);
            });
        });
    }



    private void GetFlowersPriceToCheck()
    {
        List<FlowerIds> flowersIds = _dbConnection.Query<FlowerIds>("select flowers.id from flowers");

        flowersIds.ForEach(fId =>
        {
            string query = $@"select flowers.name, popularity_story.popularity_level, flowers.popularity_coefficient, flowers.market_price 
                            from flowers
                            join popularity_story on popularity_story.flower_id = {fId.id} and 
                            flowers.id = {fId.id} order by popularity_story.id desc limit 1;";

            var pswf = _dbConnection.Query<PopularityStoryWithFlower>(query).First();

            Debug.Log($"{pswf.name} price = {pswf.market_price * (pswf.popularity_level * pswf.popularity_coefficient / 10)}");
        });
    }

    #endregion

    #region ��������� ������ ��������
    //��������� ����� ��� �������� ������� ��������� ������������ ������
    private void CreateInitialPopularityPatterns() 
    {
        var patternsList = new List<PopularityPatterns>();
        patternsList.Add(new PopularityPatterns { pattern = "45.12-47.34-45.67-49.01-40.50-57.89-42.34-51.23-49.78-50.12-48.34-46.78-52.01-55.12-53.45" });
        patternsList.Add(new PopularityPatterns { pattern = "50.12-48.34-46.78-52.01-55.12-47.89-49.01-53.45-40.12-49.34-50.67-54.89-42.34-51.23-48.90" });
        patternsList.Add(new PopularityPatterns { pattern = "55.12-47.89-49.01-53.45-40.12-49.34-50.67-54.89-42.34-51.23-48.90-57.12-45.12-47.34-45.67" });
        patternsList.Add(new PopularityPatterns { pattern = "40.12-49.34-50.67-54.89-42.34-51.23-48.90-57.12-45.12-47.34-45.67-49.01-50.12-48.34-46.78" });
        patternsList.Add(new PopularityPatterns { pattern = "42.34-51.23-48.90-57.12-45.12-47.34-45.67-49.01-50.12-48.34-46.78-52.01-55.12-47.89-49.01" });

        foreach (var p in patternsList)
        {
            _dbConnection.Insert(p);
        }
    }

    //��������� ����� ��� �������� ���������� ����� ������
    private void CreateInitialFlowers()
    {
        var flowersList = new List<Flowers>();
        flowersList.Add(new Flowers { name = "����", popularity_pattern = 3, popularity_coefficient = 1.14f, market_price = 50.0f, noise = 0.15f });
        flowersList.Add(new Flowers { name = "�������", popularity_pattern = 2, popularity_coefficient = 2.45f, market_price = 35.5f, noise = 0.22f });
        flowersList.Add(new Flowers { name = "������", popularity_pattern = 5, popularity_coefficient = 1.87f, market_price = 45.3f, noise = 0.18f });
        flowersList.Add(new Flowers { name = "����", popularity_pattern = 1, popularity_coefficient = 3.12f, market_price = 120.7f, noise = 0.55f });
        flowersList.Add(new Flowers { name = "�������", popularity_pattern = 4, popularity_coefficient = 2.78f, market_price = 90.2f, noise = 0.42f });
        flowersList.Add(new Flowers { name = "��������", popularity_pattern = 3, popularity_coefficient = 1.56f, market_price = 25.8f, noise = 0.30f });
        flowersList.Add(new Flowers { name = "�����", popularity_pattern = 2, popularity_coefficient = 2.90f, market_price = 60.4f, noise = 0.25f });
        flowersList.Add(new Flowers { name = "�������", popularity_pattern = 5, popularity_coefficient = 1.23f, market_price = 40.6f, noise = 0.12f });
        flowersList.Add(new Flowers { name = "����������", popularity_pattern = 1, popularity_coefficient = 3.45f, market_price = 85.9f, noise = 0.48f });
        flowersList.Add(new Flowers { name = "����", popularity_pattern = 4, popularity_coefficient = 2.10f, market_price = 55.0f, noise = 0.33f });

        foreach (var f in flowersList)
        {
            _dbConnection.Insert(f);
        }
    }
    #endregion



    #region ������ ��������� ������
    public List<PopularityStoryWithFlower> GetFlowersPrice()
    {
        List<FlowerIds> flowersIds = _dbConnection.Query<FlowerIds>("select flowers.id from flowers");
        List<PopularityStoryWithFlower> storiesList = new();

        Debug.Log($"found: {flowersIds.Count}");

        flowersIds.ForEach(fId =>
        {
            Debug.Log(fId.id);
            string query = $@"select flowers.name, popularity_story.popularity_level, flowers.popularity_coefficient, flowers.market_price 
                            from flowers
                            join popularity_story on popularity_story.flower_id = {fId.id} and 
                            flowers.id = {fId.id} order by popularity_story.id desc limit 1;";

            storiesList.Add(_dbConnection.Query<PopularityStoryWithFlower>(query).First());
        });


        return storiesList;
    }

    public List<PopularityStory> GetFlowerPopularityStory(string flowerName)
    {
        int flowerId = _dbConnection.Query<FlowerIds>($"select id from flowers where name = \"{flowerName}\"").First().id;

        List<PopularityStory> popularityStories = _dbConnection.Query<PopularityStory>(@$"select id, flower_id, popularity_level
            from popularity_story where flower_id = {flowerId}");

        return popularityStories;
    }
    #endregion


    #region ������ ��������� ������
    
    public bool BuyFlower(string flowerName, int count, float marketPrice)
    {
        int flowerId = _dbConnection.Query<FlowerIds>($"select flowers.id from flowers where flowers.name = \"{flowerName}\"").First().id;

        string getFlowerInfoQuery = $"select * from shop_flowers where flower_id = {flowerId}";

        List<ShopFlowers> shopFlowers = _dbConnection.Query<ShopFlowers>(getFlowerInfoQuery);

        Debug.Log($"flower id = {flowerId} flower name = {flowerName}");

        if (shopFlowers.Count == 0)
        {
            string query = @$"insert into shop_flowers (count_in_stock, count_on_sale, flower_id, price) 
                            values ({count}, 0, {flowerId}, 0)";

            var newVal = new ShopFlowers { count_in_stock = count, count_on_sale = 0, flower_id = flowerId, price = 0};

            _dbConnection.Insert(newVal);
            return true;
        }
        else
        {
            int newCount = shopFlowers[0].count_in_stock + count;
            string query = $@"update shop_flowers set count_in_stock = {newCount} where flower_id = {flowerId}";
            _dbConnection.Execute(query);
            return true;
        };
    }
    #endregion

    void OnDestroy()
    {
        // �������� ���������� � ����� ������
        _dbConnection?.Close();
    }


    #region ��������������� ������ ��� ������� �� ��
    class FlowerIds
    {
        public int id { get; set; }
    }
    #endregion
}
