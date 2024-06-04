using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Models
{
    [Table("DeleteQueue")]
    public class DeleteQueue
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; }
        [Column("type")]
        public DocumentType Type { get; set; }
        [Column("user")]
        public string User { get; set; }
    }

    public enum DocumentType
    {
        Note = 0,
        Notebook = 1,
        Event = 2,
        Todo = 3,
        SubToDo = 4,
        Routine = 5,
        RoutineRegistry = 6
    }
}
