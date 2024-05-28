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
        public DateTime Start { get; set; } = DateTime.Today;
        [Column("end")]
        public DateTime? End { get; set; }
        [Column("text")]
        public string Text { get; set; } = "";
        [Column("description")]
        public string Description { get; set; } = "";
        [Column("modified")]
        public DateTime Modified { get; set; }

        [Column("color")]
        public string Color { get; set; } = "inherit";
        [Column("icon")]
        public string Icon { get; set; } = @Icons.Material.Rounded.Lightbulb;
        [Column("status")]
        public Status Status { get; set; } = Status.ToDo;
        [Column("priority")]
        public int Priority { get; set; } = 0;
        [Column("completedat")]
        public DateTime? CompletedAt { get; set; }
        [Column("calendar")]
        public bool AddToCalendar { get; set; } = false;
        [Column("repeat")]
        public bool Repeat { get; set; } = false;

        public bool Done => Status == Status.Done;
    }

    public enum Status
    {
        ToDo = 0,
        InProgress = 1,
        Done = 2,
    }
}
