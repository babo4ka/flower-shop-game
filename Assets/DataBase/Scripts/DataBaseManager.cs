using UnityEngine;
using SQLite;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class DataBaseManager: MonoBehaviour
{
    private SQLiteConnection _dbConnection;

    void Start()
    {
        // Путь к базе данных
        string databasePath = Application.dataPath + "/DataBase/DB/gameDatabase.db";

        // Создание подключения к базе данных
        _dbConnection = new SQLiteConnection(databasePath);

        CreateDataBase();

        //CreateInitialPopularityPatterns();
        //CreateInitialFlowers();
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
    }

    //временный метод для создания базовых паттернов популярности цветов
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

    //временный метод для создания нескольких видов цветов
    private void CreateInitialFlowers()
    {
        var flowersList = new List<Flowers>();
        flowersList.Add(new Flowers { name = "Роза", popularity_pattern = 3, popularity_coefficient = 1.14f, market_price = 50.0f, noise = 0.15f });
        flowersList.Add(new Flowers { name = "Тюльпан", popularity_pattern = 2, popularity_coefficient = 2.45f, market_price = 35.5f, noise = 0.22f });
        flowersList.Add(new Flowers { name = "Ландыш", popularity_pattern = 5, popularity_coefficient = 1.87f, market_price = 45.3f, noise = 0.18f });
        flowersList.Add(new Flowers { name = "Пион", popularity_pattern = 1, popularity_coefficient = 3.12f, market_price = 120.7f, noise = 0.55f });
        flowersList.Add(new Flowers { name = "Орхидея", popularity_pattern = 4, popularity_coefficient = 2.78f, market_price = 90.2f, noise = 0.42f });
        flowersList.Add(new Flowers { name = "Гвоздика", popularity_pattern = 3, popularity_coefficient = 1.56f, market_price = 25.8f, noise = 0.30f });
        flowersList.Add(new Flowers { name = "Лилия", popularity_pattern = 2, popularity_coefficient = 2.90f, market_price = 60.4f, noise = 0.25f });
        flowersList.Add(new Flowers { name = "Гербера", popularity_pattern = 5, popularity_coefficient = 1.23f, market_price = 40.6f, noise = 0.12f });
        flowersList.Add(new Flowers { name = "Хризантема", popularity_pattern = 1, popularity_coefficient = 3.45f, market_price = 85.9f, noise = 0.48f });
        flowersList.Add(new Flowers { name = "Ирис", popularity_pattern = 4, popularity_coefficient = 2.10f, market_price = 55.0f, noise = 0.33f });

        foreach (var f in flowersList)
        {
            _dbConnection.Insert(f);
        }
    }

    void AddPlayer(string name, int score)
    {
        var player = new Player { Name = name, Score = score };
        _dbConnection.Insert(player);
    }

    void OnDestroy()
    {
        // Закрытие соединения с базой данных
        _dbConnection?.Close();
    }

}
