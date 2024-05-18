using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Models
{
    [Table("note")]
    public class Note : IBiblioteca
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; }
        [Column("user")]
        public string User { get; set; }
        [Column("name")]
        public string Name { get; set; } = "";
        [Column("backgroundcolor")]
        public string BackgroundColor { get; set; } = "inherit";
        [Column("textcolor")]
        public string TextColor { get; set; } = "inherit";
        [Column("content")]
        public string Content { get; set; } = "";
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("modified")]
        public DateTime Modified { get; set; }
        //[Column("tags")]
        //public List<string> Tags { get; set; }
        [Column("favorite")]
        public bool Favorite { get; set; } = false;
        [Column("notebook")]
        public string Notebook { get; set; } = "";
        [Column("position")]
        public int Position { get; set; } = -1;
    }
}
