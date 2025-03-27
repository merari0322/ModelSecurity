using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    class ExperienciaDTO
    {
        public int Id { get; set; }
        public string NombreExperiencia { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime DuracionDias { get; set; }
        public DateTime DuracionMeses { get; set; }
        public DateTime DuracionAños { get; set; }
        public string Resumen { get; set; }
        public string Metodologias { get; set; }
        public string Tranferencia { get; set; }
        public string FechaRegistro { get; set; }
        public int UsuarioId1 { get; set; }
        public int InstitucionId1 { get; set; }
        public int IntitucionId2 { get; set; }

    }
}
