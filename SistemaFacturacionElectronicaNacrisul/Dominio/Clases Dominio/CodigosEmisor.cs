using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class CodigosEmisor
    {
        [Key]
        public int IdCodigoEmisor { get; set; }

        public String rut { get; set; }

        public String CodTerminal { get; set; }

        public String CodComercio { get; set; }

        public String Contrasena { get; set; }
    }
}
