using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Dominio.Clases_Dominio
{
    public class Grupo
    {
        [Key]
        public int IdGrupo { get; set; }

        [Required, MaxLength(10)]
        public String Codigo { get; set; }

        [Required, MaxLength(100)]
        public String Descripcion { get; set; }

        public String rut { get; set; }
        
        [Required]
        public bool Activo { get; set; }

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
