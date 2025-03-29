using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    class ExperiencieDTO
    {
        public int Id { get; set; }
        public string nameExperience { get; set; }
        public DateTime dataTime { get; set; }
        public string summary { get; set; }
        public string methodologies { get; set; }
        public string transfer { get; set; }
        public int userId1 { get; set; }
        public int institutionId1 { get; set; }
        

    }
}
