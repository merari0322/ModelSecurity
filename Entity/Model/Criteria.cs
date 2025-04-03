
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
  public  class Criteria
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
              
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}