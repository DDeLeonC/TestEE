using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using System.Data;

namespace InterfazWeb.Maestros
{
    public partial class MaestrosProductos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
            if (!IsPostBack)
            {
                cbxGrupo.DataSource = Sistema.GetInstancia().ObtenerGrupos(Session["rut"].ToString());
                cbxGrupo.DataTextField = "Descripcion";
                cbxGrupo.DataValueField = "IdGrupo";
                cbxGrupo.DataBind();

                cbxGrupo.Items.Insert(0, new ListItem("Todos"));

                cbxSubGrupo.DataSource = Sistema.GetInstancia().ObtenerSubGrupos(Session["rut"].ToString());
                cbxSubGrupo.DataTextField = "Descripcion";
                cbxSubGrupo.DataValueField = "IdSubGrupo";
                cbxSubGrupo.DataBind();

                cbxSubGrupo.Items.Insert(0, new ListItem("Todos"));
            }
        }


        private void BindData()
        {
            try
            {
                if (!IsPostBack)
                {
                    CheckBoxActivo.Checked = true;
                    int? grupo = null;
                    if (!String.IsNullOrEmpty(cbxSubGrupo.SelectedValue) && cbxSubGrupo.SelectedIndex != 0)
                    {
                        grupo = Int32.Parse(cbxSubGrupo.SelectedValue);
                    }
                    gridViewClientes.DataSource = Sistema.GetInstancia().BuscarProductos(txbNombre.Text, txtCodigo.Text, grupo, CheckBoxActivo.Checked, Session["rut"].ToString());
                    gridViewClientes.DataBind();
                    txbNombre.Focus();

                }
            }
            catch (Exception ex)
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxGrupo.SelectedValue != null)
            {
                try
                {
                    int IdGrupo = Int32.Parse(cbxGrupo.SelectedValue);
                    List<Dominio.Clases_Dominio.SubGrupo> subgrupos = Sistema.GetInstancia().ObtenerSubGruposGrupo(IdGrupo, Session["rut"].ToString()).ToList();
                    cbxSubGrupo.DataSource = subgrupos;
                    cbxSubGrupo.DataTextField = "Descripcion";
                    cbxSubGrupo.DataValueField = "IdSubGrupo";
                    cbxSubGrupo.DataBind();

                    cbxSubGrupo.Items.Insert(0, new ListItem("Todos"));
                }
                catch
                {
                    cbxSubGrupo.DataSource = Sistema.GetInstancia().ObtenerSubGrupos(Session["rut"].ToString());
                    cbxSubGrupo.DataTextField = "Descripcion";
                    cbxSubGrupo.DataValueField = "IdSubGrupo";
                    cbxSubGrupo.DataBind();

                    cbxSubGrupo.Items.Insert(0, new ListItem("Todos"));
                }
            }

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                    int? subgrupo = null;
                    if (!String.IsNullOrEmpty(cbxSubGrupo.SelectedValue) && cbxSubGrupo.SelectedIndex != 0)
                    {
                        subgrupo = Int32.Parse(cbxSubGrupo.SelectedValue);
                    }
                    gridViewClientes.DataSource = Sistema.GetInstancia().BuscarProductos(txbNombre.Text, txtCodigo.Text, subgrupo, CheckBoxActivo.Checked, Session["rut"].ToString());
                    gridViewClientes.DataBind();
                    
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void gridViewClientes_RowCreated(object sender, GridViewRowEventArgs e)
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

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (!CheckBoxActivo.Checked)
                    {
                        e.Row.FindControl("lnkRemove").Visible = false;
                    }
                    else
                    {
                        e.Row.FindControl("lnkRemove").Visible = true;
                    }
                }
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
                gridViewClientes.EditIndex = e.NewEditIndex;
                CheckBoxActivo.Checked = true;
                int? gpo = null;
                if (!String.IsNullOrEmpty(cbxSubGrupo.SelectedValue) && cbxSubGrupo.SelectedIndex != 0)
                {
                    gpo = Int32.Parse(cbxSubGrupo.SelectedValue);
                }
                gridViewClientes.DataSource = Sistema.GetInstancia().BuscarProductos(txbNombre.Text, txtCodigo.Text, gpo, CheckBoxActivo.Checked, Session["rut"].ToString());
                gridViewClientes.DataBind();
                int index = gridViewClientes.EditIndex;
                GridViewRow row = gridViewClientes.Rows[index];
                bool activo = ((CheckBox)row.FindControl("ckActivo")).Checked;
                DropDownList ddlSubGrupo = (row.FindControl("ddlSubGrupo") as DropDownList);
                LinkButton lnkRemove = (row.FindControl("lnkRemove") as LinkButton);
                lnkRemove.Visible = false;
                if (ddlSubGrupo != null)
                {
                    ddlSubGrupo.DataSource = Sistema.GetInstancia().ObtenerSubGrupos(Session["rut"].ToString());
                    ddlSubGrupo.DataTextField = "Descripcion";
                    ddlSubGrupo.DataValueField = "IdSubGrupo";
                    ddlSubGrupo.DataBind();

                    Label grupo = row.FindControl("lblSubGrupo") as Label;
                    ddlSubGrupo.Items.FindByText(grupo.Text).Selected = true;

                }
                
                DropDownList ddlindicador = (row.FindControl("ddlindicador") as DropDownList);
                if (ddlindicador != null)
                {
                    ddlindicador.DataSource = Sistema.GetInstancia().ObtenerIndicadores();
                    ddlindicador.DataTextField = "Nombre";
                    ddlindicador.DataValueField = "IdIndicador";
                    ddlindicador.DataBind();

                    Label grupo = row.FindControl("lblindicador") as Label;
                    ddlindicador.Items.FindByText(grupo.Text).Selected = true;

                }
                if (!activo)
                {
                    try
                    {
                        gridViewClientes.EditIndex = -1;
                        int? grupo = null;
                        if (!String.IsNullOrEmpty(cbxSubGrupo.SelectedValue) && cbxSubGrupo.SelectedIndex != 0)
                        {
                            grupo = Int32.Parse(cbxSubGrupo.SelectedValue);
                        }
                        gridViewClientes.DataSource = Sistema.GetInstancia().BuscarProductos(txbNombre.Text, txtCodigo.Text, grupo, CheckBoxActivo.Checked, Session["rut"].ToString());
                        gridViewClientes.DataBind();
                    }
                    catch
                    {
                        string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    string script2 = @"<script type='text/javascript'> alert('" + "El producto se encuentra eliminado: No se puede modificar" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script2, false);
                }

            }
            catch
            {
                try
                {
                    gridViewClientes.EditIndex = -1;
                    int? grupo = null;
                    if (!String.IsNullOrEmpty(cbxSubGrupo.SelectedValue) && cbxSubGrupo.SelectedIndex != 0)
                    {
                        grupo = Int32.Parse(cbxSubGrupo.SelectedValue);
                    }
                    gridViewClientes.DataSource = Sistema.GetInstancia().BuscarProductos(txbNombre.Text, txtCodigo.Text, grupo, CheckBoxActivo.Checked, Session["rut"].ToString());
                    gridViewClientes.DataBind();
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

        protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gridViewClientes.EditIndex = -1;
                int? grupo = null;
                if (!String.IsNullOrEmpty(cbxSubGrupo.SelectedValue) && cbxSubGrupo.SelectedIndex != 0)
                {
                    grupo = Int32.Parse(cbxSubGrupo.SelectedValue);
                }
                gridViewClientes.DataSource = Sistema.GetInstancia().BuscarProductos(txbNombre.Text, txtCodigo.Text, grupo, CheckBoxActivo.Checked, Session["rut"].ToString());
                gridViewClientes.DataBind();
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void Modificar(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int index = gridViewClientes.EditIndex;
                GridViewRow row = gridViewClientes.Rows[index];
                string id = ((Label)row.FindControl("lblIdProducto")).Text;
                string codigo = ((TextBox)row.FindControl("txtCodigo")).Text;
                String subGrupo = ((DropDownList)row.FindControl("ddlSubGrupo")).SelectedValue;
                string nombre = ((TextBox)row.FindControl("txtNombre")).Text;
                string descripcion = ((TextBox)row.FindControl("txtDescripcion")).Text;
                string precio = ((TextBox)row.FindControl("txtPrecio")).Text;
                string unidad = ((TextBox)row.FindControl("txtUnidad")).Text;
                String indicador = ((DropDownList)row.FindControl("ddlindicador")).SelectedValue;
                bool error = false;


                if (!error)
                {
                    if (String.IsNullOrEmpty(nombre))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un nombre" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                }
                Dominio.Clases_Dominio.Producto producto = new Dominio.Clases_Dominio.Producto();
                if (!error)
                {
                    if (String.IsNullOrEmpty(codigo))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un codigo" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    
                }
                
                if (!error)
                {
                    if (String.IsNullOrEmpty(subGrupo))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un subGrupo" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    else
                    {
                        try
                        {
                            int idSubGrupo = Int32.Parse(subGrupo);
                            producto.IdSubGrupo = idSubGrupo;

                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un subGrupo" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                }
                if (!error)
                {
                    if (String.IsNullOrEmpty(indicador))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un indicador" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    else
                    {
                        try
                        {
                            int idIndicador = Int32.Parse(indicador);
                            producto.IdIndicador = idIndicador;

                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un indicador" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                }
                if (!error)
                {
                    if (String.IsNullOrEmpty(precio))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un precio" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    else
                    {
                        try
                        {
                            decimal pre = decimal.Parse(precio);
                            producto.Precio = pre;
                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un decimal en precio" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                }

                if (!error)
                {


                    producto.IdProducto = Int32.Parse(id);
                    producto.Codigo = codigo;
                    producto.Descripcion = descripcion;
                    producto.Nombre = nombre;
                    producto.unidadMedida = unidad;
                    String msg = Sistema.GetInstancia().ModificarProducto(producto);
                    string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);

                    gridViewClientes.EditIndex = -1;

                    int? subgrupo = null;
                    if (!String.IsNullOrEmpty(cbxSubGrupo.SelectedValue) && cbxSubGrupo.SelectedIndex != 0)
                    {
                        subgrupo = Int32.Parse(cbxSubGrupo.SelectedValue);
                    }
                    gridViewClientes.DataSource = Sistema.GetInstancia().BuscarProductos(txbNombre.Text, txtCodigo.Text, subgrupo, CheckBoxActivo.Checked, Session["rut"].ToString());
                    gridViewClientes.DataBind();
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

                int index = gridViewClientes.EditIndex + 1;
                GridViewRow row = gridViewClientes.Rows[index];
                bool activo = CheckBoxActivo.Checked;

                if (activo)
                {
                    Dominio.Clases_Dominio.Producto producto = new Dominio.Clases_Dominio.Producto();
                    producto.IdProducto = Int32.Parse(id.Trim());
                    String msg = Sistema.GetInstancia().EliminarProducto(producto);
                    string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                try
                {

                    int? subgrupo = null;
                    if (!String.IsNullOrEmpty(cbxSubGrupo.SelectedValue) && cbxSubGrupo.SelectedIndex != 0)
                    {
                        subgrupo = Int32.Parse(cbxSubGrupo.SelectedValue);
                    }
                    gridViewClientes.DataSource = Sistema.GetInstancia().BuscarProductos(txbNombre.Text, txtCodigo.Text, subgrupo, CheckBoxActivo.Checked, Session["rut"].ToString());
                    gridViewClientes.DataBind();
                    txbNombre.Focus();


                }
                catch (Exception ex)
                {
                    string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }

            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al eliminar" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                BindData();
                gridViewClientes.PageIndex = e.NewPageIndex;
                gridViewClientes.DataBind();
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {

            this.txbNombre.Focus();

        }

        
    }
}