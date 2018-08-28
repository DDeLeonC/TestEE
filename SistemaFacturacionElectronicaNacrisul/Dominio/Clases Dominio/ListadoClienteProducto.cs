using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio.Clases_Dominio
{
    public class ListadoClienteProducto
    {
        public String Cliente { get; set; }
        public String RUT { get; set; }
        public String Producto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Kilos { get; set; }
        public decimal Total { get; set; }
    }
}
