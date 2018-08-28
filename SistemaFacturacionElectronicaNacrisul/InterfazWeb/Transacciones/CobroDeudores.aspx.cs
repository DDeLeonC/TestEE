using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;

using Dominio.Clases_Dominio;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using System.Text.RegularExpressions;


namespace InterfazWeb.Transacciones
{
    public partial class CobroDeudores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["rut"] == null || String.IsNullOrEmpty(Session["rut"].ToString()))
            {
                Session.Abandon();
                Response.Redirect("~/Logon.aspx");
            }
            if (!IsPostBack)
            {
                Pendientes.Visible = false;
                Importe.Visible = false;
                limpiarFormulario();

                List<String> lista = new List<String>();
                lista.Add("Pesos");
                lista.Add("Dolar");

                ddlMoneda.DataSource = lista;
                ddlMoneda.DataBind();

                ddlClientes.DataSource = Sistema.GetInstancia().ObtenerClientes();
                ddlClientes.DataTextField = "ddlDescription";
                ddlClientes.DataValueField = "IdCliente";
                ddlClientes.DataBind();
                ddlClientes.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Buscar Cliente...", "0"));
                //txtNroRecibo.Text = Sistema.GetInstancia().NroRecibo(Session["rut"].ToString()).ToString();
                // Sistema.GetInstancia().deuda = 0;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                limpiarFormulario();
                Sistema.GetInstancia().PDFActual = null;
                int idCliente = Int32.Parse(ddlClientes.SelectedValue);
                List<Dominio.Clases_Dominio.DeudaClientes> cuentas = Sistema.GetInstancia().ObtenerDeudaCliente(idCliente,Session["rut"].ToString(),ddlMoneda.SelectedValue);
                gridViewEstadoCuenta.DataSource = cuentas;
                gridViewEstadoCuenta.DataBind();
                if (cuentas != null && cuentas.Count > 0)
                {
                    Pendientes.Visible = true;
                    //MedioDePago.Visible = true;
                    Importe.Visible = true;
                    txtSaldoTotal.Text = cuentas.ElementAt(cuentas.Count - 1).SaldoTotal.ToString();
                }

            }
            catch { }
        }

        protected void gridViewDocumentos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //Add CSS class on header row.
                if (e.Row.RowType == DataControlRowType.Header)
                    e.Row.CssClass = "header";

                //Add CSS class on normal row.
                if (e.Row.RowType == DataControlRowType.DataRow &&
                          e.Row.RowState == DataControlRowState.Normal)
                    e.Row.CssClass = "normal";

                //Add CSS class on alternate row.
                if (e.Row.RowType == DataControlRowType.DataRow &&
                          e.Row.RowState == DataControlRowState.Alternate)
                    e.Row.CssClass = "alternate";


            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            try
            {

                gridViewEstadoCuenta.PageIndex = e.NewPageIndex;
                int idCliente = Int32.Parse(ddlClientes.SelectedValue);
                List<Dominio.Clases_Dominio.DeudaClientes> cuentas = Sistema.GetInstancia().ObtenerDeudaCliente(idCliente, Session["rut"].ToString(), ddlMoneda.SelectedValue);
                gridViewEstadoCuenta.DataSource = cuentas;
                gridViewEstadoCuenta.DataBind();
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        private void limpiarFormulario()
        {
            Pendientes.Visible = false;
            txtObservaciones.Text = String.Empty;
           // MedioDePago.Visible = false;
            Importe.Visible = false;
            gridViewEstadoCuenta.DataSource = null;
            gridViewEstadoCuenta.DataBind();
           // Sistema.GetInstancia().deuda = 0;
            //Sistema.GetInstancia().PDFActual = null;
            Session.Contents.Remove("pdf");
            txtSaldoTotal.Text = "";
        }

        protected void btnCobrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtNroRecibo.Text))
                {
                    CabezalRecibo rec = Sistema.GetInstancia().ObtenerReciboNro(txtNroRecibo.Text);
                    if (rec == null)
                    {
                        decimal importe = Convert.ToDecimal(txtSaldo.Text);

                        int idCliente = Int32.Parse(ddlClientes.SelectedValue);
                        if (idCliente != 0)
                        {
                            //int idMedio = Int32.Parse(ddlMedioPago.SelectedValue);
                            List<Dominio.Clases_Dominio.DeudaClientes> cuentas = Sistema.GetInstancia().ObtenerDeudaCliente(idCliente, Session["rut"].ToString(), ddlMoneda.SelectedValue);
                            String msg = Sistema.GetInstancia().CobroDeudores(cuentas, importe, idCliente, ddlMoneda.SelectedValue, txtNroRecibo.Text, Session["rut"].ToString(), txtObservaciones.Text);
                            if (msg.Equals("El pago se realizó correctamente"))
                            {
                                GenerarPDF(importe, txtNroRecibo.Text, txtObservaciones.Text);
                            }
                            else
                            {
                                string script = @"<script type='text/javascript'> alert('" + msg + "" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                        }
                        else
                        {
                            string script = @"<script type='text/javascript'> alert('Debe seleccionar cliente');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }

                    }
                    else
                    {
                        string script = @"<script type='text/javascript'> alert('" + "Ya existe un recibo con el numero ingresado" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                }
                else
                {
                    string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un numero de recibo" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
            }
            catch { }
        }

        private void GenerarPDF(decimal importe, string nroRecibo, string Observaciones)
        {
            try
            {
                Document doc = new Document(PageSize.LETTER);
                // Indicamos donde vamos a guardar el documento
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //PdfWriter writer = PdfWriter.GetInstance(doc,new FileStream(@"C:\recibo.pdf", FileMode.Create));
                doc.AddTitle("Recibo Pago");
                doc.AddCreator("E&E Integra");
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, ms);
                doc.Open();
                // Creamos el tipo de Font que vamos utilizar
                iTextSharp.text.Font _tituloFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font _tituloFont2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font _cuotaFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font texto = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                string imageURL = Server.MapPath("~//Imagenes") + "\\logo.png";
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_LEFT;
                jpg.ScaleAbsolute(177f, 100f);
                doc.Add(jpg);
                // Escribimos el encabezamiento en el documento
                Paragraph paragraph1 = new Paragraph(@"                            RUT: " + Session["rut"].ToString(), _tituloFont);
                paragraph1.Alignment = Element.ALIGN_JUSTIFIED;
                doc.Add(paragraph1);
                Paragraph paragraph2 = new Paragraph(@"                            Recibo Oficial",_tituloFont);
                paragraph2.Alignment = Element.ALIGN_JUSTIFIED;
                doc.Add(paragraph2);
                Paragraph paragraph3 = new Paragraph(@"                            Nro. Recibo: " + nroRecibo.ToString(), _tituloFont2);
                paragraph3.Alignment = Element.ALIGN_JUSTIFIED;
                doc.Add(paragraph3);

                Paragraph paragraph4 = new Paragraph(@"                            Moneda: " + ddlMoneda.SelectedValue, _tituloFont2);
                paragraph4.Alignment = Element.ALIGN_JUSTIFIED;
                doc.Add(paragraph4);

                Paragraph paragraph5 = new Paragraph(@"                            Importe " + importe, _tituloFont);
                paragraph5.Alignment = Element.ALIGN_JUSTIFIED;
                doc.Add(paragraph5);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);

                String nom = ddlClientes.SelectedItem.Text;
                Paragraph paragraph6 = new Paragraph(@"Recibimos de " +nom+" la cantidad de " + importe+ " por concepto de pago de deuda" );
                paragraph6.Alignment = Element.ALIGN_LEFT;
                doc.Add(paragraph6);
                doc.Add(Chunk.NEWLINE);

                Paragraph paragraph7 = new Paragraph(@"Observaciones:", _tituloFont);
                paragraph7.Alignment = Element.ALIGN_LEFT;
                doc.Add(paragraph7);
                String resultString = Regex.Replace(Observaciones, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                Paragraph paragraph8 = new Paragraph(@resultString, _cuotaFont);
                paragraph8.Alignment = Element.ALIGN_LEFT;
                doc.Add(paragraph8);
                doc.Add(Chunk.NEWLINE);


                Paragraph paragraph9 = new Paragraph(@"Paysandú, " + DateTime.Now.Day +" de " + MonthName(DateTime.Now.Month) + " de "+ DateTime.Now.Year);
                paragraph9.Alignment = Element.ALIGN_RIGHT;
                doc.Add(paragraph9);

                float antes = writer.GetVerticalPosition(true);
                float diff = antes - 353;
                for (int i = 0; i < diff / 20; i++)
                {
                    doc.Add(Chunk.NEWLINE);
                }

                doc.Add(jpg);
                doc.Add(paragraph1);
                doc.Add(paragraph2);
                doc.Add(paragraph3);
                doc.Add(paragraph4);
                
                doc.Add(paragraph5);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                
                doc.Add(paragraph6);
                doc.Add(Chunk.NEWLINE);
                doc.Add(paragraph7);
                doc.Add(paragraph8);
                doc.Add(Chunk.NEWLINE);
                doc.Add(paragraph9);

                doc.Close();
                byte[] Result = ms.ToArray();
                writer.Close();
                Sistema.GetInstancia().PDFActual = Result;
                //Sistema.GetInstancia().AumentarNroRecibo(Session["rut"].ToString());
                // Session["pdf"] = Result;
                if (Sistema.GetInstancia().PDFActual != null)
                {
                    //Response.Redirect("VisorPDF.aspx");
                    Response.Write("<script>");
                    Response.Write("window.open('CobroDeudores.aspx', '_blank');");
                    Response.Write("window.location.href = 'VisorPDF.aspx';");
                    Response.Write("</script>");
                    
                }
                
            }
            catch { }
            
        }

        public string MonthName(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetMonthName(month);
        }
        
    }
}