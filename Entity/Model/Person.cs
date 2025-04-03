using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
  public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Phone { get; set; }
        public string Active { get; set; } = string.Empty;
       
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}