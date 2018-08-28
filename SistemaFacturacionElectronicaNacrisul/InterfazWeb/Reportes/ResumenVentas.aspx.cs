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
using System.Data;
using ClosedXML.Excel;

using System.Reflection;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace InterfazWeb.Reportes
{
    public partial class ResumenVentas : System.Web.UI.Page
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
                lista.Add("Todos");
                lista.Add("Recibos");
                lista.Add("Documentos");
                ddlIncluye.DataSource = lista;
                ddlIncluye.DataBind();

                txtFechaDesde.Text = DateTime.Now.ToShortDateString();
                txtFechaHasta.Text = DateTime.Now.ToShortDateString();

                bool? vanrecibos = null;
                if (ddlIncluye.SelectedIndex == 1)
                {
                    vanrecibos = true;
                }
                else if (ddlIncluye.SelectedIndex == 2) {
                    vanrecibos = false;
                }
                gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerResumenVentas(DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text),vanrecibos);
                gridViewDocumentos.DataBind();

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                bool? vanrecibos = null;
                if (ddlIncluye.SelectedIndex == 1)
                {
                    vanrecibos = true;
                }
                else if (ddlIncluye.SelectedIndex == 2)
                {
                    vanrecibos = false;
                }
                gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerResumenVentas(DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), vanrecibos);
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
                bool? vanrecibos = null;
                if (ddlIncluye.SelectedIndex == 1)
                {
                    vanrecibos = true;
                }
                else if (ddlIncluye.SelectedIndex == 2)
                {
                    vanrecibos = false;
                }
                gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerResumenVentas(DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), vanrecibos);
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
                        if (cell.Text.Equals("Importe"))
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
                    Label lblDetalle = row.FindControl("lblDetalle") as Label;
                    if (lblDetalle != null)
                    {
                        dr[1] = lblDetalle.Text;
                    }
                    Label lblImporte = row.FindControl("lblImporte") as Label;
                    if (lblImporte != null)
                    {
                        decimal importe = Convert.ToDecimal(lblImporte.Text);
                        dr[2] = importe;

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
                    Response.AddHeader("content-disposition", "attachment;filename=ResumenVentas.xlsx");
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
            // Escribimos el encabezamiento en el documento
            Paragraph paragraph1 = new Paragraph(@"                         RESUMEN DE VENTAS", _tituloFont);
            paragraph1.Alignment = Element.ALIGN_LEFT;
            doc.Add(paragraph1);
            Paragraph paragraph2 = new Paragraph(@"                                                    ", _tituloFont);
            paragraph2.Alignment = Element.ALIGN_LEFT;
            doc.Add(paragraph2);

            //Creating iTextSharp Table from the DataTable data
            PdfPTable pdfTable = new PdfPTable(3);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;

            //Adding Header row
            pdfTable.AddCell("Fecha");
            pdfTable.AddCell("Detalle");
            pdfTable.AddCell("Importe");

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
                Label lblDetalle = row.FindControl("lblDetalle") as Label;
                if (lblDetalle != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblDetalle.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblImporte = row.FindControl("lblImporte") as Label;
                if (lblImporte != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblImporte.Text, _cellFont)));
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