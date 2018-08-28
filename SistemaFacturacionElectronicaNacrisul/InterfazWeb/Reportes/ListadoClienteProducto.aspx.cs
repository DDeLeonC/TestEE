﻿using System;
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
    public partial class ListadoClienteProducto : System.Web.UI.Page
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

                ddlCliente.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos"));

                ddlProducto.DataSource = Sistema.GetInstancia().ObtenerProductos(Session["rut"].ToString());
                ddlProducto.DataTextField = "Nombre";
                ddlProducto.DataValueField = "IdProducto";
                ddlProducto.DataBind();

                ddlProducto.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos"));

                ddlZona.DataSource = Sistema.GetInstancia().ObtenerZonas();
                ddlZona.DataTextField = "Nombre";
                ddlZona.DataValueField = "IdZona";
                ddlZona.DataBind();

                ddlZona.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todas"));

                ddlVendedor.DataSource = Sistema.GetInstancia().ObtenerVendedores();
                ddlVendedor.DataTextField = "Nombre";
                ddlVendedor.DataValueField = "IdVendedor";
                ddlVendedor.DataBind();

                ddlVendedor.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos"));

                txtFechaDesde.Text = DateTime.Now.ToShortDateString();
                txtFechaHasta.Text = DateTime.Now.ToShortDateString();

                int? idCliente = null;
                if (!String.IsNullOrEmpty(ddlCliente.SelectedValue) && ddlCliente.SelectedIndex != 0)
                {
                    idCliente = Int32.Parse(ddlCliente.SelectedValue);
                }
                int? idVendedor = null;
                if (!String.IsNullOrEmpty(ddlVendedor.SelectedValue) && ddlVendedor.SelectedIndex != 0)
                {
                    idVendedor = Int32.Parse(ddlVendedor.SelectedValue);
                }
                int? idZona = null;
                if (!String.IsNullOrEmpty(ddlZona.SelectedValue) && ddlZona.SelectedIndex != 0)
                {
                    idZona = Int32.Parse(ddlZona.SelectedValue);
                }
                int? idProducto = null;
                if (!String.IsNullOrEmpty(ddlProducto.SelectedValue) && ddlProducto.SelectedIndex != 0)
                {
                    idProducto = Int32.Parse(ddlProducto.SelectedValue);
                }
                gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerListadoClienteProducto(idCliente, idVendedor, idZona, idProducto, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text));
                gridViewDocumentos.DataBind();

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                int? idCliente = null;
                if (!String.IsNullOrEmpty(ddlCliente.SelectedValue) && ddlCliente.SelectedIndex != 0)
                {
                    idCliente = Int32.Parse(ddlCliente.SelectedValue);
                }
                int? idVendedor = null;
                if (!String.IsNullOrEmpty(ddlVendedor.SelectedValue) && ddlVendedor.SelectedIndex != 0)
                {
                    idVendedor = Int32.Parse(ddlVendedor.SelectedValue);
                }
                int? idZona = null;
                if (!String.IsNullOrEmpty(ddlZona.SelectedValue) && ddlZona.SelectedIndex != 0)
                {
                    idZona = Int32.Parse(ddlZona.SelectedValue);
                }
                int? idProducto = null;
                if (!String.IsNullOrEmpty(ddlProducto.SelectedValue) && ddlProducto.SelectedIndex != 0)
                {
                    idProducto = Int32.Parse(ddlProducto.SelectedValue);
                }
                gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerListadoClienteProducto(idCliente, idVendedor, idZona, idProducto, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text));
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
                int? idCliente = null;
                if (!String.IsNullOrEmpty(ddlCliente.SelectedValue) && ddlCliente.SelectedIndex != 0)
                {
                    idCliente = Int32.Parse(ddlCliente.SelectedValue);
                }
                int? idVendedor = null;
                if (!String.IsNullOrEmpty(ddlVendedor.SelectedValue) && ddlVendedor.SelectedIndex != 0)
                {
                    idVendedor = Int32.Parse(ddlVendedor.SelectedValue);
                }
                int? idZona = null;
                if (!String.IsNullOrEmpty(ddlZona.SelectedValue) && ddlZona.SelectedIndex != 0)
                {
                    idZona = Int32.Parse(ddlZona.SelectedValue);
                }
                int? idProducto = null;
                if (!String.IsNullOrEmpty(ddlProducto.SelectedValue) && ddlProducto.SelectedIndex != 0)
                {
                    idProducto = Int32.Parse(ddlProducto.SelectedValue);
                }
                gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerListadoClienteProducto(idCliente, idVendedor, idZona, idProducto, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text));
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
                        if (cell.Text.Equals("Cantidad"))
                        {
                            DataColumn colDecimal = new DataColumn(cell.Text);
                            colDecimal.DataType = System.Type.GetType("System.Decimal");
                            dt.Columns.Add(colDecimal);

                        }
                        else if (cell.Text.Equals("Kilos"))
                        {
                            DataColumn colDecimal = new DataColumn(cell.Text);
                            colDecimal.DataType = System.Type.GetType("System.Decimal");
                            dt.Columns.Add(colDecimal);

                        }
                        else if (cell.Text.Equals("Total"))
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
                    Label lblRUT = row.FindControl("lblRUT") as Label;
                    if (lblRUT != null)
                    {
                        dr[1] = lblRUT.Text;
                    }
                    Label lblProducto = row.FindControl("lblProducto") as Label;
                    if (lblProducto != null)
                    {
                        dr[2] = lblProducto.Text;
                    }
                    Label lblCantidad = row.FindControl("lblCantidad") as Label;
                    if (lblCantidad != null)
                    {
                        decimal vidaUtil = Convert.ToDecimal(lblCantidad.Text);
                        dr[3] = vidaUtil;

                    }
                    Label lblKilos = row.FindControl("lblKilos") as Label;
                    if (lblKilos != null)
                    {
                        decimal vidaUtil = Convert.ToDecimal(lblKilos.Text);
                        dr[4] = vidaUtil;

                    }
                    Label lblTotal = row.FindControl("lblTotal") as Label;
                    if (lblTotal != null)
                    {
                        decimal vidaUtil = Convert.ToDecimal(lblTotal.Text);
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
                    Response.AddHeader("content-disposition", "attachment;filename=ListadoClienteProducto.xlsx");
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
            Paragraph paragraph1 = new Paragraph(@"LISTADO CLIENTES - PRODUCTOS", _tituloFont);
            paragraph1.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph1);
            Paragraph paragraph10 = new Paragraph(@"CLIENTE: " + ddlCliente.SelectedItem, _tituloFont);
            paragraph10.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph10);
            Paragraph paragraph11 = new Paragraph(@"DESDE: " + txtFechaDesde.Text + " - HASTA: " + txtFechaHasta.Text, _tituloFont);
            paragraph11.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph11);
            Paragraph paragraph2 = new Paragraph(@"                                                    ", _tituloFont);
            paragraph2.Alignment = Element.ALIGN_LEFT;
            doc.Add(paragraph2);

            //Creating iTextSharp Table from the DataTable data
            PdfPTable pdfTable = new PdfPTable(6);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;

            //Adding Header row
            pdfTable.AddCell("Cliente");
            pdfTable.AddCell("Documento");
            pdfTable.AddCell("Producto");
            pdfTable.AddCell("Cantidad");
            pdfTable.AddCell("Kilos");
            pdfTable.AddCell("Total");

            //Adding DataRow
            foreach (GridViewRow row in gridViewDocumentos.Rows)
            {
                Label lblCliente = row.FindControl("lblCliente") as Label;
                if (lblCliente != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblCliente.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblRUT = row.FindControl("lblRUT") as Label;
                if (lblRUT != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblRUT.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblProducto = row.FindControl("lblProducto") as Label;
                if (lblProducto != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblProducto.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblCantidad = row.FindControl("lblCantidad") as Label;
                if (lblCantidad != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblCantidad.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblKilos = row.FindControl("lblKilos") as Label;
                if (lblKilos != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblKilos.Text, _cellFont)));
                }
                else
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase("", _cellFont)));
                }
                Label lblTotal = row.FindControl("lblTotal") as Label;
                if (lblTotal != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblTotal.Text, _cellFont)));
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