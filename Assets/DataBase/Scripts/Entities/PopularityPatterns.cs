using UnityEngine;
using SQLite;

[Table("popularity_patterns")]
public class PopularityPatterns
{
    [PrimaryKey, AutoIncrement]
    public int id { get; }

    public string pattern {  get; set; }
}
