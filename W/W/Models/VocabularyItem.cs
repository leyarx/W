using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace W.Models
{
    public class VocabularyItem
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Word { get; set; }
        public string Translation { get; set; }
        public int Id_group { get; set; }
        public byte[] Voice { get; set; }
        public int KnowledgeLevel { get; set; }
    }
}
