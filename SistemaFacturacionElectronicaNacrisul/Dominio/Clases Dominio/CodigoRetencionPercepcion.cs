using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class CodigoRetencionPercepcion
    {
        [Key]
        public int IdCodigoRetencionPercepcion { get; set; }

        public int NroForm { get; set; }

        public int NroLinea { get; set; }

        public int PlanCuentas { get; set; }

        [Required]
        public String Descripcion { get; set; }

        public override String ToString()
        {

            return Descripcion;
        }
    }
}
