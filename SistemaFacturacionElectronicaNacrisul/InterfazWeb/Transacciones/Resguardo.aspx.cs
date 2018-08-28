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
    public partial class Resguardo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["rut"] == null || String.IsNullOrEmpty(Session["rut"].ToString()))
            {
                Session.Abandon();
                Response.Redirect("~/Logon.aspx");
            }
            if (Session["idLocal"] == null || Session["idUsuario"] == null || String.IsNullOrEmpty(Session["idLocal"].ToString()))
            {
                Session.Abandon();
                Response.Redirect("~/Logon.aspx");
            }
            if (!IsPostBack)
            {
                List<String> lista = new List<String>();
                lista.Add("Pesos");
                lista.Add("Dolar");

                ddlCliente.DataSource = Sistema.GetInstancia().ObtenerClientes();
                ddlCliente.DataTextField = "ddlDescription";
                ddlCliente.DataValueField = "IdCliente";
                ddlCliente.DataBind();

                ddlRetencionPercepcion.DataSource = Sistema.GetInstancia().ObtenerCodigosRetencionPercepcion();
                ddlRetencionPercepcion.DataTextField = "Descripcion";
                ddlRetencionPercepcion.DataValueField = "IdCodigoRetencionPercepcion";
                ddlRetencionPercepcion.DataBind();
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {

            this.txtFecha.Focus();

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
                    doc.TipoDocumento = "182";
                    doc.retenciones = ObtenerRetenciones();

                    if (!error)
                    {
                        XmlDocument xml = null;
                        xml = GenerarXmlResguardo(doc);
                        String xmlTexto = "";
                        xmlTexto = AjustarCFE(xml.InnerXml, "eResg");

                        try
                        {

                            var client = new CfeServiceClient();
                            var client2 = new ConsultaCfeClient();
                            client.ClientCredentials.UserName.UserName = "120185850015";
                            client.ClientCredentials.UserName.Password = Sistema.GetInstancia().Contrasena(Session["rut"].ToString());
                            client2.ClientCredentials.UserName.UserName = "120185850015";
                            client2.ClientCredentials.UserName.Password = Sistema.GetInstancia().Contrasena(Session["rut"].ToString());
                            ReqBody solicitud = new ReqBody();
                            solicitud.CodComercio = Sistema.GetInstancia().CodComercio(Session["idLocal"].ToString());
                            solicitud.CodTerminal = Sistema.GetInstancia().CodTerminal(Session["idLocal"].ToString());
                            solicitud.HMAC = "";
                            RequerimientoParaUcfe req = new RequerimientoParaUcfe();
                            req.TipoMensaje = 310;
                            // req.Uuid = "10000222";
                            req.Uuid = "RES" + Sistema.GetInstancia().ObtenerProximoCodigo(Session["rut"].ToString()).ToString();
                            req.TipoCfe = doc.TipoDocumento;
                            req.IdReq = "1";
                            req.FechaReq = doc.Fecha.Year + "" + doc.Fecha.Month + "" + doc.Fecha.Day;
                            req.HoraReq = doc.Fecha.Hour + "" + doc.Fecha.Minute + "" + doc.Fecha.Second;
                            req.CodComercio = Sistema.GetInstancia().CodComercio(Session["idLocal"].ToString());
                            req.CodTerminal = Sistema.GetInstancia().CodTerminal(Session["idLocal"].ToString());
                            if (!String.IsNullOrEmpty(txtAdenda.Text))
                            {
                                req.Adenda = txtAdenda.Text;
                            }
                            req.CfeXmlOTexto = xmlTexto;
                            if (doc.retenciones != null)
                            {
                                String adenda = "";
                                foreach (RetencionPercepcionResguardos retencion in doc.retenciones)
                                {
                                    adenda += retencion.CodigoPercepcionRetencion.NroForm.ToString() + retencion.CodigoPercepcionRetencion.NroLinea.ToString() + " : " + retencion.CodigoPercepcionRetencion.Descripcion;
                                    adenda += System.Environment.NewLine;
                                }
                                req.Adenda = adenda;
                            }

                            solicitud.Req = req;
                            solicitud.RequestDate = doc.Fecha.Year + "-" + doc.Fecha.Month + "-" + doc.Fecha.Day + "T" + doc.Fecha.Hour + ":" + doc.Fecha.Minute + ":" + doc.Fecha.Second;
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
                                        doc.NroSerie = Int32.Parse(respuesta.Resp.NumeroCfe);
                                        doc.Serie = respuesta.Resp.Serie;
                                        doc.xmlFirmado = respuesta.Resp.XmlCfeFirmado;
                                        doc.EstadoDGI = "Procesado";
                                        String msg = Sistema.GetInstancia().GuardarDocumento(doc);
                                        try
                                        {
                                            byte[] pdf = client2.ObtenerPdf("120185850015", Int32.Parse(respuesta.Resp.TipoCfe), respuesta.Resp.Serie, doc.NroSerie);
                                            // Sistema.GetInstancia().PDFActual = pdf;
                                            Session["pdf"] = pdf;
                                            //if (Sistema.GetInstancia().PDFActual != null)
                                            //{
                                            //    Response.Redirect("VisorPDF.aspx");
                                            //}
                                            if (Session["pdf"] != null)
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
                                        try
                                        {
                                            byte[] pdf = client2.ObtenerPdf("120185850015", Int32.Parse(respuesta.Resp.TipoCfe), respuesta.Resp.Serie, doc.NroSerie);
                                            //Sistema.GetInstancia().PDFActual = pdf;
                                            Session["pdf"] = pdf;
                                            //if (Sistema.GetInstancia().PDFActual != null)
                                            //{
                                            //    Response.Redirect("VisorPDF.aspx");
                                            //}
                                            if (Session["pdf"] != null)
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
                                        string script = @"<script type='text/javascript'> alert('" + "Error en formato CFE" + respuesta.Resp.MensajeRta + "');</script>";
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

        public DataTable ObtenerDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("IdCodigoRetencionPercepcion", typeof(string));
            table.Columns.Add("CodigoPercepcionRetencion", typeof(string));
            table.Columns.Add("tasa", typeof(string));
            table.Columns.Add("monto", typeof(string));
            table.Columns.Add("valor", typeof(string));
            return table;
        }

        private void BindData(String tasa, String monto, String valor, int CantFilas)
        {
            DataTable dt = ObtenerDataTable();
            DataRow dr;
            int cont = 1;
            foreach (GridViewRow gvr in gridViewRetenciones.Rows)
            {
                dr = dt.NewRow();

                Label idCodigo = gvr.FindControl("lblIdCodigoRetencionPercepcion") as Label;
                Label codigo = gvr.FindControl("lblCodigoPercepcionRetencion") as Label;
                Label tas = gvr.FindControl("lblTasa") as Label;
                Label mont = gvr.FindControl("lblMonto") as Label;
                Label val = gvr.FindControl("lblValor") as Label;
                if (idCodigo != null)
                {
                    dr[0] = idCodigo.Text;
                }
                if (codigo != null)
                {
                    dr[1] = codigo.Text;
                }
                if (tas != null)
                {
                    dr[2] = tas.Text;
                }
                if (mont != null)
                {
                    dr[3] = mont.Text;
                }
                if (val != null)
                {
                    dr[4] = val.Text;
                }
                if (cont == CantFilas)
                {
                    dr[2] = tasa;
                    dr[3] = monto;
                    dr[4] = valor;
                }
                dt.Rows.Add(dr);
                cont++;
            }
            gridViewRetenciones.DataSource = dt;
            gridViewRetenciones.DataBind();
        }

        private void BindData()
        {
            DataTable dt = ObtenerDataTable();
            DataRow dr;

            foreach (GridViewRow gvr in gridViewRetenciones.Rows)
            {
                dr = dt.NewRow();

                Label idCodigo = gvr.FindControl("lblIdCodigoRetencionPercepcion") as Label;
                Label codigo = gvr.FindControl("lblCodigoPercepcionRetencion") as Label;
                Label tas = gvr.FindControl("lblTasa") as Label;
                Label mont = gvr.FindControl("lblMonto") as Label;
                Label val = gvr.FindControl("lblValor") as Label;
                if (idCodigo != null)
                {
                    dr[0] = idCodigo.Text;
                }
                if (codigo != null)
                {
                    dr[1] = codigo.Text;
                }
                if (tas != null)
                {
                    dr[2] = tas.Text;
                }
                if (mont != null)
                {
                    dr[3] = mont.Text;
                }
                if (val != null)
                {
                    dr[4] = val.Text;
                }

                dt.Rows.Add(dr);

            }
            List<TablaRetencion> tabla = ConvertirDataTable(dt);
            gridViewRetenciones.DataSource = tabla;
            gridViewRetenciones.DataBind();
        }

        private List<TablaRetencion> ConvertirDataTable(DataTable dt)
        {
            List<TablaRetencion> tabla = new List<TablaRetencion>();
            foreach (DataRow row in dt.Rows)
            {
                TablaRetencion det = new TablaRetencion();
                det.IdCodigo = row[0].ToString();
                det.CodigoRetencionPercepcion = row[1].ToString();
                det.tasa = row[2].ToString();
                det.monto = row[3].ToString();
                det.valor = row[4].ToString();

                tabla.Add(det);
            }
            return tabla;
        }

        private List<RetencionPercepcionResguardos> ObtenerRetenciones()
        {

            List<RetencionPercepcionResguardos> retorno = new List<RetencionPercepcionResguardos>();
            foreach (GridViewRow gvr in gridViewRetenciones.Rows)
            {

                RetencionPercepcionResguardos detalle = new RetencionPercepcionResguardos();
                Label id = gvr.FindControl("lblIdCodigoRetencionPercepcion") as Label;
                Label tasa = gvr.FindControl("lblTasa") as Label;
                Label monto = gvr.FindControl("lblMonto") as Label;
                Label valor = gvr.FindControl("lblValor") as Label;

                if (id != null && !string.IsNullOrEmpty(id.Text))
                {
                    int idCodigo = Int32.Parse(id.Text);
                    CodigoRetencionPercepcion codigoRet = Sistema.GetInstancia().BuscarCodigoRetencionPercepcionId(idCodigo);
                    detalle.CodigoPercepcionRetencion = codigoRet;
                    detalle.IdCodigoPercepcionRetencion = idCodigo;
                }


                if (tasa != null)
                {
                    if (!String.IsNullOrEmpty(tasa.Text))
                    {
                        detalle.tasa = decimal.Parse(tasa.Text);
                    }
                }
                if (monto != null)
                {
                    if (!String.IsNullOrEmpty(monto.Text))
                    {
                        detalle.monto = decimal.Parse(monto.Text);
                    }
                }
                if (valor != null)
                {
                    if (!String.IsNullOrEmpty(valor.Text))
                    {
                        detalle.valor = decimal.Parse(valor.Text);
                    }
                }
                retorno.Add(detalle);

            }
            return retorno;
        }

        private void limpiarFormulario()
        {
            Sistema.GetInstancia().SubTotal = 0;
            Sistema.GetInstancia().Total = 0;
            Sistema.GetInstancia().Impuestos = 0;

            //Sistema.GetInstancia().PDFActual = null;
            Session.Contents.Remove("pdf");
        }

        private XmlDocument GenerarXmlResguardo(Documento doc)
        {
            XmlDocument xml = new XmlDocument();
            String result = "";

            CFEDefTypeEResg factura = new CFEDefTypeEResg();

            //Encabezado

            CFEDefTypeEResgEncabezado encabezado = new CFEDefTypeEResgEncabezado();

            // IdDoc
            try
            {
                IdDoc_Resg idDoc = new IdDoc_Resg();
                idDoc.FchEmis = doc.Fecha;

                idDoc.TipoCFE = IdDoc_ResgTipoCFE.Item182;

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

                Receptor_Resg receptor = new Receptor_Resg();
                Cliente cli = Sistema.GetInstancia().BuscarClienteId(doc.IdCliente);

                receptor.Item = cli.nroDoc.ToString();
                if (cli.Pais.Codigo.Equals("UY"))
                {
                    receptor.ItemElementName = ItemChoiceType4.DocRecep;
                    receptor.CodPaisRecep = CodPaisType.UY;
                }
                else
                {
                    receptor.ItemElementName = ItemChoiceType4.DocRecepExt;
                    receptor.CodPaisRecep = (CodPaisType)Enum.Parse(typeof(CodPaisType), cli.Pais.Codigo, true);
                }
                receptor.CodPaisRecepSpecified = true;
                receptor.TipoDocRecepSpecified = true;
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
                receptor.RznSocRecep = cli.Nombre;
                receptor.DirRecep = cli.Direccion;
                receptor.Item = cli.nroDoc.ToString();
                encabezado.Receptor = receptor;

            }
            catch (Exception e)
            {

            }

            //Detalle
            decimal MontoTotalRetenido = 0;
            decimal MontoTotalCreditoFiscal = 0;
            try
            {
                Item_Resg[] detalle = new Item_Resg[1];
                int cont = 0;


                Item_Resg item = new Item_Resg();
                item.NroLinDet = "1";
                RetPerc_Resg[] retenciones = new RetPerc_Resg[doc.retenciones.Count];
                foreach (RetencionPercepcionResguardos ret in doc.retenciones)
                {
                    RetPerc_Resg retencion = new RetPerc_Resg();
                    retencion.CodRet = ret.CodigoPercepcionRetencion.NroForm.ToString() + ret.CodigoPercepcionRetencion.NroLinea.ToString();
                    retencion.MntSujetoaRet = ret.monto;
                    if (ret.tasa > 0)
                    {
                        retencion.Tasa = ret.tasa;
                        retencion.TasaSpecified = true;
                    }
                    else
                    {
                        retencion.TasaSpecified = false;
                    }
                    retencion.ValRetPerc = ret.valor;

                    retenciones[cont] = retencion;
                    if (ret.CodigoPercepcionRetencion.NroForm == 2181)
                    {
                        MontoTotalCreditoFiscal += ret.valor;
                    }
                    else
                    {
                        MontoTotalRetenido += ret.valor;
                    }
                    cont++;
                }
                item.RetencPercep = retenciones;
                detalle[0] = item;
                factura.Detalle = detalle;

            }
            catch (Exception e)
            {

            }

            // Totales

            try
            {

                Totales_Resg totales = new Totales_Resg();


                totales.TpoMoneda = TipMonType.UYU;
                totales.CantLinDet = "1";
                totales.TpoCambioSpecified = false;


                Totales_ResgRetencPercep[] retenciones = new Totales_ResgRetencPercep[doc.retenciones.Count];
                int cont = 0;
                foreach (RetencionPercepcionResguardos ret in doc.retenciones)
                {
                    Totales_ResgRetencPercep retencion = new Totales_ResgRetencPercep();
                    retencion.CodRet = ret.CodigoPercepcionRetencion.NroForm.ToString() + ret.CodigoPercepcionRetencion.NroLinea.ToString();
                    retencion.ValRetPerc = ret.valor;

                    retenciones[cont] = retencion;
                    cont++;
                }
                totales.RetencPercep = retenciones;
                totales.MntTotCredFisc = Math.Round(MontoTotalCreditoFiscal, 3);
                totales.MntTotRetenido = Math.Round(MontoTotalRetenido, 3);

                encabezado.Totales = totales;

            }
            catch (Exception ex)
            {

            }

            factura.Encabezado = encabezado;


            XmlSerializer ser = new XmlSerializer(typeof(CFEDefTypeEResg));
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
                    if (xmlCortado[i].Contains("CFEDefTypeEFact") || xmlCortado[i].Contains("CFEDefTypeETck") || xmlCortado[i].Contains("CFEDefTypeEResg"))
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

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ddlRetencionPercepcion.SelectedValue != null && !ddlRetencionPercepcion.SelectedValue.Equals(""))
            {
                try
                {
                    int idCodigo = Int32.Parse(ddlRetencionPercepcion.SelectedValue);
                    CodigoRetencionPercepcion codigo = Sistema.GetInstancia().BuscarCodigoRetencionPercepcionId(idCodigo);
                    if (codigo != null)
                    {
                        LineaRetencion linea = new LineaRetencion();
                        List<LineaRetencion> lineas = new List<LineaRetencion>();
                        DataTable dt = ObtenerDataTable();
                        DataRow dr;
                        int cont = -1;
                        linea.CodigoRetencion = codigo.Descripcion;
                        linea.IdCodigoRetencion = codigo.IdCodigoRetencionPercepcion;


                        foreach (GridViewRow gvr in gridViewRetenciones.Rows)
                        {
                            dr = dt.NewRow();

                            Label idCod = gvr.FindControl("lblIdCodigoRetencionPercepcion") as Label;
                            Label cod = gvr.FindControl("lblCodigoPercepcionRetencion") as Label;
                            Label tasa = gvr.FindControl("lblTasa") as Label;
                            Label monto = gvr.FindControl("lblMonto") as Label;
                            Label valor = gvr.FindControl("lblValor") as Label;

                            if (idCod != null)
                            {
                                dr[0] = idCod.Text;
                            }
                            if (cod != null)
                            {
                                dr[1] = cod.Text;
                            }
                            if (tasa != null)
                            {
                                dr[2] = tasa.Text;
                            }
                            if (monto != null)
                            {
                                dr[3] = monto.Text;
                            }
                            if (valor != null)
                            {
                                dr[4] = valor.Text;
                            }

                            dt.Rows.Add(dr);
                            cont++;
                            LineaRetencion nueva = new LineaRetencion();
                            if (!String.IsNullOrEmpty(tasa.Text))
                            {
                                nueva.tasa = decimal.Parse(tasa.Text);
                            }
                            if (!String.IsNullOrEmpty(monto.Text))
                            {
                                nueva.monto = decimal.Parse(monto.Text);
                            }
                            if (!String.IsNullOrEmpty(valor.Text))
                            {
                                nueva.valor = Int32.Parse(valor.Text);
                            }

                            nueva.IdCodigoRetencion = Int32.Parse(idCod.Text);
                            nueva.CodigoRetencion = cod.Text;

                            lineas.Add(nueva);
                        }
                        if (lineas.Contains(linea))
                        {
                            string script = @"<script type='text/javascript'> alert('" + "El código se encuentra seleccionado" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                        else
                        {
                            dr = dt.NewRow();
                            dr[0] = codigo.IdCodigoRetencionPercepcion;
                            dr[1] = codigo.Descripcion;

                            dt.Rows.Add(dr);
                            cont++;
                            gridViewRetenciones.EditIndex = cont;
                            gridViewRetenciones.DataSource = dt;
                            gridViewRetenciones.DataBind();
                        }
                    }
                }
                catch { }
            }
            else
            {
                string script = @"<script type='text/javascript'> alert('" + "Debe seleccionar un código" + "');</script>";
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

                foreach (GridViewRow gvr in gridViewRetenciones.Rows)
                {
                    dr = dt.NewRow();

                    Label idCod = gvr.FindControl("lblIdCodigoRetencionPercepcion") as Label;
                    Label cod = gvr.FindControl("lblCodigoPercepcionRetencion") as Label;
                    Label tasa = gvr.FindControl("lblTasa") as Label;
                    Label monto = gvr.FindControl("lblMonto") as Label;
                    Label valor = gvr.FindControl("lblValor") as Label;

                    if (idCod != null)
                    {
                        dr[0] = idCod.Text;
                    }
                    if (cod != null)
                    {
                        dr[1] = cod.Text;
                    }
                    if (tasa != null)
                    {
                        dr[2] = tasa.Text;
                    }
                    if (monto != null)
                    {
                        dr[3] = monto.Text;
                    }
                    if (valor != null)
                    {
                        dr[4] = valor.Text;
                    }
                    int ide = 0;
                    try
                    {
                        ide = Int32.Parse(idCod.Text);

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

                        }
                        catch { }
                    }
                }
                gridViewRetenciones.DataSource = dt;
                gridViewRetenciones.DataBind();


            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al quitar" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
        }

        protected void Editar(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gridViewRetenciones.EditIndex = e.NewEditIndex;

            }
            catch
            {
                try
                {
                    gridViewRetenciones.EditIndex = -1;

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
                int index = gridViewRetenciones.EditIndex;
                GridViewRow row = gridViewRetenciones.Rows[index];
                string id = ((Label)row.FindControl("lblIdCodigoRetencionPercepcion")).Text;
                string tas = ((TextBox)row.FindControl("txtTasa")).Text;
                string mon = ((TextBox)row.FindControl("txtMonto")).Text;
                string val = ((TextBox)row.FindControl("txtValor")).Text;

                decimal tasa = 0;
                decimal monto = 0;
                decimal valor = 0;

                bool error = false;
                if (!error)
                {
                    if (!String.IsNullOrEmpty(tas))
                    {
                        try
                        {
                            tasa = decimal.Parse(tas);
                            if (tasa <= 0)
                            {
                                error = true;
                                string script = @"<script type='text/javascript'> alert('" + "Error: La tasa debe ser mayor a 0" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar un decimal en tasa" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }
                    else
                    {
                        error = true;
                        string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar la tasa" + "');</script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    }
                }
                if (!error)
                {
                    if (!String.IsNullOrEmpty(mon))
                    {
                        try
                        {
                            monto = decimal.Parse(mon);
                            if (monto <= 0)
                            {
                                error = true;
                                string script = @"<script type='text/javascript'> alert('" + "Error: El monto debe ser mayor a 0" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar un decimal en monto" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }

                }
                if (!error)
                {
                    if (!String.IsNullOrEmpty(val))
                    {
                        try
                        {
                            valor = decimal.Parse(val);
                            if (valor <= 0)
                            {
                                error = true;
                                string script = @"<script type='text/javascript'> alert('" + "Error: El valor debe ser mayor a 0" + "');</script>";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                            }
                        }
                        catch
                        {
                            error = true;
                            string script = @"<script type='text/javascript'> alert('" + "Error: Debe ingresar un decimal en valor" + "');</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                        }
                    }

                }

                if (!error)
                {
                    try
                    {

                        gridViewRetenciones.EditIndex = -1;
                        int cantFilas = index + 1;
                        BindData(tasa.ToString(), monto.ToString(), valor.ToString(), cantFilas);
                    }
                    catch { }


                }
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('" + "Error al modificar" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }

        }
    }
}