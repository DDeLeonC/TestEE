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

using System.Reflection;
using iTextSharp.text.pdf;
using iTextSharp.text;

using System.Data;
using ClosedXML.Excel;

namespace InterfazWeb.Reportes
{
    public partial class ConsultaDocumentos : System.Web.UI.Page
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

                List<String> lista3 = new List<String>();
                lista3.Add("Todos");
                lista3.Add("Factura");
                lista3.Add("Nota Crédito");
                lista3.Add("Nota Débito");
                lista3.Add("Remito");
                lista3.Add("Resguardo");

                ddlTipoDocumento.DataSource = lista3;
                ddlTipoDocumento.DataBind();

                List<String> lista = new List<String>();
                lista.Add("Todos");
                lista.Add("Aceptado");
                lista.Add("Procesado");
                lista.Add("Anulado");
                

                ddlEstado.DataSource = lista;
                ddlEstado.DataBind();

                txtFechaDesde.Text = DateTime.Now.ToShortDateString();
                txtFechaHasta.Text = DateTime.Now.ToShortDateString();

                int? idCliente = null;
                if (!String.IsNullOrEmpty(ddlCliente.SelectedValue) && ddlCliente.SelectedIndex != 0)
                {
                    idCliente = Int32.Parse(ddlCliente.SelectedValue);
                }
                String tipoDoc = "";
                if (!String.IsNullOrEmpty(ddlTipoDocumento.SelectedValue) && ddlTipoDocumento.SelectedIndex != 0)
                {
                    tipoDoc = ddlTipoDocumento.SelectedValue;
                }
                String estado = "";
                if (!String.IsNullOrEmpty(ddlEstado.SelectedValue) && ddlEstado.SelectedIndex != 0)
                {
                    estado = ddlEstado.SelectedValue;
                }

