using UnityEngine;
using SQLite;

public class DataBaseManager: MonoBehaviour
{
    private SQLiteConnection _dbConnection;

    void Start()
    {
        // Путь к базе данных
        string databasePath = Application.dataPath + "/DataBase/DB/gameDatabase.db";

        // Создание подключения к базе данных
        _dbConnection = new SQLiteConnection(databasePath);

        // Создание таблицы Player
        _dbConnection.CreateTable<Player>();

        // Добавление тестовых данных
        AddPlayer("John", 100);
        AddPlayer("Alice", 200);

        // Получение и вывод данных
        var players = _dbConnection.Table<Player>().ToList();
        foreach (var player in players)
        {
            Debug.Log($"ID: {player.Id}, Name: {player.Name}, Score: {player.Score}");
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
