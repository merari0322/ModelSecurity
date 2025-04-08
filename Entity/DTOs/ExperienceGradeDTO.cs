using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class ExperienceGradeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ExperienceId { get; set; }
        public Grade GradeId { get; set; } = new Grade();
    }
}