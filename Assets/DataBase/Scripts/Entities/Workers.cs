using SQLite;
using UnityEngine;

[Table("workers")]
public class Workers
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set;  }

    public string name { get; set; }

    public int motivation { get; set; }
}
