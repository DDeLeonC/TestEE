using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Sockets;

namespace Interfaz_Web.MasterPages
{
    public partial class Reportes : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["idUsuario"] == null)
                {
                    Response.Redirect("~/Logon.aspx");
                }
            }
            lblIpTitle.Text = LocalIPAddress().ToString();
        }

        protected void lbCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Logon.aspx");
        }

        protected void lnkMaestros_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Maestros/ListaMaestros.aspx");
        }

        protected void lnkTransacciones_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Transacciones/ListaTransacciones.aspx");
        }

        protected void lnkReportes_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Reportes/ListaReportes.aspx");
        }

        protected IPAddress LocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}