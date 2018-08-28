using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Dominio.Clases_Dominio;

namespace InterfazWeb.Transacciones
{
    public partial class AnularRecibo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtNroRecibo.Text))
                {
                    CabezalRecibo recibo = Sistema.GetInstancia().ObtenerRecibo(Int32.Parse(txtNroRecibo.Text));
                    if (recibo != null)
                    {
                        lblCliente.Text = "Cliente: " + recibo.cliente.ddlDescription;
                        lblFecha.Text = "Fecha: " + recibo.Fecha.ToString();
                        lblMonto.Text = "Importe: " + recibo.Importe.ToString();
                        lblNroRecibo.Text = txtNroRecibo.Text;
                        Pendientes.Visible = true;
                        if (recibo.Anulado)
                        {
                            pnlAnular.Visible = false;
                            lblEstadoRecibo.Text = "Estado: ANULADO";
                            lblEstadoRecibo.CssClass = "red_font";
                        }
                        else
                        {
                            pnlAnular.Visible = true;
                            lblEstadoRecibo.Text = "Estado: ACTIVO";
                            lblEstadoRecibo.CssClass = "";
                        }
                    }
                    else
                    {
                        Pendientes.Visible = false;
                    }
                }
            }
            catch { }
        }
        protected void btnAnular_Click(object sender, EventArgs e)
        {
            try
            {
                String NroRecibo = lblNroRecibo.Text;
                if (!String.IsNullOrEmpty(NroRecibo))
                {
                    String msj = Sistema.GetInstancia().AnularRecibo(NroRecibo, txtObservaciones.Text);
                }
            }
            catch { }
        }
    }
}