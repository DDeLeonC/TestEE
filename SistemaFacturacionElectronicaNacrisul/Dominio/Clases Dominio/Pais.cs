using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;


namespace Dominio.Clases_Dominio
{
    public class Pais
    {
        [Key]
        public int IdPais { get; set; }

        public String Codigo { get; set; }

        public String Codigo3 { get; set; }

        [Required]
        public String Nombre { get; set; }

        public override String ToString()
        {
            
            return Nombre;
        }
    }
}
