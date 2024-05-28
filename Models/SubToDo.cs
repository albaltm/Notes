using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Models
{
    [Table("subtodo")]
    public class SubToDo
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; }
        [Column("user")]
        public string User { get; set; }
        [Column("idtodo")]
        public string ToDo { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [Column("created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;
        [Column("modified")]
        public DateTime Modified { get; set; }
        [Column("status")]
        public Status Status { get; set; } = Status.ToDo;

        public bool Done => Status == Status.Done;
    }
}
