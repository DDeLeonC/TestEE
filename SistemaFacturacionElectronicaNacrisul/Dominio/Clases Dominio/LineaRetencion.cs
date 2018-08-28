using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio.Clases_Dominio
{
    public class LineaRetencion
    {
        public int IdCodigoRetencion { get; set; }
        public String CodigoRetencion { get; set; }
        public decimal tasa { get; set; }
        public decimal monto { get; set; }
        public decimal valor { get; set; }

        public override bool Equals(Object obj)
        {
            LineaRetencion linea = (LineaRetencion)obj;
            return linea.IdCodigoRetencion == this.IdCodigoRetencion;
        }
    }
}
