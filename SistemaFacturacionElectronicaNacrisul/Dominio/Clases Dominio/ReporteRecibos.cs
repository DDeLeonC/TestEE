using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio.Clases_Dominio
{
    public class ReporteRecibos
    {
        public int IdRecibo { get; set; }
        public DateTime Fecha { get; set; }
        public String Cliente { get; set; }
        public int Numero { get; set; }
        public String StrAnulado { get; set; }
        public String Moneda { get; set; }
        public Decimal Importe { get; set; }
    }
}
