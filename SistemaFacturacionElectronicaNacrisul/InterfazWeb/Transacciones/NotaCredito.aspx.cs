using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Dominio.Clases_Dominio;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography;
using InterfazWeb.ServiceReference1Prod;
using InterfazWeb.ServiceReference2Prod;

namespace InterfazWeb.Transacciones
{
    public partial class NotaCredito : System.Web.UI.Page
    {
        public const string SELECTED_CUSTOMERS_INDEX = "SelectedCustomersIndex";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["rut"] == null || String.IsNullOrEmpty(Session["rut"].ToString()))
            {
                Session.Abandon();
                Response.Redirect("~/Logon.aspx");
            }
            if (!IsPostBack)
            {
                ActualizarEstados();
                Sistema.GetInstancia().SubTotal = 0;
                Sistema.GetInstancia().Total = 0;
                Sistema.GetInstancia().Impuestos = 0;
                txtImpuestos.Enabled = false;
                txtSubTotal.Enabled = false;
                txtTotal.Enabled = false;
                Sistema.GetInstancia().PDFActual = null;

                txtImpuestos.Text = Sistema.GetInstancia().Impuestos.ToString();
                txtSubTotal.Text = Sistema.GetInstancia().SubTotal.ToString();
                txtTotal.Text = Sistema.GetInstancia().Total.ToString();
                txtFecha.Text = DateTime.Now.Date.ToShortDateString();
                TipoCambio.Visible = false;
                AsociarDocumentos.Visible = false;
                List<String> lista = new List<String>();
                lista.Add("Pesos");
                lista.Add("Dolar");

                ddlMoneda.DataSource = lista;
                ddlMoneda.DataBind();

                List<String> lista2 = new List<String>();
                lista2.Add("No");
                lista2.Add("Si");

                ddlAsociar.DataSource = lista2;
                ddlAsociar.DataBind();

                List<String> lista4 = new List<String>();
                //lista4.Add("Contado");
                lista4.Add("Credito");
                lista4.Add("Contado");

                ddlFormaPago.DataSource = lista4;
                ddlFormaPago.DataBind();

                List<String> lista3 = new List<String>();
                lista3.Add("Todos");
                lista3.Add("Factura");
                lista3.Add("Nota Débito");
                lista3.Add("Remito");
                lista3.Add("Resguardo");

                ddlTipoDocumento.DataSource = lista3;
                ddlTipoDocumento.DataBind();


                ddlCliente.DataSource = Sistema.GetInstancia().ObtenerClientes();
                ddlCliente.DataTextField = "ddlDescription";
                ddlCliente.DataValueField = "IdCliente";
                ddlCliente.DataBind();
                ddlCliente.Items.Insert(0, new ListItem("Buscar Cliente..."));

                List<Grupo> grupos = Sistema.GetInstancia().ObtenerGrupos(Session["rut"].ToString());
                ddlGrupo.DataSource = grupos;
                ddlGrupo.DataTextField = "Descripcion";
                ddlGrupo.DataValueField = "IdGrupo";
                ddlGrupo.DataBind();

                ddlGrupo.Items.Insert(0, new ListItem("Todos"));

                if (grupos != null && grupos.Count > 0)
                {
                    int IdGrupo = 0;
                    if (!String.IsNullOrEmpty(ddlGrupo.SelectedValue) && ddlGrupo.SelectedIndex != 0)
                    {
                        IdGrupo = Int32.Parse(ddlGrupo.SelectedValue);
                    }
                    if (IdGrupo != 0)
                    {
                        List<SubGrupo> subgrupos = Sistema.GetInstancia().ObtenerSubGruposGrupo(IdGrupo, Session["rut"].ToString()).ToList();
                        ddlSubGrupo.DataSource = subgrupos;
                        ddlSubGrupo.DataTextField = "Descripcion";
                        ddlSubGrupo.DataValueField = "IdSubGrupo";
                        ddlSubGrupo.DataBind();
                        ddlSubGrupo.Items.Insert(0, new ListItem("Todos"));
                    }
                    else
                    {
                        List<SubGrupo> subgrupos = Sistema.GetInstancia().ObtenerSubGrupos(Session["rut"].ToString()).ToList();
                        ddlSubGrupo.DataSource = subgrupos;
                        ddlSubGrupo.DataTextField = "Descripcion";
                        ddlSubGrupo.DataValueField = "IdSubGrupo";
                        ddlSubGrupo.DataBind();
                        ddlSubGrupo.Items.Insert(0, new ListItem("Todos"));
                    }

                    int IdSubGrupo = 0;
                    if (!String.IsNullOrEmpty(ddlSubGrupo.SelectedValue) && ddlSubGrupo.SelectedIndex != 0)
                    {
                        IdSubGrupo = Int32.Parse(ddlSubGrupo.SelectedValue);
                    }
                    if (IdSubGrupo != 0)
                    {
                        ddlProductos.DataSource = Sistema.GetInstancia().BuscarProductosSubGrupo(IdSubGrupo, Session["rut"].ToString()).ToList();
                        ddlProductos.DataTextField = "ddlDescription";
                        ddlProductos.DataValueField = "IdProducto";
                        ddlProductos.DataBind();
                    }
                    else
                    {
                        ddlProductos.DataSource = Sistema.GetInstancia().ObtenerProductos(Session["rut"].ToString()).ToList();
                        ddlProductos.DataTextField = "ddlDescription";
                        ddlProductos.DataValueField = "IdProducto";
                        ddlProductos.DataBind();
                    }
                }


            }
            txtImpuestos.Text = Sistema.GetInstancia().Impuestos.ToString();
            txtSubTotal.Text = Sistema.GetInstancia().SubTotal.ToString();
            txtTotal.Text = Sistema.GetInstancia().Total.ToString();
        }

        private void ActualizarEstados()
        {
            try
            {
                List<Documento> documentos = Sistema.GetInstancia().ObtenerDocumentosAceptadosAnulados();
                if (documentos != null)
                {
                    var client = new CfeServiceClient();
                    client.ClientCredentials.UserName.UserName = Session["rut"].ToString();
                    client.ClientCredentials.UserName.Password = Sistema.GetInstancia().Contrasena(Session["rut"].ToString());
                    foreach (Documento documento in documentos)
                    {

                        ReqBody solicitud = new ReqBody();
                        solicitud.CodComercio = Sistema.GetInstancia().CodComercio(Session["rut"].ToString());
                        solicitud.CodTerminal = Sistema.GetInstancia().CodTerminal(Session["rut"].ToString());
                        solicitud.HMAC = "";
                        RequerimientoParaUcfe req = new RequerimientoParaUcfe();
                        req.TipoMensaje = 360;
                        // req.Uuid = "10000222";
                        req.TipoCfe = documento.TipoDocumento;
                        req.Serie = documento.Serie;
                        req.NumeroCfe = documento.NroSerie.ToString();
                        req.IdReq = "1";
                        req.FechaReq = documento.Fecha.Year + "" + documento.Fecha.Month + "" + documento.Fecha.Day;
                        req.HoraReq = documento.Fecha.Hour + "" + documento.Fecha.Minute + "" + documento.Fecha.Second;
                        req.CodComercio = Sistema.GetInstancia().CodComercio(Session["rut"].ToString());
                        req.CodTerminal = Sistema.GetInstancia().CodTerminal(Session["rut"].ToString());

                        solicitud.Req = req;
                        solicitud.RequestDate = documento.Fecha.Year + "-" + documento.Fecha.Month + "-" + documento.Fecha.Day + "T" + documento.Fecha.Hour + ":" + documento.Fecha.Minute + ":" + documento.Fecha.Second;
                        solicitud.Tout = 120000;
                        RespBody respuesta = null;
                        if (client.InnerChannel.State != System.ServiceModel.CommunicationState.Faulted)
                        {
                            respuesta = client.Invoke(solicitud);
                        }
                        if (respuesta != null)
                        {
                            if (respuesta.ErrorCode != 0)
                            {
                                string script = @"<script type='text/javascript'> alert('" + "Error: " + respuesta.ErrorMessage + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                            else
                            {
                                if (respuesta.Resp.CodRta.Equals("00"))
                                {
                                    Sistema.GetInstancia().ModificarEstado(documento.IdDocumento, "Procesado", null);

                                }
                                else if (respuesta.Resp.CodRta.Equals("11"))
                                {

                                }
                                else if (respuesta.Resp.CodRta.Equals("01") || respuesta.Resp.CodRta.Equals("05"))
                                {
                                    Sistema.GetInstancia().ModificarEstado(documento.IdDocumento, "Anulado", respuesta.Resp.MensajeRta);

                                }
                                else if (respuesta.Resp.CodRta.Equals("03"))
                                {
                                    string script = @"<script type='text/javascript'> alert('" + "Comercio invalido" + "');</script>";
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                }
                                else if (respuesta.Resp.CodRta.Equals("30"))
                                {
                                    string script = @"<script type='text/javascript'> alert('" + "Error en formato" + "');</script>";
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                }
                                else if (respuesta.Resp.CodRta.Equals("31"))
                                {
                                    string script = @"<script type='text/javascript'> alert('" + "Error en formato CFE" + "');</script>";
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                }
                                else if (respuesta.Resp.CodRta.Equals("89"))
                                {
                                    string script = @"<script type='text/javascript'> alert('" + "Terminal invalida" + "');</script>";
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                }
                                else if (respuesta.Resp.CodRta.Equals("96"))
                                {
                                    string script = @"<script type='text/javascript'> alert('" + "Error en sistema" + "');</script>";
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                }
                                else
                                {

                                    string script = @"<script type='text/javascript'> alert('" + "Denegado: " + respuesta.Resp.MensajeRta + "');</script>";
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);

                                }

                            }
                        }
                        else
                        {
                            string script = @"<script type='text/javascript'> alert('" + "Error de conexión con el punto de emisión" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                }
                string script2 = @"<script type='text/javascript'> alert('" + "Los estados se actualizaron correctamente" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script2, false);
            }
            catch
            {
                string script3 = @"<script type='text/javascript'> alert('" + "Error al actualizar los estados" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script3, false);
            }
        }

        protected void gridViewFacturas_RowCreated(object sender, GridViewRowEventArgs e)
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

        protected void Editar(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gridViewFacturas.EditIndex = e.NewEditIndex;

            }
            catch
            {
                try
                {
                    gridViewFacturas.EditIndex = -1;

                }
                catch
                {
                    string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                string script2 = @"<script type='text/javascript'> alert('" + "Error al editar" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script2, false);
            }

        }

        protected void Modificar(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int index = gridViewFacturas.EditIndex;
                GridViewRow row = gridViewFacturas.Rows[index];
                string id = ((Label)row.FindControl("lblIdProducto")).Text;
                string cant = ((TextBox)row.FindControl("txtCantidad")).Text;
                string ki = ((TextBox)row.FindControl("txtKilos")).Text;
                string pre = ((TextBox)row.FindControl("txtPrecio")).Text;
                string des = ((TextBox)row.FindControl("txtDescuento")).Text;
                string rec = ((TextBox)row.FindControl("txtRecargo")).Text;
                string imp = ((Label)row.FindControl("lblImpuesto")).Text;

                decimal cantidad = 0;
                decimal precio = 0;
                decimal descuento = 0;
                decimal recargo = 0;
                decimal impuesto = 0;
                decimal Kilos = 0;

                bool error = false;
                if (!error)
                {
                    if (!String.IsNullOrEmpty(cant))
                    {
                        try
                        {
                            cantidad = decimal.Parse(cant);
                            if (cantidad <= 0)
                            {
                                error = true;
                                string script = @"<script type='text/javascript'> alert('" + "Error: La cantidad debe ser mayor a 0" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar un decimal en cantidad" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                    else
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar la cantidad" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                }
                if (!error)
                {
                    if (!String.IsNullOrEmpty(ki))
                    {
                        try
                        {
                            Kilos = decimal.Parse(ki);
                            if (Kilos <= 0)
                            {
                                error = true;
                                string script = @"<script type='text/javascript'> alert('" + "Error: Los kilos deben ser mayor a 0" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar un decimal en kilos" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                    else
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar los kilos" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                }
                if (!error)
                {
                    if (!String.IsNullOrEmpty(pre))
                    {
                        try
                        {
                            precio = decimal.Parse(pre);
                            if (precio <= 0)
                            {
                                error = true;
                                string script = @"<script type='text/javascript'> alert('" + "Error: El precio debe ser mayor a 0" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar un decimal en precio" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                    else
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar el precio" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                }
                if (!error)
                {
                    if (!String.IsNullOrEmpty(rec))
                    {
                        try
                        {
                            recargo = decimal.Parse(rec);
                            if (recargo < 0)
                            {
                                error = true;
                                string script = @"<script type='text/javascript'> alert('" + "Error: El recargo no puede ser negativo" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar un decimal en recargo" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }

                }
                if (!error)
                {
                    if (!String.IsNullOrEmpty(des))
                    {
                        try
                        {
                            descuento = decimal.Parse(des);
                            if (descuento < 0 || descuento > 100)
                            {
                                error = true;
                                string script = @"<script type='text/javascript'> alert('" + "Error: El descuento debe estar entre 0 y 100" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar un decimal en descuento" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }

                }
                if (!error)
                {
                    decimal montoTotal = 0;
                    decimal impu = 0;
                    decimal subtotales = 0;


                    try
                    {
                        impuesto = decimal.Parse(imp);
                        decimal monto = Kilos * precio;
                        decimal montoImp = 0;
                        if (impuesto > 0)
                        {
                           // montoImp = (100 * monto) / (100 - impuesto);
                            decimal mult = Convert.ToDecimal(("1." + impuesto), System.Globalization.CultureInfo.InvariantCulture);
                            montoImp = monto * mult;
                        }
                        else
                        {
                            montoImp = monto;
                        }
                        montoTotal = montoImp;
                        if (descuento > 0)
                        {
                            montoTotal = montoTotal - ((montoTotal * descuento) / 100);
                            monto = monto - ((monto * descuento) / 100);
                        }
                        if (recargo > 0)
                        {
                            montoTotal = montoTotal + ((montoTotal * recargo) / 100);
                            monto = monto + ((monto * recargo) / 100);
                        }
                        montoTotal = Math.Round(montoTotal, 2);
                        monto = Math.Round(monto, 2);
                        String montoString = ((Label)row.FindControl("lblMonto")).Text;
                        String subTotalString = ((Label)row.FindControl("lblSubTotal")).Text;
                        if (!String.IsNullOrEmpty(montoString))
                        {
                            Sistema.GetInstancia().SubTotal -= decimal.Parse(subTotalString);
                            Sistema.GetInstancia().Total -= decimal.Parse(montoString);
                            Sistema.GetInstancia().Impuestos -= ((decimal.Parse(subTotalString) * impuesto) / 100);

                            Sistema.GetInstancia().Total = Math.Round(Sistema.GetInstancia().Total, 2);
                            Sistema.GetInstancia().SubTotal = Math.Round(Sistema.GetInstancia().SubTotal, 2);
                            Sistema.GetInstancia().Impuestos = Math.Round(Sistema.GetInstancia().Impuestos, 2);

                        }
                        ((Label)row.FindControl("lblMonto")).Text = montoTotal.ToString();
                        ((Label)row.FindControl("lblSubTotal")).Text = monto.ToString();
                        impu = (monto * impuesto) / 100;
                        subtotales = montoTotal - impu;
                        gridViewFacturas.EditIndex = -1;
                        int cantFilas = index + 1;
                        BindData(cant,ki, des, rec, cantFilas,precio.ToString());
                    }
                    catch { }

                    try
                    {

                        Sistema.GetInstancia().Total += montoTotal;
                        Sistema.GetInstancia().SubTotal += subtotales;
                        Sistema.GetInstancia().Impuestos += impu;

                        Sistema.GetInstancia().Total = Math.Round(Sistema.GetInstancia().Total, 2);
                        Sistema.GetInstancia().SubTotal = Math.Round(Sistema.GetInstancia().SubTotal, 2);
                        Sistema.GetInstancia().Impuestos = Math.Round(Sistema.GetInstancia().Impuestos, 2);

                        txtSubTotal.Text = Sistema.GetInstancia().SubTotal.ToString();

                        txtImpuestos.Text = Sistema.GetInstancia().Impuestos.ToString();
                        txtTotal.Text = Sistema.GetInstancia().Total.ToString();


                    }
                    catch (Exception ex) { }
                }
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al modificar" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }

        }

        protected void Eliminar(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkRemove = (LinkButton)sender;
                String id = lnkRemove.CommandArgument;

                //int index = gridViewFacturas.EditIndex + 1;
                //GridViewRow row = gridViewFacturas.Rows[index];

                DataTable dt = ObtenerDataTable();
                DataRow dr;

                foreach (GridViewRow gvr in gridViewFacturas.Rows)
                {
                    dr = dt.NewRow();

                    Label idProd = gvr.FindControl("lblIdProducto") as Label;
                    Label prod = gvr.FindControl("lblProducto") as Label;
                    Label unidad = gvr.FindControl("lblUnidad") as Label;
                    Label cantidad = gvr.FindControl("lblCantidad") as Label;
                    Label kilos = gvr.FindControl("lblKilos") as Label;
                    Label precio = gvr.FindControl("lblPrecio") as Label;
                    Label impuesto = gvr.FindControl("lblImpuesto") as Label;
                    Label descuento = gvr.FindControl("lblDescuento") as Label;
                    Label recargo = gvr.FindControl("lblRecargo") as Label;
                    Label monto = gvr.FindControl("lblMonto") as Label;
                    Label sub = gvr.FindControl("lblSubTotal") as Label;
                    if (idProd != null)
                    {
                        dr[0] = idProd.Text;
                    }
                    if (prod != null)
                    {
                        dr[1] = prod.Text;
                    }
                    if (unidad != null)
                    {
                        dr[2] = unidad.Text;
                    }
                    if (cantidad != null)
                    {
                        dr[3] = cantidad.Text;
                    }
                    if (kilos != null)
                    {
                        dr[4] = kilos.Text;
                    }
                    if (precio != null)
                    {
                        dr[5] = precio.Text;
                    }
                    if (impuesto != null)
                    {
                        dr[6] = impuesto.Text;
                    }
                    if (descuento != null)
                    {
                        dr[7] = descuento.Text;
                    }
                    if (recargo != null)
                    {
                        dr[8] = recargo.Text;
                    }
                    if (monto != null)
                    {
                        dr[9] = sub.Text;
                    }
                    if (monto != null)
                    {
                        dr[10] = monto.Text;
                    }
                    int ide = 0;
                    try
                    {
                        ide = Int32.Parse(idProd.Text);

                    }
                    catch { }
                    if (ide != Int32.Parse(id))
                    {
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        try
                        {
                            decimal montoTotal = 0;
                            decimal subTotal = 0;
                            decimal impuestos = 0;
                            if (monto != null)
                            {
                                montoTotal = decimal.Parse(monto.Text);
                            }
                            if (sub != null)
                            {
                                subTotal = decimal.Parse(sub.Text);
                            }
                            if (impuesto != null)
                            {
                                impuestos = decimal.Parse(impuesto.Text);
                            }
                            decimal impu = (subTotal * impuestos) / 100;
                            Sistema.GetInstancia().Total -= montoTotal;
                            Sistema.GetInstancia().SubTotal -= subTotal;
                            Sistema.GetInstancia().Impuestos -= impu;

                            Sistema.GetInstancia().Total = Math.Round(Sistema.GetInstancia().Total, 2);
                            Sistema.GetInstancia().SubTotal = Math.Round(Sistema.GetInstancia().SubTotal, 2);
                            Sistema.GetInstancia().Impuestos = Math.Round(Sistema.GetInstancia().Impuestos, 2);

                            txtSubTotal.Text = Sistema.GetInstancia().SubTotal.ToString();
                            txtTotal.Text = Sistema.GetInstancia().Total.ToString();
                            txtImpuestos.Text = Sistema.GetInstancia().Impuestos.ToString();
                        }
                        catch { }
                    }
                }
                gridViewFacturas.DataSource = dt;
                gridViewFacturas.DataBind();


            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al quitar" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gridViewFacturas.PageIndex = e.NewPageIndex;
                BindData();
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void OnPaging2(object sender, GridViewPageEventArgs e)
        {
            try
            {
                
                gridViewDocumentos.PageIndex = e.NewPageIndex;
                
               // BindData2();
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {

            this.txtFecha.Focus();

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ddlProductos.SelectedValue != null && !ddlProductos.SelectedValue.Equals(""))
            {
                try
                {
                    int idProducto = Int32.Parse(ddlProductos.SelectedValue);
                    Producto producto = Sistema.GetInstancia().BuscarProductoId(idProducto);
                    if (producto != null)
                    {
                        Linea linea = new Linea();
                        List<Linea> lineas = new List<Linea>();
                        DataTable dt = ObtenerDataTable();
                        DataRow dr;
                        int cont = -1;
                        linea.Producto = producto.Nombre;
                        linea.Precio = producto.Precio;
                        linea.UnidadMedida = producto.unidadMedida;
                        linea.IdProducto = producto.IdProducto;


                        foreach (GridViewRow gvr in gridViewFacturas.Rows)
                        {
                            dr = dt.NewRow();

                            Label idProd = gvr.FindControl("lblIdProducto") as Label;
                            Label prod = gvr.FindControl("lblProducto") as Label;
                            Label unidad = gvr.FindControl("lblUnidad") as Label;
                            Label cantidad = gvr.FindControl("lblCantidad") as Label;
                            Label kilos = gvr.FindControl("lblKilos") as Label;
                            Label precio = gvr.FindControl("lblPrecio") as Label;
                            Label impuesto = gvr.FindControl("lblImpuesto") as Label;
                            Label descuento = gvr.FindControl("lblDescuento") as Label;
                            Label recargo = gvr.FindControl("lblRecargo") as Label;
                            Label monto = gvr.FindControl("lblMonto") as Label;
                            Label sub = gvr.FindControl("lblSubTotal") as Label;
                            if (idProd != null)
                            {
                                dr[0] = idProd.Text;
                            }
                            if (prod != null)
                            {
                                dr[1] = prod.Text;
                            }
                            if (unidad != null)
                            {
                                dr[2] = unidad.Text;
                            }
                            if (cantidad != null)
                            {
                                dr[3] = cantidad.Text;
                            }
                            if (kilos != null)
                            {
                                dr[4] = kilos.Text;
                            }
                            if (precio != null)
                            {
                                dr[5] = precio.Text;
                            }
                            if (impuesto != null)
                            {
                                dr[6] = impuesto.Text;
                            }
                            if (descuento != null)
                            {
                                dr[7] = descuento.Text;
                            }
                            if (recargo != null)
                            {
                                dr[8] = recargo.Text;
                            }
                            if (sub != null)
                            {
                                dr[9] = sub.Text;
                            }
                            if (monto != null)
                            {
                                dr[10] = monto.Text;
                            }
                            dt.Rows.Add(dr);
                            cont++;
                            Linea nueva = new Linea();
                            if (!String.IsNullOrEmpty(cantidad.Text))
                            {
                                nueva.Cantidad = decimal.Parse(cantidad.Text);
                            }
                            if (!String.IsNullOrEmpty(kilos.Text))
                            {
                                nueva.Kilos = decimal.Parse(kilos.Text);
                            }
                            if (!String.IsNullOrEmpty(precio.Text))
                            {
                                nueva.Precio = decimal.Parse(precio.Text);
                            }
                            if (!String.IsNullOrEmpty(impuesto.Text))
                            {
                                nueva.Impuesto = Int32.Parse(impuesto.Text);
                            }
                            if (!String.IsNullOrEmpty(cantidad.Text))
                            {
                                nueva.Cantidad = decimal.Parse(cantidad.Text);
                            }
                            if (!String.IsNullOrEmpty(descuento.Text))
                            {
                                nueva.Descuento = decimal.Parse(descuento.Text);
                            }
                            if (!String.IsNullOrEmpty(recargo.Text))
                            {
                                nueva.Recargo = decimal.Parse(recargo.Text);
                            }
                            if (!String.IsNullOrEmpty(monto.Text))
                            {
                                nueva.MontoTotal = decimal.Parse(monto.Text);
                            }
                            nueva.IdProducto = Int32.Parse(idProd.Text);
                            nueva.Producto = prod.Text;
                            nueva.UnidadMedida = unidad.Text;

                            lineas.Add(nueva);
                        }
                        if (lineas.Contains(linea))
                        {
                            string script = @"<script type='text/javascript'> alert('" + "El producto se encuentra seleccionado" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                        else
                        {
                            dr = dt.NewRow();
                            dr[0] = producto.IdProducto;
                            dr[1] = producto.Nombre;
                            dr[2] = producto.unidadMedida;
                            dr[5] = producto.Precio;
                            if (producto.indicador.Codigo == 2)
                            {
                                dr[6] = Sistema.GetInstancia().TasaMinima().ToString();
                            }
                            else if (producto.indicador.Codigo == 3)
                            {
                                dr[6] = Sistema.GetInstancia().TasaBasica().ToString();
                            }
                            else
                            {
                                dr[6] = "0";
                            }
                            dt.Rows.Add(dr);
                            cont++;
                            gridViewFacturas.EditIndex = cont;
                            gridViewFacturas.DataSource = dt;
                            gridViewFacturas.DataBind();
                        }
                    }
                }
                catch { }
            }
            else
            {
                string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un producto" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    Documento doc = new Documento();
                    doc.Activo = true;
                    bool error = false;
                    Cliente cli = Sistema.GetInstancia().BuscarClienteId(Int32.Parse(ddlCliente.SelectedValue));
                    doc.cliente = cli;
                    doc.Fecha = DateTime.Parse(txtFecha.Text);
                    doc.IdCliente = cli.IdCliente;
                    doc.Moneda = ddlMoneda.SelectedValue;
                    doc.FormaPago = ddlFormaPago.SelectedValue;
                    doc.rut = Session["rut"].ToString();
                    doc.IdUsuario = Int32.Parse(Session["idUsuario"].ToString());
                    doc.Usuario = Sistema.GetInstancia().BuscarUsuarioId(doc.IdUsuario);
                    doc.EstadoCredito = "DEBE";
                    doc.DBCR = "CR";
                    if (!error)
                    {
                        if (doc.Moneda.Equals("Dolar"))
                        {
                            try
                            {
                                doc.tipoCambio = decimal.Parse(txtTipoCambio.Text);
                            }
                            catch
                            {
                                error = true;
                                string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar un decimal en tipo de cambio" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                        }
                    }

                    if (cli.tipoDocumento.Equals("RUT"))
                    {
                        doc.TipoDocumento = "112";
                    }
                    else
                    {
                        doc.TipoDocumento = "102";

                    }
                    doc.detalle = ObtenerDetalle();
                    doc.Total = Sistema.GetInstancia().Total;
                    doc.Total = Math.Round(doc.Total, 0);
                    doc.documentosAsociados = DocumentosAsociados();
                    if (!error)
                    {
                        XmlDocument xml = null;
                        String xmlTexto = "";
                        if (doc.cliente.tipoDocumento.Equals("RUT"))
                        {
                            xml = GenerarXmlFactura(doc);
                            xmlTexto = AjustarCFE(xml.InnerXml, "eFact");
                        }
                        else
                        {
                            xml = GenerarXmlTicket(doc);
                            xmlTexto = AjustarCFE(xml.InnerXml, "eTck");
                        }
                        doc.Total = Sistema.GetInstancia().Total;
                        try
                        {

                            var client = new CfeServiceClient();
                            var client2 = new ConsultaCfeClient();
                            client.ClientCredentials.UserName.UserName = Session["rut"].ToString();
                            client.ClientCredentials.UserName.Password = Sistema.GetInstancia().Contrasena(Session["rut"].ToString());
                            client2.ClientCredentials.UserName.UserName = Session["rut"].ToString();
                            client2.ClientCredentials.UserName.Password = Sistema.GetInstancia().Contrasena(Session["rut"].ToString());
                            ReqBody solicitud = new ReqBody();
                            solicitud.CodComercio = Sistema.GetInstancia().CodComercio(Session["rut"].ToString()); 
                            solicitud.CodTerminal = Sistema.GetInstancia().CodTerminal(Session["rut"].ToString()); 
                            solicitud.HMAC = "";
                            RequerimientoParaUcfe req = new RequerimientoParaUcfe();
                            req.TipoMensaje = 310;
                            // req.Uuid = "10000222";
                            req.Uuid = "NC" + Sistema.GetInstancia().ObtenerProximoCodigo(Session["rut"].ToString()).ToString();
                            req.TipoCfe = doc.TipoDocumento;
                            req.IdReq = "1";
                            req.FechaReq = doc.Fecha.Year + "" + doc.Fecha.Month + "" + doc.Fecha.Day;
                            req.HoraReq = doc.Fecha.Hour + "" + doc.Fecha.Minute + "" + doc.Fecha.Second;
                            req.CodComercio = Sistema.GetInstancia().CodComercio(Session["rut"].ToString()); 
                            req.CodTerminal = Sistema.GetInstancia().CodTerminal(Session["rut"].ToString());
                            if (cli != null && !String.IsNullOrEmpty(cli.Mail))
                            {
                                req.EmailEnvioPdfReceptor = cli.Mail;

                            }
                            if (!String.IsNullOrEmpty(txtAdenda.Text)) {
                                req.Adenda = txtAdenda.Text;
                            }
                            req.CfeXmlOTexto = xmlTexto;
                            solicitud.Req = req;
                            solicitud.RequestDate = doc.Fecha.Year + "-" + doc.Fecha.Month + "-" + doc.Fecha.Day + "T" + doc.Fecha.Hour + ":" + doc.Fecha.Minute + ":" + doc.Fecha.Second;
                            solicitud.Tout = 120000;
                            RespBody respuesta = null;
                            if (client.InnerChannel.State != System.ServiceModel.CommunicationState.Faulted)
                            {
                                respuesta = client.Invoke(solicitud);
                            }
                            if(respuesta !=null){
                                if (respuesta.ErrorCode != 0)
                                {
                                    string script = @"<script type='text/javascript'> alert('" + "Error: " + respuesta.ErrorMessage + "');</script>";
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                }
                                else
                                {
                                    if (respuesta.Resp.CodRta.Equals("00"))
                                    {
                                        doc.NroSerie = Int32.Parse(respuesta.Resp.NumeroCfe);
                                        doc.Serie = respuesta.Resp.Serie;
                                        doc.xmlFirmado = respuesta.Resp.XmlCfeFirmado;
                                        doc.EstadoDGI = "Procesado";
                                        String msg = Sistema.GetInstancia().GuardarDocumento(doc);
                                        if (doc != null & doc.detalle != null) {
                                            foreach (Detalle det in doc.detalle) {
                                                Sistema.GetInstancia().AumentarStock(det.IdProducto,det.Cantidad,det.Kilos);
                                            }
                                        }
                                        try
                                        {
                                            byte[] pdf = client2.ObtenerPdf(Session["rut"].ToString(), Int32.Parse(respuesta.Resp.TipoCfe), respuesta.Resp.Serie, doc.NroSerie);
                                            Sistema.GetInstancia().PDFActual = pdf;
                                            if (Sistema.GetInstancia().PDFActual != null)
                                            {
                                                Response.Redirect("VisorPDF.aspx");
                                            }
                                            
                                        }
                                        catch (Exception ex) { }

                                    }
                                    else if (respuesta.Resp.CodRta.Equals("11"))
                                    {
                                        doc.NroSerie = Int32.Parse(respuesta.Resp.NumeroCfe);
                                        doc.Serie = respuesta.Resp.Serie;
                                        doc.xmlFirmado = respuesta.Resp.XmlCfeFirmado;
                                        doc.EstadoDGI = "Aceptado";
                                        String msg = Sistema.GetInstancia().GuardarDocumento(doc);
                                        if (doc != null & doc.detalle != null)
                                        {
                                            foreach (Detalle det in doc.detalle)
                                            {
                                                Sistema.GetInstancia().AumentarStock(det.IdProducto, det.Cantidad, det.Kilos);
                                            }
                                        }
                                        try
                                        {
                                            byte[] pdf = client2.ObtenerPdf(Session["rut"].ToString(), Int32.Parse(respuesta.Resp.TipoCfe), respuesta.Resp.Serie, doc.NroSerie);
                                            Sistema.GetInstancia().PDFActual = pdf;
                                            if (Sistema.GetInstancia().PDFActual != null)
                                            {
                                                Response.Redirect("VisorPDF.aspx");
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            
                                        }

                                    }
                                    else if (respuesta.Resp.CodRta.Equals("01"))
                                    {
                                        string script = @"<script type='text/javascript'> alert('" + "Denegado: " + respuesta.Resp.MensajeRta + "');</script>";
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                    }
                                    else if (respuesta.Resp.CodRta.Equals("03"))
                                    {
                                        string script = @"<script type='text/javascript'> alert('" + "Comercio invalido" + "');</script>";
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                    }
                                    else if (respuesta.Resp.CodRta.Equals("05"))
                                    {
                                        string script = @"<script type='text/javascript'> alert('" + "CFE Rechazado por DGI" + "');</script>";
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                    }
                                    else if (respuesta.Resp.CodRta.Equals("12"))
                                    {
                                        string script = @"<script type='text/javascript'> alert('" + "Requerimiento invalido" + "');</script>";
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                    }
                                    else if (respuesta.Resp.CodRta.Equals("30"))
                                    {
                                        string script = @"<script type='text/javascript'> alert('" + "Error en formato: " + respuesta.Resp.MensajeRta + "');</script>";
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                    }
                                    else if (respuesta.Resp.CodRta.Equals("31"))
                                    {
                                        string script = @"<script type='text/javascript'> alert('" + "Error en formato CFE" + "');</script>";
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                    }
                                    else if (respuesta.Resp.CodRta.Equals("89"))
                                    {
                                        string script = @"<script type='text/javascript'> alert('" + "Terminal invalida" + "');</script>";
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                    }
                                    else if (respuesta.Resp.CodRta.Equals("96"))
                                    {
                                        string script = @"<script type='text/javascript'> alert('" + "Error en sistema" + "');</script>";
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                    }
                                    else if (respuesta.Resp.CodRta.Equals("99"))
                                    {
                                        string script = @"<script type='text/javascript'> alert('" + "Sesion no iniciada" + "');</script>";
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                    }
                                    else
                                    {

                                        string script = @"<script type='text/javascript'> alert('" + "Denegado: " + respuesta.Resp.MensajeRta + "');</script>";
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);

                                    }
                                }
                            }
                            else
                            {
                                string script = @"<script type='text/javascript'> alert('" + "Error de conexión con el punto de emisión" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }


                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException == null)
                            {
                                string script = @"<script type='text/javascript'> alert('" + "Error: " + ex.Message + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                            else
                            {
                                string script = @"<script type='text/javascript'> alert('" + "Error: " + ex.InnerException.Message + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                        }

                        //Guardar serie y nro

                    }
                }
                catch (Exception ex)
                {
                    string script = @"<script type='text/javascript'> alert('" + "Error al guardar" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);

                }
            }
        }

        protected void btnObtenerDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gridViewDocumentos.Rows)
                {
                    var chkBox = row.FindControl("ckActivo") as CheckBox;

                    IDataItemContainer container = (IDataItemContainer)chkBox.NamingContainer;

                    if (chkBox.Checked)
                    {
                        PersistRowIndex(container.DataItemIndex);
                    }
                    else
                    {
                        RemoveRowIndex(container.DataItemIndex);
                    }
                }

                DataTable dt = ObtenerDataTable();
                DataRow dr;
                List<Linea> lineas = new List<Linea>();
                if (gridViewFacturas != null && gridViewFacturas.Rows != null)
                {
                    foreach (GridViewRow gvr in gridViewFacturas.Rows)
                    {
                        dr = dt.NewRow();

                        Label idProd = gvr.FindControl("lblIdProducto") as Label;
                        Label prod = gvr.FindControl("lblProducto") as Label;
                        Label unidad = gvr.FindControl("lblUnidad") as Label;
                        Label cantidad = gvr.FindControl("lblCantidad") as Label;
                        Label kilos = gvr.FindControl("lblKilos") as Label;
                        Label precio = gvr.FindControl("lblPrecio") as Label;
                        Label impuesto = gvr.FindControl("lblImpuesto") as Label;
                        Label descuento = gvr.FindControl("lblDescuento") as Label;
                        Label recargo = gvr.FindControl("lblRecargo") as Label;
                        Label monto = gvr.FindControl("lblMonto") as Label;
                        Label sub = gvr.FindControl("lblSubTotal") as Label;
                        if (idProd != null)
                        {
                            dr[0] = idProd.Text;
                        }
                        if (prod != null)
                        {
                            dr[1] = prod.Text;
                        }
                        if (unidad != null)
                        {
                            dr[2] = unidad.Text;
                        }
                        if (cantidad != null)
                        {
                            dr[3] = cantidad.Text;
                        }
                        if (kilos != null)
                        {
                            dr[4] = kilos.Text;
                        }
                        if (precio != null)
                        {
                            dr[5] = precio.Text;
                        }
                        if (impuesto != null)
                        {
                            dr[6] = impuesto.Text;
                        }
                        if (descuento != null)
                        {
                            dr[7] = descuento.Text;
                        }
                        if (recargo != null)
                        {
                            dr[8] = recargo.Text;
                        }
                        if (monto != null)
                        {
                            dr[9] = sub.Text;
                        }
                        if (monto != null)
                        {
                            dr[10] = monto.Text;
                        }
                        dt.Rows.Add(dr);

                        Linea nueva = new Linea();
                        if (!String.IsNullOrEmpty(cantidad.Text))
                        {
                            nueva.Cantidad = decimal.Parse(cantidad.Text);
                        }
                        if (!String.IsNullOrEmpty(kilos.Text))
                        {
                            nueva.Kilos = decimal.Parse(kilos.Text);
                        }
                        if (!String.IsNullOrEmpty(precio.Text))
                        {
                            nueva.Precio = decimal.Parse(precio.Text);
                        }
                        if (!String.IsNullOrEmpty(impuesto.Text))
                        {
                            nueva.Impuesto = Int32.Parse(impuesto.Text);
                        }
                        if (!String.IsNullOrEmpty(cantidad.Text))
                        {
                            nueva.Cantidad = decimal.Parse(cantidad.Text);
                        }
                        if (!String.IsNullOrEmpty(descuento.Text))
                        {
                            nueva.Descuento = decimal.Parse(descuento.Text);
                        }
                        if (!String.IsNullOrEmpty(recargo.Text))
                        {
                            nueva.Recargo = decimal.Parse(recargo.Text);
                        }
                        if (!String.IsNullOrEmpty(monto.Text))
                        {
                            nueva.MontoTotal = decimal.Parse(monto.Text);
                        }
                        nueva.IdProducto = Int32.Parse(idProd.Text);
                        nueva.Producto = prod.Text;
                        nueva.UnidadMedida = unidad.Text;

                        lineas.Add(nueva);
                    }
                }
                List<Documento> documentos = DocumentosAsociados();
                decimal sumaSubTotal = 0;
                decimal sumaTotal = 0;
                foreach (Documento doc in documentos) {

                    if (doc.detalle != null) {
                        foreach (Detalle detalle in doc.detalle) {
                            Linea linea = new Linea();
                            linea.Producto = detalle.producto.Nombre;
                            linea.Precio = detalle.producto.Precio;
                            linea.UnidadMedida = detalle.producto.unidadMedida;
                            linea.IdProducto = detalle.producto.IdProducto;
                            if (lineas.Contains(linea))
                            {
                                string script = @"<script type='text/javascript'> alert('" + "El producto se encuentra seleccionado" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                            else
                            {
                                dr = dt.NewRow();
                                dr[0] = detalle.producto.IdProducto;
                                dr[1] = detalle.producto.Nombre;
                                dr[2] = detalle.producto.unidadMedida;
                                dr[3] = detalle.Cantidad;
                                dr[4] = detalle.Kilos;
                                dr[5] = detalle.producto.Precio;
                                if (detalle.producto.indicador.Codigo == 2)
                                {
                                    dr[6] = Sistema.GetInstancia().TasaMinima().ToString();
                                }
                                else if (detalle.producto.indicador.Codigo == 3)
                                {
                                    dr[6] = Sistema.GetInstancia().TasaBasica().ToString();
                                }
                                else
                                {
                                    dr[6] = "0";
                                }
                                dr[7] = detalle.Descuento;
                                
                                dr[8] = detalle.Recargo;

                                if (detalle.producto.indicador.Codigo == 2)
                                {
                                    decimal mntIVA = Convert.ToDecimal(("1." + Sistema.GetInstancia().TasaMinima()), System.Globalization.CultureInfo.InvariantCulture);
                                    dr[9] = detalle.MontoTotal / mntIVA;
                                }
                                else if (detalle.producto.indicador.Codigo == 3)
                                {
                                    decimal mntIVA = Convert.ToDecimal(("1." + Sistema.GetInstancia().TasaBasica()),System.Globalization.CultureInfo.InvariantCulture);
                                    dr[9] = detalle.MontoTotal / mntIVA;
                                }
                                else {
                                    dr[9] = detalle.MontoTotal;
                                }
                                
                                dr[10] = detalle.MontoTotal;
                                sumaTotal += detalle.MontoTotal;
                                sumaSubTotal += decimal.Parse(dr[9].ToString());

                                lineas.Add(linea);
                                dt.Rows.Add(dr);

                                
                            }
                        }
                    }   
                }
                try
                {
                    if (Sistema.GetInstancia().Total != null)
                    {
                        Sistema.GetInstancia().Total = (Sistema.GetInstancia().Total + sumaTotal);
                    }
                    else { 
                        Sistema.GetInstancia().Total = sumaTotal;
                    }
                    Sistema.GetInstancia().Total = Math.Round(Sistema.GetInstancia().Total, 2);
                    if (Sistema.GetInstancia().SubTotal !=null)
                    {
                        Sistema.GetInstancia().SubTotal = (Sistema.GetInstancia().SubTotal + sumaSubTotal);
                    }
                    else {
                        Sistema.GetInstancia().SubTotal = sumaSubTotal;
                    }
                    Sistema.GetInstancia().SubTotal = Math.Round(Sistema.GetInstancia().SubTotal, 2);
                    if (Sistema.GetInstancia().Impuestos != null)
                    {
                        Sistema.GetInstancia().Impuestos = (Sistema.GetInstancia().Impuestos + (sumaTotal - sumaSubTotal));
                    }
                    else {
                        Sistema.GetInstancia().Impuestos = (sumaTotal - sumaSubTotal);
                    }
                    Sistema.GetInstancia().Impuestos = Math.Round(Sistema.GetInstancia().Impuestos, 2);
                    txtImpuestos.Text = Sistema.GetInstancia().Impuestos.ToString();
                    txtSubTotal.Text = Sistema.GetInstancia().SubTotal.ToString();
                    txtTotal.Text = Sistema.GetInstancia().Total.ToString();
                }
                catch { }
                gridViewFacturas.DataSource = dt;
                gridViewFacturas.DataBind();
            }
            catch(Exception ex) { }
        }

        public DataTable ObtenerDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("IdProducto", typeof(string));
            table.Columns.Add("Producto", typeof(string));
            table.Columns.Add("UnidadMedida", typeof(string));
            table.Columns.Add("Cantidad", typeof(string));
            table.Columns.Add("Kilos", typeof(string));
            table.Columns.Add("Precio", typeof(string));
            table.Columns.Add("Impuesto", typeof(string));
            table.Columns.Add("Descuento", typeof(string));
            table.Columns.Add("Recargo", typeof(string));
            table.Columns.Add("SubTotal", typeof(string));
            table.Columns.Add("MontoTotal", typeof(string));
            return table;
        }

        private void BindData(String cant, String ki, String des, String rec, int CantFilas, String pre)
        {
            DataTable dt = ObtenerDataTable();
            DataRow dr;
            int cont = 1;
            foreach (GridViewRow gvr in gridViewFacturas.Rows)
            {
                dr = dt.NewRow();

                Label idProd = gvr.FindControl("lblIdProducto") as Label;
                Label prod = gvr.FindControl("lblProducto") as Label;
                Label unidad = gvr.FindControl("lblUnidad") as Label;
                Label cantidad = gvr.FindControl("lblCantidad") as Label;
                Label kilos = gvr.FindControl("lblKilos") as Label;
                Label precio = gvr.FindControl("lblPrecio") as Label;
                Label impuesto = gvr.FindControl("lblImpuesto") as Label;
                Label descuento = gvr.FindControl("lblDescuento") as Label;
                Label recargo = gvr.FindControl("lblRecargo") as Label;
                Label monto = gvr.FindControl("lblMonto") as Label;
                Label sub = gvr.FindControl("lblSubTotal") as Label;
                if (idProd != null)
                {
                    dr[0] = idProd.Text;
                }
                if (prod != null)
                {
                    dr[1] = prod.Text;
                }
                if (unidad != null)
                {
                    dr[2] = unidad.Text;
                }
                if (cantidad != null)
                {
                    dr[3] = cantidad.Text;
                }
                if (kilos != null)
                {
                    dr[4] = kilos.Text;
                }
                if (precio != null)
                {
                    dr[5] = precio.Text;
                }
                if (impuesto != null)
                {
                    dr[6] = impuesto.Text;
                }
                if (descuento != null)
                {
                    dr[7] = descuento.Text;
                }
                if (recargo != null)
                {
                    dr[8] = recargo.Text;
                }
                if (sub != null)
                {
                    dr[9] = sub.Text;
                }
                if (monto != null)
                {
                    dr[10] = monto.Text;
                }

                if (cont == CantFilas)
                {
                    dr[3] = cant;
                    dr[4] = ki;
                    dr[5] = pre;
                    dr[7] = des;
                    dr[8] = rec;
                }
                dt.Rows.Add(dr);
                cont++;
            }
            gridViewFacturas.DataSource = dt;
            gridViewFacturas.DataBind();
        }

        private void BindData()
        {
            DataTable dt = ObtenerDataTable();
            DataRow dr;

            foreach (GridViewRow gvr in gridViewFacturas.Rows)
            {
                dr = dt.NewRow();

                Label idProd = gvr.FindControl("lblIdProducto") as Label;
                Label prod = gvr.FindControl("lblProducto") as Label;
                Label unidad = gvr.FindControl("lblUnidad") as Label;
                Label cantidad = gvr.FindControl("lblCantidad") as Label;
                Label kilos = gvr.FindControl("lblKilos") as Label;
                Label precio = gvr.FindControl("lblPrecio") as Label;
                Label impuesto = gvr.FindControl("lblImpuesto") as Label;
                Label descuento = gvr.FindControl("lblDescuento") as Label;
                Label recargo = gvr.FindControl("lblRecargo") as Label;
                Label monto = gvr.FindControl("lblMonto") as Label;
                Label sub = gvr.FindControl("lblSubTotal") as Label;
                if (idProd != null)
                {
                    dr[0] = idProd.Text;
                }
                if (prod != null)
                {
                    dr[1] = prod.Text;
                }
                if (unidad != null)
                {
                    dr[2] = unidad.Text;
                }
                if (cantidad != null)
                {
                    dr[3] = cantidad.Text;
                }
                if (kilos != null)
                {
                    dr[4] = kilos.Text;
                }
                if (precio != null)
                {
                    dr[5] = precio.Text;
                }
                if (impuesto != null)
                {
                    dr[6] = impuesto.Text;
                }
                if (descuento != null)
                {
                    dr[7] = descuento.Text;
                }
                if (recargo != null)
                {
                    dr[8] = recargo.Text;
                }
                if (sub != null)
                {
                    dr[9] = sub.Text;
                }
                if (monto != null)
                {
                    dr[10] = monto.Text;
                }


                dt.Rows.Add(dr);

            }
            List<TablaDetalle> detalle = ConvertirDataTable(dt);
            gridViewFacturas.DataSource = detalle;
            gridViewFacturas.DataBind();
        }

        private void BindData2()
        {
            String tipoDocumento = "";
            if (!String.IsNullOrEmpty(ddlCliente.SelectedValue))
            {
                if (!String.IsNullOrEmpty(ddlTipoDocumento.SelectedValue) && !ddlTipoDocumento.SelectedValue.Equals("Todos"))
                {
                    tipoDocumento = ddlTipoDocumento.SelectedValue;
                }
                int idcliente = Int32.Parse(ddlCliente.SelectedValue);
                List<Documento> documentos = Sistema.GetInstancia().ObtenerDocumentosCliente(idcliente, tipoDocumento, Session["rut"].ToString()).ToList();
                gridViewDocumentos.DataSource = documentos; 
                gridViewDocumentos.DataBind();
            }
            RePopulateCheckBoxes(); 
            
        }

        private List<TablaDetalle> ConvertirDataTable(DataTable dt)
        {
            List<TablaDetalle> tabla = new List<TablaDetalle>();
            foreach (DataRow row in dt.Rows) {
                TablaDetalle det = new TablaDetalle();
                det.IdProducto = row[0].ToString();
                det.Producto = row[1].ToString();
                det.UnidadMedida = row[2].ToString();
                det.Cantidad = row[3].ToString();
                det.Kilos = row[4].ToString();
                det.Precio = row[5].ToString();
                det.Impuesto = row[6].ToString();
                det.Descuento = row[7].ToString();
                det.Recargo = row[8].ToString();
                det.SubTotal = row[9].ToString();
                det.MontoTotal = row[10].ToString();
                tabla.Add(det);
            }
            return tabla;
        }

        protected void gvCustomersPageChanging(object sender, GridViewPageEventArgs e)
        {
            foreach (GridViewRow row in gridViewDocumentos.Rows)
            {
                var chkBox = row.FindControl("ckActivo") as CheckBox;

                IDataItemContainer container = (IDataItemContainer)chkBox.NamingContainer;

                if (chkBox.Checked)
                {
                    PersistRowIndex(container.DataItemIndex);
                }
                else
                {
                    RemoveRowIndex(container.DataItemIndex);
                }
            }

            gridViewDocumentos.PageIndex = e.NewPageIndex;
            BindData2();
        }

        private List<Int32> SelectedCustomersIndex
        {
            get
            {
                if (ViewState[SELECTED_CUSTOMERS_INDEX] == null)
                {
                    ViewState[SELECTED_CUSTOMERS_INDEX] = new List<Int32>();
                }

                return (List<Int32>)ViewState[SELECTED_CUSTOMERS_INDEX];
            }
        }

        private void RemoveRowIndex(int index)
        {
            SelectedCustomersIndex.Remove(index);
        }

        private void PersistRowIndex(int index)
        {
            if (!SelectedCustomersIndex.Exists(i => i == index))
            {
                SelectedCustomersIndex.Add(index);
            }
        }

        private void RePopulateCheckBoxes()
        {
            foreach (GridViewRow row in gridViewDocumentos.Rows)
            {
                var chkBox = row.FindControl("ckActivo") as CheckBox;

                IDataItemContainer container = (IDataItemContainer)chkBox.NamingContainer;

                if (SelectedCustomersIndex != null)
                {
                    if (SelectedCustomersIndex.Exists(i => i == container.DataItemIndex))
                    {
                        chkBox.Checked = true;
                    }
                    else {
                        chkBox.Checked = false;
                    }
                }
            }
        }

        private List<Detalle> ObtenerDetalle()
        {

            List<Detalle> retorno = new List<Detalle>();
            int cont = 1;
            foreach (GridViewRow gvr in gridViewFacturas.Rows)
            {

                Detalle detalle = new Detalle();
                Label idProd = gvr.FindControl("lblIdProducto") as Label;
                Label cantidad = gvr.FindControl("lblCantidad") as Label;
                Label kilos = gvr.FindControl("lblKilos") as Label;
                Label descuento = gvr.FindControl("lblDescuento") as Label;
                Label recargo = gvr.FindControl("lblRecargo") as Label;
                Label monto = gvr.FindControl("lblMonto") as Label;
                Label sub = gvr.FindControl("lblSubTotal") as Label;
                Label precioU = gvr.FindControl("lblPrecio") as Label;
                if (precioU != null) {
                    detalle.PrecioUnitario = decimal.Parse(precioU.Text);
                }
                if (idProd != null)
                {
                    Producto producto = Sistema.GetInstancia().BuscarProductoId(Int32.Parse(idProd.Text));
                    detalle.producto = producto;
                    detalle.IdProducto = producto.IdProducto;
                }

                if (cantidad != null)
                {
                    detalle.Cantidad = decimal.Parse(cantidad.Text);
                }
                if (kilos != null)
                {
                    detalle.Kilos = decimal.Parse(kilos.Text);
                }

                if (descuento != null)
                {
                    if (!String.IsNullOrEmpty(descuento.Text))
                    {
                        detalle.Descuento = decimal.Parse(descuento.Text);
                    }
                }
                if (recargo != null)
                {
                    if (!String.IsNullOrEmpty(recargo.Text))
                    {
                        detalle.Recargo = decimal.Parse(recargo.Text);
                    }
                }

                if (monto != null)
                {
                    detalle.MontoTotal = decimal.Parse(monto.Text);
                }

                detalle.NroLinea = 1;
                cont++;
                retorno.Add(detalle);

            }
            return retorno;
        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGrupo.SelectedValue != null)
            {
                int IdGrupo = Int32.Parse(ddlGrupo.SelectedValue);
                List<SubGrupo> subgrupos = Sistema.GetInstancia().ObtenerSubGruposGrupo(IdGrupo, Session["rut"].ToString()).ToList();
                ddlSubGrupo.DataSource = subgrupos;
                ddlSubGrupo.DataTextField = "Descripcion";
                ddlSubGrupo.DataValueField = "IdSubGrupo";
                ddlSubGrupo.DataBind();
                if (subgrupos != null && subgrupos.Count > 0)
                {
                    ddlProductos.DataSource = Sistema.GetInstancia().BuscarProductosSubGrupo(subgrupos.ElementAt(0).IdSubGrupo, Session["rut"].ToString());
                    ddlProductos.DataTextField = "ddlDescription";
                    ddlProductos.DataValueField = "IdProducto";
                    ddlProductos.DataBind();
                }
            }

        }

        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMoneda.SelectedValue != null)
            {
                if (ddlMoneda.SelectedValue.Equals("Dolar"))
                {
                    TipoCambio.Visible = true;
                    float dolar = Sistema.GetInstancia().ObtenerCotizacionDolar();
                    if (dolar > 0.0)
                    {
                        txtTipoCambio.Text = dolar.ToString();
                    }
                    else
                    {
                        txtTipoCambio.Text = "0.0";
                    }
                }
                else
                {
                    TipoCambio.Visible = false;
                }
            }

        }

        protected void ddlSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSubGrupo.SelectedValue != null)
            {

                int idSubGrupo = Int32.Parse(ddlSubGrupo.SelectedValue);
                List<Producto> productos = Sistema.GetInstancia().BuscarProductosSubGrupo(idSubGrupo, Session["rut"].ToString()).ToList();
                ddlProductos.DataSource = productos;
                ddlProductos.DataTextField = "ddlDescription";
                ddlProductos.DataValueField = "IdProducto";
                ddlProductos.DataBind();
            }

        }

        private void limpiarFormulario()
        {
            Sistema.GetInstancia().SubTotal = 0;
            Sistema.GetInstancia().Total = 0;
            Sistema.GetInstancia().Impuestos = 0;

            Sistema.GetInstancia().PDFActual = null;
        }

        private XmlDocument GenerarXmlFactura(Documento doc)
        {
            XmlDocument xml = new XmlDocument();
            String result = "";

            CFEDefTypeEFact factura = new CFEDefTypeEFact();

            //Encabezado

            CFEDefTypeEFactEncabezado encabezado = new CFEDefTypeEFactEncabezado();

            // IdDoc
            try
            {
                IdDoc_Fact idDoc = new IdDoc_Fact();
                //idDoc.Nro = "";
                //idDoc.Serie = "";
                idDoc.FchEmis = doc.Fecha;
                idDoc.FchVencSpecified = false;
                if (doc.FormaPago.Equals("Credito"))
                {
                    idDoc.FmaPago = IdDoc_FactFmaPago.Item2;

                }
                else
                {
                    idDoc.FmaPago = IdDoc_FactFmaPago.Item1;
                }
                idDoc.TipoCFE = IdDoc_FactTipoCFE.Item112;
                idDoc.MntBruto = IdDoc_FactMntBruto.Item1;
                idDoc.MntBrutoSpecified = true;
                encabezado.IdDoc = idDoc;
            }
            catch (Exception ex)
            {

            }

            //Emisor

            try
            {
                DatosEmisor datos = Sistema.GetInstancia().ObtenerDatosEmisor(Session["rut"].ToString());
                Emisor emisor = new Emisor();

                emisor.RznSoc = datos.razonSocial;
                emisor.RUCEmisor = datos.ruc;
                emisor.NomComercial = datos.nomComercial;

                emisor.Telefono = datos.Telefonos;

                emisor.CorreoEmisor = datos.Correo;
                emisor.EmiSucursal = datos.Sucursal;
                emisor.CdgDGISucur = datos.CodigoSucursal;
                emisor.DomFiscal = datos.DomicilioFiscal;
                emisor.Ciudad = datos.Ciudad;
                emisor.Departamento = datos.Departamento;
                emisor.GiroEmis = datos.Giro;

                encabezado.Emisor = emisor;

            }
            catch (Exception ex)
            {

            }

            //Receptor
            try
            {

                Receptor_Fact receptor = new Receptor_Fact();
                Cliente cli = Sistema.GetInstancia().BuscarClienteId(doc.IdCliente);
                receptor.CodPaisRecep = CodPaisType.UY;
                if (!String.IsNullOrEmpty(cli.Ciudad))
                {
                    receptor.CiudadRecep = cli.Ciudad;
                }
                if (!String.IsNullOrEmpty(cli.CodigoPostal))
                {
                    receptor.CP = cli.CodigoPostal;
                }
                if (!String.IsNullOrEmpty(cli.Direccion))
                {
                    receptor.DirRecep = cli.Direccion;
                }
                receptor.DocRecep = cli.nroDoc.ToString();
                receptor.RznSocRecep = cli.Nombre;
                receptor.TipoDocRecep = DocType.Item2;

                encabezado.Receptor = receptor;

            }
            catch (Exception e)
            {

            }

            //Detalle
            decimal sumaMontoNoGravados = 0;
            decimal sumaMontoNoFacturable = 0;
            decimal sumaMontoImpuestoPercibido = 0;
            decimal sumaMontoIvaEnSuspenso = 0;
            decimal sumaMontoNetoIVAMinima = 0;
            decimal sumaMontoNetoIVABasica = 0;
            
            try
            {
                Item_Det_Fact[] detalle = new Item_Det_Fact[doc.detalle.Count];
                int cont = 0;



                foreach (Detalle det in doc.detalle)
                {
                    Item_Det_Fact item = new Item_Det_Fact();
                    int nro = cont + 1;

                    item.NroLinDet = nro.ToString();
                    item.NomItem = det.producto.Nombre;
                    item.IndAgenteRespSpecified = false;
                    item.Cantidad = det.Kilos;
                    item.UniMed = det.producto.unidadMedida;
                    decimal precio = det.PrecioUnitario;
                    item.PrecioUnitario = precio;

                    decimal impuesto = 0;

                    if (det.producto.indicador.Codigo == 2)
                    {
                        impuesto = Sistema.GetInstancia().TasaMinima();

                    }
                    else if (det.producto.indicador.Codigo == 3)
                    {
                        impuesto = Sistema.GetInstancia().TasaBasica();

                    }
                    decimal mntIVA = Convert.ToDecimal(("1." + impuesto),System.Globalization.CultureInfo.InvariantCulture);
                    decimal mntT = (det.PrecioUnitario * det.Kilos) * mntIVA;
                    if (impuesto > 0)
                    {
                        item.PrecioUnitario = item.PrecioUnitario * mntIVA;
                    }
                    if (det.Descuento != null)
                    {
                        item.DescuentoPct = decimal.Round((decimal)det.Descuento, 2);
                        item.DescuentoPctSpecified = true;

                        item.DescuentoMonto = (mntT * (decimal)det.Descuento) / 100;
                        item.DescuentoMontoSpecified = true;
                        item.DescuentoMonto = decimal.Round(item.DescuentoMonto, 2);
                    }
                    else
                    {
                        item.DescuentoPct = 0;
                        item.DescuentoPctSpecified = false;
                        item.DescuentoMontoSpecified = false;
                    }

                    if (det.Recargo != null && det.Descuento == null)
                    {
                        item.RecargoPct = decimal.Round((decimal)det.Recargo, 2);
                        item.RecargoPctSpecified = true;
                        item.RecargoMnt = (mntT * (decimal)det.Recargo) / 100;
                        item.RecargoMntSpecified = true;
                        item.RecargoMnt = decimal.Round(item.RecargoMnt, 2);
                    }
                    else if (det.Recargo == null)
                    {
                        item.RecargoPct = 0;
                        item.RecargoPctSpecified = false;
                        item.RecargoMntSpecified = false;
                    }
                    else
                    {
                        item.RecargoPct = decimal.Round((decimal)det.Recargo, 2);
                        item.RecargoPctSpecified = true;
                        item.RecargoMnt = ((mntT - item.DescuentoMonto) * (decimal)det.Recargo) / 100;
                        item.RecargoMntSpecified = true;
                        item.RecargoMnt = decimal.Round(item.RecargoMnt, 2);
                    }

                    item.DscItem = det.producto.Descripcion;

                    item.PrecioUnitario = decimal.Round(item.PrecioUnitario, 2);
                    item.MontoItem = det.MontoTotal;
                    item.MontoItem = decimal.Round(item.MontoItem, 2);


                    item.IndFact = ObtenerIndicadorFacturacion(det.producto.indicador.Codigo);
                    if (det.producto.indicador.Codigo == 1 || det.producto.indicador.Codigo == 5)
                    {
                        sumaMontoNoGravados += det.MontoTotal;
                    }
                    else if (det.producto.indicador.Codigo == 2)
                    {
                        sumaMontoNetoIVAMinima += (det.MontoTotal / mntIVA);

                    }
                    else if (det.producto.indicador.Codigo == 3)
                    {
                        sumaMontoNetoIVABasica += (det.MontoTotal / mntIVA);

                    }
                    else if (det.producto.indicador.Codigo == 11)
                    {
                        sumaMontoImpuestoPercibido += det.MontoTotal;
                    }
                    else if (det.producto.indicador.Codigo == 12)
                    {
                        sumaMontoIvaEnSuspenso += det.MontoTotal;
                    }
                    else if (det.producto.indicador.Codigo == 6)
                    {
                        sumaMontoNoFacturable += det.MontoTotal;
                    }

                    detalle[cont] = item;
                    cont++;
                }

                factura.Detalle = detalle;

            }
            catch (Exception e)
            {

            }

            // Totales

            try
            {

                Totales totales = new Totales();


                if (doc.Moneda.Equals("Dolar"))
                {
                    totales.TpoMoneda = TipMonType.USD;

                }
                else
                {
                    totales.TpoMoneda = TipMonType.UYU;
                }

                totales.CantLinDet = doc.detalle.Count.ToString();
                if (doc.Moneda.Equals("Dolar"))
                {
                    totales.TpoCambio = (decimal)doc.tipoCambio;
                    totales.TpoCambio = decimal.Round(totales.TpoCambio, 2);
                    totales.TpoCambioSpecified = true;
                }
                else
                {
                    totales.TpoCambioSpecified = false;
                }

                totales.IVATasaBasica = Sistema.GetInstancia().TasaBasica();
                totales.IVATasaBasicaSpecified = true;
                totales.IVATasaMin = Sistema.GetInstancia().TasaMinima();
                totales.IVATasaMinSpecified = true;

                if (sumaMontoImpuestoPercibido > 0)
                {
                    totales.MntImpuestoPerc = sumaMontoImpuestoPercibido;
                    totales.MntImpuestoPerc = decimal.Round(totales.MntImpuestoPerc, 2);
                    totales.MntImpuestoPercSpecified = true;
                }
                else
                {
                    totales.MntImpuestoPercSpecified = false;
                }
                if (sumaMontoIvaEnSuspenso > 0)
                {
                    totales.MntIVaenSusp = sumaMontoIvaEnSuspenso;
                    totales.MntIVaenSusp = decimal.Round(totales.MntIVaenSusp, 2);
                    totales.MntIVaenSuspSpecified = true;
                }
                else
                {
                    totales.MntIVaenSuspSpecified = false;
                }
                if (sumaMontoNetoIVABasica > 0)
                {
                    totales.MntNetoIVATasaBasica = sumaMontoNetoIVABasica;
                    totales.MntNetoIVATasaBasica = decimal.Round(totales.MntNetoIVATasaBasica, 2);
                    totales.MntNetoIVATasaBasicaSpecified = true;
                }
                else
                {
                    totales.MntNetoIVATasaBasicaSpecified = false;
                }

                if (sumaMontoNetoIVAMinima > 0)
                {
                    totales.MntNetoIvaTasaMin = sumaMontoNetoIVAMinima;
                    totales.MntNetoIvaTasaMin = decimal.Round(totales.MntNetoIvaTasaMin, 2);
                    totales.MntNetoIvaTasaMinSpecified = true;
                }
                else
                {
                    totales.MntNetoIvaTasaMinSpecified = false;
                }

                if (sumaMontoNoGravados > 0)
                {
                    totales.MntNoGrv = sumaMontoNoGravados;
                    totales.MntNoGrv = decimal.Round(totales.MntNoGrv, 2);
                    totales.MntNoGrvSpecified = true;
                }
                else
                {
                    totales.MntNoGrvSpecified = false;
                }
                if (sumaMontoNetoIVABasica > 0)
                {
                    totales.MntIVATasaBasica = (sumaMontoNetoIVABasica * Sistema.GetInstancia().TasaBasica()) / 100;
                    totales.MntIVATasaBasica = decimal.Round(totales.MntIVATasaBasica, 2);
                    totales.MntIVATasaBasicaSpecified = true;
                }
                else
                {
                    totales.MntIVATasaBasicaSpecified = false;
                }
                if (sumaMontoNetoIVAMinima > 0)
                {
                    totales.MntIVATasaMin = (sumaMontoNetoIVAMinima * Sistema.GetInstancia().TasaMinima()) / 100;
                    totales.MntIVATasaMin = decimal.Round(totales.MntIVATasaMin, 2);
                    totales.MntIVATasaMinSpecified = true;
                }
                else
                {
                    totales.MntIVATasaMinSpecified = false;
                }
                if (sumaMontoNoFacturable > 0)
                {
                    totales.MontoNF = sumaMontoNoFacturable;
                    totales.MontoNF = decimal.Round(totales.MontoNF, 2);
                    totales.MontoNFSpecified = true;
                }
                else
                {
                    totales.MontoNFSpecified = false;
                }

                totales.MntTotal = totales.MntNetoIVAOtra + totales.MntNetoIVATasaBasica + totales.MntNetoIvaTasaMin + totales.MntIVAOtra + totales.MntIVATasaBasica + totales.MntIVATasaMin + totales.MntNoGrv + totales.MntIVaenSusp + totales.MntImpuestoPerc;
                totales.MntPagar = totales.MntTotal + totales.MontoNF;
                decimal redondeo = totales.MntPagar - decimal.Round(totales.MntPagar);
                totales.MntTotal = Math.Round((totales.MntTotal - redondeo), 2);
                totales.MntPagar = Math.Round((totales.MntPagar - redondeo), 2);
                Sistema.GetInstancia().Total = totales.MntPagar;
                if (totales.MntTotal == 0 && totales.MontoNF > 0)
                {
                    totales.MontoNF = Math.Round((totales.MontoNF - redondeo), 2);
                }
                if (totales.MntNoGrv > 0)
                {
                    totales.MntNoGrv = Math.Round((totales.MntNoGrv - redondeo), 2);
                }
                if (totales.MntNetoIVATasaBasica > 0 && totales.MntNetoIvaTasaMin == 0)
                {
                    totales.MntNetoIVATasaBasica = Math.Round((totales.MntNetoIVATasaBasica - redondeo), 2);
                    totales.MntIVATasaBasica = (totales.MntNetoIVATasaBasica * Sistema.GetInstancia().TasaBasica()) / 100;
                    totales.MntIVATasaBasica = decimal.Round(totales.MntIVATasaBasica, 2);
                }
                else
                {
                    totales.MntNetoIvaTasaMin = Math.Round((totales.MntNetoIvaTasaMin - redondeo), 2);
                    totales.MntIVATasaMin = (totales.MntNetoIvaTasaMin * Sistema.GetInstancia().TasaMinima()) / 100;
                    totales.MntIVATasaMin = decimal.Round(totales.MntIVATasaMin, 2);
                }
                encabezado.Totales = totales;

            }
            catch (Exception ex)
            {

            }

            factura.Encabezado = encabezado;

            //Referencia
            List<Documento> documentos = DocumentosAsociados();
            
            try
            {
                int num = 1;

                if (documentos == null || documentos.Count == 0)
                {
                    ReferenciaReferencia[] referencia = new ReferenciaReferencia[1];
                    ReferenciaReferencia refe = new ReferenciaReferencia();
                    refe.NroLinRef = "1";
                    refe.IndGlobal = ReferenciaReferenciaIndGlobal.Item1;
                    refe.IndGlobalSpecified = true;
                    refe.TpoDocRefSpecified = false;
                    refe.RazonRef = "Referencia a documento no codificado";
                    refe.FechaCFErefSpecified = false;
                    referencia[0] = refe;
                    factura.Referencia = referencia;
                    
                }
                else if (documentos.Count > 40)
                {
                    ReferenciaReferencia[] referencia = new ReferenciaReferencia[1];
                    ReferenciaReferencia refe = new ReferenciaReferencia();
                    refe.NroLinRef = "1";
                    refe.IndGlobal = ReferenciaReferenciaIndGlobal.Item1;
                    refe.IndGlobalSpecified = true;
                    refe.TpoDocRefSpecified = false;
                    refe.RazonRef = "Afecta a mas de 40 CFE";
                    refe.FechaCFErefSpecified = false;
                    referencia[0] = refe;
                    factura.Referencia = referencia;
                    
                }
                else
                {
                    ReferenciaReferencia[] referencia = new ReferenciaReferencia[documentos.Count];
                    foreach (Documento docu in documentos)
                    {
                        ReferenciaReferencia refe = new ReferenciaReferencia();
                        refe.NroLinRef = num.ToString();
                        String serie = docu.Serie;
                        int nro = docu.NroSerie;
                        int tipo = Int32.Parse(docu.TipoDocumento);
                        if (String.IsNullOrEmpty(serie) || nro == null || nro == 0)
                        {
                            refe.IndGlobal = ReferenciaReferenciaIndGlobal.Item1;
                            refe.IndGlobalSpecified = true;
                            refe.TpoDocRefSpecified = false;
                            refe.RazonRef = "Referencia a documento no codificado";
                            refe.FechaCFErefSpecified = false;

                        }
                        else
                        {
                            refe.IndGlobalSpecified = false;
                            refe.TpoDocRefSpecified = true;
                            refe.TpoDocRef = (CFEType)Enum.Parse(typeof(CFEType), "Item"+docu.TipoDocumento, true);
                         
                            refe.Serie = serie;
                            refe.NroCFERef = nro.ToString();
                            refe.FechaCFErefSpecified = false;
                        }

                        referencia[num - 1] = refe;
                        num++;
                        
                    }

                    factura.Referencia = referencia;
                }
            }
            catch (Exception ex) { }

            XmlSerializer ser = new XmlSerializer(typeof(CFEDefTypeEFact));
            using (var stream = new MemoryStream())
            {
                ser.Serialize(stream, factura);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                xml.Load(stream);
                result = xml.InnerXml;
            }

            return xml;
        }

        private XmlDocument GenerarXmlTicket(Documento doc)
        {
            XmlDocument xml = new XmlDocument();
            String result = "";

            CFEDefTypeETck factura = new CFEDefTypeETck();

            ////CAE

            //CAEDataType cae = new CAEDataType();
            //cae.CAE_ID = "";
            //cae.DNro = "";
            //cae.FecVenc = new DateTime();
            //cae.HNro = "";

            //factura.CAEData = cae;

            //Encabezado

            CFEDefTypeETckEncabezado encabezado = new CFEDefTypeETckEncabezado();

            // IdDoc
            try
            {
                IdDoc_Tck idDoc = new IdDoc_Tck();
                //idDoc.Nro = "";
                //idDoc.Serie = "";
                idDoc.FchEmis = doc.Fecha;
                idDoc.FchVencSpecified = false;
                if (doc.FormaPago.Equals("Credito"))
                {
                    idDoc.FmaPago = IdDoc_TckFmaPago.Item2;

                }
                else
                {
                    idDoc.FmaPago = IdDoc_TckFmaPago.Item1;
                }
                idDoc.TipoCFE = IdDoc_TckTipoCFE.Item102;
                idDoc.MntBruto = IdDoc_TckMntBruto.Item1;
                idDoc.MntBrutoSpecified = true;
                encabezado.IdDoc = idDoc;
            }
            catch (Exception ex)
            {

            }

            //Emisor

            try
            {
                DatosEmisor datos = Sistema.GetInstancia().ObtenerDatosEmisor(Session["rut"].ToString());
                Emisor emisor = new Emisor();

                //emisor.RUCEmisor = "120305090012";
                emisor.RznSoc = datos.razonSocial;
                emisor.RUCEmisor = datos.ruc;
                emisor.NomComercial = datos.nomComercial;

                emisor.Telefono = datos.Telefonos;

                emisor.CorreoEmisor = datos.Correo;
                emisor.EmiSucursal = datos.Sucursal;
                emisor.CdgDGISucur = datos.CodigoSucursal;
                emisor.DomFiscal = datos.DomicilioFiscal;
                emisor.Ciudad = datos.Ciudad;
                emisor.Departamento = datos.Departamento;
                emisor.GiroEmis = datos.Giro;

                encabezado.Emisor = emisor;

            }
            catch (Exception ex)
            {

            }

            //Receptor
            try
            {

                Receptor_Tck receptor = new Receptor_Tck();
                Cliente cli = Sistema.GetInstancia().BuscarClienteId(doc.IdCliente);
                if (!String.IsNullOrEmpty(cli.Ciudad))
                {
                    receptor.CiudadRecep = cli.Ciudad;
                }
                if (!String.IsNullOrEmpty(cli.CodigoPostal))
                {
                    receptor.CP = cli.CodigoPostal;
                }
                if (!String.IsNullOrEmpty(cli.Direccion))
                {
                    receptor.DirRecep = cli.Direccion;
                }
                receptor.Item = cli.nroDoc.ToString();
                if (cli.Pais.Codigo.Equals("UY"))
                {
                    receptor.ItemElementName = ItemChoiceType.DocRecep;
                    receptor.CodPaisRecep = CodPaisType.UY;
                }
                else
                {
                    receptor.ItemElementName = ItemChoiceType.DocRecepExt;
                    receptor.CodPaisRecep = (CodPaisType)Enum.Parse(typeof(CodPaisType), cli.Pais.Codigo, true);
                }

                receptor.RznSocRecep = cli.Nombre;
                if (cli.tipoDocumento.Equals("RUT"))
                {
                    receptor.TipoDocRecep = DocType.Item2;
                }
                else if (cli.tipoDocumento.Equals("CI"))
                {
                    receptor.TipoDocRecep = DocType.Item3;
                }
                else if (cli.tipoDocumento.Equals("OTROS"))
                {
                    receptor.TipoDocRecep = DocType.Item4;
                }
                else if (cli.tipoDocumento.Equals("PASAPORTE"))
                {
                    receptor.TipoDocRecep = DocType.Item5;
                }
                else if (cli.tipoDocumento.Equals("DNI"))
                {
                    receptor.TipoDocRecep = DocType.Item6;
                }
                if (receptor.CodPaisRecep == CodPaisType.UY)
                {
                    receptor.ItemElementName = ItemChoiceType.DocRecep;
                }
                else
                {
                    receptor.ItemElementName = ItemChoiceType.DocRecepExt;
                }
                
                receptor.Item = cli.nroDoc.ToString();
                receptor.CodPaisRecepSpecified = true;
                receptor.TipoDocRecepSpecified = true;
                encabezado.Receptor = receptor;

            }
            catch (Exception e)
            {

            }

            //Detalle
            decimal sumaMontoNoGravados = 0;
            decimal sumaMontoNoFacturable = 0;
            decimal sumaMontoImpuestoPercibido = 0;
            decimal sumaMontoIvaEnSuspenso = 0;
            decimal sumaMontoNetoIVAMinima = 0;
            decimal sumaMontoNetoIVABasica = 0;
            
            try
            {
                Item_Det_Fact[] detalle = new Item_Det_Fact[doc.detalle.Count];
                int cont = 0;



                foreach (Detalle det in doc.detalle)
                {
                    Item_Det_Fact item = new Item_Det_Fact();
                    int nro = cont + 1;

                    item.NroLinDet = nro.ToString();
                    item.NomItem = det.producto.Nombre;
                    item.IndAgenteRespSpecified = false;
                    item.Cantidad = det.Kilos;
                    item.UniMed = det.producto.unidadMedida;
                    decimal precio = det.PrecioUnitario;
                    item.PrecioUnitario = precio;
                    decimal impuesto = 0;

                    if (det.producto.indicador.Codigo == 2)
                    {
                        impuesto = Sistema.GetInstancia().TasaMinima();

                    }
                    else if (det.producto.indicador.Codigo == 3)
                    {
                        impuesto = Sistema.GetInstancia().TasaBasica();

                    }
                    decimal mntIVA = Convert.ToDecimal(("1." + impuesto),System.Globalization.CultureInfo.InvariantCulture);
                    decimal mntT = (det.PrecioUnitario * det.Kilos) * mntIVA;
                    if (impuesto > 0)
                    {
                        item.PrecioUnitario = item.PrecioUnitario * mntIVA;
                    }
                    if (det.Descuento != null)
                    {
                        item.DescuentoPct = decimal.Round((decimal)det.Descuento, 2);
                        item.DescuentoPctSpecified = true;

                        item.DescuentoMonto = (mntT * (decimal)det.Descuento) / 100;
                        item.DescuentoMontoSpecified = true;
                        item.DescuentoMonto = decimal.Round(item.DescuentoMonto, 2);
                    }
                    else
                    {
                        item.DescuentoPct = 0;
                        item.DescuentoPctSpecified = false;
                        item.DescuentoMontoSpecified = false;
                    }

                    if (det.Recargo != null && det.Descuento == null)
                    {
                        item.RecargoPct = decimal.Round((decimal)det.Recargo, 2);
                        item.RecargoPctSpecified = true;
                        item.RecargoMnt = (mntT * (decimal)det.Recargo) / 100;
                        item.RecargoMntSpecified = true;
                        item.RecargoMnt = decimal.Round(item.RecargoMnt, 2);
                    }
                    else if (det.Recargo == null)
                    {
                        item.RecargoPct = 0;
                        item.RecargoPctSpecified = false;
                        item.RecargoMntSpecified = false;
                    }
                    else
                    {
                        item.RecargoPct = decimal.Round((decimal)det.Recargo, 2);
                        item.RecargoPctSpecified = true;
                        item.RecargoMnt = ((mntT - item.DescuentoMonto) * (decimal)det.Recargo) / 100;
                        item.RecargoMntSpecified = true;
                        item.RecargoMnt = decimal.Round(item.RecargoMnt, 2);
                    }

                    item.DscItem = det.producto.Descripcion;

                    item.PrecioUnitario = decimal.Round(item.PrecioUnitario, 2);
                    item.MontoItem = det.MontoTotal;
                    item.MontoItem = decimal.Round(item.MontoItem, 2);


                    item.IndFact = ObtenerIndicadorFacturacion(det.producto.indicador.Codigo);
                    if (det.producto.indicador.Codigo == 1 || det.producto.indicador.Codigo == 5)
                    {
                        sumaMontoNoGravados += det.MontoTotal;
                    }
                    else if (det.producto.indicador.Codigo == 2)
                    {
                        sumaMontoNetoIVAMinima += (det.MontoTotal / mntIVA);

                    }
                    else if (det.producto.indicador.Codigo == 3)
                    {
                        sumaMontoNetoIVABasica += (det.MontoTotal / mntIVA);

                    }
                    else if (det.producto.indicador.Codigo == 11)
                    {
                        sumaMontoImpuestoPercibido += det.MontoTotal;
                    }
                    else if (det.producto.indicador.Codigo == 12)
                    {
                        sumaMontoIvaEnSuspenso += det.MontoTotal;
                    }
                    else if (det.producto.indicador.Codigo == 6)
                    {
                        sumaMontoNoFacturable += det.MontoTotal;
                    }

                    detalle[cont] = item;
                    cont++;
                }

                factura.Detalle = detalle;

            }
            catch (Exception e)
            {

            }

            // Totales

            try
            {

                Totales totales = new Totales();


                if (doc.Moneda.Equals("Dolar"))
                {
                    totales.TpoMoneda = TipMonType.USD;

                }
                else
                {
                    totales.TpoMoneda = TipMonType.UYU;
                }

                totales.CantLinDet = doc.detalle.Count.ToString();
                if (doc.Moneda.Equals("Dolar"))
                {
                    totales.TpoCambio = (decimal)doc.tipoCambio;
                    totales.TpoCambio = decimal.Round(totales.TpoCambio, 2);
                    totales.TpoCambioSpecified = true;
                }
                else
                {
                    totales.TpoCambioSpecified = false;
                }

                totales.IVATasaBasica = Sistema.GetInstancia().TasaBasica();
                totales.IVATasaBasicaSpecified = true;
                totales.IVATasaMin = Sistema.GetInstancia().TasaMinima();
                totales.IVATasaMinSpecified = true;

                if (sumaMontoImpuestoPercibido > 0)
                {
                    totales.MntImpuestoPerc = sumaMontoImpuestoPercibido;
                    totales.MntImpuestoPerc = decimal.Round(totales.MntImpuestoPerc, 2);
                    totales.MntImpuestoPercSpecified = true;
                }
                else
                {
                    totales.MntImpuestoPercSpecified = false;
                }
                if (sumaMontoIvaEnSuspenso > 0)
                {
                    totales.MntIVaenSusp = sumaMontoIvaEnSuspenso;
                    totales.MntIVaenSusp = decimal.Round(totales.MntIVaenSusp, 2);
                    totales.MntIVaenSuspSpecified = true;
                }
                else
                {
                    totales.MntIVaenSuspSpecified = false;
                }
                if (sumaMontoNetoIVABasica > 0)
                {
                    totales.MntNetoIVATasaBasica = sumaMontoNetoIVABasica;
                    totales.MntNetoIVATasaBasica = decimal.Round(totales.MntNetoIVATasaBasica, 2);
                    totales.MntNetoIVATasaBasicaSpecified = true;
                }
                else
                {
                    totales.MntNetoIVATasaBasicaSpecified = false;
                }

                if (sumaMontoNetoIVAMinima > 0)
                {
                    totales.MntNetoIvaTasaMin = sumaMontoNetoIVAMinima;
                    totales.MntNetoIvaTasaMin = decimal.Round(totales.MntNetoIvaTasaMin, 2);
                    totales.MntNetoIvaTasaMinSpecified = true;
                }
                else
                {
                    totales.MntNetoIvaTasaMinSpecified = false;
                }

                if (sumaMontoNoGravados > 0)
                {
                    totales.MntNoGrv = sumaMontoNoGravados;
                    totales.MntNoGrv = decimal.Round(totales.MntNoGrv, 2);
                    totales.MntNoGrvSpecified = true;
                }
                else
                {
                    totales.MntNoGrvSpecified = false;
                }
                if (sumaMontoNetoIVABasica > 0)
                {
                    totales.MntIVATasaBasica = (sumaMontoNetoIVABasica * Sistema.GetInstancia().TasaBasica()) / 100;
                    totales.MntIVATasaBasica = decimal.Round(totales.MntIVATasaBasica, 2);
                    totales.MntIVATasaBasicaSpecified = true;
                }
                else
                {
                    totales.MntIVATasaBasicaSpecified = false;
                }
                if (sumaMontoNetoIVAMinima > 0)
                {
                    totales.MntIVATasaMin = (sumaMontoNetoIVAMinima * Sistema.GetInstancia().TasaMinima()) / 100;
                    totales.MntIVATasaMin = decimal.Round(totales.MntIVATasaMin, 2);
                    totales.MntIVATasaMinSpecified = true;
                }
                else
                {
                    totales.MntIVATasaMinSpecified = false;
                }
                if (sumaMontoNoFacturable > 0)
                {
                    totales.MontoNF = sumaMontoNoFacturable;
                    totales.MontoNF = decimal.Round(totales.MontoNF, 2);
                    totales.MontoNFSpecified = true;
                }
                else
                {
                    totales.MontoNFSpecified = false;
                }

                totales.MntTotal = totales.MntNetoIVAOtra + totales.MntNetoIVATasaBasica + totales.MntNetoIvaTasaMin + totales.MntIVAOtra + totales.MntIVATasaBasica + totales.MntIVATasaMin + totales.MntNoGrv + totales.MntIVaenSusp + totales.MntImpuestoPerc;
                totales.MntPagar = totales.MntTotal + totales.MontoNF;
                decimal redondeo = totales.MntPagar - decimal.Round(totales.MntPagar);
                totales.MntTotal = Math.Round((totales.MntTotal - redondeo), 2);
                totales.MntPagar = Math.Round((totales.MntPagar - redondeo), 2);
                Sistema.GetInstancia().Total = totales.MntPagar;
                if (totales.MntTotal == 0 && totales.MontoNF > 0)
                {
                    totales.MontoNF = Math.Round((totales.MontoNF - redondeo), 2);
                }
                if (totales.MntNoGrv > 0)
                {
                    totales.MntNoGrv = Math.Round((totales.MntNoGrv - redondeo), 2);
                }
                if (totales.MntNetoIVATasaBasica > 0 && totales.MntNetoIvaTasaMin == 0)
                {
                    totales.MntNetoIVATasaBasica = Math.Round((totales.MntNetoIVATasaBasica - redondeo), 2);
                    totales.MntIVATasaBasica = (totales.MntNetoIVATasaBasica * Sistema.GetInstancia().TasaBasica()) / 100;
                    totales.MntIVATasaBasica = decimal.Round(totales.MntIVATasaBasica, 2);
                }
                else
                {
                    totales.MntNetoIvaTasaMin = Math.Round((totales.MntNetoIvaTasaMin - redondeo), 2);
                    totales.MntIVATasaMin = (totales.MntNetoIvaTasaMin * Sistema.GetInstancia().TasaMinima()) / 100;
                    totales.MntIVATasaMin = decimal.Round(totales.MntIVATasaMin, 2);
                }
                encabezado.Totales = totales;

            }
            catch (Exception ex)
            {

            }

            factura.Encabezado = encabezado;

            //Referencia
            List<Documento> documentos = DocumentosAsociados();
            try
            {
                int num = 1;

                if (documentos == null || documentos.Count == 0)
                {
                    ReferenciaReferencia[] referencia = new ReferenciaReferencia[1];
                    ReferenciaReferencia refe = new ReferenciaReferencia();
                    refe.NroLinRef = "1";
                    refe.IndGlobal = ReferenciaReferenciaIndGlobal.Item1;
                    refe.IndGlobalSpecified = true;
                    refe.TpoDocRefSpecified = false;
                    refe.RazonRef = "Referencia a documento no codificado";
                    refe.FechaCFErefSpecified = false;
                    referencia[0] = refe;
                    factura.Referencia = referencia;

                }
                else if (documentos.Count > 40)
                {
                    ReferenciaReferencia[] referencia = new ReferenciaReferencia[1];
                    ReferenciaReferencia refe = new ReferenciaReferencia();
                    refe.NroLinRef = "1";
                    refe.IndGlobal = ReferenciaReferenciaIndGlobal.Item1;
                    refe.IndGlobalSpecified = true;
                    refe.TpoDocRefSpecified = false;
                    refe.RazonRef = "Afecta a mas de 40 CFE";
                    refe.FechaCFErefSpecified = false;
                    referencia[0] = refe;
                    factura.Referencia = referencia;

                }
                else
                {
                    ReferenciaReferencia[] referencia = new ReferenciaReferencia[documentos.Count];
                    foreach (Documento docu in documentos)
                    {
                        ReferenciaReferencia refe = new ReferenciaReferencia();
                        refe.NroLinRef = num.ToString();
                        String serie = docu.Serie;
                        int nro = docu.NroSerie;
                        int tipo = Int32.Parse(docu.TipoDocumento);
                        if (String.IsNullOrEmpty(serie) || nro == null || nro == 0)
                        {
                            refe.IndGlobal = ReferenciaReferenciaIndGlobal.Item1;
                            refe.IndGlobalSpecified = true;
                            refe.TpoDocRefSpecified = false;
                            refe.RazonRef = "Referencia a documento no codificado";
                            refe.FechaCFErefSpecified = false;

                        }
                        else
                        {
                            refe.IndGlobalSpecified = false;
                            refe.TpoDocRefSpecified = true;
                            refe.TpoDocRef = (CFEType)Enum.Parse(typeof(CFEType), "Item" + docu.TipoDocumento, true);

                            refe.Serie = serie;
                            refe.NroCFERef = nro.ToString();
                            refe.FechaCFErefSpecified = false;
                        }

                        referencia[num - 1] = refe;
                        num++;

                    }

                    factura.Referencia = referencia;
                }
            }
            catch (Exception ex) { }

            XmlSerializer ser = new XmlSerializer(typeof(CFEDefTypeETck));
            using (var stream = new MemoryStream())
            {
                ser.Serialize(stream, factura);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                xml.Load(stream);
                result = xml.InnerXml;
            }

            return xml;
        }

        public Item_Det_FactIndFact ObtenerIndicadorFacturacion(int cod)
        {
            if (cod == 1)
            {
                return Item_Det_FactIndFact.Item1;
            }
            else if (cod == 2)
            {
                return Item_Det_FactIndFact.Item2;
            }
            else if (cod == 3)
            {
                return Item_Det_FactIndFact.Item3;
            }
            else if (cod == 4)
            {
                return Item_Det_FactIndFact.Item4;
            }
            else if (cod == 5)
            {
                return Item_Det_FactIndFact.Item5;
            }
            else if (cod == 6)
            {
                return Item_Det_FactIndFact.Item6;
            }
            else if (cod == 11)
            {
                return Item_Det_FactIndFact.Item11;
            }
            else if (cod == 12)
            {
                return Item_Det_FactIndFact.Item12;
            }
            else
            {
                return Item_Det_FactIndFact.Item3;
            }
        }

        public String AjustarCFE(String xmlFactura, String tipo)
        {
            String[] xmlCortado = xmlFactura.Split(new char[] { '<', '>' });
            String xml = "";
            for (int i = 1; i < xmlCortado.Length; i = i + 2)
            {
                if (i == 3)
                {
                    String aux = "http://www.w3.org/2001/XMLSchema";
                    String aux2 = "http://www.w3.org/2001/XMLSchema-instance";

                    xml += "<CFE xmlns=\"http://cfe.dgi.gub.uy\" xmlns:xsd=\"" + aux + "\"" + " xmlns:xsi=\"" + aux2 + "\"" + " version=\"1.0\">";
                }
                else if (i == 5)
                {

                    xml += "<" + tipo + ">";

                }
                else if (i > 8)
                {
                    if (xmlCortado[i].Contains("CFEDefTypeEFact") || xmlCortado[i].Contains("CFEDefTypeETck"))
                    {
                        xml += "</" + tipo + ">";
                        xml += "</CFE>";
                    }
                    else
                    {
                        if (xmlCortado.Length > (i + 1))
                        {
                            xml += ("<" + xmlCortado[i] + ">" + xmlCortado[i + 1]);
                        }
                        else
                        {
                            xml += ("<" + xmlCortado[i] + ">");
                        }
                    }
                }


            }


            return xml;
        }

        protected void ddlAsociar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAsociar.SelectedValue != null)
            {
                if (ddlAsociar.SelectedValue.Equals("Si"))
                {
                    AsociarDocumentos.Visible = true;
                    String tipoDocumento = "";
                    if (!String.IsNullOrEmpty(ddlCliente.SelectedValue))
                    {
                        if (!String.IsNullOrEmpty(ddlTipoDocumento.SelectedValue) && !ddlTipoDocumento.SelectedValue.Equals("Todos"))
                        {
                            tipoDocumento = ddlTipoDocumento.SelectedValue;
                        }
                        int idcliente = Int32.Parse(ddlCliente.SelectedValue);
                        List<Documento> documentos = Sistema.GetInstancia().ObtenerDocumentosCliente(idcliente, tipoDocumento, Session["rut"].ToString()).ToList();
                        if (documentos != null)
                        {

                            gridViewDocumentos.DataSource = documentos;
                            gridViewDocumentos.DataBind();
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(ddlTipoDocumento.SelectedValue) && !ddlTipoDocumento.SelectedValue.Equals("Todos"))
                        {
                            string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un cliente" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                }
                else
                {
                    AsociarDocumentos.Visible = false;
                }
            }

        }

        protected void ddlSeleccionarCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            String tipoDocumento = "";
            if (!String.IsNullOrEmpty(ddlAsociar.SelectedValue) && ddlAsociar.SelectedValue.Equals("Si"))
            {
                if (!String.IsNullOrEmpty(ddlCliente.SelectedValue))
                {
                    if (!String.IsNullOrEmpty(ddlTipoDocumento.SelectedValue) && !ddlTipoDocumento.SelectedValue.Equals("Todos"))
                    {
                        tipoDocumento = ddlTipoDocumento.SelectedValue;
                    }
                    int idcliente = Int32.Parse(ddlCliente.SelectedValue);
                    List<Documento> documentos = Sistema.GetInstancia().ObtenerDocumentosCliente(idcliente, tipoDocumento, Session["rut"].ToString()).ToList();
                    if (documentos != null)
                    {

                        gridViewDocumentos.DataSource = documentos;
                        gridViewDocumentos.DataBind();
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(ddlTipoDocumento.SelectedValue) && !ddlTipoDocumento.SelectedValue.Equals("Todos"))
                    {
                        string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un cliente" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                }
            }
        }

        private List<Documento> DocumentosAsociados() {
            List<Documento> documentos = new List<Documento>();
            gridViewDocumentos.AllowPaging = false;
            BindData2();
            foreach (GridViewRow gvr in gridViewDocumentos.Rows)
            {
                try
                {
                    Label idDoc = gvr.FindControl("lblIdDocumento") as Label;
                    //Label fecha = gvr.FindControl("lblFecha") as Label;
                    //Label tipoDoc = gvr.FindControl("lblTipoDocumento") as Label;
                    //Label serie = gvr.FindControl("lblSerie") as Label;
                    //Label nroSerie = gvr.FindControl("lblNroSerie") as Label;
                    CheckBox asociar = gvr.FindControl("ckActivo") as CheckBox;

                    if (asociar.Checked)
                    {
                        Documento doc = Sistema.GetInstancia().ObtenerDocumentoId(Int32.Parse(idDoc.Text));
                        
                        //if (fecha != null)
                        //{
                        //    doc.Fecha = DateTime.Parse(fecha.Text);

                        //}
                        //if (tipoDoc != null)
                        //{
                        //    doc.TipoDocumento = tipoDoc.Text;
                        //}
                        //if (serie != null)
                        //{
                        //    doc.Serie = serie.Text;
                        //}
                        //if (nroSerie != null)
                        //{
                        //    doc.NroSerie = Int32.Parse(nroSerie.Text);
                        //}
                        documentos.Add(doc);
                    }
                }
                catch { }
                gridViewDocumentos.AllowPaging = true;
                BindData2();
            }

            return documentos;
        }
       
    }
}