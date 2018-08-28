using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Dominio.Clases_Dominio;

namespace Interfaz_Web.Maestros
{
    public partial class NuevoSubGrupo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                List<Grupo> grupos = Sistema.GetInstancia().ObtenerGrupos(Session["rut"].ToString());
                ddlGrupo.DataSource = grupos;
                ddlGrupo.DataTextField = "Descripcion";
                ddlGrupo.DataValueField = "IdGrupo";
                ddlGrupo.DataBind();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Dominio.Clases_Dominio.SubGrupo grupo = new Dominio.Clases_Dominio.SubGrupo();
                grupo.Codigo = txtCodigo.Text;
                grupo.Descripcion = txtNombre.Text;
                grupo.IdGrupo = Int32.Parse(ddlGrupo.SelectedValue);
                grupo.rut = Session["rut"].ToString();
                String msg = Sistema.GetInstancia().GuardarSubGrupo(grupo);
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
            ((DropDownList)ddlGrupo).SelectedValue = null;
            txtCodigo.Focus();
        }
    }
}