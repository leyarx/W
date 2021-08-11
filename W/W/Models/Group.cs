using SQLite;
using System;

namespace W.Models
{
    public class Group
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}