<%@ Page Language="C#" MasterPageFile="~/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="ReporteDocumentosVencidos.aspx.cs" Inherits="InterfazWeb.Reportes.ReporteDocumentosVencidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloGridView.css"/>
    
    <div class="divTitulo">
    <asp:Label ID="lblReportes" runat="server" Text="Listado Documentos Vencidos"  Font-Bold="true"></asp:Label>
    </div>
    <div></div>
    <div>
    <asp:Table ID="Table1" runat="server" Width="742px">     
                
                <asp:TableRow ID="TableRow3" runat="server">
                    <asp:TableCell ID="TableCell7" runat="server" Width="200px">
                        <asp:Label ID="Label2" runat="server" Text="Moneda *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell11" runat="server" Width="350px">
                        <asp:DropDownList ID="ddlMoneda" Width="300px"  runat="server" AutoPostBack="true"></asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell12" runat="server" HorizontalAlign="Left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlMoneda" 
                        ErrorMessage="Debe seleccionar la moneda" ForeColor="Red" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe seleccionar la moneda" ></asp:RequiredFieldValidator>
                           
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
                            <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Fecha">
                                <ItemTemplate>
                                    <asp:Label ID="lblFecha" runat="server"
                                            Text='<%# Eval("Fecha", "{0:d}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Fecha Vencimiento">
                                <ItemTemplate>
                                    <asp:Label ID="lblFechaVencimiento" runat="server"
                                            Text='<%# Eval("FechaVencimiento", "{0:d}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "150px"  HeaderText = "Cliente">
                                <ItemTemplate>
                                    <asp:Label ID="lblCliente" runat="server"
                                            Text='<%# Eval("Cliente")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "80px"  HeaderText = "TipoDocumento">
                                <ItemTemplate>
                                    <asp:Label ID="lblTipoDocumento" runat="server"
                                            Text='<%# Eval("TipoDocumento")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Serie">
                                <ItemTemplate>
                                    <asp:Label ID="lblSerie" runat="server"
                                            Text='<%# Eval("Serie")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Nro. Serie">
                                <ItemTemplate>
                                    <asp:Label ID="lblNroSerie" runat="server"
                                            Text='<%# Eval("Numero")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "80px"  HeaderText = "Moneda">
                                <ItemTemplate>
                                    <asp:Label ID="lblMoneda" runat="server"
                                            Text='<%# Eval("Moneda")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "80px"  HeaderText = "Monto Total">
                                <ItemTemplate>
                                    <asp:Label ID="lblMontoTotal" runat="server"
                                            Text='<%# Eval("MontoTotal")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "80px"  HeaderText = "Monto Pagado">
                                <ItemTemplate>
                                    <asp:Label ID="lblMontoPagado" runat="server"
                                            Text='<%# Eval("MontoPagado")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Saldo">
                                <ItemTemplate>
                                    <asp:Label ID="lblSaldo" runat="server"
                                            Text='<%# Eval("Saldo")%>'></asp:Label>
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
        <div>
         <asp:Table ID="Table5" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="TableRow2" runat="server">
                    <asp:TableCell ID="TableCell9" runat="server" Width="500px" HorizontalAlign="Right">
                        <asp:Label ID="Label1" runat="server" Text="Saldo: " CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell10" runat="server" HorizontalAlign="Right">
                        <asp:TextBox ID="txtSaldoTotal" Width="150px" runat="server" Enabled="false"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell3" runat="server" HorizontalAlign="Left">
                    
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>
     <div>
            <asp:Image ID="Image7" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
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
        
</asp:Content>