                bool error = false;
                int nro = 0;
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
                    string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un entero en el campo Nro. Serie" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                if (!error)
                {
                    gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerDocumentos(idCliente, tipoDoc, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), txtSerie.Text, nro, estado, Session["rut"].ToString(), txtNroGuia.Text, txtNroRemito.Text);
                    gridViewDocumentos.DataBind();
                }
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
                String tipoDoc = "";
                if (!String.IsNullOrEmpty(ddlTipoDocumento.SelectedValue) && ddlTipoDocumento.SelectedIndex != 0)
                {
                    tipoDoc = ddlTipoDocumento.SelectedValue;
                }
                String estado = "";
                if (!String.IsNullOrEmpty(ddlEstado.SelectedValue) && ddlEstado.SelectedIndex != 0)
                {
                    estado = ddlEstado.SelectedValue;
                }
                bool error = false;
                int nro = 0;
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
                    string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un entero en el campo Nro. Serie" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                if (!error)
                {
                    gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerDocumentos(idCliente, tipoDoc, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), txtSerie.Text, nro, estado, Session["rut"].ToString(), txtNroGuia.Text, txtNroRemito.Text);
                    gridViewDocumentos.DataBind();
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
                int? idCliente = null;
                if (!String.IsNullOrEmpty(ddlCliente.SelectedValue) && ddlCliente.SelectedIndex != 0)
                {
                    idCliente = Int32.Parse(ddlCliente.SelectedValue);
                }
                String tipoDoc = "";
                if (!String.IsNullOrEmpty(ddlTipoDocumento.SelectedValue) && ddlTipoDocumento.SelectedIndex != 0)
                {
                    tipoDoc = ddlTipoDocumento.SelectedValue;
                }
                String estado = "";
                if (!String.IsNullOrEmpty(ddlEstado.SelectedValue) && ddlEstado.SelectedIndex != 0)
                {
                    estado = ddlEstado.SelectedValue;
                }
                bool error = false;
                int nro = 0;
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
                    string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un entero en el campo Nro. Serie" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                if (!error)
                {
                    gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerDocumentos(idCliente, tipoDoc, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), txtSerie.Text, nro, estado, Session["rut"].ToString(), txtNroGuia.Text, txtNroRemito.Text);
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
            
            try {
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
                    Label lblMoneda = row.FindControl("lblMoneda") as Label;
                    if (lblMoneda != null)
                    {
                        dr[4] = lblMoneda.Text;
                    }
                    Label lbltipoCambio = row.FindControl("lbltipoCambio") as Label;
                    if (lbltipoCambio != null)
                    {
                        dr[5] = lbltipoCambio.Text;
                    }
                    Label lblMontoTotal = row.FindControl("lblMontoTotal") as Label;
                    if (lblMontoTotal != null)
                    {
                        decimal vidaUtil = Convert.ToDecimal(lblMontoTotal.Text);
                        dr[6] = vidaUtil;
                    }
                    Label lblFormaPago = row.FindControl("lblFormaPago") as Label;
                    if (lblFormaPago != null)
                    {
                        dr[7] = lblFormaPago.Text;
                    }
                    Label lblEstado = row.FindControl("lblEstado") as Label;
                    if (lblEstado != null)
                    {
                        dr[8] = lblEstado.Text;
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
                    Response.AddHeader("content-disposition", "attachment;filename=ConsultaDocumentos.xlsx");
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
                int? idCliente = null;
                if (!String.IsNullOrEmpty(ddlCliente.SelectedValue) && ddlCliente.SelectedIndex != 0)
                {
                    idCliente = Int32.Parse(ddlCliente.SelectedValue);
                }
                String tipoDoc = "";
                if (!String.IsNullOrEmpty(ddlTipoDocumento.SelectedValue) && ddlTipoDocumento.SelectedIndex != 0)
                {
                    tipoDoc = ddlTipoDocumento.SelectedValue;
                }
                String estado = "";
                if (!String.IsNullOrEmpty(ddlEstado.SelectedValue) && ddlEstado.SelectedIndex != 0)
                {
                    estado = ddlEstado.SelectedValue;
                }
                bool error = false;
                int nro = 0;
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
                    string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un entero en el campo Nro. Serie" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                if (!error)
                {
                    gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerDocumentos(idCliente, tipoDoc, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), txtSerie.Text, nro, estado, Session["rut"].ToString(), txtNroGuia.Text, txtNroRemito.Text);
                    gridViewDocumentos.DataBind();
                }
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void VerPDF(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkRemove = (LinkButton)sender;
                String id = lnkRemove.CommandArgument;
                Documento doc = Sistema.GetInstancia().ObtenerDocumentoId(Int32.Parse(id));
                var client2 = new ConsultaCfeClient();
                client2.ClientCredentials.UserName.UserName = Session["rut"].ToString();
                client2.ClientCredentials.UserName.Password = Sistema.GetInstancia().Contrasena(Session["rut"].ToString());
                byte[] pdf = client2.ObtenerPdf(Session["rut"].ToString(), Int32.Parse(doc.TipoDocumento), doc.Serie, doc.NroSerie);
                Sistema.GetInstancia().PDFActual = pdf;
                if (Sistema.GetInstancia().PDFActual != null)
                {
                    //Response.Redirect("VisorPDFReportes.aspx");
                    Response.Write("<script>");
                    Response.Write("window.open('VisorPDFReportes.aspx', '_blank');");
                    Response.Write("</script>");
                }
                else {
                    string script = @"<script type='text/javascript'> alert('" + "Error al obtener PDF" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
            }
            catch { 
            
            }
        }

        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            gridViewDocumentos.AllowPaging = false;
            BindData();
            Document doc = new Document(PageSize.LETTER);
            doc.AddTitle("Consulta Documentos");
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
            Paragraph paragraph1 = new Paragraph(@"                                 CONSULTA DOCUMENTOS", _tituloFont);
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
            pdfTable.AddCell("Cliente");
            pdfTable.AddCell("Serie");
            pdfTable.AddCell("Nro Doc");
            pdfTable.AddCell("Moneda");
            pdfTable.AddCell("Tipo Cambio");
            pdfTable.AddCell("Monto Total");
            pdfTable.AddCell("Forma Pago");
            pdfTable.AddCell("Estado");

            //Adding DataRow
            foreach (GridViewRow row in gridViewDocumentos.Rows)
            {
                Label lblFecha = row.FindControl("lblFecha") as Label;
                if (lblFecha != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblFecha.Text, _cellFont)));
                }
                Label lblCliente = row.FindControl("lblCliente") as Label;
                if (lblCliente != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblCliente.Text, _cellFont)));
                }
                Label lblSerie = row.FindControl("lblSerie") as Label;
                if (lblSerie != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblSerie.Text, _cellFont)));
                }
                Label lblNroSerie = row.FindControl("lblNroSerie") as Label;
                if (lblNroSerie != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblNroSerie.Text, _cellFont)));
                }
                Label lblMoneda = row.FindControl("lblMoneda") as Label;
                if (lblMoneda != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblMoneda.Text, _cellFont)));
                }
                Label lbltipoCambio = row.FindControl("lbltipoCambio") as Label;
                if (lbltipoCambio != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lbltipoCambio.Text, _cellFont)));
                }
                Label lblMontoTotal = row.FindControl("lblMontoTotal") as Label;
                if (lblMontoTotal != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblMontoTotal.Text, _cellFont)));
                }
                Label lblFormaPago = row.FindControl("lblFormaPago") as Label;
                if (lblFormaPago != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblFormaPago.Text, _cellFont)));
                }
                Label lblEstado = row.FindControl("lblEstado") as Label;
                if (lblEstado != null)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(lblEstado.Text, _cellFont)));
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