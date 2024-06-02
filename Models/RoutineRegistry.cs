using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Models
{
    [Table("routineregistry")]
    public class RoutineRegistry : IEvent
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; }
        [Column("routine")]
        public string Routine { get; set; }
        [Column("user")]
        public string User { get; set; }
        [Column("amount")]
        public double Amount { get; set; } = 0;
        [Column("status")]
        public Status Status { get; set; } = Status.ToDo;
        [Column("start")]
        public DateTime Start { get; set; } = DateTime.Today;
        [Column("end")]
        public DateTime? End { get; set; } = DateTime.Today.AddDays(1).AddSeconds(-1);
        [Column("modified")]
        public DateTime Modified { get; set; } = DateTime.UtcNow;
        [Column("note")]
        public string Note { get; set; } = "";
        public string Text { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
    }
}
