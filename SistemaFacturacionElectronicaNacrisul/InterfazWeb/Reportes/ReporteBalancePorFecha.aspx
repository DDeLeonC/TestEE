<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Reportes.Master" CodeBehind="ReporteBalancePorFecha.aspx.cs" Inherits="InterfazWeb.Reportes.ReporteBalancePorFecha" %>
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
            TargetControlID="txtFecha" PopupButtonID="btnFecha" 
            Format="dd/MM/yyyy" />
    <div class="divTitulo">
    <asp:Label ID="lblReportes" runat="server" Text="Listado de Deudores"  Font-Bold="true"></asp:Label>
    </div>
    <div></div>
    <div>
             
             <asp:Table ID="Table6" runat="server" Width="742px" Height="20px">
                    <asp:TableRow ID="TableRow2" runat="server">
                        <asp:TableCell ID="TableCell3" runat="server" Width="200px">
                            <asp:Label ID="Label2" runat="server" Text="Moneda *" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell7" runat="server" Width="350px">
                            <asp:DropDownList ID="ddlMoneda" Width="300px"  runat="server" AutoPostBack="true"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell8" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlMoneda" 
                           ErrorMessage="Debe seleccionar la moneda" ForeColor="Red" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe seleccionar la moneda" ></asp:RequiredFieldValidator>
                           
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow3" runat="server">
                        <asp:TableCell ID="TableCell4" runat="server" Width="200px">
                            <asp:Label ID="Label1" runat="server" Text="Vendedor" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell5" runat="server" Width="350px">
                            <asp:DropDownList ID="ddlVendedores" Width="300px"  runat="server" AutoPostBack="true"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell9" runat="server" HorizontalAlign="Left">
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow6" runat="server">
                        <asp:TableCell ID="TableCell10" runat="server" Width="200px">
                            <asp:Label ID="Label3" runat="server" Text="Zona" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell11" runat="server" Width="350px">
                            <asp:DropDownList ID="ddlZonas" Width="300px"  runat="server" AutoPostBack="true"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell12" runat="server" HorizontalAlign="Left">
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow8" runat="server">
                        <asp:TableCell ID="TableCell16" runat="server" Width="200px">
                            <asp:Label ID="Label4" runat="server" Text="Cliente" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell17" runat="server" Width="350px">
                            <asp:DropDownList ID="ddlClientes" Width="300px"  runat="server" CssClass="chzn-select" AutoPostBack="true"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell18" runat="server" HorizontalAlign="Left">
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow7" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell13" runat="server" Width="210px">
                            <asp:Label ID="lblFecha" runat="server" Text="Fecha *" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell14" runat="server" Width="350px">
                            <asp:ImageButton ID="btnFecha" CausesValidation="false" runat="server" ImageUrl="~/Imagenes/datetime.png" Height="24px" Width="24px" ImageAlign="Right" />                
                            <asp:TextBox ID="txtFecha" runat="server" Width = "300px"></asp:TextBox> 
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell15" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFecha" 
                           ErrorMessage="Debe ingresar la fecha" ForeColor="Red" ToolTip="Debe ingresar la fecha" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
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
                        <asp:Button ID="btnBuscar" runat="server"  CssClass="botonGenerarReporte" Text="Buscar" onclick="btnBuscar_Click"  />              
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
                            OnPageIndexChanging="OnPaging"  PageSize="15">
                            <Columns>
                            <asp:TemplateField ItemStyle-Width = "300px"  HeaderText = "Cliente">
                                <ItemTemplate>
                                    <asp:Label ID="lblCliente" runat="server"
                                            Text='<%# Eval("Cliente")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "250px"  HeaderText = "Saldo">
                                <ItemTemplate>
                                    <asp:Label ID="lblSaldoTotal" runat="server"
                                            Text='<%# Eval("SaldoTotal")%>'></asp:Label>
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
                    <asp:TableCell ID="TableCell25" runat="server" Width="742px" HorizontalAlign="Center">
                        <asp:Button ID="Button2" runat="server"  CssClass="botonGenerarReporte" Text="Vista de Impresión" onclick="btnExportPdf_Click"  />              
                    </asp:TableCell>
                </asp:TableRow>
        </asp:Table>
            
        </div>
        <div>
            <asp:Image ID="Image4" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
        </div>
        
</asp:Content>