using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;


namespace InterfazWeb.Maestros
{
    public partial class NuevoVendedor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Dominio.Clases_Dominio.Vendedor vendedor = new Dominio.Clases_Dominio.Vendedor();
                vendedor.Codigo = txtCodigo.Text;
                vendedor.Nombre = txtNombre.Text;
                String msg = Sistema.GetInstancia().GuardarVendedor(vendedor);
                string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                limpiarFomulario();

            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al guardar" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }
        protected void limpiarFomulario()
        {
            ((TextBox)txtCodigo).Text = string.Empty;
            ((TextBox)txtNombre).Text = string.Empty;

            txtCodigo.Focus();
        }
    }
}