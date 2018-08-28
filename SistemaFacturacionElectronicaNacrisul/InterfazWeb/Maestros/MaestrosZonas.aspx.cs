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
    public partial class MaestrosZonas : System.Web.UI.Page
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
                    gridViewZonas.DataSource = Sistema.GetInstancia().BuscarZonas(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                    gridViewZonas.DataBind();
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
                gridViewZonas.DataSource = Sistema.GetInstancia().BuscarZonas(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                gridViewZonas.DataBind();
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void gridViewZonas_RowCreated(object sender, GridViewRowEventArgs e)
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
                gridViewZonas.EditIndex = e.NewEditIndex;
                CheckBoxActivo.Checked = true;
                gridViewZonas.DataSource = Sistema.GetInstancia().BuscarZonas(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                gridViewZonas.DataBind();
                int index = gridViewZonas.EditIndex;
                GridViewRow row = gridViewZonas.Rows[index];
                bool activo = ((CheckBox)row.FindControl("ckActivo")).Checked;
                LinkButton lnkRemove = (row.FindControl("lnkRemove") as LinkButton);
                lnkRemove.Visible = false;
                if (!activo)
                {
                    try
                    {
                        gridViewZonas.EditIndex = -1;
                        gridViewZonas.DataSource = Sistema.GetInstancia().BuscarZonas(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                        gridViewZonas.DataBind();
                    }
                    catch
                    {
                        string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    string script2 = @"<script type='text/javascript'> alert('" + "La zona se encuentra eliminada: No se puede modificar" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script2, false);
                }
            }
            catch
            {
                try
                {
                    gridViewZonas.EditIndex = -1;
                    gridViewZonas.DataSource = Sistema.GetInstancia().BuscarZonas(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                    gridViewZonas.DataBind();
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
                gridViewZonas.EditIndex = -1;
                gridViewZonas.DataSource = Sistema.GetInstancia().BuscarZonas(txbNombre.Text, txbCodigo.Text,CheckBoxActivo.Checked);
                gridViewZonas.DataBind();
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
                int index = gridViewZonas.EditIndex;
                GridViewRow row = gridViewZonas.Rows[index];
                string id = ((Label)row.FindControl("lblIdZona")).Text;
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
                    Dominio.Clases_Dominio.Zona zona = new Dominio.Clases_Dominio.Zona();
                    zona.IdZona = Int32.Parse(id);
                    zona.Codigo = codigo;
                    zona.Nombre = Nombre;
                    String msg = Sistema.GetInstancia().ModificarZona(zona);
                    string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);

                    gridViewZonas.EditIndex = -1;
                    gridViewZonas.DataSource = Sistema.GetInstancia().BuscarZonas(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                    gridViewZonas.DataBind();
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

                int index = gridViewZonas.EditIndex + 1;
                GridViewRow row = gridViewZonas.Rows[index];
                bool activo = CheckBoxActivo.Checked;

                if (activo)
                {
                    Dominio.Clases_Dominio.Zona zona = new Dominio.Clases_Dominio.Zona();
                    zona.IdZona = Int32.Parse(id.Trim());
                    String msg = Sistema.GetInstancia().EliminarZona(zona);
                    string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                try
                {


                    gridViewZonas.DataSource = Sistema.GetInstancia().BuscarZonas(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked);
                    gridViewZonas.DataBind();
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
                gridViewZonas.PageIndex = e.NewPageIndex;
                gridViewZonas.DataBind();
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