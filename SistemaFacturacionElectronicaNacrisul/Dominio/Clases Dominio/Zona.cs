using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class Zona
    {
        [Key]
        public int IdZona { get; set; }

        [Required]
        public String Nombre { get; set; }

        [Required]
        public String Codigo { get; set; }

        public bool Activo { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
