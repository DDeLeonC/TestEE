using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Dominio.Clases_Dominio;
using ClosedXML.Excel;
using System.IO;

using System.Data;

using System.Reflection;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace InterfazWeb.Reportes
{
    public partial class ReporteDocumentosVencidos : System.Web.UI.Page
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
                List<String> lista = new List<String>();
                lista.Add("Pesos");
                lista.Add("Dolar");

                ddlMoneda.DataSource = lista;
                ddlMoneda.DataBind();

                List<DocumentoVencido> documentos = Sistema.GetInstancia().ObtenerDocumentosVencidos(ddlMoneda.SelectedValue, Session["rut"].ToString());
                gridViewDocumentos.DataSource = documentos;
                gridViewDocumentos.DataBind();

                if (documentos != null && documentos.Count > 0) {
                    DocumentoVencido doc = documentos.ElementAt(documentos.Count - 1);
                    txtSaldoTotal.Text = doc.SumaMontos.ToString();
                }

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                List<DocumentoVencido> documentos = Sistema.GetInstancia().ObtenerDocumentosVencidos(ddlMoneda.SelectedValue, Session["rut"].ToString());
                gridViewDocumentos.DataSource = documentos;
                gridViewDocumentos.DataBind();

                if (documentos != null && documentos.Count > 0)
                {
                    DocumentoVencido doc = documentos.ElementAt(documentos.Count - 1);
                    txtSaldoTotal.Text = doc.SumaMontos.ToString();
                }
            }
            catch (Exception ex)
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        private void BindData()
        {
            try
            {
                List<DocumentoVencido> documentos = Sistema.GetInstancia().ObtenerDocumentosVencidos(ddlMoneda.SelectedValue, Session["rut"].ToString());
                gridViewDocumentos.DataSource = documentos;
                gridViewDocumentos.DataBind();

                if (documentos != null && documentos.Count > 0)
                {
                    DocumentoVencido doc = documentos.ElementAt(documentos.Count - 1);
                    txtSaldoTotal.Text = doc.SumaMontos.ToString();
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
                        else if (cell.Text.Equals("Monto Pagado"))
                        {
                            DataColumn colDecimal = new DataColumn(cell.Text);
                            colDecimal.DataType = System.Type.GetType("System.Decimal");
                            dt.Columns.Add(colDecimal);
                        }
                        else if (cell.Text.Equals("Saldo"))
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
                    Label lblFechaVencimiento = row.FindControl("lblFechaVencimiento") as Label;
                    if (lblFechaVencimiento != null)
                    {
                        dr[1] = lblFechaVencimiento.Text;
                    }
                    Label lblCliente = row.FindControl("lblCliente") as Label;
                    if (lblCliente != null)
                    {
                        dr[2] = lblCliente.Text;
                    }
                    Label lblTipoDocumento = row.FindControl("lblTipoDocumento") as Label;
                    if (lblTipoDocumento != null)
                    {
                        dr[3] = lblTipoDocumento.Text;
                    }
                    
                    Label lblSerie = row.FindControl("lblSerie") as Label;
                    if (lblSerie != null)
                    {
                        dr[4] = lblSerie.Text;
                    }
                    Label lblNroSerie = row.FindControl("lblNroSerie") as Label;
                    if (lblNroSerie != null)
                    {
                        dr[5] = lblNroSerie.Text;
                    }
                    Label lblMoneda = row.FindControl("lblMoneda") as Label;
                    if (lblMoneda != null)
                    {
                        dr[6] = lblMoneda.Text;
                    }
                    Label lblMontoTotal = row.FindControl("lblMontoTotal") as Label;
                    if (lblMontoTotal != null)
                    {
                        decimal vidaUtil = Convert.ToDecimal(lblMontoTotal.Text);
                        dr[7] = vidaUtil;
                    }
                    Label lblMontoPagado = row.FindControl("lblMontoPagado") as Label;
                    if (lblMontoPagado != null)
                    {
                        decimal vidaUtil = Convert.ToDecimal(lblMontoPagado.Text);
                        dr[8] = vidaUtil;
                    }
                    Label lblSaldo = row.FindControl("lblSaldo") as Label;
                    if (lblSaldo != null)
                    {
                        decimal vidaUtil = Convert.ToDecimal(lblSaldo.Text);
                        dr[9] = vidaUtil;
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
                    Response.AddHeader("content-disposition", "attachment;filename=ListadoDocumentosVencidos.xlsx");
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
            doc.AddTitle("Estado de Cuenta");
            doc.AddCreator("E&E Integra");
            System.IO.MemoryStream stream = new System.IO.MemoryStream();

            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, stream);
            doc.Open();
            // Creamos el tipo de Font que vamos utilizar
            iTextSharp.text.Font _tituloFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font _tituloFont2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font texto = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            // Escribimos el encabezamiento en el documento
            Paragraph paragraph1 = new Paragraph(@"                         DOCUMENTOS VENCIDOS", _tituloFont);
            paragraph1.Alignment = Element.ALIGN_LEFT;
            doc.Add(paragraph1);
            Paragraph paragraph2 = new Paragraph(@"                                                    ", _tituloFont);
            paragraph2.Alignment = Element.ALIGN_LEFT;
            doc.Add(paragraph2);

            //Creating iTextSharp Table from the DataTable data
            PdfPTable pdfTable = new PdfPTable(10);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;

            //Adding Header row
            pdfTable.AddCell("Fecha");
            pdfTable.AddCell("Fecha Venc.");
            pdfTable.AddCell("Cliente");
            pdfTable.AddCell("Tipo Documento");
            pdfTable.AddCell("Serie");
            pdfTable.AddCell("Nro. Serie");
            pdfTable.AddCell("Moneda");
            pdfTable.AddCell("Monto Total");
            pdfTable.AddCell("Monto Pagado");
            pdfTable.AddCell("Saldo");

            //Adding DataRow
            foreach (GridViewRow row in gridViewDocumentos.Rows)
            {
                Label lblFecha = row.FindControl("lblFecha") as Label;
                if (lblFecha != null)
                {
                    pdfTable.AddCell(lblFecha.Text);
                }
                else
                {
                    pdfTable.AddCell("");
                }
                Label lblFechaVencimiento = row.FindControl("lblFechaVencimiento") as Label;
                if (lblFechaVencimiento != null)
                {
                    pdfTable.AddCell(lblFechaVencimiento.Text);
                }
                else
                {
                    pdfTable.AddCell("");
                }
                Label lblCliente = row.FindControl("lblCliente") as Label;
                if (lblCliente != null)
                {
                    pdfTable.AddCell(lblCliente.Text);
                }
                else
                {
                    pdfTable.AddCell("");
                }
                Label lblTipoDocumento = row.FindControl("lblTipoDocumento") as Label;
                if (lblTipoDocumento != null)
                {
                    pdfTable.AddCell(lblTipoDocumento.Text);
                }
                else
                {
                    pdfTable.AddCell("");
                }
                Label lblSerie = row.FindControl("lblSerie") as Label;
                if (lblSerie != null)
                {
                    pdfTable.AddCell(lblSerie.Text);
                }
                else
                {
                    pdfTable.AddCell("");
                }
                Label lblNroSerie = row.FindControl("lblNroSerie") as Label;
                if (lblNroSerie != null)
                {
                    pdfTable.AddCell(lblNroSerie.Text);
                }
                else
                {
                    pdfTable.AddCell("");
                }
                Label lblMoneda = row.FindControl("lblMoneda") as Label;
                if (lblMoneda != null)
                {
                    pdfTable.AddCell(lblMoneda.Text);
                }
                else
                {
                    pdfTable.AddCell("");
                }
                Label lblMontoTotal = row.FindControl("lblMontoTotal") as Label;
                if (lblMontoTotal != null)
                {
                    pdfTable.AddCell(lblMontoTotal.Text);
                }
                else
                {
                    pdfTable.AddCell("");
                }
                Label lblMontoPagado = row.FindControl("lblMontoPagado") as Label;
                if (lblMontoPagado != null)
                {
                    pdfTable.AddCell(lblMontoPagado.Text);
                }
                else
                {
                    pdfTable.AddCell("");
                }
                Label lblSaldo = row.FindControl("lblSaldo") as Label;
                if (lblSaldo != null)
                {
                    pdfTable.AddCell(lblSaldo.Text);
                }
                else
                {
                    pdfTable.AddCell("");
                }
                
            }

            doc.Add(pdfTable);
            doc.Close();
            stream.Close();
            byte[] Result = stream.ToArray();
            Sistema.GetInstancia().PDFActual = Result;
            // Session["pdf"] = Result;
            gridViewDocumentos.AllowPaging = true;
            if (Sistema.GetInstancia().PDFActual != null)
            {
                Response.Redirect("VisorPDFReportes.aspx");
            }
        }
    }
}