using UnityEngine;
using SQLite;

[Table("shop")]
public class Shop
{
    [PrimaryKey, AutoIncrement]
    public int id { get;}

    public float cash { get; set;}
    public float debt { get; set;}
    public int rating { get; set;}
}
