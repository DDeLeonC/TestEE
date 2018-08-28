using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Dominio.Clases_Dominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Clases_Dominio
{
    public class Documento
    {
        [Key]
        public int IdDocumento { get; set; }

        public DateTime Fecha { get; set; }

        public String Moneda { get; set; }

        public decimal? tipoCambio { get; set; }

        //public decimal? montoTotal { get; set; }

        public String FormaPago { get; set; }

        public decimal Total { get; set; }

        public decimal Redondeo { get; set; }

        public String LugarDestino { get; set; }

        public String TipoTraslado { get; set; }

        public int IdCliente { get; set; }

        [Required, ForeignKey("IdCliente")]
        public Cliente cliente { get; set; }

        public virtual List<Detalle> detalle { get; set; }

        [Required]
        public String TipoDocumento { get; set; }

        [Required]
        public String Serie { get; set; }

        [Required]
        public int NroSerie { get; set; }

        public String EstadoDGI { get; set; }

        public String MotivoRechazo { get; set; }

        public virtual List<Documento> documentosAsociados { get; set; }

        public virtual List<RetencionPercepcionResguardos> retenciones { get; set; }

        public String xmlFirmado { get; set; }

        [Required]
        public bool Activo { get; set; }

        public String rut { get; set; }

        public int IdUsuario { get; set; }

        [Required, ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }

        public String EstadoCredito { get; set; }

        public decimal MontoPagado { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public String DBCR { get; set; }

        public String NroGuia { get; set; }

        public String NroRemito { get; set; }

        public override bool Equals(Object obj)
        {
            Documento fac = (Documento)obj;
            return fac.IdDocumento == this.IdDocumento;
        }

        

        [NotMapped]
        public bool Asociar { get; set; }
    }
}
