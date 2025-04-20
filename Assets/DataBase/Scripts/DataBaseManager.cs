using UnityEngine;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Security.Cryptography;

public class DataBaseManager : MonoBehaviour
{
    #region события обновления информации
    public static Action<Shop> updateShopData;
    public static Action<List<FlowersPrice>> updateFlowersPricesData;
    public static Action<List<ShopFlowers>> updateShopFlowersData;
    public static Action<List<Workers>> updateWorkersData;
    #endregion

    private SQLiteConnection _dbConnection;

    public static bool loaded { get; private set; } = false;

    void Start()
    {
        // Путь к базе данных
        string databasePath = Application.dataPath + "/DataBase/DB/gameDatabase.db";

        // Создание подключения к базе данных
        _dbConnection = new SQLiteConnection(databasePath);
        
        CreateDataBase();

        //CreateInitialShopData();
        GetShopData();
        GetShopFlowersData();
        GetFlowersPrice();
        GetWorkersData();
        //UpdateFlowers();
        //GetFlowersDataToCheck();
        //CreateInitialPopularityPatterns();
        //CreateInitialFlowers();
        //GetFlowersPriceToCheck();
        loaded = true;
    }


    private void CreateDataBase()
    {
        //создание таблиц в бд
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


    #region временные методы загрузки данных
    //временный метод для получения данных цветов и отображения
    private void GetFlowersDataToCheck()
    {
        var flowersList2 = _dbConnection.Query<FlowersWithPattern>(
            @"select flowers.name as flower_name, popularity_patterns.pattern as popularity_pattern, flowers.popularity_coefficient, flowers.market_price, flowers.noise 
            from flowers join popularity_patterns on flowers.popularity_pattern = popularity_patterns.Id"
            );

        flowersList2.ForEach(f =>
        {
            Debug.Log($"{f.flower_name} {f.popularity_pattern} {f.popularity_coefficient} {f.market_price} {f.noise}");

            List<float> storyPatternAsFloat = new();
            var story = f.popularity_pattern.Split("-").ToList();
            story.ForEach(it => { storyPatternAsFloat.Add(float.Parse(it, CultureInfo.InvariantCulture.NumberFormat)); });

            storyPatternAsFloat.ForEach(s =>
            {
                var sInT = new PopularityStory { flower_name = f.flower_name, popularity_level = s };
                _dbConnection.Insert(sInT);
            });
        });
    }



    private void GetFlowersPriceToCheck()
    {
        List<FlowerNames> names = _dbConnection.Query<FlowerNames>("select flowers.name from flowers");

        names.ForEach(name =>
        {
            string query = $@"select flowers.name as flower_name, popularity_story.popularity_level, flowers.popularity_coefficient, flowers.market_price 
                            from flowers
                            join popularity_story on popularity_story.flower_name = {name.flower_name} and 
                            flowers.name = {name.flower_name} order by popularity_story.id desc limit 1;";

            var pswf = _dbConnection.Query<FlowersPrice>(query).First();

            Debug.Log($"{pswf.flower_name} price = {pswf.market_price * (pswf.popularity_level * pswf.popularity_coefficient / 10)}");
        });
    }

    #endregion

    #region временные методы создания
    //временный метод для создания базовых паттернов популярности цветов
    private void CreateInitialPopularityPatterns()
    {
        var patternsList = new List<PopularityPatterns>
        {
            new() { pattern = "45.12-47.34-45.67-49.01-40.50-57.89-42.34-51.23-49.78-50.12-48.34-46.78-52.01-55.12-53.45" },
            new() { pattern = "50.12-48.34-46.78-52.01-55.12-47.89-49.01-53.45-40.12-49.34-50.67-54.89-42.34-51.23-48.90" },
            new() { pattern = "55.12-47.89-49.01-53.45-40.12-49.34-50.67-54.89-42.34-51.23-48.90-57.12-45.12-47.34-45.67" },
            new() { pattern = "40.12-49.34-50.67-54.89-42.34-51.23-48.90-57.12-45.12-47.34-45.67-49.01-50.12-48.34-46.78" },
            new() { pattern = "42.34-51.23-48.90-57.12-45.12-47.34-45.67-49.01-50.12-48.34-46.78-52.01-55.12-47.89-49.01" }
        };

        foreach (var p in patternsList)
        {
            _dbConnection.Insert(p);
        }
    }

    //временный метод для создания нескольких видов цветов
    private void CreateInitialFlowers()
    {
        var flowersList = new List<Flowers>
        {
            new() { name = "Роза", popularity_pattern = 3, popularity_coefficient = 1.14f, market_price = 50.0f, noise = 0.15f, popularity_step = 15 },
            new() { name = "Тюльпан", popularity_pattern = 2, popularity_coefficient = 2.45f, market_price = 35.5f, noise = 0.22f, popularity_step = 15 },
            new() { name = "Ландыш", popularity_pattern = 5, popularity_coefficient = 1.87f, market_price = 45.3f, noise = 0.18f, popularity_step = 15 },
            new() { name = "Пион", popularity_pattern = 1, popularity_coefficient = 3.12f, market_price = 120.7f, noise = 0.55f, popularity_step = 15 },
            new() { name = "Орхидея", popularity_pattern = 4, popularity_coefficient = 2.78f, market_price = 90.2f, noise = 0.42f, popularity_step = 15 },
            new() { name = "Гвоздика", popularity_pattern = 3, popularity_coefficient = 1.56f, market_price = 25.8f, noise = 0.30f, popularity_step = 15 },
            new() { name = "Лилия", popularity_pattern = 2, popularity_coefficient = 2.90f, market_price = 60.4f, noise = 0.25f, popularity_step = 15 },
            new() { name = "Гербера", popularity_pattern = 5, popularity_coefficient = 1.23f, market_price = 40.6f, noise = 0.12f, popularity_step = 15 },
            new() { name = "Хризантема", popularity_pattern = 1, popularity_coefficient = 3.45f, market_price = 85.9f, noise = 0.48f, popularity_step = 15 },
            new() { name = "Ирис", popularity_pattern = 4, popularity_coefficient = 2.10f, market_price = 55.0f, noise = 0.33f, popularity_step = 15 }
        };

        foreach (var f in flowersList)
        {
            _dbConnection.Insert(f);
        }
    }

    private void UpdateFlowers()
    {
        var flo = _dbConnection.Query<Flowers>("select * from flowers");
        flo.ForEach(f =>
        {
            f.popularity_step = 15;
            _dbConnection.Update(f);
        });


    }
    //создание записи в сущности shop
    private void CreateInitialShopData()
    {
        var shop = new Shop() { cash = 500000f, daysGone = 0, debt = 0f, rating = 1 };

        _dbConnection.Insert(shop);
    }
    #endregion



    #region методы получения данных
    //ЦВЕТЫ
    private void GetFlowersPrice()
    {
        List<FlowerNames> names = _dbConnection.Query<FlowerNames>("select flowers.name as flower_name from flowers");
        List<FlowersPrice> storiesList = new();

        names.ForEach(name =>
        {
            string query = $@"select flowers.name as flower_name, popularity_story.popularity_level, flowers.popularity_coefficient, flowers.market_price 
                            from flowers
                            join popularity_story on popularity_story.flower_name = ""{name.flower_name}"" and 
                            flowers.name = ""{name.flower_name}"" order by popularity_story.id desc limit 1;";

            storiesList.Add(_dbConnection.Query<FlowersPrice>(query).First());
        });

        updateFlowersPricesData?.Invoke(storiesList);
    }

    public List<PopularityStory> GetFlowerPopularityStory(string flowerName)
    {
        List<PopularityStory> popularityStories = _dbConnection.Query<PopularityStory>(@$"select id, flower_name, popularity_level
            from popularity_story where flower_name = ""{flowerName}""");

        return popularityStories;
    }

    private void GetShopFlowersData()
    {
        List<ShopFlowers> shopFlowers = _dbConnection.Query<ShopFlowers>("select * from shop_flowers");
        updateShopFlowersData?.Invoke(shopFlowers);
    }

    public bool HasFlower(string flowerName)
    {
        var flowers = _dbConnection.Query<ShopFlowers>($"select * from shop_flowers where flower_name = \"{flowerName}\"");
        if(flowers.Count() == 0) return false;

        if(flowers.First().count_on_sale == 0) return false;

        return true;
    }

    //МАГАЗИН
    private void GetShopData()
    {
        updateShopData?.Invoke(_dbConnection.Query<Shop>("select * from shop").First());
    }

    //ПЕРСОНАЛ
    private void GetWorkersData()
    {
        updateWorkersData?.Invoke(_dbConnection.Query<Workers>("select * from workers"));
    }


    //СТАТИСТИКА
    public List<WorkDays> GetStats()
    {

        return _dbConnection.Query<WorkDays>("select * from work_days");
    }
    #endregion


    #region методы изменения данных

    //ЦВЕТЫ
    public void BuyFlower(string flowerName, int count, float sum)
    {
        string getFlowerInfoForQuery = $"select * from shop_flowers where flower_name = \"{flowerName}\"";

        List<ShopFlowers> shopFlowers = _dbConnection.Query<ShopFlowers>(getFlowerInfoForQuery);

        Debug.Log($"flower name = {flowerName}");

        if (shopFlowers.Count == 0)
        {
            var newVal = new ShopFlowers() { count_in_stock = count, count_on_sale = 0, flower_name = flowerName, price = 0f };
            _dbConnection.Insert(newVal);
        }
        else
        {
            shopFlowers[0].count_in_stock += count;
            _dbConnection.Update(shopFlowers[0]);
        };

        Shop shop = _dbConnection.Query<Shop>("select * from shop").First();
        shop.cash -= sum;

        _dbConnection.Update(shop);

        updateShopData?.Invoke(shop);
    }

    //изменение цены цветка
    public void ChangeFlowerPrice(string flowerName, float price)
    {
        ShopFlowers flower = _dbConnection.Query<ShopFlowers>($"select * from shop_flowers where flower_name = \"{flowerName}\"").First();

        flower.price = price;

        _dbConnection.Update(flower);

        updateShopFlowersData?.Invoke(_dbConnection.Query<ShopFlowers>("select * from shop_flowers"));
    }


    //изменение цветков на продаже
    public void ToggleSaleFlowers(string flowerName, int count, ToggleSaleAction action)
    {
        ShopFlowers flower = _dbConnection.Query<ShopFlowers>($"select * from shop_flowers where flower_name = \"{flowerName}\"").First();

        switch (action)
        {
            case ToggleSaleAction.PUT:
                flower.count_on_sale += count;
                flower.count_in_stock -= count;
                break;

            case ToggleSaleAction.REMOVE:
                flower.count_on_sale -= count;
                flower.count_in_stock += count;
                break;
        }

        _dbConnection.Update(flower);

        updateShopFlowersData?.Invoke(_dbConnection.Query<ShopFlowers>("select * from shop_flowers"));
    }

    public void SpendFlower(string flowerName)
    {
        ShopFlowers flower = _dbConnection.Query<ShopFlowers>($"select * from shop_flowers where flower_name = \"{flowerName}\"").First();

        flower.count_on_sale--;
        _dbConnection.Update(flower);
        updateShopFlowersData?.Invoke(_dbConnection.Query<ShopFlowers>("select * from shop_flowers"));
    }

    public void UpdateFlowersPopulartityStory()
    {
        var flowers = _dbConnection.Query<Flowers>("select * from flowers");

        flowers.ForEach(flower =>
        {
            var pattern = _dbConnection.Query<PopularityPatterns>($"select * from popularity_patterns where id = {flower.popularity_pattern}").First();
            var pattern_parts = pattern.pattern.Split("-").Select(pp => float.Parse(pp, CultureInfo.InvariantCulture.NumberFormat)).ToList();
            var story = _dbConnection.Query<PopularityStory>($"select * from popularity_story where flower_name = \"{flower.name}\"");
            _dbConnection.Delete(story.First());
            flower.popularity_step = flower.popularity_step == 15? 1:flower.popularity_step+1;

            float newLevel = UnityEngine.Random.Range(0f, 1f) >= 0.7 ? UnityEngine.Random.Range(0f, 100f) : pattern_parts[flower.popularity_step - 1];

            PopularityStory newStory = new PopularityStory() { flower_name = flower.name, popularity_level = newLevel };

            _dbConnection.Update(flower);
            _dbConnection.Insert(newStory);
        });
    }

    //ПЕРСОНАЛ
    public void HireWorker(Workers worker)
    {
        _dbConnection.Insert(worker);
        updateWorkersData?.Invoke(_dbConnection.Query<Workers>("select * from workers"));
    }

    public void FireWorker(Workers worker)
    {
        _dbConnection.Delete(worker);
        updateWorkersData?.Invoke(_dbConnection.Query<Workers>("select * from workers"));
    }

    public void DecreaseWorkerMotivation(Workers worker, float amount)
    {
        worker.motivation -= amount;
        _dbConnection.Update(worker);
        updateWorkersData?.Invoke(_dbConnection.Query<Workers>("select * from workers"));
    }

    public void IncreaseWorkerMotivation(Workers worker, float amount)
    {
        worker.motivation += amount;
        _dbConnection.Update(worker);
        updateWorkersData?.Invoke(_dbConnection.Query<Workers>("select * from workers"));
    }

    public void UpdateWorker(Workers worker)
    {
        _dbConnection.Update(worker);
        updateWorkersData?.Invoke(_dbConnection.Query<Workers>("select * from workers"));
    }

    //МАГАЗИН
    public void AddCash(float amount)
    {
        var shop = _dbConnection.Query<Shop>("select * from shop").First();
        shop.cash += amount;

        _dbConnection.Update(shop);
        updateShopData?.Invoke(shop);
    }

    public void IncreaseDay()
    {
        var shop = _dbConnection.Query<Shop>("select * from shop").First();
        shop.daysGone++;
        _dbConnection.Update(shop);
        updateShopData?.Invoke(shop);
    }

    //СТАТИСТИКА
    public void CountStats(int flowersSold, float money_earned, int clientsCount, float averageClientsSatisfaction)
    {
        _dbConnection.Insert(new WorkDays() { flowers_sold = flowersSold, money_earned = money_earned, 
            clients_count = clientsCount, average_clients_motivation = averageClientsSatisfaction });
    }

    public enum ToggleSaleAction{
        PUT, REMOVE
    }
    #endregion

    void OnDestroy()
    {
        // Закрытие соединения с базой данных
        _dbConnection?.Close();
    }


    #region вспомогательные классы для выборки из бд
    class FlowerNames
    {
        public string flower_name { get; set; }
    }
    #endregion
}
