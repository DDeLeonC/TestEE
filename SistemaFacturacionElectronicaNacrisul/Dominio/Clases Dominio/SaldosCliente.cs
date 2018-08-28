using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class SaldosCliente
    {
        [Key]
        public int IdSaldoCliente { get; set; }

        public int Año { get; set; }

        public int Mes { get; set; }

        public decimal Debe { get; set; }

        public decimal Haber { get; set; }  

        public int IdCliente { get; set; }

        [Required, ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }
    }
}
