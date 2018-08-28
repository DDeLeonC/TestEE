using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Dominio.Clases_Dominio;
using System.IO;
using System.Drawing;
using InterfazWeb.ServiceReference2Prod;

using System.Data;
using ClosedXML.Excel;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using System.Text.RegularExpressions;

namespace InterfazWeb.Reportes
{
    public partial class ConsultaRecibos : System.Web.UI.Page
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
                Detalle.Visible = false;
                ddlCliente.DataSource = Sistema.GetInstancia().ObtenerClientes();
                ddlCliente.DataTextField = "ddlDescription";
                ddlCliente.DataValueField = "IdCliente";
                ddlCliente.DataBind();

                ddlCliente.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos"));
                DateTime dateValue = DateTime.Now;
                txtFechaDesde.Text = dateValue.ToShortDateString();
                txtFechaHasta.Text = dateValue.AddHours(23).ToShortDateString();

                List<Zona> zonas = Sistema.GetInstancia().ObtenerZonas();
                ddlZonas.DataSource = zonas;
                ddlZonas.DataTextField = "Nombre";
                ddlZonas.DataValueField = "IdZona";
                ddlZonas.DataBind();

                ddlZonas.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todas"));
                

                List<Vendedor> vendedores = Sistema.GetInstancia().ObtenerVendedores();
                ddlVendedores.DataSource = vendedores;
                ddlVendedores.DataTextField = "Nombre";
                ddlVendedores.DataValueField = "IdVendedor";
                ddlVendedores.DataBind();

                ddlVendedores.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos"));

                List<String> lista = new List<String>();
                lista.Add("Activos");
                lista.Add("Anulados");
                lista.Add("Todos");

                ddlEstadoRecibo.DataSource = lista;
                ddlEstadoRecibo.DataBind();

                BindData();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            try
            {
                Detalle.Visible = false;
                int? idCliente = null;
                if (!String.IsNullOrEmpty(ddlCliente.SelectedValue) && ddlCliente.SelectedIndex != 0)
                {
                    idCliente = Int32.Parse(ddlCliente.SelectedValue);
                }
                bool error = false;
                int? nro = null;
                int? idVendedor = null;
                int? idZona = null;
                if (!String.IsNullOrEmpty(ddlVendedores.SelectedValue) && ddlVendedores.SelectedIndex != 0)
                {
                    idVendedor = Int32.Parse(ddlVendedores.SelectedItem.Value);
                }
                if (!String.IsNullOrEmpty(ddlZonas.SelectedValue) && ddlZonas.SelectedIndex != 0)
                {
                    idZona = Int32.Parse(ddlZonas.SelectedItem.Value);
                }
                try
                {
                    if (!String.IsNullOrEmpty(txtNroSerie.Text))
                    {
                        nro = Int32.Parse(txtNroSerie.Text);
                    }
                }
                catch
                {
                    error = true;
                    string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un entero en el campo Numero" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                if (!error)
                {
                    gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerRecibos(idCliente, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), nro, Session["rut"].ToString(), ddlEstadoRecibo.SelectedValue, idVendedor, idZona);
                    gridViewDocumentos.DataBind();
                }
            }
            catch (Exception ex)
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
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

