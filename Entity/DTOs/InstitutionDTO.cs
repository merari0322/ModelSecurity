using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class InstitutionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Phone { get; set; }
        public string EmailInstitution { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Commune { get; set; } = string.Empty;
    }
}
