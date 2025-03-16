using UnityEngine;
using SQLite;

[Table("shop_flowers")]
public class ShopFlowers
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set;  }

    public int count_in_stock { get; set; }

    public int count_on_sale { get; set; }

    //внешний ключ к таблице flowers
    public string flower_name { get; set; }

    public float price { get; set; }
}
