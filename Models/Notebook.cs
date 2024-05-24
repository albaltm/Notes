using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Models
{
    [Table("notebook")]
    public class Notebook
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; }
        [Column("user")]
        public string User { get; set; }
        [Column("name")]
        public string Name { get; set; } = "";
        [Column("icon")]
        public string Icon { get; set; } = "";
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("modified")]
        public DateTime Modified { get; set; }
        [Column("bookmark")]
        public string Bookmark { get; set; } = "";
    }
}
