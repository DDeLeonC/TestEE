using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Dominio.Clases_Dominio;

namespace InterfazWeb.Maestros
{
    public partial class MaestrosVendedores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            try
            {
                if (!IsPostBack)
                {
                    CheckBoxActivo.Checked = true;
                    gridViewVendedores.DataSource = Sistema.GetInstancia().BuscarVendedores(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                    gridViewVendedores.DataBind();
                    txbNombre.Focus();

                }
            }
            catch (Exception ex)
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                gridViewVendedores.DataSource = Sistema.GetInstancia().BuscarVendedores(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                gridViewVendedores.DataBind();
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void gridViewVendedores_RowCreated(object sender, GridViewRowEventArgs e)
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
                gridViewVendedores.EditIndex = e.NewEditIndex;
                CheckBoxActivo.Checked = true;
                gridViewVendedores.DataSource = Sistema.GetInstancia().BuscarVendedores(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                gridViewVendedores.DataBind();
                int index = gridViewVendedores.EditIndex;
                GridViewRow row = gridViewVendedores.Rows[index];
                bool activo = ((CheckBox)row.FindControl("ckActivo")).Checked;
                LinkButton lnkRemove = (row.FindControl("lnkRemove") as LinkButton);
                lnkRemove.Visible = false;
                if (!activo)
                {
                    try
                    {
                        gridViewVendedores.EditIndex = -1;
                        gridViewVendedores.DataSource = Sistema.GetInstancia().BuscarVendedores(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                        gridViewVendedores.DataBind();
                    }
                    catch
                    {
                        string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    string script2 = @"<script type='text/javascript'> alert('" + "El vendedor se encuentra eliminado: No se puede modificar" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script2, false);
                }
            }
            catch
            {
                try
                {
                    gridViewVendedores.EditIndex = -1;
                    gridViewVendedores.DataSource = Sistema.GetInstancia().BuscarVendedores(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                    gridViewVendedores.DataBind();
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
                gridViewVendedores.EditIndex = -1;
                gridViewVendedores.DataSource = Sistema.GetInstancia().BuscarVendedores(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                gridViewVendedores.DataBind();
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
                int index = gridViewVendedores.EditIndex;
                GridViewRow row = gridViewVendedores.Rows[index];
                string id = ((Label)row.FindControl("lblIdVendedor")).Text;
                string Nombre = ((TextBox)row.FindControl("txtNombre")).Text;
                string codigo = ((TextBox)row.FindControl("txtCodigo")).Text;
                
                bool error = false;
                if (!error)
                {
                    if (String.IsNullOrEmpty(codigo))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un código" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                }

                if (!error)
                {
                    if (String.IsNullOrEmpty(Nombre))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un nombre" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                }
                

                if (!error)
                {
                    Dominio.Clases_Dominio.Vendedor vendedor = new Dominio.Clases_Dominio.Vendedor();
                    vendedor.IdVendedor = Int32.Parse(id);
                    vendedor.Codigo = codigo;
                    vendedor.Nombre = Nombre;
                    String msg = Sistema.GetInstancia().ModificarVendedor(vendedor);
                    string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);

                    gridViewVendedores.EditIndex = -1;
                    gridViewVendedores.DataSource = Sistema.GetInstancia().BuscarVendedores(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                    gridViewVendedores.DataBind();
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

                int index = gridViewVendedores.EditIndex + 1;
                GridViewRow row = gridViewVendedores.Rows[index];
                bool activo = CheckBoxActivo.Checked;

                if (activo)
                {
                    Dominio.Clases_Dominio.Vendedor vendedor = new Dominio.Clases_Dominio.Vendedor();
                    vendedor.IdVendedor = Int32.Parse(id.Trim());
                    String msg = Sistema.GetInstancia().EliminarVendedor(vendedor);
                    string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                try
                {


                    gridViewVendedores.DataSource = Sistema.GetInstancia().BuscarVendedores(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                    gridViewVendedores.DataBind();
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
                gridViewVendedores.PageIndex = e.NewPageIndex;
                gridViewVendedores.DataBind();
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.txbCodigo.Focus();
            this.txbNombre.Focus();

        }
    }
}