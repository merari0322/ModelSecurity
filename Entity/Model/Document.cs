using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
  public  class Document
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;        
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}