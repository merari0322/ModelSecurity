
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public class ExperienceGrade
    {
        public int Id { get; set; }
                
        public int ExperienceId { get; set; }
        public Experience Experience { get; set; } = new Experience();

        public Grade GradeId { get; set; } = new Grade();
        public Grade Grade { get; set; } = new Grade();
 
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}