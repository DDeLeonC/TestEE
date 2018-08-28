using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class Parametros
    {
        [Key]
        public int IdParametros { get; set; }

        public int TasaMinima { get; set; }

        public int TasaBasica { get; set; }

        
    }
}
