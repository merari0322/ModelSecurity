using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
  public  class Evaluation
    {
        public int Id { get; set; }
        public string TypeEvaluation { get; set; } = string.Empty;

        public string Comments { get; set; } = string.Empty;

        public DateTime DataTime { get; set; }

        public int UserId1 { get; set; }

        public int StateId { get; set; }

        public int ExperienceId1 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeleteAt { get; set; }
    }
}