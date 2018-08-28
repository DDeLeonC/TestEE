using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Dominio;

namespace InterfazWeb.Reportes
{
    public partial class webFormExcel : System.Web.UI.Page
    {
        public void Mostrar()
        {
            var context = HttpContext.Current;
            context.Response.Clear();

            byte[] pdf = Sistema.GetInstancia().ExcelActual;
            context.Response.ContentType = "application/ms-excel";
            context.Response.AddHeader("content-length", pdf.Length.ToString());
            context.Response.BinaryWrite(pdf);

            context.Response.Close();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Mostrar();
            }

        }
    }
}