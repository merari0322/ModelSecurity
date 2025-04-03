using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public  class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;        
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }
        public string Level { get; set; } = string.Empty;
        public object CreatedAt { get; set; }
        public object UpdatedAt { get; set; }
    }
}