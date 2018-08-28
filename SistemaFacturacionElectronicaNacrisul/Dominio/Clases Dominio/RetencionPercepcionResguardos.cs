using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class RetencionPercepcionResguardos
    {
        [Key]
        public int IdPercepcionRetencion { get; set; }

        public int IdCodigoPercepcionRetencion { get; set; }

        [Required, ForeignKey("IdCodigoPercepcionRetencion")]
        public CodigoRetencionPercepcion CodigoPercepcionRetencion { get; set; }

        
        public decimal tasa { get; set; }

        public decimal monto { get; set; }

        public decimal valor { get; set; }
    }
}
