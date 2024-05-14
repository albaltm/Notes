﻿using SQLite;
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
    }

    public enum DocumentType
    {
        Note = 0,
        Notebook = 1,
    }
}
