using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    class AssessmentDTO
    {
        public int Id { get; set; }
        public string TypeEvaluation { get; set; }
        public string comments { get; set; }    
        public DateTime DataTime { get; set; }
        public string UserId1 { get; set; } 
        public string Experience { get; set; }

    }
}
