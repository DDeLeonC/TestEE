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

using System.Reflection;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace InterfazWeb.Reportes
{
    public partial class InformeCobrosCliente : System.Web.UI.Page
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
                int? idVendedor = null;
                if (!String.IsNullOrEmpty(ddlVendedores.SelectedValue) && ddlVendedores.SelectedIndex != 0)
                {
                    idVendedor = Int32.Parse(ddlVendedores.SelectedValue);
                }
                int? idZona = null;
                if (!String.IsNullOrEmpty(ddlZonas.SelectedValue) && ddlZonas.SelectedIndex != 0)
                {
                    idZona = Int32.Parse(ddlZonas.SelectedValue);
                }
                gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerInformeCobroClientes(Session["rut"].ToString(), ddlMoneda.SelectedValue, idZona, idVendedor);
                gridViewDocumentos.DataBind();

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                int? idVendedor = null;
                if (!String.IsNullOrEmpty(ddlVendedores.SelectedValue) && ddlVendedores.SelectedIndex != 0)
                {
                    idVendedor = Int32.Parse(ddlVendedores.SelectedValue);
                }
                int? idZona = null;
                if (!String.IsNullOrEmpty(ddlZonas.SelectedValue) && ddlZonas.SelectedIndex != 0)
                {
                    idZona = Int32.Parse(ddlZonas.SelectedValue);
                }
                gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerInformeCobroClientes(Session["rut"].ToString(), ddlMoneda.SelectedValue, idZona, idVendedor);
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
                int? idVendedor = null;
                if (!String.IsNullOrEmpty(ddlVendedores.SelectedValue) && ddlVendedores.SelectedIndex != 0)
                {
                    idVendedor = Int32.Parse(ddlVendedores.SelectedValue);
                }
                int? idZona = null;
                if (!String.IsNullOrEmpty(ddlZonas.SelectedValue) && ddlZonas.SelectedIndex != 0)
                {
                    idZona = Int32.Parse(ddlZonas.SelectedValue);
                }
                gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerInformeCobroClientes(Session["rut"].ToString(), ddlMoneda.SelectedValue, idZona, idVendedor);
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
                        if (cell.Text.Equals("Total Cuenta"))
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
                        else if (cell.Text.Equals("Saldo Total"))
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
                    Label lblCliente = row.FindControl("lblCliente") as Label;
                    if (lblCliente != null)
                    {
                        dr[0] = lblCliente.Text;
                    }
                    
                    Label lblFechaVencimiento = row.FindControl("lblFechaVencimiento") as Label;
                    if (lblFechaVencimiento != null)
                    {
                        dr[1] = lblFechaVencimiento.Text;
                    }
                    Label lblDetalle = row.FindControl("lblDetalle") as Label;
                    if (lblDetalle != null)
                    {
                        dr[2] = lblDetalle.Text;
                    }
                    Label lblTotalCuenta = row.FindControl("lblTotalCuenta") as Label;
                    if (lblTotalCuenta != null)
                    {
                        decimal vidaUtil = 0;
                        if (!lblTotalCuenta.Text.Equals(""))
                        {
                            vidaUtil = Convert.ToDecimal(lblTotalCuenta.Text);
                        }
                        dr[3] = vidaUtil;
                    }
                    Label lblSaldo = row.FindControl("lblSaldo") as Label;
                    if (lblSaldo != null)
                    {
                        decimal vidaUtil = 0;
                        if (!lblSaldo.Text.Equals(""))
                        {
                            vidaUtil = Convert.ToDecimal(lblSaldo.Text);
                        }
                        dr[4] = vidaUtil;
                    }
                    Label lblSaldoTotal = row.FindControl("lblSaldoTotal") as Label;
                    if (lblSaldoTotal != null)
                    {
                        decimal vidaUtil = 0;
                        if (!lblSaldoTotal.Text.Equals(""))
                        {
                            vidaUtil = Convert.ToDecimal(lblSaldoTotal.Text);
                        }
                        dr[5] = vidaUtil;
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
                    Response.AddHeader("content-disposition", "attachment;filename=InformeCobroClientes.xlsx");
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
            iTextSharp.text.Font _cellFontBOLD = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

            // Escribimos el encabezamiento en el documento
            Paragraph paragraph1 = new Paragraph(@"                         INFORME COBROS POR CLIENTE", _tituloFont);
            paragraph1.Alignment = Element.ALIGN_LEFT;
            doc.Add(paragraph1);
            Paragraph paragraph2 = new Paragraph(@"                                                    ", _tituloFont);
            paragraph2.Alignment = Element.ALIGN_LEFT;
            doc.Add(paragraph2);

            //Creating iTextSharp Table from the DataTable data
            PdfPTable pdfTable = new PdfPTable(6);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;
            pdfTable.DefaultCell.MinimumHeight = 15;

            //Adding Header row
            pdfTable.AddCell("Cliente");
            pdfTable.AddCell("Fecha Vencimiento");
            pdfTable.AddCell("Detalle");
            pdfTable.AddCell("Total Cuenta");
            pdfTable.AddCell("Saldo");
            pdfTable.AddCell("Saldo Total");

            //Adding DataRow
            foreach (GridViewRow row in gridViewDocumentos.Rows)
            {
                
                Label lblCliente = row.FindControl("lblCliente") as Label;
                if (lblCliente != null)
                {
                    if (String.IsNullOrEmpty(lblCliente.Text))
                    {
                        pdfTable.AddCell(new PdfPCell(new Phrase(".", _cellFont)));
                    }
                    else
                    {
                        pdfTable.AddCell(new PdfPCell(new Phrase(lblCliente.Text, _cellFont)));
                    }
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                
                Label lblFechaVencimiento = row.FindControl("lblFechaVencimiento") as Label;
                if (lblFechaVencimiento != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblFechaVencimiento.Text, _cellFont)));
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
                Label lblTotalCuenta = row.FindControl("lblTotalCuenta") as Label;
                if (lblTotalCuenta != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblTotalCuenta.Text, _cellFont)));
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
                Label lblSaldoTotal = row.FindControl("lblSaldoTotal") as Label;
                if (lblSaldoTotal != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblSaldoTotal.Text, _cellFontBOLD)));
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