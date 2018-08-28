using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class DatosEmisor
    {
        [Key]
        public int IdEmisor { get; set; }

        public String ruc { get; set; }

        [Required]
        public String razonSocial { get; set; }

        
        [Required]
        public String nomComercial { get; set; }

        
        public String Correo { get; set; }

        [Required]
        public String Sucursal { get; set; }

        [Required]
        public String CodigoSucursal { get; set; }

        [Required]
        public String DomicilioFiscal { get; set; }

        [Required]
        public String Ciudad { get; set; }

        
        public String Giro { get; set; }

        [Required]
        public String Departamento { get; set; }

        public String[] Telefonos { get; set; }

        public int ultimoDocumento { get; set; }

        public int NroRecibo { get; set; }
    }
}
