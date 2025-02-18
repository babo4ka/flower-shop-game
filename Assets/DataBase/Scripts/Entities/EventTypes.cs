using SQLite;
using UnityEngine;

[Table("event_types")]
public class EventTypes
{
    [PrimaryKey, AutoIncrement]
    public int id { get; }

    public string name { get; set; }
    public float cost { get; set; }

}
