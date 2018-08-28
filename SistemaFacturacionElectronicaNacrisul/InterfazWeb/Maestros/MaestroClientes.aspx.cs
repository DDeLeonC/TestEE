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
    public partial class MaestroClientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
            if (!IsPostBack)
            {
                List<Vendedor> vendedores = Sistema.GetInstancia().ObtenerVendedores();
                ddlVendedores.DataSource = vendedores;
                ddlVendedores.DataTextField = "Nombre";
                ddlVendedores.DataValueField = "IdVendedor";
                ddlVendedores.DataBind();

                ddlVendedores.Items.Insert(0, new ListItem("Todos"));
                int? idVendedor = null;
                if (!String.IsNullOrEmpty(ddlVendedores.SelectedValue) && ddlVendedores.SelectedIndex != 0)
                {
                    idVendedor = Int32.Parse(ddlVendedores.SelectedValue);
                }
            }
        }


        private void BindData()
        {
            try
            {
                if (!IsPostBack)
                {
                    CheckBoxActivo.Checked = true;
                    int? idVendedor = null;
                    gridViewClientes.DataSource = Sistema.GetInstancia().BuscarClientes(txbNombre.Text, CheckBoxActivo.Checked, Session["rut"].ToString(),idVendedor);
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                int? idVendedor = null;
                if (!String.IsNullOrEmpty(ddlVendedores.SelectedValue) && ddlVendedores.SelectedIndex != 0)
                {
                    idVendedor = Int32.Parse(ddlVendedores.SelectedValue);
                }
                gridViewClientes.DataSource = Sistema.GetInstancia().BuscarClientes(txbNombre.Text, CheckBoxActivo.Checked, Session["rut"].ToString(), idVendedor);
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
                bool aux = IsPostBack;
                gridViewClientes.EditIndex = e.NewEditIndex;
                CheckBoxActivo.Checked = true;
                int? idVendedor = null;
                gridViewClientes.DataSource = Sistema.GetInstancia().BuscarClientes(txbNombre.Text, CheckBoxActivo.Checked, Session["rut"].ToString(), idVendedor);
                gridViewClientes.DataBind();
                int index = gridViewClientes.EditIndex;
                GridViewRow row = gridViewClientes.Rows[index];
                bool activo = ((CheckBox)row.FindControl("ckActivo")).Checked;
                DropDownList ddlTipo = (row.FindControl("ddltipoDocumento") as DropDownList);
                DropDownList ddlPais = (row.FindControl("ddlPais") as DropDownList);
                DropDownList ddlZonas = (row.FindControl("ddlZonas") as DropDownList);
                DropDownList ddlVendedores = (row.FindControl("ddlVendedores") as DropDownList);
                LinkButton lnkRemove = (row.FindControl("lnkRemove") as LinkButton);
                lnkRemove.Visible = false;
                if (ddlTipo != null)
                {
                    List<String> lista = new List<String>();
                    lista.Add("RUT");
                    lista.Add("CI");
                    lista.Add("OTROS");
                    lista.Add("PASAPORTE");
                    lista.Add("DNI");
                    ddlTipo.DataSource = lista;
                    ddlTipo.DataBind();

                    Label tipo = row.FindControl("lbltipoDocumento") as Label;
                    ddlTipo.Items.FindByText(tipo.Text).Selected = true;

                }
                if (ddlPais != null)
                {
                    ddlPais.DataSource = Sistema.GetInstancia().ObtenerPaises();
                    ddlPais.DataTextField = "Nombre";
                    ddlPais.DataValueField = "IdPais";
                    ddlPais.DataBind();

                    Label zona = row.FindControl("lblPais") as Label;
                    ddlPais.Items.FindByText(zona.Text).Selected = true;

                }

                if (ddlZonas != null)
                {
                    ddlZonas.DataSource = Sistema.GetInstancia().ObtenerZonas();
                    ddlZonas.DataTextField = "Nombre";
                    ddlZonas.DataValueField = "IdZona";
                    ddlZonas.DataBind();

                    Label zona = row.FindControl("lblZona") as Label;
                    ddlZonas.Items.FindByText(zona.Text).Selected = true;

                }
                if (ddlVendedores != null)
                {
                    ddlVendedores.DataSource = Sistema.GetInstancia().ObtenerVendedores();
                    ddlVendedores.DataTextField = "Nombre";
                    ddlVendedores.DataValueField = "IdVendedor";
                    ddlVendedores.DataBind();

                    Label vendedor = row.FindControl("lblVendedor") as Label;
                    ddlVendedores.Items.FindByText(vendedor.Text).Selected = true;

                }

                if (!activo)
                {
                    try
                    {
                        gridViewClientes.DataSource = Sistema.GetInstancia().BuscarClientes(txbNombre.Text, CheckBoxActivo.Checked, Session["rut"].ToString(), idVendedor);
                        gridViewClientes.DataBind();
                    }
                    catch
                    {
                        string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    string script2 = @"<script type='text/javascript'> alert('" + "El cliente se encuentra eliminado: No se puede modificar" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script2, false);
                }

            }
            catch
            {
                try
                {
                    int? idVendedor = null;
                    gridViewClientes.DataSource = Sistema.GetInstancia().BuscarClientes(txbNombre.Text, CheckBoxActivo.Checked, Session["rut"].ToString(), idVendedor);
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
                int? idVendedor = null;
                gridViewClientes.DataSource = Sistema.GetInstancia().BuscarClientes(txbNombre.Text, CheckBoxActivo.Checked, Session["rut"].ToString(), idVendedor);
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
                string id = ((Label)row.FindControl("lblIdCliente")).Text;
                string Nombre = ((TextBox)row.FindControl("txtNombre")).Text;
                String tipoDoc = ((DropDownList)row.FindControl("ddltipoDocumento")).SelectedValue;
                string nroDoc = ((TextBox)row.FindControl("txtnroDoc")).Text;
                string dir = ((TextBox)row.FindControl("txtDireccion")).Text;
                string tel = ((TextBox)row.FindControl("txtTel")).Text;
                string ciud = ((TextBox)row.FindControl("txtCiudad")).Text;
                String pai = ((DropDownList)row.FindControl("ddlPais")).SelectedValue;
                string codPostal = ((TextBox)row.FindControl("txtCodigoPostal")).Text;
                String mail = ((TextBox)row.FindControl("txtMail")).Text;
                String zo = ((DropDownList)row.FindControl("ddlZonas")).SelectedValue;
                String vend = ((DropDownList)row.FindControl("ddlVendedores")).SelectedValue;
                bool error = false;


                if (!error)
                {
                    if (String.IsNullOrEmpty(Nombre))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un nombre" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                }
                Dominio.Clases_Dominio.Cliente cliente = new Dominio.Clases_Dominio.Cliente();
                if (!error)
                {
                    if (String.IsNullOrEmpty(pai))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un pais de origen" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    else
                    {
                        try
                        {
                            int idPais = Int32.Parse(pai);
                            cliente.IdPais = idPais;

                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un pais" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                }
                if (!error)
                {
                    if (String.IsNullOrEmpty(zo))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar una zona" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    else
                    {
                        try
                        {
                            int idZona = Int32.Parse(zo);
                            cliente.IdZona = idZona;

                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar una zona" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                }
                if (!error)
                {
                    if (String.IsNullOrEmpty(vend))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un vendedor" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    else
                    {
                        try
                        {
                            int idVendedor = Int32.Parse(vend);
                            cliente.IdVendedor = idVendedor;

                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un vendedor" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                }
                if (!error)
                {
                    if (String.IsNullOrEmpty(tipoDoc))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un tipo de documento" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    else
                    {
                        cliente.tipoDocumento = tipoDoc;
                    }
                }
                Dominio.Clases_Dominio.Pais pais = Sistema.GetInstancia().ObtenerPaisId(cliente.IdPais);
                if (!error)
                {
                    if (String.IsNullOrEmpty(nroDoc))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un nro. de documento" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                    else {
                        
                        try {
                            
                            if (cliente.tipoDocumento.Equals("RUT"))
                            {
                                bool esvalido = RucValido(nroDoc);
                                if (esvalido)
                                {
                                    cliente.nroDoc = nroDoc;
                                    if (pais != null && !pais.Codigo.Equals("UY"))
                                    {
                                        error = true;
                                        string script = @"<script type='text/javascript'> alert('" + "Error: Según el tipo de documento el país debe ser Uruguay" + "');</script>";
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                    }
                                }
                                else
                                {
                                    error = true;
                                    string script = @"<script type='text/javascript'> alert('" + "Error: RUT Inválido" + "');</script>";
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                }
                            }
                            else if (cliente.tipoDocumento.Equals("CI"))
                            {
                                bool esvalido = CIValida(nroDoc);
                                if (esvalido)
                                {
                                    if (pais != null && !pais.Codigo.Equals("UY"))
                                    {
                                        error = true;
                                        string script = @"<script type='text/javascript'> alert('" + "Error: Según el tipo de documento el país debe ser Uruguay" + "');</script>";
                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                    }
                                    cliente.nroDoc = nroDoc;
                                }
                                else
                                {
                                    error = true;
                                    string script = @"<script type='text/javascript'> alert('" + "Error: CI Inválida" + "');</script>";
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                }
                            }
                            else if (cliente.tipoDocumento.Equals("DNI"))
                            {
                                cliente.nroDoc = nroDoc;
                                if (pais != null && !pais.Codigo.Equals("AR") && !pais.Codigo.Equals("BR") && !pais.Codigo.Equals("CL") && !pais.Codigo.Equals("PY"))
                                {
                                    error = true;
                                    string script = @"<script type='text/javascript'> alert('" + "Error: Según el tipo de documento el país debe ser Argentina, Brasil, Chile o Paraguay" + "');</script>";
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                }
                            }
                            else
                            {
                                cliente.nroDoc = nroDoc;
                            };
                        }
                        catch {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un nro. de documento" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                }

                if (!error)
                {
                    if (pais != null && pais.Codigo.Equals("UY") && (!cliente.tipoDocumento.Equals("CI") && !cliente.tipoDocumento.Equals("RUT")))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Error: El tipo de documento debe ser CI o RUT" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                }
                if (!error) {
                    if (!String.IsNullOrEmpty(tel))
                    {
                        try
                        {
                            long tele = long.Parse(tel);
                            cliente.Tel = tele;
                        }
                        catch {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar un entero en el campo Tel" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                }


                if (!error)
                {


                    cliente.IdCliente = Int32.Parse(id);
                    cliente.Ciudad = ciud;
                    cliente.CodigoPostal = codPostal;
                    cliente.Direccion = dir;
                    cliente.Nombre = Nombre;
                    cliente.Mail = mail;
                    String msg = Sistema.GetInstancia().ModificarCliente(cliente);
                    string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);

                    gridViewClientes.EditIndex = -1;
                    int? idVendedor = null;
                    gridViewClientes.DataSource = Sistema.GetInstancia().BuscarClientes(txbNombre.Text, CheckBoxActivo.Checked, Session["rut"].ToString(), idVendedor);
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
                    Dominio.Clases_Dominio.Cliente cliente = new Dominio.Clases_Dominio.Cliente();
                    cliente.IdCliente = Int32.Parse(id.Trim());
                    String msg = Sistema.GetInstancia().EliminarCliente(cliente);
                    string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                try
                {
                    int? idVendedor = null;
                    gridViewClientes.DataSource = Sistema.GetInstancia().BuscarClientes(txbNombre.Text, CheckBoxActivo.Checked, Session["rut"].ToString(), idVendedor);
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