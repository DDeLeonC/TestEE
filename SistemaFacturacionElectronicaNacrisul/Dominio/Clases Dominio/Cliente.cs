using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        [Required]
        public String tipoDocumento { get; set; }
        
        public String nroDoc { get; set; }

        [Required]
        public String Nombre { get; set; }

        public int IdPais{ get; set; }

        [Required, ForeignKey("IdPais")]
        public Pais Pais { get; set; }

        public String Direccion { get; set; }

        public String Ciudad { get; set; }

        public String CodigoPostal { get; set; }

        public String Mail { get; set; }

        public String rut { get; set; }

        public long? Tel { get; set; }

        [Required]
        public bool Activo { get; set; }

        public int IdZona { get; set; }

        [Required, ForeignKey("IdZona")]
        public Zona Zona { get; set; }

        public int IdVendedor { get; set; }

        [Required, ForeignKey("IdVendedor")]
        public Vendedor Vendedor { get; set; }

        public override bool Equals(Object obj)
        {
            Cliente cli = (Cliente)obj;
            return cli.IdCliente == this.IdCliente;
        }

        public override string ToString()
        {
            return Nombre;
        }

        public string ddlDescription
        {
            get
            {
                return string.Format("{0} - {1}", nroDoc, Nombre);
            }
        }
    }
}
