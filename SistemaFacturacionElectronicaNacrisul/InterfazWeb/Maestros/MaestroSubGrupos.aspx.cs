using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Dominio.Clases_Dominio;

namespace Interfaz_Web.Maestros
{
    public partial class MaestroSubGrupos : System.Web.UI.Page
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
                    if (!String.IsNullOrEmpty(cbxGrupo.SelectedValue) && cbxGrupo.SelectedIndex != 0)
                    {
                        grupo = Int32.Parse(cbxGrupo.SelectedValue);
                    }
                    gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarSubGrupos(txbNombre.Text, txbCodigo.Text, grupo, CheckBoxActivo.Checked, Session["rut"].ToString());
                    gridViewGrupos.DataBind();
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
                int? grupo = null;
                if (!String.IsNullOrEmpty(cbxGrupo.SelectedValue) && cbxGrupo.SelectedIndex != 0)
                {
                    grupo = Int32.Parse(cbxGrupo.SelectedValue);
                }
                gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarSubGrupos(txbNombre.Text, txbCodigo.Text, grupo, CheckBoxActivo.Checked, Session["rut"].ToString());
                gridViewGrupos.DataBind();
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void gridViewGrupos_RowCreated(object sender, GridViewRowEventArgs e)
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
                gridViewGrupos.EditIndex = e.NewEditIndex;
                CheckBoxActivo.Checked = true;
                int? gpo = null;
                if (!String.IsNullOrEmpty(cbxGrupo.SelectedValue) && cbxGrupo.SelectedIndex != 0)
                {
                    gpo = Int32.Parse(cbxGrupo.SelectedValue);
                }
                gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarSubGrupos(txbNombre.Text, txbCodigo.Text, gpo, CheckBoxActivo.Checked, Session["rut"].ToString());
                gridViewGrupos.DataBind();
                int index = gridViewGrupos.EditIndex;
                GridViewRow row = gridViewGrupos.Rows[index];
                bool activo = ((CheckBox)row.FindControl("ckActivo")).Checked;
                DropDownList ddlGrupo = (row.FindControl("ddlGrupo") as DropDownList);
                LinkButton lnkRemove = (row.FindControl("lnkRemove") as LinkButton);
                lnkRemove.Visible = false;
                if (ddlGrupo != null)
                {
                    ddlGrupo.DataSource = Sistema.GetInstancia().ObtenerGrupos(Session["rut"].ToString());
                    ddlGrupo.DataTextField = "Descripcion";
                    ddlGrupo.DataValueField = "IdGrupo";
                    ddlGrupo.DataBind();

                    Label grupo = row.FindControl("lblGrupo") as Label;
                    ddlGrupo.Items.FindByText(grupo.Text).Selected = true;

                }

                if (!activo)
                {
                    try
                    {
                        gridViewGrupos.EditIndex = -1;
                        int? grupo = null;
                        if (!String.IsNullOrEmpty(cbxGrupo.SelectedValue) && cbxGrupo.SelectedIndex != 0)
                        {
                            grupo = Int32.Parse(cbxGrupo.SelectedValue);
                        }
                        gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarSubGrupos(txbNombre.Text, txbCodigo.Text, grupo, CheckBoxActivo.Checked, Session["rut"].ToString());
                        gridViewGrupos.DataBind();
                    }
                    catch
                    {
                        string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    string script2 = @"<script type='text/javascript'> alert('" + "El subgrupo se encuentra eliminado: No se puede modificar" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script2, false);
                }

            }
            catch
            {
                try
                {
                    gridViewGrupos.EditIndex = -1;
                    int? grupo = null;
                    if (!String.IsNullOrEmpty(cbxGrupo.SelectedValue) && cbxGrupo.SelectedIndex != 0)
                    {
                        grupo = Int32.Parse(cbxGrupo.SelectedValue);
                    }
                    gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarSubGrupos(txbNombre.Text, txbCodigo.Text, grupo, CheckBoxActivo.Checked, Session["rut"].ToString());
                    gridViewGrupos.DataBind();
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
                gridViewGrupos.EditIndex = -1;
                int? grupo = null;
                if (!String.IsNullOrEmpty(cbxGrupo.SelectedValue) && cbxGrupo.SelectedIndex != 0)
                {
                    grupo = Int32.Parse(cbxGrupo.SelectedValue);
                }
                gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarSubGrupos(txbNombre.Text, txbCodigo.Text, grupo, CheckBoxActivo.Checked, Session["rut"].ToString());
                gridViewGrupos.DataBind();
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
                int index = gridViewGrupos.EditIndex;
                GridViewRow row = gridViewGrupos.Rows[index];
                string id = ((Label)row.FindControl("lblIdGrupo")).Text;
                string Nombre = ((TextBox)row.FindControl("txtNombre")).Text;
                string codigo = ((TextBox)row.FindControl("txtCodigo")).Text;
                String grupo = ((DropDownList)row.FindControl("ddlGrupo")).SelectedValue;

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
                Dominio.Clases_Dominio.SubGrupo subgrupo = new Dominio.Clases_Dominio.SubGrupo();
                if (!error)
                {
                    if (String.IsNullOrEmpty(grupo))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un Grupo" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    else
                    {
                        try
                        {
                            int idGrupo = Int32.Parse(grupo);
                            subgrupo.IdGrupo = idGrupo;
                            subgrupo.Grupo = Sistema.GetInstancia().BuscarGrupoId(idGrupo);
                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un grupo" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                }
                if (!error)
                {
                    
                    subgrupo.IdSubGrupo = Int32.Parse(id);
                    subgrupo.Codigo = codigo;
                    subgrupo.Descripcion = Nombre;

                    String msg = Sistema.GetInstancia().ModificarSubGrupo(subgrupo);
                    string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);

                    gridViewGrupos.EditIndex = -1;
                    int? grupo2 = null;
                    if (!String.IsNullOrEmpty(cbxGrupo.SelectedValue) && cbxGrupo.SelectedIndex != 0)
                    {
                        grupo2 = Int32.Parse(cbxGrupo.SelectedValue);
                    }
                    gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarSubGrupos(txbNombre.Text, txbCodigo.Text, grupo2, CheckBoxActivo.Checked, Session["rut"].ToString());
                    gridViewGrupos.DataBind();
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

                int index = gridViewGrupos.EditIndex + 1;
                GridViewRow row = gridViewGrupos.Rows[index];
                bool activo = CheckBoxActivo.Checked;

                if (activo)
                {
                    Dominio.Clases_Dominio.SubGrupo grupo = new Dominio.Clases_Dominio.SubGrupo();
                    grupo.IdSubGrupo = Int32.Parse(id.Trim());
                    String msg = Sistema.GetInstancia().EliminarSubGrupo(grupo);
                    string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                try
                {


                    int? grupo = null;
                    if (!String.IsNullOrEmpty(cbxGrupo.SelectedValue) && cbxGrupo.SelectedIndex != 0)
                    {
                        grupo = Int32.Parse(cbxGrupo.SelectedValue);
                    }
                    gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarSubGrupos(txbNombre.Text, txbCodigo.Text, grupo, CheckBoxActivo.Checked, Session["rut"].ToString());
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
                gridViewGrupos.PageIndex = e.NewPageIndex;
                gridViewGrupos.DataBind();
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