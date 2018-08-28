using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio.Clases_Dominio
{
    public class DocumentoVencido
    {
        public DateTime Fecha { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public String Cliente { get; set; }
        public String TipoDocumento { get; set; }
        public String Serie { get; set; }
        public String Numero { get; set; }
        public String Moneda { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal Saldo { get; set; }
        public decimal SumaMontos { get; set; }
    }
}
