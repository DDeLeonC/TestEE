using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class StockProducto
    {
        [Key]
        public int IdStock { get; set; }

        public int IdProducto { get; set; }

        [Required, ForeignKey("IdProducto")]
        public Producto Producto { get; set; }

        public decimal Kilos { get; set; }

        public decimal Cantidad { get; set; }
    }
}
