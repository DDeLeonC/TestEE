using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Dominio;

namespace InterfazWeb.Transacciones
{
    public partial class webformPDF : System.Web.UI.Page
    {
        public void Mostrar()
        {
            var context = HttpContext.Current;
            context.Response.Clear();
            byte[] pdf = Sistema.GetInstancia().PDFActual;
            if (pdf != null)
            {
                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("content-length", pdf.Length.ToString());
                context.Response.BinaryWrite(pdf);

                context.Response.End();
            }
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