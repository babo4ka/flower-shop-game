using UnityEngine;
using SQLite;

[Table("popularity_story")]
public class PopularityStory
{

    [PrimaryKey, AutoIncrement]
    public int id { get; set; }

    //внешний ключ к таблице flowers
    public string flower_name { get; set; }

    public float popularity_level { get; set; }
}
