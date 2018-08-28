using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio.Clases_Dominio
{
    public class EstadoCuentaCliente
    {
        public DateTime Fecha { get; set; }
        public String TipoDocumento { get; set; }
        public String Serie { get; set; }
        public String Numero {get;set;}
        public String Detalle { get; set; }
        public String Moneda { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public decimal Saldo { get; set; }

    }
}
