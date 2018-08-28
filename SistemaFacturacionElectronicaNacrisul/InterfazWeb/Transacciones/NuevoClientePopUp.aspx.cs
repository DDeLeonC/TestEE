using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Dominio.Clases_Dominio;

namespace InterfazWeb.Transacciones
{
    public partial class NuevoClientePopUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<String> lista = new List<String>();
                lista.Add("RUT");
                lista.Add("CI");
                lista.Add("OTROS");
                lista.Add("PASAPORTE");
                lista.Add("DNI");
                ddlTipoDocumento.DataSource = lista;
                ddlTipoDocumento.DataBind();

                List<Pais> paises = Sistema.GetInstancia().ObtenerPaises();
                ddlPais.DataSource = paises;
                ddlPais.DataTextField = "Nombre";
                ddlPais.DataValueField = "IdPais";
                ddlPais.DataBind();

                List<Zona> zonas = Sistema.GetInstancia().ObtenerZonas();
                ddlZonas.DataSource = zonas;
                ddlZonas.DataTextField = "Nombre";
                ddlZonas.DataValueField = "IdZona";
                ddlZonas.DataBind();

                List<Vendedor> vendedores = Sistema.GetInstancia().ObtenerVendedores();
                ddlVendedores.DataSource = vendedores;
                ddlVendedores.DataTextField = "Nombre";
                ddlVendedores.DataValueField = "IdVendedor";
                ddlVendedores.DataBind();

                try
                {
                    Pais paiss = paises.FirstOrDefault(med => med.Nombre.ToUpper().Equals("URUGUAY"));
                    ddlPais.SelectedValue = paiss.IdPais.ToString();
                }
                catch { }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Dominio.Clases_Dominio.Cliente cliente = new Dominio.Clases_Dominio.Cliente();
                bool error = false;
                cliente.Nombre = txtNombre.Text;
                cliente.IdPais = Int32.Parse(ddlPais.SelectedValue);
                cliente.IdZona = Int32.Parse(ddlZonas.SelectedValue);
                cliente.IdVendedor = Int32.Parse(ddlVendedores.SelectedValue);
                Dominio.Clases_Dominio.Pais pais = Sistema.GetInstancia().ObtenerPaisId(cliente.IdPais);
                cliente.tipoDocumento = ddlTipoDocumento.SelectedValue;
                cliente.Ciudad = txtCiudad.Text;
                cliente.CodigoPostal = txtCodigoPostal.Text;
                cliente.Direccion = txtDireccion.Text;
                cliente.Mail = txtMail.Text;
                cliente.rut = Session["rut"].ToString();
                if (cliente.tipoDocumento.Equals("RUT"))
                {
                    bool esvalido = RucValido(txtNroDoc.Text);
                    if (esvalido)
                    {
                        cliente.nroDoc = txtNroDoc.Text;
                        if (cliente.Ciudad == null || cliente.Direccion == null || cliente.Ciudad.Equals("") || cliente.Direccion.Equals(""))
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar dirección y ciudad" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
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
                    bool esvalido = CIValida(txtNroDoc.Text);
                    if (esvalido)
                    {
                        if (pais != null && !pais.Codigo.Equals("UY"))
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Error: Según el tipo de documento el país debe ser Uruguay" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                        cliente.nroDoc = txtNroDoc.Text;
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
                    cliente.nroDoc = txtNroDoc.Text;
                    if (pais != null && !pais.Codigo.Equals("AR") && !pais.Codigo.Equals("BR") && !pais.Codigo.Equals("CL") && !pais.Codigo.Equals("PY"))
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Error: Según el tipo de documento el país debe ser Argentina, Brasil, Chile o Paraguay" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                }
                else
                {
                    cliente.nroDoc = txtNroDoc.Text;
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

                if (!error)
                {
                    if (!String.IsNullOrEmpty(txtTel.Text))
                    {
                        try
                        {
                            long tel = long.Parse(txtTel.Text);
                            cliente.Tel = tel;
                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar un entero en el campo Tel" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                }

                if (!error)
                {
                    String msg = Sistema.GetInstancia().GuardarCliente(cliente);
                    string script = @"<script type='text/javascript'> alert('" + msg + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    limpiarFomulario();
                }
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al guardar" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }
        protected void limpiarFomulario()
        {
            ((TextBox)txtNombre).Text = string.Empty;
            ((DropDownList)ddlTipoDocumento).SelectedValue = null;
            ((DropDownList)ddlPais).SelectedValue = null;
            ((DropDownList)ddlVendedores).SelectedValue = null;
            ((DropDownList)ddlZonas).SelectedValue = null;
            ((TextBox)txtCiudad).Text = string.Empty;
            ((TextBox)txtCodigoPostal).Text = string.Empty;
            ((TextBox)txtDireccion).Text = string.Empty;
            ((TextBox)txtMail).Text = string.Empty;
            ((TextBox)txtNombre).Text = string.Empty;
            ((TextBox)txtNroDoc).Text = string.Empty;
            ((TextBox)txtTel).Text = string.Empty;
        }


        private bool RucValido(String ruc)
        {
            bool result = false;
            try
            {
                if (ruc.Length != 12)
                {
                    return false;
                }
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