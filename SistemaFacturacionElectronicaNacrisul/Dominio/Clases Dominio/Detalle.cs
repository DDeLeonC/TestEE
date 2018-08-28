using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class Detalle
    {
        [Key]
        public int IdDetalle { get; set; }

        public int NroLinea { get; set; }

        public int IdProducto { get; set; }

        [Required, ForeignKey("IdProducto")]
        public Producto producto { get; set; }

        public decimal Cantidad { get; set; }
        public decimal Kilos { get; set; }

        public decimal? Descuento { get; set; }
        public decimal? Recargo { get; set; }
        public decimal MontoTotal { get; set; }

        [NotMapped]
        public decimal PrecioUnitario { get; set; }

        public override bool Equals(Object obj)
        {
            Detalle det = (Detalle)obj;
            return det.IdProducto == this.IdProducto;
        }
    }
}
