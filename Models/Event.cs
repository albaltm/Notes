using MudBlazor;
using SQLite;

namespace AppNotes.Models
{
    [Table("event")]
    public class Event : IEvent
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; }
        [Column("user")]
        public string User { get; set; }
        [Column("start")]
        public DateTime Start { get; set; } = DateTime.Today;
        [Column("end")]
        public DateTime? End { get; set; } = DateTime.Today.AddDays(1).AddSeconds(-1);
        [Column("text")]
        public string Text { get; set; } = "";
        [Column("color")]
        public string Color { get; set; } = "inherit";
        [Column("icon")]
        public string Icon { get; set; } = @Icons.Material.Rounded.Lightbulb;
        [Column("status")]
        public Status Status { get; set; } = 0;
        [Column("modified")]
        public DateTime Modified { get; set; }

        public bool AllDay => Start == Start.Date;
    }
}
