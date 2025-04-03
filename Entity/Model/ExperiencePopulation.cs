using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public class ExperiencePopulation
    {
        public DateTime CreatedAt;
        public DateTime UpdatedAt;

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ExperienceId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Experience { get; set; }
        public int PopulationGradeId { get; set; }
        public PopulationGrade PopulationGrade { get; set; } = new PopulationGrade();
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

    }
}