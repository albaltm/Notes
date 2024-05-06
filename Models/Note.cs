using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Models
{
    [Table("note")]
    public class Note
    {
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; }
        [Column("user")]
        public long User { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("color")]
        public string Color { get; set; }
        [Column("icon")]
        public string Icon { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("modified")]
        public DateTime Modified { get; set; }
        //[Column("tags")]
        //public List<string> Tags { get; set; }
        [Column("favorite")]
        public bool Favorite { get; set; }
        [Column("notebook")]
        public string? Notebook { get; set; }
        [Column("position")]
        public int Position { get; set; }
        [Column("saved")]
        public bool Saved { get; set; }
        [Column("synchronized")]
        public bool Synchronized { get; set; }
    }
}
