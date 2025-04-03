using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model;


namespace Entity.ModelExperience
{
    public class RolPermission
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int PermissionId { get; set; }
    }
}