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

using System.Reflection;
using iTextSharp.text.pdf;
using iTextSharp.text;

using System.Data;

namespace InterfazWeb.Reportes
{
    public partial class ReporteEstadoCuentaCliente : System.Web.UI.Page
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
                ddlCliente.DataSource = Sistema.GetInstancia().ObtenerClientes();
                ddlCliente.DataTextField = "ddlDescription";
                ddlCliente.DataValueField = "IdCliente";
                ddlCliente.DataBind();

                List<String> lista = new List<String>();
                lista.Add("Pesos");
                lista.Add("Dolar");

                ddlMoneda.DataSource = lista;
                ddlMoneda.DataBind();

                txtFechaDesde.Text = DateTime.Now.ToShortDateString();
                
                gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerReporteEstadoCuentaClientes(Int32.Parse(ddlCliente.SelectedValue), DateTime.Parse(txtFechaDesde.Text), ddlMoneda.SelectedValue);
                gridViewDocumentos.DataBind();
                
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                
                
                    gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerReporteEstadoCuentaClientes(Int32.Parse(ddlCliente.SelectedValue), DateTime.Parse(txtFechaDesde.Text),ddlMoneda.SelectedValue);
                    gridViewDocumentos.DataBind();
                
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
                
                
                    gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerReporteEstadoCuentaClientes(Int32.Parse(ddlCliente.SelectedValue), DateTime.Parse(txtFechaDesde.Text), ddlMoneda.SelectedValue);
                    gridViewDocumentos.DataBind();
                
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
                        if (cell.Text.Equals("Debe"))
                        {
                            DataColumn colDecimal = new DataColumn(cell.Text);
                            colDecimal.DataType = System.Type.GetType("System.Decimal");
                            dt.Columns.Add(colDecimal);
                        }
                        else if (cell.Text.Equals("Haber"))
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
                    Label lblTipoDocumento = row.FindControl("lblTipoDocumento") as Label;
                    if (lblTipoDocumento != null)
                    {
                        dr[1] = lblTipoDocumento.Text;
                    }
                    Label lblSerie = row.FindControl("lblSerie") as Label;
                    if (lblSerie != null)
                    {
                        dr[2] = lblSerie.Text;
                    }
                    Label lblNroSerie = row.FindControl("lblNroSerie") as Label;
                    if (lblNroSerie != null)
                    {
                        dr[3] = lblNroSerie.Text;
                    }
                    Label lblDetalle = row.FindControl("lblDetalle") as Label;
                    if (lblDetalle != null)
                    {
                        dr[4] = lblDetalle.Text;
                    }
                    Label lblMoneda = row.FindControl("lblMoneda") as Label;
                    if (lblMoneda != null)
                    {
                        dr[5] = lblMoneda.Text;
                    }
                    Label lblDebe = row.FindControl("lblDebe") as Label;
                    if (lblDebe != null)
                    {
                        decimal vidaUtil = Convert.ToDecimal(lblDebe.Text);
                        dr[6] = vidaUtil;
                    }
                    Label lblHaber = row.FindControl("lblHaber") as Label;
                    if (lblHaber != null)
                    {
                        decimal vidaUtil = Convert.ToDecimal(lblHaber.Text);
                        dr[7] = vidaUtil;
                    }
                    Label lblSaldo = row.FindControl("lblSaldo") as Label;
                    if (lblSaldo != null)
                    {
                        decimal vidaUtil = Convert.ToDecimal(lblSaldo.Text);
                        dr[8] = vidaUtil;
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
                    Response.AddHeader("content-disposition", "attachment;filename=EstadoCuentaCliente.xlsx");
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
            iTextSharp.text.Font _cellFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            Cliente cliente = Sistema.GetInstancia().BuscarClienteId(Int32.Parse(ddlCliente.SelectedValue));
            // Escribimos el encabezamiento en el documento
            Paragraph paragraph1 = new Paragraph(@"                         ESTADO DE CUENTA: " + cliente.Nombre+ " - Moneda: "+ddlMoneda.SelectedValue.ToString(), _tituloFont);
            paragraph1.Alignment = Element.ALIGN_LEFT;
            doc.Add(paragraph1);
            Paragraph paragraph2 = new Paragraph(@"                                                    ", _tituloFont);
            paragraph2.Alignment = Element.ALIGN_LEFT;
            doc.Add(paragraph2);

            //Creating iTextSharp Table from the DataTable data
            PdfPTable pdfTable = new PdfPTable(9);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;

            //Adding Header row
            pdfTable.AddCell("Fecha");
            pdfTable.AddCell("Tipo Documento");
            pdfTable.AddCell("Serie");
            pdfTable.AddCell("Nro Serie");
            pdfTable.AddCell("Detalle");
            pdfTable.AddCell("Moneda");
            pdfTable.AddCell("Debe");
            pdfTable.AddCell("Haber");
            pdfTable.AddCell("Saldo");

            //Adding DataRow
            foreach (GridViewRow row in gridViewDocumentos.Rows)
            {
                Label lblFecha = row.FindControl("lblFecha") as Label;
                if (lblFecha != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblFecha.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblTipoDocumento = row.FindControl("lblTipoDocumento") as Label;
                if (lblTipoDocumento != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblTipoDocumento.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblSerie = row.FindControl("lblSerie") as Label;
                if (lblSerie != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblSerie.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblNroSerie = row.FindControl("lblNroSerie") as Label;
                if (lblNroSerie != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblNroSerie.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblDetalle = row.FindControl("lblDetalle") as Label;
                if (lblDetalle != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblDetalle.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblMoneda = row.FindControl("lblMoneda") as Label;
                if (lblMoneda != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblMoneda.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblDebe = row.FindControl("lblDebe") as Label;
                if (lblDebe != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblDebe.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblHaber = row.FindControl("lblHaber") as Label;
                if (lblHaber != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblHaber.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblSaldo = row.FindControl("lblSaldo") as Label;
                if (lblSaldo != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblSaldo.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
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