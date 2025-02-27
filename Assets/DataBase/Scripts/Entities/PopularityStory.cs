using UnityEngine;
using SQLite;

[Table("popularity_story")]
public class PopularityStory
{

    [PrimaryKey, AutoIncrement]
    public int id { get; set; }

    //внешний ключ к таблице flowers
    public int flower_id { get; set; }

    public float popularity_level { get; set; }
}
