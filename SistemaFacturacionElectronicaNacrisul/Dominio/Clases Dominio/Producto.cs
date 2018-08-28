using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required, MaxLength(20)]
        public String Codigo { get; set; }

        [Required]
        public String Nombre { get; set; }

        public String Descripcion { get; set; }

        public String unidadMedida { get; set; }
        public decimal Precio { get; set; }

        public int IdSubGrupo { get; set; }

        [Required, ForeignKey("IdSubGrupo")]
        public SubGrupo SubGrupo { get; set; }

        public int IdIndicador { get; set; }

        public String rut { get; set; }

        [Required, ForeignKey("IdIndicador")]
        public IndicadorFacturacion indicador { get; set; }

        [Required]
        public bool Activo { get; set; }

        public override bool Equals(Object obj)
        {
            Producto prod = (Producto)obj;
            return prod.IdProducto == this.IdProducto;
        }

        public string ddlDescription
        {
            get
            {
                return string.Format("{0} - {1}", Codigo, Nombre);
            }
        }
    }
}
