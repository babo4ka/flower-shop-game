using SQLite;
using UnityEngine;

[Table("workers")]
public class Workers
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set;  }

    public string name { get; set; }

    public float motivation { get; set; }

    public int minimal_shop_rating {  get; set; }
    
    public float minimal_hour_salary { get; set; }

    [Ignore]
    public float hour_salary { get; set; }
}
