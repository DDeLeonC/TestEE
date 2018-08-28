using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using Dominio.Clases_Dominio;
using InterfazWeb.ServiceReference2Prod;

namespace InterfazWeb.Reportes
{
    public partial class ConsultaDocumentosAnulados : System.Web.UI.Page
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

                ddlCliente.Items.Insert(0, new ListItem("Todos"));

                List<String> lista3 = new List<String>();
                lista3.Add("Todos");
                lista3.Add("Factura");
                lista3.Add("Nota Crédito");
                lista3.Add("Nota Débito");
                lista3.Add("Remito");
                lista3.Add("Resguardo");

                ddlTipoDocumento.DataSource = lista3;
                ddlTipoDocumento.DataBind();

                
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
                
                bool error = false;
                
                if (!error)
                {
                    gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerDocumentosAnulados(idCliente, tipoDoc, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), Session["rut"].ToString());
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
                
                bool error = false;
                
                if (!error)
                {
                    gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerDocumentosAnulados(idCliente, tipoDoc, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), Session["rut"].ToString());
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
                
                bool error = false;
                
                if (!error)
                {
                    gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerDocumentosAnulados(idCliente, tipoDoc, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), Session["rut"].ToString());
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
                        dt.Columns.Add(cell.Text);
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
                    Label lblSerie = row.FindControl("lblSerie") as Label;
                    if (lblSerie != null)
                    {
                        dr[1] = lblSerie.Text;
                    }
                    Label lblNroSerie = row.FindControl("lblNroSerie") as Label;
                    if (lblNroSerie != null)
                    {
                        dr[2] = lblNroSerie.Text;
                    }
                    Label lblMoneda = row.FindControl("lblMoneda") as Label;
                    if (lblMoneda != null)
                    {
                        dr[3] = lblMoneda.Text;
                    }
                    Label lbltipoCambio = row.FindControl("lbltipoCambio") as Label;
                    if (lbltipoCambio != null)
                    {
                        dr[4] = lbltipoCambio.Text;
                    }
                    Label lblmontoTotal = row.FindControl("lblmontoTotal") as Label;
                    if (lblmontoTotal != null)
                    {
                        dr[5] = lblmontoTotal.Text;
                    }
                    Label lblFormaPago = row.FindControl("lblFormaPago") as Label;
                    if (lblFormaPago != null)
                    {
                        dr[6] = lblFormaPago.Text;
                    }
                    Label lblEstado = row.FindControl("lblMotivo") as Label;
                    if (lblEstado != null)
                    {
                        dr[7] = lblEstado.Text;
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
                    Response.AddHeader("content-disposition", "attachment;filename=GridView.xlsx");
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
                    Response.Redirect("VisorPDFReportes.aspx");
                }
            }
            catch
            {

            }
        }
    }
}