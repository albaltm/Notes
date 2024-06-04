using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Models
{
    [Table("CreateQueue")]
    public class CreateQueue
    {
        
        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; }
        [Column("type")]
        public DocumentType Type { get; set; }
        [Column("user")]
        public string User { get; set; }
    }
}
