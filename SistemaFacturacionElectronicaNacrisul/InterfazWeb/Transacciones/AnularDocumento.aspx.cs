using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Dominio.Clases_Dominio;
using InterfazWeb.ServiceReference1Prod;

namespace InterfazWeb.Transacciones
{
    public partial class AnularDocumento : System.Web.UI.Page
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

                ddlCliente.DataSource = Sistema.GetInstancia().ObtenerClientes();
                ddlCliente.DataTextField = "ddlDescription";
                ddlCliente.DataValueField = "IdCliente";
                ddlCliente.DataBind();

                ddlCliente.Items.Insert(0, new ListItem("Todos"));

                List<String> lista3 = new List<String>();
                lista3.Add("Todos");
                lista3.Add("Factura");
                lista3.Add("Nota Crédito");
                lista3.Add("Nota Débito");
                lista3.Add("Remito");
                lista3.Add("Resguardo");

                ddlTipoDocumento.DataSource = lista3;
                ddlTipoDocumento.DataBind();

                txtFechaDesde.Text = DateTime.Now.ToShortDateString();
                txtFechaHasta.Text = DateTime.Now.ToShortDateString();

                int? idCliente = null;
                if (!String.IsNullOrEmpty(ddlCliente.SelectedValue) && ddlCliente.SelectedIndex != 0)
                {
                    idCliente = Int32.Parse(ddlCliente.SelectedValue);
                }
                String tipoDoc = "";
                if (!String.IsNullOrEmpty(ddlTipoDocumento.SelectedValue) && ddlTipoDocumento.SelectedIndex != 0)
                {
                    tipoDoc = ddlTipoDocumento.SelectedValue;
                }
                bool error = false;
                int nro = 0;
                try
                {
                    if (!String.IsNullOrEmpty(txtNroSerie.Text))
                    {
                        nro = Int32.Parse(txtNroSerie.Text);
                    }
                }
                catch
                {
                    error = true;
                    string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un entero en el campo Nro. Serie" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                if (!error)
                {
                    gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerDocumentosAceptados(idCliente, tipoDoc, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), txtSerie.Text, nro, Session["rut"].ToString());
                    gridViewDocumentos.DataBind();
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                int? idCliente = null;
                if (!String.IsNullOrEmpty(ddlCliente.SelectedValue) && ddlCliente.SelectedIndex != 0)
                {
                    idCliente = Int32.Parse(ddlCliente.SelectedValue);
                }
                String tipoDoc = "";
                if (!String.IsNullOrEmpty(ddlTipoDocumento.SelectedValue) && ddlTipoDocumento.SelectedIndex != 0)
                {
                    tipoDoc = ddlTipoDocumento.SelectedValue;
                }
                bool error = false;
                int nro = 0;
                try
                {
                    if (!String.IsNullOrEmpty(txtNroSerie.Text))
                    {
                        nro = Int32.Parse(txtNroSerie.Text);
                    }
                }
                catch
                {
                    error = true;
                    string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un entero en el campo Nro. Serie" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                if (!error)
                {
                    gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerDocumentosAceptados(idCliente, tipoDoc, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), txtSerie.Text, nro, Session["rut"].ToString());
                    gridViewDocumentos.DataBind();
                }
            }
            catch (Exception ex)
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try {
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
                List<Documento> documentos = DocumentosAsociados();
                if (documentos == null || documentos.Count == 0 || documentos.Count > 1)
                {
                    string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un Documento" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                else {
                    Documento documento = documentos.ElementAt(0);
                    var client = new CfeServiceClient();
                    client.ClientCredentials.UserName.UserName = "215521750017";
                    client.ClientCredentials.UserName.Password = "t3D1oo/NRJMSlLVRQB34Dw==";
                    ReqBody solicitud = new ReqBody();
                    solicitud.CodComercio = "UWTEST01";
                    solicitud.CodTerminal = "UWCAJA01";
                    solicitud.HMAC = "";
                    RequerimientoParaUcfe req = new RequerimientoParaUcfe();
                    req.TipoMensaje = 320;
                    // req.Uuid = "10000222";
                    req.TipoCfe = documento.TipoDocumento;
                    req.Serie = documento.Serie;
                    req.NumeroCfe = documento.NroSerie.ToString();
                    req.IdReq = "1";
                    req.FechaReq = documento.Fecha.Year + "" + documento.Fecha.Month + "" + documento.Fecha.Day;
                    req.HoraReq = documento.Fecha.Hour + "" + documento.Fecha.Minute + "" + documento.Fecha.Second;
                    req.CodComercio = "UWTEST01";
                    req.CodTerminal = "UWCAJA01";

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
                                Sistema.GetInstancia().AnularDocumento(documento.IdDocumento);
                                string script = @"<script type='text/javascript'> alert('" + "El documento se anulo correctamente" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                                limpiarSeleccion();
                            }
                            else
                            {
                                string script = @"<script type='text/javascript'> alert('" + respuesta.Resp.MensajeRta + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                        }
                    }
                    else {
                        string script = @"<script type='text/javascript'> alert('" + "Error de conexión con el punto de emisión" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                }
            }
            catch { }
        }

        private void BindData()
        {
            try
            {
                int? idCliente = null;
                if (!String.IsNullOrEmpty(ddlCliente.SelectedValue) && ddlCliente.SelectedIndex != 0)
                {
                    idCliente = Int32.Parse(ddlCliente.SelectedValue);
                }
                String tipoDoc = "";
                if (!String.IsNullOrEmpty(ddlTipoDocumento.SelectedValue) && ddlTipoDocumento.SelectedIndex != 0)
                {
                    tipoDoc = ddlTipoDocumento.SelectedValue;
                }
                bool error = false;
                int nro = 0;
                try
                {
                    if (!String.IsNullOrEmpty(txtNroSerie.Text))
                    {
                        nro = Int32.Parse(txtNroSerie.Text);
                    }
                }
                catch
                {
                    error = true;
                    string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un entero en el campo Nro. Serie" + "');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                }
                if (!error)
                {
                    gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerDocumentosAceptados(idCliente, tipoDoc, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), txtSerie.Text, nro, Session["rut"].ToString());
                    gridViewDocumentos.DataBind();
                    RePopulateCheckBoxes(); 
                }
            }
            catch (Exception ex)
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
            RePopulateCheckBoxes(); 
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
            BindData();
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

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            try
            {

                gridViewDocumentos.PageIndex = e.NewPageIndex;
                BindData();
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al cargar los datos" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
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
                    else
                    {
                        chkBox.Checked = false;
                    }
                }
            }
        }

        private List<Documento> DocumentosAsociados()
        {
            List<Documento> documentos = new List<Documento>();
            gridViewDocumentos.AllowPaging = false;
            BindData();
            foreach (GridViewRow gvr in gridViewDocumentos.Rows)
            {
                try
                {
                    Label idDoc = gvr.FindControl("lblIdDocumento") as Label;
                    CheckBox asociar = gvr.FindControl("ckActivo") as CheckBox;

                    if (asociar.Checked)
                    {
                        Documento doc = Sistema.GetInstancia().ObtenerDocumentoId(Int32.Parse(idDoc.Text));

                        documentos.Add(doc);
                    }
                }
                catch { }
                
            }
            gridViewDocumentos.AllowPaging = true;
            BindData();

            return documentos;
        }

        public void limpiarSeleccion() {
            int? idCliente = null;
            if (!String.IsNullOrEmpty(ddlCliente.SelectedValue) && ddlCliente.SelectedIndex != 0)
            {
                idCliente = Int32.Parse(ddlCliente.SelectedValue);
            }
            String tipoDoc = "";
            if (!String.IsNullOrEmpty(ddlTipoDocumento.SelectedValue) && ddlTipoDocumento.SelectedIndex != 0)
            {
                tipoDoc = ddlTipoDocumento.SelectedValue;
            }
            bool error = false;
            int nro = 0;
            try
            {
                if (!String.IsNullOrEmpty(txtNroSerie.Text))
                {
                    nro = Int32.Parse(txtNroSerie.Text);
                }
            }
            catch
            {
                error = true;
                string script = @"<script type='text/javascript'> alert('" + "Debe ingresar un entero en el campo Nro. Serie" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
            if (!error)
            {
                gridViewDocumentos.DataSource = Sistema.GetInstancia().ObtenerDocumentosAceptados(idCliente, tipoDoc, DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(txtFechaHasta.Text), txtSerie.Text, nro, Session["rut"].ToString());
                gridViewDocumentos.DataBind();
            }
        }
    }
}