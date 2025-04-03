using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class ExperiencieLineThematicDTO
    {
        public int Id { get; set; }
        public string LineThematicId1 { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int LineThematicId { get; set; }
        public int ExperienceId { get; set; }


    }
}
