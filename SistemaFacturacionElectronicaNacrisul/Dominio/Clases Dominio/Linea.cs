using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio.Clases_Dominio
{
    public class Linea
    {
        public int IdProducto { get; set; }
        public String Producto { get; set; }
        public String UnidadMedida { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Kilos { get; set; }
        public decimal Precio { get; set; }
        public int Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Recargo { get; set; }
        public decimal SubTotal { get; set; }
        public decimal MontoTotal { get; set; }

        public override bool Equals(Object obj)
        {
            Linea linea = (Linea)obj;
            return linea.IdProducto == this.IdProducto;
        }
    }
}
