using UnityEngine;
using SQLite;
using System;

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
