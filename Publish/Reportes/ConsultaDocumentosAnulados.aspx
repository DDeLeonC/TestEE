<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Reportes.Master" CodeBehind="ConsultaDocumentosAnulados.aspx.cs" Inherits="InterfazWeb.Reportes.ConsultaDocumentosAnulados" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloGridView.css"/>
    
    <link rel="stylesheet" href="../Styles/chosen.css" />
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
	<script src="../Scripts/chosen.jquery.js" type="text/javascript"></script>
	<script type="text/javascript">
	    $(document).ready(function () {
	        $('#Contenido_ddlCliente').change(function () {
	            $('#Contenido_CargarVendedor').click();
	        });
	        $(".chzn-select").chosen();
	        $(".chzn-select-deselect").chosen({ allow_single_deselect: true })
	        $('#Contenido_ddlCliente_chzn_o_0').hide();

	    });
    </script>

    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
            TargetControlID="txtFechaDesde" PopupButtonID="btnFechaDesde" 
            Format="dd/MM/yyyy" />
    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" 
            TargetControlID="txtFechaHasta" PopupButtonID="btnFechaHasta" 
            Format="dd/MM/yyyy" />
    <div class="divTitulo">
    <asp:Label ID="lblReportes" runat="server" Text="Consulta Documentos Anulados"  Font-Bold="true"></asp:Label>
    </div>
    <div></div>
    <div>
    <asp:Table ID="Table1" runat="server" Width="742px">     
                <asp:TableRow ID="TableRow6" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell8" runat="server" Width="210px">
                            <asp:Label ID="lblFechaDesde" runat="server" Text="Fecha Desde *" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell9" runat="server" Width="350px">
                            <asp:ImageButton ID="btnFechaDesde" CausesValidation="false" runat="server" ImageUrl="~/Imagenes/datetime.png" Height="24px" Width="24px" ImageAlign="Right" />                
                            <asp:TextBox ID="txtFechaDesde" runat="server" Width = "300px"></asp:TextBox> 
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell10" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFechaDesde" 
                           ErrorMessage="Debe ingresar la fecha" ForeColor="Red" ToolTip="Debe ingresar la fecha" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </asp:TableCell>
               
                  </asp:TableRow> 
                  <asp:TableRow ID="TableRow7" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell11" runat="server" Width="210px">
                            <asp:Label ID="Label2" runat="server" Text="Fecha Hasta *" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell12" runat="server" Width="350px">
                            <asp:ImageButton ID="btnFechaHasta" CausesValidation="false" runat="server" ImageUrl="~/Imagenes/datetime.png" Height="24px" Width="24px" ImageAlign="Right" />                
                            <asp:TextBox ID="txtFechaHasta" runat="server" Width = "300px"></asp:TextBox> 
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell13" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFechaHasta" 
                           ErrorMessage="Debe ingresar la fecha" ForeColor="Red" ToolTip="Debe ingresar la fecha" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </asp:TableCell>
               
                  </asp:TableRow>    
                  <asp:TableRow ID="TableRow2" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell3" runat="server" Width="210px">
                            <asp:Label ID="Label1" runat="server" Text="Cliente" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell4" runat="server" Width="350px">
                            
                            <asp:DropDownList ID="ddlCliente" class="chzn-select" Width="300px"  runat="server" ></asp:DropDownList>
                        
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell5" runat="server" HorizontalAlign="Left">
                         
                        </asp:TableCell>
               
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow3" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell7" runat="server" Width="210px">
                            <asp:Label ID="Label3" runat="server" Text="Tipo Documento" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell14" runat="server" Width="350px">
                            <asp:DropDownList ID="ddlTipoDocumento" Width="300px"  runat="server" ></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell16" runat="server" HorizontalAlign="Left">
                        <asp:Label ID="Label7" runat="server" Text="" ></asp:Label>  
                        </asp:TableCell>
               
                  </asp:TableRow>
                  
        </asp:Table>
        </div>
        <div>
            <asp:Image ID="Image1" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
        </div>
        <div >
        <asp:Table ID="Table3" runat="server">
                 <asp:TableRow ID="TableRow1" runat="server" Width="742px">
                    <asp:TableCell ID="TableCell1" runat="server" Width="742px" HorizontalAlign="Center">
                        <asp:Button ID="btnBuscar" runat="server"  CssClass="botonGenerarReporte" Text="Consultar" onclick="btnBuscar_Click"  />              
                    </asp:TableCell>
                    
                </asp:TableRow>
        </asp:Table>
            
        </div>
        
        <div>
            <asp:Image ID="Image2" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
        </div>

         <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:Table ID="Table2" runat="server">
                
                <asp:TableRow ID="TableRow4" runat="server">
                    <asp:TableCell ID="TableCell6" runat="server">
                    
                        <asp:GridView ID="gridViewDocumentos" runat="server"  CssClass="grid-view" AutoGenerateColumns ="false" CellSpacing="0" 
                            CellPadding="0" BorderStyle="None" onrowcreated="gridViewDocumentos_RowCreated" AllowPaging ="true" 
                            PageSize = "15" OnPageIndexChanging = "OnPaging">
                            
                            <Columns>
                            <asp:TemplateField ItemStyle-Width = "25px"  HeaderText = "Id" Visible = "false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdDocumento" runat="server"
                                    Text='<%# Eval("IdDocumento")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "120px"  HeaderText = "Fecha">
                                <ItemTemplate>
                                    <asp:Label ID="lblFecha" runat="server"
                                            Text='<%# Eval("Fecha", "{0:d}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "120px"  HeaderText = "Serie">
                                <ItemTemplate>
                                    <asp:Label ID="lblSerie" runat="server"
                                            Text='<%# Eval("Serie")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "120px"  HeaderText = "Nro. Serie">
                                <ItemTemplate>
                                    <asp:Label ID="lblNroSerie" runat="server"
                                            Text='<%# Eval("NroSerie")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField ItemStyle-Width = "200px"  HeaderText = "Moneda">
                                <ItemTemplate>
                                    <asp:Label ID="lblMoneda" runat="server"
                                            Text='<%# Eval("Moneda")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "150px"  HeaderText = "Tipo Cambio">
                                <ItemTemplate>
                                    <asp:Label ID="lbltipoCambio" runat="server"
                                            Text='<%# Eval("tipoCambio")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "150px"  HeaderText = "Monto Total">
                                <ItemTemplate>
                                    <asp:Label ID="lblmontoTotal" runat="server"
                                            Text='<%# Eval("montoTotal")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Forma Pago">
                                <ItemTemplate>
                                    <asp:Label ID="lblFormaPago" runat="server"
                                            Text='<%# Eval("FormaPago")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Motivo Rechazo">
                                <ItemTemplate>
                                    <asp:Label ID="lblMotivo" runat="server"
                                            Text='<%# Eval("MotivoRechazo")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "60px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRemove" runat="server" 
                                        CommandArgument = '<%# Eval("IdDocumento")%>'                                    
                                    OnClick = "VerPDF"><img src="../Imagenes/pdf.png" alt="delete group" /></asp:LinkButton>
                 
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            </Columns>
                            
                            </asp:GridView>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            </ContentTemplate>
        </asp:UpdatePanel>  
        
        <div>
            <asp:Image ID="Image3" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
        </div>
        <div >
        <asp:Table ID="Table4" runat="server">
                 <asp:TableRow ID="TableRow5" runat="server" Width="742px">
                    <asp:TableCell ID="TableCell2" runat="server" Width="742px" HorizontalAlign="Center">
                        <asp:Button ID="Button1" runat="server"  CssClass="botonGenerarReporte" Text="Exportar a Excel" onclick="exportar_Click"  />              
                    </asp:TableCell>
                    
                </asp:TableRow>
        </asp:Table>
            
        </div>
        
</asp:Content>





