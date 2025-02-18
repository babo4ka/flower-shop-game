using SQLite;
using UnityEngine;

[Table("events_happen")]
public class EventsHappen
{
    [PrimaryKey, AutoIncrement]
    public int id { get; }

    //внешний ключ к таблице work_days
    public int work_day { get; set; }
    
    //внешний ключ к таблице event_types
    [Column("event")]
    public int _event { get; set; }
}
