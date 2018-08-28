using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Dominio.Clases_Dominio;

namespace InterfazWeb
{
    public partial class Logon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbMensaje.Text = "";
                ddlEmisores.DataSource = Sistema.GetInstancia().ObtenerEmisores();
                ddlEmisores.DataTextField = "nomComercial";
                ddlEmisores.DataValueField = "IdEmisor";
                ddlEmisores.DataBind();

                String idEmisor = ddlEmisores.SelectedValue;
                if (!String.IsNullOrEmpty(idEmisor))
                {
                    try
                    {
                        int id = Int32.Parse(idEmisor);
                        DatosEmisor emisor = Sistema.GetInstancia().ObtenerDatosEmisorId(id);
                        txtRUC.Text = emisor.ruc;
                    }
                    catch { }
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                lbMensaje.Text = "";
                Usuario usu = Sistema.GetInstancia().ValidarUsuario(txtUsuario.Text, txtClave.Text);
                if (usu == null)
                {
                    lbMensaje.Text = "Usuario y/o clave incorrecto";
                }
                else
                {
                    Session["idUsuario"] = usu.IdUsuario;
                    String idEmisor = ddlEmisores.SelectedValue;
                    if (!String.IsNullOrEmpty(idEmisor))
                    {
                        try
                        {
                            int id = Int32.Parse(idEmisor);
                            DatosEmisor emisor = Sistema.GetInstancia().ObtenerDatosEmisorId(id);
                            Session["rut"] = emisor.ruc;

                        }
                        catch { }
                    }
                    bool actualizo = Sistema.GetInstancia().ActualizarSaldosClientes();
                    if (actualizo)
                    {
                        System.Web.Security.FormsAuthentication.RedirectFromLoginPage(usu.Nombre.ToString(), false);
                        //Response.Redirect("~/Maestros/Principal.aspx");
                    }
                    else {
                        lbMensaje.Text = "ERROR AL ACTUALIZAR SALDOS";
                    }
                }
            }
        }
    }
}