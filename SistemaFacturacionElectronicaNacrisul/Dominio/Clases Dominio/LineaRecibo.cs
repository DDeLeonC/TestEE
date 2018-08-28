using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class LineaRecibo
    {
        [Key]
        public int IdLineaRecibo { get; set; }

        public decimal ImportePagado { get; set; }

        public int IdDocumento { get; set; }

        [Required, ForeignKey("IdDocumento")]
        public Documento Documento { get; set; }
    }
}
