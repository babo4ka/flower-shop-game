using SQLite;
using UnityEngine;

[Table("credits")]
public class Credits
{
    [PrimaryKey, AutoIncrement]
    public int id { get; }


    public float amount { get; set; }

    public string status { get; set; }
}
