using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    class HistoryExperience
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string action { get; set; }
        public DateTime dateTime { get; set; }
        public string userId1 { get; set; }
        public string tableName { get; set; }
        public string userId { get; set; }
        public string observation { get; set; }
        public string afectedId { get; set; }
        public bool active { get; set; }

    }
}
