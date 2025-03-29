using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    class Evaluation
    {
        public int Id { get; set; }
        public string typeEvaluation { get; set; }
        public string comments { get; set; }
        public DateTime dateTime { get; set; }
        public int userId1 { get; set; }
        public int experiencieId1 { get; set; }
    }
}
