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

namespace InterfazWeb.Transacciones
{
    public partial class AsignarPagos : System.Web.UI.Page
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
                Sistema.GetInstancia().PDFActual = null;
                Pendientes.Visible = false;
                //Importe.Visible = false;
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
                // Sistema.GetInstancia().deuda = 0;
            }
        }

        protected void chkPagar_checkedChanged(object sender, EventArgs e)
        {
            decimal totalCobrar = 0;
            foreach (GridViewRow row in gridViewEstadoCuenta.Rows)
            {

                CheckBox chkPagar = row.FindControl("chkPagar") as CheckBox;
                if (chkPagar != null)
                {
                    if (chkPagar.Checked)
                    {
                        Label ImporteCuota = row.FindControl("lblRestante") as Label;
                        if (ImporteCuota != null)
                        {
                            decimal vidaUtil = Convert.ToDecimal(ImporteCuota.Text);
                            totalCobrar += vidaUtil;
                        }
                    }
                }
            }
            foreach (GridViewRow row in gridViewRecibos.Rows)
            {

                CheckBox chkPagar = row.FindControl("chkPagar") as CheckBox;
                if (chkPagar != null)
                {
                    if (chkPagar.Checked)
                    {
                        Label ImporteCuota = row.FindControl("lblRestante") as Label;
                        if (ImporteCuota != null)
                        {
                            decimal vidaUtil = Convert.ToDecimal(ImporteCuota.Text);
                            totalCobrar += vidaUtil;
                        }
                    }
                }
            }
            txtSaldo.Text = totalCobrar.ToString();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                limpiarFormulario();
                int idCliente = Int32.Parse(ddlClientes.SelectedValue);
                List<Dominio.Clases_Dominio.DeudaClientes> cuentas = Sistema.GetInstancia().ObtenerDeudaCliente(idCliente, Session["rut"].ToString(), ddlMoneda.SelectedValue);
                List<Dominio.Clases_Dominio.DeudaClientes> recibos = Sistema.GetInstancia().ObtenerPagosPendientes(idCliente, Session["rut"].ToString(), ddlMoneda.SelectedValue);
                gridViewEstadoCuenta.DataSource = cuentas;
                gridViewEstadoCuenta.DataBind();
                gridViewRecibos.DataSource = recibos;
                gridViewRecibos.DataBind();
                if ((cuentas != null && cuentas.Count > 0) || (recibos != null && recibos.Count > 0))
                {
                    Pendientes.Visible = true;
                    //MedioDePago.Visible = true;
                    Importe.Visible = true;
                    decimal importeHaber = 0;
                    decimal importeDebe = 0;
                    if(cuentas.Count>0)
                    {
                        importeDebe = cuentas.ElementAt(cuentas.Count - 1).SaldoTotal; 
                    }
                    if(recibos.Count>0)
                    {
                        importeHaber = recibos.ElementAt(cuentas.Count - 1).SaldoTotal; 
                    }
                    txtSaldoTotal.Text = (importeDebe - importeHaber).ToString();
                }
                txtSaldo.Text = "";

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
                List<int> idCuotas = new List<int>();
                List<int> idRecibos = new List<int>();
                foreach (GridViewRow row in gridViewEstadoCuenta.Rows)
                {

                    CheckBox chkPagar = row.FindControl("chkPagar") as CheckBox;
                    if (chkPagar != null)
                    {
                        if (chkPagar.Checked)
                        {
                            Label IdCuota = row.FindControl("lblIdDocumento") as Label;
                            if (IdCuota != null)
                            {
                                idCuotas.Add(Int32.Parse(IdCuota.Text));
                            }
                        }
                    }
                }
                foreach (GridViewRow row in gridViewRecibos.Rows)
                {

                    CheckBox chkPagar = row.FindControl("chkPagar") as CheckBox;
                    if (chkPagar != null)
                    {
                        if (chkPagar.Checked)
                        {
                            Label IdRecibo = row.FindControl("lblIdRecibo") as Label;
                            if (IdRecibo != null)
                            {
                                idRecibos.Add(Int32.Parse(IdRecibo.Text));
                            }
                        }
                    }
                }
                int idCliente = Int32.Parse(ddlClientes.SelectedValue);

                String msg = Sistema.GetInstancia().AsignarPagos(idCuotas, idRecibos, idCliente, ddlMoneda.SelectedValue, Session["rut"].ToString());
                string script = @"<script type='text/javascript'> alert('" + msg + "" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
               
                
                
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