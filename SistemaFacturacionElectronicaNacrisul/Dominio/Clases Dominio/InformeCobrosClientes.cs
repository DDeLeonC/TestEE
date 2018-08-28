using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio.Clases_Dominio
{
    public class InformeCobrosClientes
    {
        public String Cliente { get; set; }
        public String Telefono { get; set; }
        public String FechaVencimiento { get; set; }
        public String Detalle { get; set; }
        public decimal? Total { get; set; }
        public decimal? Saldo { get; set; }
        public decimal? SaldoTotal { get; set; }
    }
}
