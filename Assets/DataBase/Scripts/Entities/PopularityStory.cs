using UnityEngine;
using SQLite;

[Table("popularity_story")]
public class PopularityStory
{

    [PrimaryKey, AutoIncrement]
    public int id { get; set; }

    //������� ���� � ������� flowers
    public int flower_id { get; set; }

    public float popularity_level { get; set; }
}
