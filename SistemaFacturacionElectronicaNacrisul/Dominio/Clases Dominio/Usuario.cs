using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        public String Nombre { get; set; }

        [Required]
        public String Contrasena { get; set; }

        
    }
}
