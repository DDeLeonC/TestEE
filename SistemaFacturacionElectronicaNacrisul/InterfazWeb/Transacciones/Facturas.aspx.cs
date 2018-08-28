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
    public partial class Facturas : System.Web.UI.Page
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
                DataTable dt = ObtenerDataTable();
                DataRow dr = dt.NewRow();
                dr[0] = "";
                dr[1] = "";
                dr[2] = "";
                dr[3] = "";
                dr[4] = "";
                dr[5] = "";
                dr[6] = "";
                dr[7] = "";
                dr[8] = "";
                dr[9] = "";
                dt.Rows.Add(dr);
                gridViewFacturas.DataSource = dt;
                gridViewFacturas.DataBind();


                Sistema.GetInstancia().SubTotal = 0;
                Sistema.GetInstancia().Total = 0;
                Sistema.GetInstancia().Impuestos = 0;
                txtImpuestos.Enabled = false;
                txtSubTotal.Enabled = false;
                txtTotal.Enabled = false;
                //Sistema.GetInstancia().PDFActual = null;

                txtFechaVencimiento.Text = DateTime.Now.ToShortDateString();
                txtFecha.Text = DateTime.Now.ToShortDateString();
                txtImpuestos.Text = Sistema.GetInstancia().Impuestos.ToString();
                txtSubTotal.Text = Sistema.GetInstancia().SubTotal.ToString();
                txtTotal.Text = Sistema.GetInstancia().Total.ToString();
                txtFecha.Text = DateTime.Now.Date.ToShortDateString();

                TipoCambio.Visible = false;
                List<String> lista = new List<String>();
                lista.Add("Pesos");
                lista.Add("Dolar");
                
                ddlMoneda.DataSource = lista;
                ddlMoneda.DataBind();

                List<String> lista2 = new List<String>();
                lista2.Add("Credito");
                lista2.Add("Contado");
                

                ddlFormaPago.DataSource = lista2;
                ddlFormaPago.DataBind();

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

        protected void gridViewFacturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //e.Row.Cells[3].Controls[1].Focus();
                    Control control = e.Row.FindControl("txtCantidad");
                    if (control != null)
                    {
                        control.Focus();
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label idProd = e.Row.FindControl("lblIdProducto") as Label;
                    if (idProd != null)
                    {
                        if (String.IsNullOrEmpty(idProd.Text))
                        {
                            e.Row.FindControl("lnkRemove").Visible = false;
                        }
                    }
                }
            }catch
            {   
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
                decimal kilos = 0;

                bool error = false;
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
                if (!error) {
                    if (!String.IsNullOrEmpty(cant))
                    {
                        try
                        {
                            cantidad = decimal.Parse(cant);
                            if (cantidad <= 0) {
                                error = true;
                                string script = @"<script type='text/javascript'> alert('" + "Error: La cantidad debe ser mayor a 0" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                        }
                        catch {
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
                            kilos = decimal.Parse(ki);
                            if (kilos <= 0)
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
                       // precio = decimal.Parse(pre);
                        impuesto = decimal.Parse(imp);
                        decimal monto = kilos * precio;
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
                        if (!String.IsNullOrEmpty(montoString)) {
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
                        BindData(cant,ki,des,rec,pre,cantFilas);
                    }
                    catch { }

                    try {
                        
                        Sistema.GetInstancia().Total += montoTotal;
                        Sistema.GetInstancia().SubTotal += subtotales;
                        Sistema.GetInstancia().Impuestos += impu;

                        Sistema.GetInstancia().Total = Math.Round(Sistema.GetInstancia().Total,2);
                        Sistema.GetInstancia().SubTotal = Math.Round(Sistema.GetInstancia().SubTotal, 2);
                        Sistema.GetInstancia().Impuestos = Math.Round(Sistema.GetInstancia().Impuestos, 2);

                        txtSubTotal.Text = Sistema.GetInstancia().SubTotal.ToString();
                        
                        txtImpuestos.Text = Sistema.GetInstancia().Impuestos.ToString();
                        txtTotal.Text = Sistema.GetInstancia().Total.ToString();
                        
                        ddlProductos.Focus();
                    }
                    catch(Exception ex) { }
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {

            this.txtFecha.Focus();

        }

        protected void btnAgregar_Click(object sender, EventArgs e) {
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
                            if (!String.IsNullOrEmpty(idProd.Text))
                            {
                                dt.Rows.Add(dr);
                                cont++;
                            }
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
                            if (String.IsNullOrEmpty(idProd.Text))
                            {
                                nueva.IdProducto = 0;
                            }
                            else
                            {
                                nueva.IdProducto = Int32.Parse(idProd.Text);
                                nueva.Producto = prod.Text;
                                nueva.UnidadMedida = unidad.Text;
                                lineas.Add(nueva);
                            }
                            
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

                            dr = dt.NewRow();
                            dr[0] = "";
                            dr[1] = "";
                            dr[2] = "";
                            dr[4] = "";
                            dr[5] = "";
                            dt.Rows.Add(dr);

                            gridViewFacturas.EditIndex = cont;
                            
                            gridViewFacturas.DataSource = dt;
                            gridViewFacturas.DataBind();

                            //ddlProductos.DataSource = Sistema.GetInstancia().ObtenerProductos(Session["rut"].ToString()).ToList();
                            //ddlProductos.DataTextField = "Nombre";
                            //ddlProductos.DataValueField = "IdProducto";
                            //ddlProductos.DataBind();
                        }
                    }
                }
                catch { }
            }
            else {
                string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un producto" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            
                try
                {
                    Sistema.GetInstancia().PDFActual = null;
                    Documento doc = new Documento();
                    doc.Activo = true;
                    bool error = false;
                    Cliente cli = Sistema.GetInstancia().BuscarClienteId(Int32.Parse(ddlCliente.SelectedValue));
                    doc.cliente = cli;
                    doc.Fecha = DateTime.Parse(txtFecha.Text);
                    doc.IdCliente = cli.IdCliente;
                    doc.LugarDestino = txtDestino.Text;
                    doc.Moneda = ddlMoneda.SelectedValue;
                    doc.FormaPago = ddlFormaPago.SelectedValue;
                    doc.NroGuia = txtNroGuia.Text;
                    doc.NroRemito = txtNroRemito.Text;
                    doc.rut = Session["rut"].ToString();
                    doc.IdUsuario = Int32.Parse(Session["idUsuario"].ToString());
                    doc.Usuario = Sistema.GetInstancia().BuscarUsuarioId(doc.IdUsuario);
                    doc.FechaVencimiento = DateTime.Parse(txtFechaVencimiento.Text);
                    if (doc.FormaPago.Equals("Contado"))
                    {
                        doc.EstadoCredito = "PAGO";
                        doc.DBCR = "DBCR";
                    }
                    else {
                        doc.EstadoCredito = "DEBE";
                        doc.DBCR = "DB";
                    }
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
                        doc.TipoDocumento = "111";
                    }
                    else {
                        doc.TipoDocumento = "101";

                    }
                    doc.detalle = ObtenerDetalle();
                    
                    doc.Total = Sistema.GetInstancia().Total;
                    doc.Redondeo = decimal.Round(doc.Total) - doc.Total;
                    doc.Total = Math.Round(doc.Total, 0);
                    if (!error) {
                        XmlDocument xml = null;
                        String xmlTexto = "";
                        if (doc.cliente.tipoDocumento.Equals("RUT"))
                        {
                            xml = GenerarXmlFactura(doc);
                            xmlTexto =  AjustarCFE(xml.InnerXml,"eFact");
                        }
                        else {
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
                                req.Uuid = "F" + Sistema.GetInstancia().ObtenerProximoCodigo(Session["rut"].ToString()).ToString();
                                req.TipoCfe = doc.TipoDocumento;
                                req.IdReq = "1";
                                req.FechaReq = doc.Fecha.Year + "" + doc.Fecha.Month + "" + doc.Fecha.Day;
                                req.HoraReq = doc.Fecha.Hour + "" + doc.Fecha.Minute + "" + doc.Fecha.Second;
                                req.CodComercio = Sistema.GetInstancia().CodComercio(Session["rut"].ToString());
                                req.CodTerminal = Sistema.GetInstancia().CodTerminal(Session["rut"].ToString());
                                String adenda = "";
                                if (!String.IsNullOrEmpty(txtNroGuia.Text))
                                {
                                    adenda += "Nro. Guia: " + txtNroGuia.Text + " - ";
                                }
                                if (!String.IsNullOrEmpty(txtNroRemito.Text))
                                {
                                    adenda += "Nro. Remito: " + txtNroRemito.Text + " - ";
                                } 
                                
                                if (!String.IsNullOrEmpty(txtAdenda.Text)) {
                                    adenda += txtAdenda.Text;
                                }
                                req.Adenda = adenda;
                                req.CfeXmlOTexto = xmlTexto;
                                if (cli != null && !String.IsNullOrEmpty(cli.Mail)) {
                                    req.EmailEnvioPdfReceptor = cli.Mail;
                                   
                                }
                                solicitud.Req = req;
                                solicitud.RequestDate = doc.Fecha.Year + "-" + doc.Fecha.Month + "-" + doc.Fecha.Day + "T" + doc.Fecha.Hour + ":" + doc.Fecha.Minute + ":" + doc.Fecha.Second;
                                solicitud.Tout = 120000;
                                RespBody respuesta = null;
                                
                                if (client.InnerChannel.State != System.ServiceModel.CommunicationState.Faulted)
                                {
                                    respuesta = client.Invoke(solicitud);
                                }
                                if(respuesta != null){
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
                                            if (doc != null && doc.detalle != null) {
                                                foreach (Detalle det in doc.detalle) {
                                                    Sistema.GetInstancia().DisminuirStock(det.IdProducto, det.Cantidad, det.Kilos);
                                                }
                                            }
                                            try
                                            {
                                                byte[] pdf = client2.ObtenerPdf(Session["rut"].ToString(), Int32.Parse(respuesta.Resp.TipoCfe), respuesta.Resp.Serie, doc.NroSerie);
                                                
                                                Sistema.GetInstancia().PDFActual = pdf;
                                                if (Sistema.GetInstancia().PDFActual != null)
                                                {
                                                    //Response.Redirect("VisorPDF.aspx");
                                                    Response.Write("<script>");
                                                    Response.Write("window.open('Facturas.aspx', '_blank');");
                                                    Response.Write("window.location.href = 'VisorPDF.aspx';");
                                                    Response.Write("</script>");
                                                }
                                                else {
                                                    string script = @"<script type='text/javascript'> alert('" + "Error al obtener PDF" + "');</script>";
                                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
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
                                                    //Response.Redirect("VisorPDF.aspx");
                                                    Response.Write("<script>");
                                                    Response.Write("window.open('Facturas.aspx', '_blank');");
                                                    Response.Write("window.location.href = 'VisorPDF.aspx';");
                                                    Response.Write("</script>");
                                                }
                                                else {
                                                    string script = @"<script type='text/javascript'> alert('" + "Error al obtener PDF" + "');</script>";
                                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
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
                        catch (Exception ex) { 
                            
                        }
                        
                        //Guardar serie y nro
                        
                    }
                    btnBuscar.Enabled = true;
                }
                catch(Exception ex) {
                    string script = @"<script type='text/javascript'> alert('" + "Error al guardar" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                
                }
            
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

        private void BindData(String cant, String ki, String des, String rec,String pre, int  CantFilas) {
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
            gridViewFacturas.DataSource = dt;
            gridViewFacturas.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

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
                Label precio = gvr.FindControl("lblPrecio") as Label;
                Label recargo = gvr.FindControl("lblRecargo") as Label;
                Label monto = gvr.FindControl("lblMonto") as Label;
                Label sub = gvr.FindControl("lblSubTotal") as Label;
                if (idProd != null)
                {
                    if (!String.IsNullOrEmpty(idProd.Text))
                    {
                        Producto producto = Sistema.GetInstancia().BuscarProductoId(Int32.Parse(idProd.Text));
                        detalle.producto = producto;
                        detalle.IdProducto = producto.IdProducto;
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
                        if (precio != null)
                        {
                            detalle.PrecioUnitario = decimal.Parse(precio.Text);
                        }
                        detalle.NroLinea = 1;
                        cont++;
                        retorno.Add(detalle);
                    }
                }
            }
            return retorno;
        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            int IdCliente = 0;
            if (!String.IsNullOrEmpty(ddlCliente.SelectedValue) && ddlCliente.SelectedIndex != 0)
            {
                IdCliente = Int32.Parse(ddlCliente.SelectedValue);
            }
            if (IdCliente!=0){
                Cliente cli = Sistema.GetInstancia().BuscarClienteId(IdCliente);
                lblVendedorAsignado.Text=cli.Vendedor.Nombre.ToString();
                trVendedorAsignado.Visible = true;
            }
        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void btnActualizarCombos_CLick(object sender, EventArgs e)
        {
            ddlCliente.DataSource = Sistema.GetInstancia().ObtenerClientes();
            ddlCliente.DataTextField = "ddlDescription";
            ddlCliente.DataValueField = "IdCliente";
            ddlCliente.DataBind();

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

        private void limpiarFormulario() {
            Sistema.GetInstancia().SubTotal = 0;
            Sistema.GetInstancia().Total = 0;
            Sistema.GetInstancia().Impuestos = 0;
            txtFechaVencimiento.Text = DateTime.Now.ToShortDateString();
            txtFecha.Text = DateTime.Now.ToShortDateString();
            Sistema.GetInstancia().PDFActual = null;
        }

        private XmlDocument GenerarXmlFactura(Documento doc) {
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
                    idDoc.FchVencSpecified = true;
                    idDoc.FchVenc = doc.FechaVencimiento;

                }
                else {
                    idDoc.FmaPago = IdDoc_FactFmaPago.Item1;
                }
                idDoc.TipoCFE = IdDoc_FactTipoCFE.Item111;
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
                        
                        if (det.producto.indicador.Codigo == 2) {
                            impuesto= Sistema.GetInstancia().TasaMinima();
                            
                        }
                        else if (det.producto.indicador.Codigo == 3) {
                            impuesto = Sistema.GetInstancia().TasaBasica();
                            
                        }

                        decimal mntIVA = Convert.ToDecimal(("1." + impuesto), System.Globalization.CultureInfo.InvariantCulture);
                        decimal mntT = (det.producto.Precio * det.Kilos) * mntIVA;
                        if (impuesto > 0)
                        {
                            item.PrecioUnitario = item.PrecioUnitario * mntIVA;
                        }
                        if (det.Descuento != null)
                        {
                            item.DescuentoPct = decimal.Round((decimal)det.Descuento, 2);
                            item.DescuentoPctSpecified = true;
                            
                            item.DescuentoMonto = (mntT * (decimal)det.Descuento)/100;
                            item.DescuentoMontoSpecified = true;
                            item.DescuentoMonto = decimal.Round(item.DescuentoMonto, 2);
                        }
                        else {
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
                        else {
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
                        if (det.producto.indicador.Codigo == 1 || det.producto.indicador.Codigo == 5 ) {
                            sumaMontoNoGravados += det.MontoTotal;
                        }
                        else if (det.producto.indicador.Codigo == 2) {
                            sumaMontoNetoIVAMinima += (det.MontoTotal / mntIVA );
                            
                        }
                        else if (det.producto.indicador.Codigo == 3) {
                            sumaMontoNetoIVABasica += (det.MontoTotal / mntIVA);
                            
                        }
                        else if (det.producto.indicador.Codigo == 11) {
                            sumaMontoImpuestoPercibido += det.MontoTotal;
                        }
                        else if (det.producto.indicador.Codigo == 12) {
                            sumaMontoIvaEnSuspenso += det.MontoTotal;
                        }
                        else if (det.producto.indicador.Codigo == 6) {
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
                else {
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
                else {
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
                    totales.MntIVATasaBasica = (sumaMontoNetoIVABasica * Sistema.GetInstancia().TasaBasica())/100;
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

                if (redondeo != 0)
                {
                    DscRcgGlobalDRG_Item[] dscrecItems = new DscRcgGlobalDRG_Item[1];
                    DscRcgGlobalDRG_Item dscrecItem = new DscRcgGlobalDRG_Item();
                    dscrecItem.GlosaDR = "Redondeo";
                    dscrecItem.NroLinDR = "1";
                    dscrecItem.TpoDR = TipoDRType.Item1;

                    if (sumaMontoNetoIVAMinima > 0)
                    {
                        dscrecItem.IndFactDR = DscRcgGlobalDRG_ItemIndFactDR.Item2;
                    }
                    else if (sumaMontoNetoIVABasica > 0)
                    {
                        dscrecItem.IndFactDR = DscRcgGlobalDRG_ItemIndFactDR.Item3;
                    }
                    else if (sumaMontoNoFacturable > 0 && redondeo > 0)
                    {
                        dscrecItem.IndFactDR = DscRcgGlobalDRG_ItemIndFactDR.Item6;
                    }
                    else if (sumaMontoNoFacturable > 0 && redondeo < 0)
                    {
                        dscrecItem.IndFactDR = DscRcgGlobalDRG_ItemIndFactDR.Item7;
                    }
                    else
                    {
                        dscrecItem.IndFactDR = DscRcgGlobalDRG_ItemIndFactDR.Item1;
                    }
                    if (redondeo > 0)
                    {
                        dscrecItem.TpoMovDR = DscRcgGlobalDRG_ItemTpoMovDR.D;
                        dscrecItem.ValorDR = redondeo;
                    }
                    else
                    {
                        dscrecItem.TpoMovDR = DscRcgGlobalDRG_ItemTpoMovDR.R;
                        dscrecItem.ValorDR = (redondeo * (-1));
                    }
                    dscrecItems[0] = dscrecItem;
                    factura.DscRcgGlobal = dscrecItems;
                }
                
            }
            catch (Exception ex)
            {

            }

            factura.Encabezado = encabezado;

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
                    idDoc.FchVencSpecified = true;
                    idDoc.FchVenc = doc.FechaVencimiento;
                }
                else
                {
                    idDoc.FmaPago = IdDoc_TckFmaPago.Item1;
                }
                idDoc.TipoCFE = IdDoc_TckTipoCFE.Item101;
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
                else {
                    receptor.ItemElementName = ItemChoiceType.DocRecepExt;
                    receptor.CodPaisRecep = (CodPaisType)Enum.Parse(typeof(CodPaisType), cli.Pais.Codigo, true);
                }
                receptor.CodPaisRecepSpecified = true;
                receptor.TipoDocRecepSpecified = true;

                receptor.RznSocRecep = cli.Nombre;
                if (cli.tipoDocumento.Equals("RUT"))
                {
                    receptor.TipoDocRecep = DocType.Item2;
                    
                }
                else if (cli.tipoDocumento.Equals("CI")) {
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
                
                receptor.Item = cli.nroDoc.ToString();
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
                    decimal mntIVA = Convert.ToDecimal(("1." + impuesto), System.Globalization.CultureInfo.InvariantCulture);
                    decimal mntT = (det.producto.Precio * det.Kilos) * mntIVA;
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

                if (redondeo != 0)
                {
                    DscRcgGlobalDRG_Item[] dscrecItems = new DscRcgGlobalDRG_Item[1];
                    DscRcgGlobalDRG_Item dscrecItem = new DscRcgGlobalDRG_Item();
                    dscrecItem.GlosaDR = "Redondeo";
                    dscrecItem.NroLinDR = "1";
                    dscrecItem.TpoDR = TipoDRType.Item1;

                    if (sumaMontoNetoIVAMinima > 0)
                    {
                        dscrecItem.IndFactDR = DscRcgGlobalDRG_ItemIndFactDR.Item2;
                    }
                    else if (sumaMontoNetoIVABasica > 0)
                    {
                        dscrecItem.IndFactDR = DscRcgGlobalDRG_ItemIndFactDR.Item3;
                    }
                    else if (sumaMontoNoFacturable > 0 && redondeo > 0)
                    {
                        dscrecItem.IndFactDR = DscRcgGlobalDRG_ItemIndFactDR.Item6;
                    }
                    else if (sumaMontoNoFacturable > 0 && redondeo < 0)
                    {
                        dscrecItem.IndFactDR = DscRcgGlobalDRG_ItemIndFactDR.Item7;
                    }
                    else
                    {
                        dscrecItem.IndFactDR = DscRcgGlobalDRG_ItemIndFactDR.Item1;
                    }
                    if (redondeo > 0)
                    {
                        dscrecItem.TpoMovDR = DscRcgGlobalDRG_ItemTpoMovDR.D;
                        dscrecItem.ValorDR = redondeo;
                    }
                    else
                    {
                        dscrecItem.TpoMovDR = DscRcgGlobalDRG_ItemTpoMovDR.R;
                        dscrecItem.ValorDR = (redondeo * (-1));
                    }
                    dscrecItems[0] = dscrecItem;
                    factura.DscRcgGlobal = dscrecItems;
                }

            }
            catch (Exception ex)
            {

            }

            factura.Encabezado = encabezado;

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

        public Item_Det_FactIndFact ObtenerIndicadorFacturacion(int cod) {
            if (cod == 1) {
                return Item_Det_FactIndFact.Item1;
            }
            else if (cod == 2) {
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
            else {
                return Item_Det_FactIndFact.Item3;
            }
        }

        private String ObtenerHashMac(String datos, String clave) {
            
            
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(clave);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            byte[] messageBytes = encoding.GetBytes(datos);
            
            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);
            return System.Convert.ToBase64String(hashmessage,0,hashmessage.Length); ;
        }

        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        public String AjustarCFE(String xmlFactura, String tipo)
        {
            String[] xmlCortado = xmlFactura.Split(new char[] { '<', '>' });
            String xml = "";
            for (int i = 1; i < xmlCortado.Length; i = i+2)
            {
                if (i == 3)
                {
                    String aux = "http://www.w3.org/2001/XMLSchema";
                    String aux2 = "http://www.w3.org/2001/XMLSchema-instance";
      
                    xml += "<CFE xmlns=\"http://cfe.dgi.gub.uy\" xmlns:xsd=\""+ aux+"\""+ " xmlns:xsi=\""+ aux2+"\""+" version=\"1.0\">";
                }
                else if (i == 5)
                {

                    xml += "<"+tipo+">";

                }
                else if(i>8)
                {
                    if (xmlCortado[i].Contains("CFEDefTypeEFact") || xmlCortado[i].Contains("CFEDefTypeETck"))
                    {
                        xml += "</"+tipo+">";
                        xml += "</CFE>";
                    }
                    else
                    {
                        if (xmlCortado.Length > (i + 1))
                        {
                            xml += ("<" + xmlCortado[i] + ">" + xmlCortado[i + 1]);
                        }
                        else {
                            xml += ("<" + xmlCortado[i] + ">");
                        }
                    }
                }
                

            }
           
           
            return xml;
        }

        protected void MycloseWindow(object sender, EventArgs e)
        {
            try
            {
                ddlCliente.DataSource = null;
                List<Cliente> clientes = Sistema.GetInstancia().ObtenerClientes();
                try
                {

                    ddlCliente.DataSource = clientes;
                    ddlCliente.DataTextField = "ddlDescription";
                    ddlCliente.DataValueField = "IdCliente";
                    ddlCliente.DataBind();
                }
                catch { }
                ddlCliente.SelectedValue = clientes.ElementAt(clientes.Count - 1).IdCliente.ToString();
            }
            catch { }
        }

        private bool RucValido(String ruc)
        {
            bool result = false;
            try
            {
                long rucNro = long.Parse(ruc);
                long ckDigOri = rucNro - (rucNro / 10) * 10;
                int suma = 0;

                suma += Int32.Parse(ruc.Substring(0, 1)) * 4;
                suma += Int32.Parse(ruc.Substring(1, 1)) * 3;
                suma += Int32.Parse(ruc.Substring(2, 1)) * 2;
                suma += Int32.Parse(ruc.Substring(3, 1)) * 9;
                suma += Int32.Parse(ruc.Substring(4, 1)) * 8;
                suma += Int32.Parse(ruc.Substring(5, 1)) * 7;
                suma += Int32.Parse(ruc.Substring(6, 1)) * 6;
                suma += Int32.Parse(ruc.Substring(7, 1)) * 5;
                suma += Int32.Parse(ruc.Substring(8, 1)) * 4;
                suma += Int32.Parse(ruc.Substring(9, 1)) * 3;
                suma += Int32.Parse(ruc.Substring(10, 1)) * 2;

                int resto = suma - (suma / 11) * 11;
                int chkDigOk = 11 - resto;

                if (chkDigOk == 11)
                {
                    chkDigOk = 0;
                }
                else if (chkDigOk == 10)
                {
                    chkDigOk = -1;
                }
                int primerosDigitos = Int32.Parse(ruc.Substring(0, 2));
                int otrosDigitos = Int32.Parse(ruc.Substring(2, 6));
                if (chkDigOk == ckDigOri && primerosDigitos >= 1 && primerosDigitos <= 21 && otrosDigitos >= 1 && otrosDigitos <= 999999)
                {
                    result = true;
                }


            }
            catch
            {
                return false;
            }


            return result;

        }

        private bool CIValida(String ci)
        {
            bool valida = false;
            try
            {
                //Control inicial sobre la cantidad de números ingresados. 
                if (ci.Length == 8 || ci.Length == 7)
                {

                    int[] _formula = { 2, 9, 8, 7, 6, 3, 4 };
                    int _suma = 0;
                    int _guion = 0;
                    int _aux = 0;
                    int[] _numero = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };

                    if (ci.Length == 8)
                    {
                        _numero[0] = Convert.ToInt32(ci[0].ToString());
                        _numero[1] = Convert.ToInt32(ci[1].ToString());
                        _numero[2] = Convert.ToInt32(ci[2].ToString());
                        _numero[3] = Convert.ToInt32(ci[3].ToString());
                        _numero[4] = Convert.ToInt32(ci[4].ToString());
                        _numero[5] = Convert.ToInt32(ci[5].ToString());
                        _numero[6] = Convert.ToInt32(ci[6].ToString());
                        _numero[7] = Convert.ToInt32(ci[7].ToString());
                    }

                    //Para cédulas menores a un millón. 
                    else if (ci.Length == 7)
                    {
                        _numero[0] = 0;
                        _numero[1] = Convert.ToInt32(ci[0].ToString());
                        _numero[2] = Convert.ToInt32(ci[1].ToString());
                        _numero[3] = Convert.ToInt32(ci[2].ToString());
                        _numero[4] = Convert.ToInt32(ci[3].ToString());
                        _numero[5] = Convert.ToInt32(ci[4].ToString());
                        _numero[6] = Convert.ToInt32(ci[5].ToString());
                        _numero[7] = Convert.ToInt32(ci[6].ToString());
                    }

                    _suma = (_numero[0] * _formula[0]) + (_numero[1] * _formula[1]) + (_numero[2] * _formula[2]) + (_numero[3] * _formula[3]) + (_numero[4] * _formula[4]) + (_numero[5] * _formula[5]) + (_numero[6] * _formula[6]);

                    for (int i = 0; i < 10; i++)
                    {
                        _aux = _suma + i;
                        if (_aux % 10 == 0)
                        {
                            _guion = _aux - _suma;
                            i = 10;
                        }
                    }

                    if (_numero[7] == _guion)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    throw new Exception("La Cédula debe tener 7 u 8 caractéres.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return valida;
        }
    }
}