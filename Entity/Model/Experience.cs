using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public class Experience
    {
        public int Id { get; set; }
        public string NameExperience { get; set; } = string.Empty;

        public string DataStart { get; set; } = string.Empty;

        public string DurationDays { get; set; } = string.Empty;

        public string DurationMonths { get; set; } = string.Empty;

        public string DurationYears { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public string Methodologies { get; set; } = string.Empty;

        public string Transfe { get; set; } = string.Empty;

        public DateTime DataRegistration { get; set; }

        public int UserId1 { get; set; }

        public int InstitutionId1 { get; set; }
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}