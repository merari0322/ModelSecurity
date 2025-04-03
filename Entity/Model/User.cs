using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Entity.Model
{
  public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Active { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime DeleteAt { get; set; }
    }
}