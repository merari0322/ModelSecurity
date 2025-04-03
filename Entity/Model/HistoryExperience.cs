using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public class HistoryExperience
    {
        public int Id { get; set; }
       
       public string Action { get; set; } = string.Empty;

       public DateTime DateTime { get; set; }

       public  int UserId { get; set; }

       public DateTime DeleteAt { get; set;  }     
       public DateTime CreateAt { get; set; }
        public string ExperienceId { get; set; } = string.Empty;


    }
}