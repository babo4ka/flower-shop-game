using UnityEngine;
using SQLite;

public class DataBaseManager: MonoBehaviour
{
    private SQLiteConnection _dbConnection;

    void Start()
    {
        // ���� � ���� ������
        string databasePath = Application.dataPath + "/DataBase/DB/gameDatabase.db";

        // �������� ����������� � ���� ������
        _dbConnection = new SQLiteConnection(databasePath);

        // �������� ������� Player
        _dbConnection.CreateTable<Player>();

        // ���������� �������� ������
        AddPlayer("John", 100);
        AddPlayer("Alice", 200);

        // ��������� � ����� ������
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
        // �������� ���������� � ����� ������
        _dbConnection?.Close();
    }

}
