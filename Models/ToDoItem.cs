using MudBlazor;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Models
{
    [Table("todoitem")]
    public class ToDoItem : IEvent
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; }
        [Column("user")]
        public string User { get; set; }
        [Column("start")]
        public DateTime Start { get; set; }
        [Column("end")]
        public DateTime? End { get; set; }
        [Column("text")]
        public string Text { get; set; } = "";
        [Column("description")]
        public string Description { get; set; }
        [Column("modified")]
        public DateTime Modified { get; set; }

        [Column("color")]
        public string Color { get; set; }
        [Column("icon")]
        public string Icon { get; set; } = @Icons.Material.Rounded.Lightbulb;
        [Column("status")]
        public Status Status { get; set; } = Status.ToDo;
        [Column("priority")]
        public Priority Important { get; set; } = 0;

        public bool Done { get; set; } = false;
    }

    public enum Status
    {
        ToDo = 0,
        InProgress = 1,
        Done = 2,
    }

    public enum Priority
    {
        NotImportantNotUrgent = 0,
        NotImportantUrgent = 1,
        ImportantNotUrgent = 2,
        ImportantUrgent = 3
    }
}
