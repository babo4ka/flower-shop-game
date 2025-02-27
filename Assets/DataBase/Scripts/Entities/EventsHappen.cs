using SQLite;
using UnityEngine;

[Table("events_happen")]
public class EventsHappen
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }

    //������� ���� � ������� work_days
    public int work_day { get; set; }
    
    //������� ���� � ������� event_types
    [Column("event")]
    public int _event { get; set; }
}
