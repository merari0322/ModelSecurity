using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public  class Objective
    {
        public int Id { get; set; }
         
         public string ObjectiveDescription { get; set; } = string.Empty;

         public string Innovation { get; set; } = string.Empty;

         public string Results { get; set; } = string.Empty;

         public string Sustainability { get; set; } = string.Empty;

         public int ExperienceId { get; set; }
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}