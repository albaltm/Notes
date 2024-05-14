using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Models
{
    [Table("notebook")]
    public class Notebook : IBiblioteca
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; }
        [Column("user")]
        public string User { get; set; }
        [Column("name")]
        public string Name { get; set; } = "";
        [Column("backgroundcolor")]
        public string BackgroundColor { get; set; } = "#dedca6";
        [Column("textcolor")]
        public string TextColor { get; set; } = "#000000";
        [Column("icon")]
        public string Icon { get; set; } = "";
        [Column("iconcolor")]
        public string IconColor { get; set; } = "#000000";
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("modified")]
        public DateTime Modified { get; set; }
        //[Column("tags")]
        //public List<string> Tags { get; set; }
        [Column("favorite")]
        public bool Favorite { get; set; } = false;
        [Column("bookmark")]
        public int Bookmark { get; set; }
        [Column("synchronized")]
        public bool Synchronized { get; set; } = false;
    }
}
