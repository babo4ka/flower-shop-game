using SQLite;
using UnityEngine;

[Table("work_days")]
public class WorkDays
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }

    public int flowers_sold { get; set; }
    public float money_earned { get; set; }
    public int events_happen { get; set; }
    public float events_total_cost { get; set; }
    public int clients_count { get; set; }
    public float average_workers_motivation { get; set; }
    public float average_clients_motivation { get; set; }

}
