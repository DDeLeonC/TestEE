<%@ Page Language="C#" MasterPageFile="~/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="InformePorcentajesMercaderia.aspx.cs" Inherits="InterfazWeb.Reportes.InformePorcentajesMercaderia" %>
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
	        $(".chzn-select").chosen();
	        $(".chzn-select-deselect").chosen({ allow_single_deselect: true })
	        $('.chzn-single span').last().text("Buscar Producto...");
	        $(".chzn-select").last().focus(function () {
	            $("#Contenido_ddlProductos").blur();
	            $('.chzn-search input').last().focus()
	            $('.chzn-single span').last().text("Buscar Producto...");
	        });
	        $(".chzn-select").last().val("nil");
	    });
    </script>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
            TargetControlID="txtFechaDesde" PopupButtonID="btnFechaDesde" 
            Format="dd/MM/yyyy" />
    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" 
            TargetControlID="txtFechaHasta" PopupButtonID="btnFechaHasta" 
            Format="dd/MM/yyyy" />
    <div class="divTitulo">
        <asp:Label ID="lblReportes" runat="server" Text="Listado por Mercaderia"  Font-Bold="true"></asp:Label>
    </div>
    <div></div>
    <div>
        <asp:Table ID="Table6" runat="server" Width="742px" Height="20px">
            <asp:TableRow ID="TableRow6" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell4" runat="server" Width="210px">
                            <asp:Label ID="lblFechaDesde" runat="server" Text="Fecha Desde *" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell9" runat="server" Width="350px">
                            <asp:ImageButton ID="btnFechaDesde" CausesValidation="false" runat="server" ImageUrl="~/Imagenes/datetime.png" Height="24px" Width="24px" ImageAlign="Right" />                
                            <asp:TextBox ID="txtFechaDesde" runat="server" Width = "300px"></asp:TextBox> 
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell10" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtFechaDesde" 
                           ErrorMessage="Debe ingresar la fecha" ForeColor="Red" ToolTip="Debe ingresar la fecha" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </asp:TableCell>
               
                  </asp:TableRow> 
                  <asp:TableRow ID="TableRow7" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell11" runat="server" Width="210px">
                            <asp:Label ID="Label1" runat="server" Text="Fecha Hasta *" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell12" runat="server" Width="350px">
                            <asp:ImageButton ID="btnFechaHasta" CausesValidation="false" runat="server" ImageUrl="~/Imagenes/datetime.png" Height="24px" Width="24px" ImageAlign="Right" />                
                            <asp:TextBox ID="txtFechaHasta" runat="server" Width = "300px"></asp:TextBox> 
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell13" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtFechaHasta" 
                           ErrorMessage="Debe ingresar la fecha" ForeColor="Red" ToolTip="Debe ingresar la fecha" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </asp:TableCell>
               
                  </asp:TableRow>   
               

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
                <asp:TableCell ID="TableCell5" runat="server" Width="200px">
                    <asp:Label ID="Label3" runat="server" Text="Vendedor" CssClass="bodyText"></asp:Label>                
                </asp:TableCell>
                <asp:TableCell ID="TableCell14" runat="server" Width="350px">
                    <asp:DropDownList ID="ddlVendedores" Width="300px"  runat="server" AutoPostBack="true"></asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell ID="TableCell15" runat="server" HorizontalAlign="Left">
                           
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow8" runat="server">
                <asp:TableCell ID="TableCell16" runat="server" Width="200px">
                    <asp:Label ID="Label4" runat="server" Text="Producto" CssClass="bodyText"></asp:Label>                
                </asp:TableCell>
                <asp:TableCell ID="TableCell17" runat="server" Width="350px">
                    <asp:DropDownList ID="ddlProductos" Width="300px" class="chzn-select" runat="server"></asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell ID="TableCell18" runat="server" HorizontalAlign="Left">
                           
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
                                <asp:TemplateField ItemStyle-Width = "200px"  HeaderText = "Codigo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCodigo" runat="server"
                                                Text='<%# Eval("CodigoProducto")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width = "200px"  HeaderText = "Cliente">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNombre" runat="server"
                                                Text='<%# Eval("Nombre")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            
                                <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Cantidad">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCantidad" runat="server"
                                                Text='<%# Eval("Cantidad")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Kilos">
                                    <ItemTemplate>
                                        <asp:Label ID="lblKilos" runat="server"
                                                Text='<%# Eval("Kilos")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Total Acumulado">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotal" runat="server"
                                                Text='<%# Eval("Total")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "% sobre Total">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPorcentaje" runat="server"
                                                Text='<%# Eval("Porcentaje")%>'></asp:Label>
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

