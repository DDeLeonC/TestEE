using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio.Clases_Dominio
{
    public class ListadoFacturacion
    {
        public DateTime Fecha { get; set; }
        public String Cliente { get; set; }
        public String TipoDocumento { get; set; }
        public String Serie { get; set; }
        public String NroSerie { get; set; }
        public String FormaPago { get; set; }
        public decimal Total { get; set; }
        public decimal MontoTotal { get; set; }
    }
}
