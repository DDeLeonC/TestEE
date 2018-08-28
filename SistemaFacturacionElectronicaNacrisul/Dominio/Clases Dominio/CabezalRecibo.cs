using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class CabezalRecibo
    {
        [Key]
        public int IdRecibo { get; set; }

        public DateTime Fecha { get; set; }

        public int Numero { get; set; }

        public String Moneda { get; set; }

        public String rut { get; set; }

        public decimal Importe { get; set; }

        public decimal ImporteAsignado { get; set; }

        public int IdCliente { get; set; }

        [Required, ForeignKey("IdCliente")]
        public Cliente cliente { get; set; }

        public virtual List<LineaRecibo> lineas { get; set; }

        public bool NotaCredito { get; set; }

        public int IdNotaCredito { get; set; }

        public bool Anulado { get; set; }

        public string Observaciones { get; set; }
        public string MotivoAnulacion { get; set; }

        public string StrAnulado
        {
            get
            {
                if (Anulado)
                {
                    return "SI";
                }
                else
                {
                    return "NO";
                }
            }
        }

        public Documento getNotaCredito()
        {
            Documento doc = Sistema.GetInstancia().ObtenerDocumentoId(IdNotaCredito);
            return doc;
        }
    }
}
