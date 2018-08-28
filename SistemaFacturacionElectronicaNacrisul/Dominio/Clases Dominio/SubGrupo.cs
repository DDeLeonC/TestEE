using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class SubGrupo
    {
        [Key]
        public int IdSubGrupo { get; set; }

        [Required, MaxLength(10)]
        public String Codigo { get; set; }

        [Required, MaxLength(100)]
        public String Descripcion { get; set; }

        public int IdGrupo { get; set; }

        [Required, ForeignKey("IdGrupo")]
        public Grupo Grupo { get; set; }

        public String rut { get; set; }

        [Required]
        public bool Activo { get; set; }

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
