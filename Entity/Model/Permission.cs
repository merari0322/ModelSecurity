using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model 
{
 public  class Permission
    {
        public int Id { get; set; }
        public string Permissiontype { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime DeleteAt { get; set; }
    }
}