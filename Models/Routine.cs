using MudBlazor;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Models
{
    [Table("routine")]
    public class Routine
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; }
        [Column("user")]
        public string User { get; set; }
        [Column("text")]
        public string Text { get; set; } = "";
        [Column("description")]
        public string Description { get; set; } = "";
        [Column("evaluationText")]
        public string EvaluationText { get; set; } = "";
        [Column("modified")]
        public DateTime Modified { get; set; }
        [Column("color")]
        public string Color { get; set; } = "inherit";
        [Column("icon")]
        public string Icon { get; set; } = @Icons.Material.Rounded.Lightbulb;
        [Column("priority")]
        public int Priority { get; set; } = 0;
        [Column("calendar")]
        public bool AddToCalendar { get; set; } = false;
        [Column("evaluation")]
        public EvaluationType Evaluation { get; set; } = 0;
        [Column("frequency")]
        public FrequencyType Frequency { get; set; } = 0;
        [Column("daysofweek")]
        public string DaysOfWeek { get; set; } = "False;False;False;False;False;False;False";
        [Column("period")]
        public int Period { get; set; } = 1;
        [Column("rest")]
        public int Rest { get; set; } = 1;
        [Column("goal")]
        public double Goal { get; set; } = 1;
        [Column("amountModifier")]
        public AmountModifier AmountModifier { get; set; }
        [Column("start")]
        public DateTime Start { get; set; } = DateTime.Today;
        [Column("active")]
        public bool Active { get; set; } = true;
    }

    public enum EvaluationType
    {
        Completed = 0,
        Amount = 1,
    }
    public enum AmountModifier
    {
        NoGoal = 0,
        Minimum = 1,
        Exact = 2,
        Maximum = 3,
    }
    public enum FrequencyType
    {
        Everyday = 0,
        DaysOfWeek = 1,
        FlexibleWeek = 2,
        Personalized = 3,
        Periods = 4
    }
}
