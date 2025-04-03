using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class HistoryExperienceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string UserId1 { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public string ExperienceId { get; set; } = string.Empty;
        public string Observation { get; set; } = string.Empty;
        public string AfectedId { get; set; } = string.Empty;
        public bool Active { get; set; }


    }
}
