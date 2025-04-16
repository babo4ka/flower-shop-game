using SQLite;
using UnityEngine;

[Table("flowers")]
public class Flowers
{
    [PrimaryKey]
    public string name { get; set; }

    //внешний ключ к таблице popularity_patterns
    public int popularity_pattern {  get; set; }

    public float popularity_coefficient { get; set; }

    public float market_price { get; set; }
    public float noise { get; set; }

    public int popularity_step { get; set; }
}
