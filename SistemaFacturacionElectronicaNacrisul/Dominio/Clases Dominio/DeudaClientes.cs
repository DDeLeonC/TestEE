using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio.Clases_Dominio
{
    public class DeudaClientes
    {
        public String FechaEmision { get; set; }
        public String FechaVencimiento { get; set; }
        public String Detalle { get; set; }
        public String Moneda { get; set; }
        public int IdDocumento { get; set; }
        public decimal Total { get; set; }
        public decimal SaldoDocumento { get; set; }
        public decimal SaldoTotal { get; set; }
    }
}
