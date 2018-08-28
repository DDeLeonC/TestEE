using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class IndicadorFacturacion
    {
        [Key]
        public int IdIndicador { get; set; }

        public int Codigo { get; set; }

        [Required]
        public String Nombre { get; set; }

        public override String ToString()
        {

            return Nombre;
        }
    }
}
