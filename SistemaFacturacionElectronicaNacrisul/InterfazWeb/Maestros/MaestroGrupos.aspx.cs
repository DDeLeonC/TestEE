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
    public partial class MaestroGrupos : System.Web.UI.Page
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
                    gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarGrupos(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked, Session["rut"].ToString());
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
                gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarGrupos(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked, Session["rut"].ToString());
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
                    else {
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
                //BindData();
                CheckBoxActivo.Checked = true;
                gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarGrupos(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked, Session["rut"].ToString());
                gridViewGrupos.DataBind();
                int index = gridViewGrupos.EditIndex;
                GridViewRow row = gridViewGrupos.Rows[index];
                bool activo = ((CheckBox)row.FindControl("ckActivo")).Checked;
                LinkButton lnkRemove = (row.FindControl("lnkRemove") as LinkButton);
                lnkRemove.Visible = false;

                if (!activo)
                {
                    try
                    {
                        gridViewGrupos.EditIndex = -1;
                        gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarGrupos(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked, Session["rut"].ToString());
                        gridViewGrupos.DataBind();
                    }
                    catch
                    {
                        string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    string script2 = @"<script type='text/javascript'> alert('" + "El grupo se encuentra eliminado: No se puede modificar" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script2, false);
                }
            }
            catch
            {
                try
                {
                    gridViewGrupos.EditIndex = -1;
                    gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarGrupos(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked, Session["rut"].ToString());
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
                gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarGrupos(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked, Session["rut"].ToString());
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
                //string vidaUtil = ((TextBox)row.FindControl("txtVidaUtil")).Text;
               

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
                //int? vida = null;
                //if (!error)
                //{
                //    try
                //    {
                //        if (!String.IsNullOrEmpty(vidaUtil))
                //        {
                //            vida = Int32.Parse(vidaUtil);
                //        }
                //    }
                //    catch {
                //        error = true;
                //        string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un entero en el campo Vida Útil" + "');</script>";
                //        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                //    }
                        
                    
                //}
                
                if (!error)
                {
                    Dominio.Clases_Dominio.Grupo grupo = new Dominio.Clases_Dominio.Grupo();
                    grupo.IdGrupo = Int32.Parse(id);
                    grupo.Codigo = codigo;
                    grupo.Descripcion = Nombre;
                    String msg = Sistema.GetInstancia().ModificarGrupo(grupo);
                    string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);

                    gridViewGrupos.EditIndex = -1;
                    gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarGrupos(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked, Session["rut"].ToString());
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
                    Dominio.Clases_Dominio.Grupo grupo = new Dominio.Clases_Dominio.Grupo();
                    grupo.IdGrupo = Int32.Parse(id.Trim());
                    String msg = Sistema.GetInstancia().EliminarGrupo(grupo);
                    string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                try
                {


                    gridViewGrupos.DataSource = Sistema.GetInstancia().BuscarGrupos(txbNombre.Text, txbCodigo.Text, CheckBoxActivo.Checked, Session["rut"].ToString());
                        gridViewGrupos.DataBind();
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