        private void ExportGridToExcel()
        {

            try
            {
                
                gridViewDocumentos.AllowPaging = false;
                BindData();
                DataTable dt = new DataTable("GridView_Data");
                foreach (TableCell cell in gridViewDocumentos.HeaderRow.Cells)
                {
                    if (!cell.Text.Equals("&nbsp;") && !cell.Text.Equals("Id"))
                    {
                        if (cell.Text.Equals("Monto Total"))
                        {
                            DataColumn colDecimal = new DataColumn(cell.Text);
                            colDecimal.DataType = System.Type.GetType("System.Decimal");
                            dt.Columns.Add(colDecimal);

                        }
                        else
                        {
                            dt.Columns.Add(cell.Text);
                        }
                    }
                }
                foreach (GridViewRow row in gridViewDocumentos.Rows)
                {
                    DataRow dr = dt.NewRow();
                    Label lblFecha = row.FindControl("lblFecha") as Label;
                    if (lblFecha != null)
                    {
                        dr[0] = lblFecha.Text;
                    }
                    Label lblCliente = row.FindControl("lblCliente") as Label;
                    if (lblCliente != null)
                    {
                        dr[1] = lblCliente.Text;
                    }
                    Label lblNroSerie = row.FindControl("lblNroSerie") as Label;
                    if (lblNroSerie != null)
                    {
                        dr[2] = lblNroSerie.Text;
                    }
                    Label lblAnulado = row.FindControl("lblAnulado") as Label;
                    if (lblAnulado != null)
                    {
                        dr[3] = lblAnulado.Text;
                    }
                    Label lblMoneda = row.FindControl("lblMoneda") as Label;
                    if (lblMoneda != null)
                    {
                        dr[4] = lblMoneda.Text;
                    }
                    Label lblmontoTotal = row.FindControl("lblmontoTotal") as Label;
                    if (lblmontoTotal != null)
                    {
                        decimal montoTotal = Convert.ToDecimal(lblmontoTotal.Text);
                        dr[5] = montoTotal;
                    }
                    dt.Rows.Add(dr);

                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Recibos.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                gridViewDocumentos.AllowPaging = true;
                BindData();
            }
            catch { }

        }

        protected void exportar_Click(object sender, EventArgs e)
        {
            ExportGridToExcel();

        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gridViewDocumentos.PageIndex = e.NewPageIndex;
                BindData();
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            gridViewDocumentos.AllowPaging = false;
            BindData();
            Document doc = new Document(PageSize.LETTER);
            doc.AddTitle("Consulta Recibos");
            doc.AddCreator("E&E Integra");
            System.IO.MemoryStream stream = new System.IO.MemoryStream();

            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, stream);
            doc.Open();
            // Creamos el tipo de Font que vamos utilizar
            iTextSharp.text.Font _tituloFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font _tituloFont2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font texto = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Escribimos el encabezamiento en el documento
            Paragraph paragraph1 = new Paragraph(@"                                 CONSULTA RECIBOS", _tituloFont);
            paragraph1.Alignment = Element.ALIGN_LEFT;
            doc.Add(paragraph1);
            Paragraph paragraph2 = new Paragraph(@"                                                    ", _tituloFont);
            paragraph2.Alignment = Element.ALIGN_LEFT;
            doc.Add(paragraph2);

            //Creating iTextSharp Table from the DataTable data
            PdfPTable pdfTable = new PdfPTable(5);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;
            iTextSharp.text.Font _cellFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            //Adding Header row
            pdfTable.AddCell("Fecha");
            pdfTable.AddCell("Numero");
            pdfTable.AddCell("Cliente");
            pdfTable.AddCell("Moneda");
            pdfTable.AddCell("Monto Total");

            //Adding DataRow
            foreach (GridViewRow row in gridViewDocumentos.Rows)
            {
                Label lblFecha = row.FindControl("lblFecha") as Label;
                if (lblFecha != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblFecha.Text, _cellFont)));
                }
                Label lblNumero = row.FindControl("lblNroSerie") as Label;
                if (lblNumero != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblNumero.Text, _cellFont)));
                }
                Label lblCliente = row.FindControl("lblCliente") as Label;
                if (lblCliente != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblCliente.Text, _cellFont)));
                }
                Label lblMoneda = row.FindControl("lblMoneda") as Label;
                if (lblMoneda != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblMoneda.Text, _cellFont)));
                }
                Label lblMontoTotal = row.FindControl("lblMontoTotal") as Label;
                if (lblMontoTotal != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblMontoTotal.Text, _cellFont)));
                }
            }

            doc.Add(pdfTable);
            doc.Close();
            stream.Close();
            byte[] Result = stream.ToArray();
            Sistema.GetInstancia().PDFActual = Result;
            // Session["pdf"] = Result;
            if (Sistema.GetInstancia().PDFActual != null)
            {
                Response.Redirect("VisorPDFReportes.aspx");
            }
        }

        protected void VerPDF(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkRemove = (LinkButton)sender;
                String id = lnkRemove.CommandArgument;
                CabezalRecibo recibo = Sistema.GetInstancia().ObtenerReciboId(Int32.Parse(id));
                if (!recibo.Equals(null))
                {
                    GenerarPDF(recibo);
                }
            }
            catch
            {

            }
        }

        private void GenerarPDF(CabezalRecibo recibo)
        {
            try
            {
                gridViewDocumentos.AllowPaging = false;
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
                iTextSharp.text.Font texto = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font cuota_font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                string imageURL = Server.MapPath("~//Imagenes") + "\\logo.png";
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.Alignment = iTextSharp.text.Image.TEXTWRAP | iTextSharp.text.Image.ALIGN_LEFT;
                jpg.ScaleAbsolute(177f, 100f);
                doc.Add(jpg);
                // Escribimos el encabezamiento en el documento
                Paragraph paragraph1 = new Paragraph(@"                            RUT: " + Session["rut"].ToString(), _tituloFont);
                paragraph1.Alignment = Element.ALIGN_JUSTIFIED;
                doc.Add(paragraph1);
                Paragraph paragraph2 = new Paragraph(@"                            Recibo Oficial", _tituloFont);
                paragraph2.Alignment = Element.ALIGN_JUSTIFIED;
                doc.Add(paragraph2);
                Paragraph paragraph3 = new Paragraph(@"                            Nro. Recibo: " + recibo.Numero.ToString(), _tituloFont2);
                paragraph3.Alignment = Element.ALIGN_JUSTIFIED;
                doc.Add(paragraph3);

                Paragraph paragraph4 = new Paragraph(@"                            Moneda: " + recibo.Moneda, _tituloFont2);
                paragraph4.Alignment = Element.ALIGN_JUSTIFIED;
                doc.Add(paragraph4);

                Paragraph paragraph5 = new Paragraph(@"                            Importe " + recibo.Importe, _tituloFont);
                paragraph5.Alignment = Element.ALIGN_JUSTIFIED;
                doc.Add(paragraph5);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);

                String nom = recibo.cliente.Nombre;
                Paragraph paragraph6 = new Paragraph(@"Recibimos de " + nom + " la cantidad de " + recibo.Importe + " por concepto de pago de deuda");
                paragraph6.Alignment = Element.ALIGN_LEFT;
                doc.Add(paragraph6);
                doc.Add(Chunk.NEWLINE);

                Paragraph paragraph7 = new Paragraph(@"Observaciones:", _tituloFont);
                paragraph7.Alignment = Element.ALIGN_LEFT;
                doc.Add(paragraph7);
                String resultString = "";
                if (!String.IsNullOrEmpty(recibo.Observaciones))
                {
                    resultString = Regex.Replace(recibo.Observaciones, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                }
                
                Paragraph paragraph8 = new Paragraph(@resultString, cuota_font);
                paragraph8.Alignment = Element.ALIGN_LEFT;
                doc.Add(paragraph8);
                if (!String.IsNullOrEmpty(recibo.MotivoAnulacion))
                {
                    Paragraph paragraph9 = new Paragraph(@"Motivo Anulación:", _tituloFont);
                    paragraph9.Alignment = Element.ALIGN_LEFT;
                    doc.Add(paragraph9);
                    String anulacionString = Regex.Replace(recibo.MotivoAnulacion, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                    Paragraph paragraph10 = new Paragraph(@anulacionString, cuota_font);
                    paragraph10.Alignment = Element.ALIGN_LEFT;
                    doc.Add(paragraph10);
                }
                doc.Add(Chunk.NEWLINE);


                Paragraph paragraph11 = new Paragraph(@"Paysandú, " + recibo.Fecha.Day + " de " + MonthName(recibo.Fecha.Month) + " de " + recibo.Fecha.Year);
                paragraph11.Alignment = Element.ALIGN_RIGHT;
                doc.Add(paragraph11);

                float antes = writer.GetVerticalPosition(true);
                float diff = antes - 353;
                for (int i = 0; i < diff / 20; i++)
                {
                    doc.Add(Chunk.NEWLINE);
                }
                doc.Add(jpg);
                doc.Add(Chunk.NEWLINE);
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
                if (!String.IsNullOrEmpty(recibo.MotivoAnulacion))
                {
                    Paragraph paragraph9 = new Paragraph(@"Motivo Anulación:", _tituloFont);
                    paragraph9.Alignment = Element.ALIGN_LEFT;
                    doc.Add(paragraph9);
                    String anulacionString = Regex.Replace(recibo.MotivoAnulacion, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                    Paragraph paragraph10 = new Paragraph(@anulacionString, cuota_font);
                    paragraph10.Alignment = Element.ALIGN_LEFT;
                    doc.Add(paragraph10);
                }
                doc.Add(Chunk.NEWLINE);
                doc.Add(paragraph11);

                doc.Close();
                byte[] Result = ms.ToArray();
                writer.Close();
                Sistema.GetInstancia().PDFActual = Result;
                Sistema.GetInstancia().AumentarNroRecibo(Session["rut"].ToString());
                // Session["pdf"] = Result;
                gridViewDocumentos.AllowPaging = true;
                if (Sistema.GetInstancia().PDFActual != null)
                {
                    //Response.Redirect("VisorPDFReportes.aspx");
                    Response.Write("<script>");
                    Response.Write("window.open('VisorPDFReportes.aspx', '_blank');");
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