using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    class RolUserDTO
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public RolDTO Rol { get; set; }
        public int IdUser { get; set; }
        public UserDTO User { get; set;  }
    }
}
