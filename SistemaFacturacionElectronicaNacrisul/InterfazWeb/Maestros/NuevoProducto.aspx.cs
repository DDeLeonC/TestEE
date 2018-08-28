using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Dominio.Clases_Dominio;

namespace InterfazWeb.Maestros
{
    public partial class NuevoProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                List<Grupo> grupos = Sistema.GetInstancia().ObtenerGrupos(Session["rut"].ToString());
                ddlGrupo.DataSource = grupos;
                ddlGrupo.DataTextField = "Descripcion";
                ddlGrupo.DataValueField = "IdGrupo";
                ddlGrupo.DataBind();

                if (grupos != null && grupos.Count > 0)
                {
                    ddlSubGrupo.DataSource = Sistema.GetInstancia().ObtenerSubGruposGrupo(grupos.ElementAt(0).IdGrupo, Session["rut"].ToString());
                    ddlSubGrupo.DataTextField = "Descripcion";
                    ddlSubGrupo.DataValueField = "IdSubGrupo";
                    ddlSubGrupo.DataBind();
                }

                List<IndicadorFacturacion> indicadores = Sistema.GetInstancia().ObtenerIndicadores();
                ddlIndicador.DataSource = indicadores;
                ddlIndicador.DataTextField = "Nombre";
                ddlIndicador.DataValueField = "IdIndicador";
                ddlIndicador.DataBind();

                
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Dominio.Clases_Dominio.Producto producto = new Dominio.Clases_Dominio.Producto();
                producto.Codigo = txtCodigo.Text;
                producto.Nombre = txtNombre.Text;
                producto.IdSubGrupo = Int32.Parse(ddlSubGrupo.SelectedValue);
                producto.Descripcion = txtDescripcion.Text;
                producto.IdIndicador = Int32.Parse(ddlIndicador.SelectedValue);
                producto.Precio = decimal.Parse(txtPrecio.Text);
                producto.unidadMedida = txtUnidad.Text;
                producto.rut = Session["rut"].ToString();
                String msg = Sistema.GetInstancia().GuardarProducto(producto);
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
            ((DropDownList)ddlSubGrupo).SelectedValue = null;
            ((DropDownList)ddlIndicador).SelectedValue = null;
            ((TextBox)txtDescripcion).Text = string.Empty;
            ((TextBox)txtPrecio).Text = string.Empty;
            ((TextBox)txtUnidad).Text = string.Empty;
            txtCodigo.Focus();
        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGrupo.SelectedValue != null)
            {
                int IdGrupo = Int32.Parse(ddlGrupo.SelectedValue);
                List<SubGrupo> subgrupos = Sistema.GetInstancia().ObtenerSubGruposGrupo(IdGrupo, Session["rut"].ToString()).ToList();
                ddlSubGrupo.DataSource = subgrupos;
                ddlSubGrupo.DataTextField = "Descripcion";
                ddlSubGrupo.DataValueField = "IdSubGrupo";
                ddlSubGrupo.DataBind();
            }

        }
    }
}