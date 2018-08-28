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
    public partial class ActualizarEstados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["rut"] == null || String.IsNullOrEmpty(Session["rut"].ToString()))
            {
                Session.Abandon();
                Response.Redirect("~/Logon.aspx");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e) {
            try
            {
                List<Documento> documentos = Sistema.GetInstancia().ObtenerDocumentosAceptados(Session["rut"].ToString());
                if (documentos != null)
                {
                    var client = new CfeServiceClient();
                    client.ClientCredentials.UserName.UserName = "215521750017";
                    client.ClientCredentials.UserName.Password = "t3D1oo/NRJMSlLVRQB34Dw==";
                    foreach (Documento documento in documentos)
                    {
                        
                        ReqBody solicitud = new ReqBody();
                        solicitud.CodComercio = "UWTEST01";
                        solicitud.CodTerminal = "UWCAJA01";
                        solicitud.HMAC = "";
                        RequerimientoParaUcfe req = new RequerimientoParaUcfe();
                        req.TipoMensaje = 360;
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
                                    Sistema.GetInstancia().ModificarEstado(documento.IdDocumento, "Procesado", null);
                                    
                                }
                                else if (respuesta.Resp.CodRta.Equals("11"))
                                {
                                    
                                }
                                else if (respuesta.Resp.CodRta.Equals("01") || respuesta.Resp.CodRta.Equals("05"))
                                {
                                    Sistema.GetInstancia().ModificarEstado(documento.IdDocumento, "Anulado",respuesta.Resp.MensajeRta);
                                   
                                }
                                else if (respuesta.Resp.CodRta.Equals("03"))
                                {
                                    string script = @"<script type='text/javascript'> alert('" + "Comercio invalido" + "');</script>";
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
                }
                string script2 = @"<script type='text/javascript'> alert('" + "Los estados se actualizaron correctamente" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script2, false);
            }
            catch {
                string script3 = @"<script type='text/javascript'> alert('" + "Error al actualizar los estados" + "');</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script3, false);
            }
        }
    }
}