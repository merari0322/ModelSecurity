using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class ExperienceDTO
    {
        public int Id { get; set; }
        public string NameExperience { get; set; } = string.Empty;
        public DateTime DataTime { get; set; }
        public string Summary { get; set; } = string.Empty;
        public string Methodologies { get; set; } = string.Empty;
        public string Transfe { get; set; } = string.Empty;
        public int UserId1 { get; set; }
        public int InstitutionId1 { get; set; }
        public DateTime DataRegistration { get; set; }



    }
}
