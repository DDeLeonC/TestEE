<%@ Page Language="C#" MasterPageFile="~/MasterPages/Maestros.Master"  AutoEventWireup="true" CodeBehind="MaestroClientes.aspx.cs" Inherits="InterfazWeb.Maestros.MaestroClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloGridView.css"/>
    <div class="divTitulo">
    <asp:Label ID="lblReportes" runat="server" Text="Clientes"  Font-Bold="true"></asp:Label>
    </div>
    <div></div>
    <div>
    <asp:Table ID="Table1" runat="server">
                 <asp:TableRow ID="Fila1" runat="server">
                <asp:TableCell ID="Celda11" runat="server">
                    <asp:Label ID="lblNombre" runat="server"   Text="Nombre" CssClass="bodyText"></asp:Label>                
                </asp:TableCell>
                <asp:TableCell ID="Celda12" runat="server">
                    <asp:TextBox ID="txbNombre"  Width="300px" runat="server" CssClass="bodyText"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            
            <asp:TableRow ID="Fila4" runat="server">
                <asp:TableCell ID="Celda41" runat="server">
                    <asp:Label ID="lblActivo" runat="server" Text="Activo" CssClass="bodyText"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="Celda42" runat="server">
                    <asp:CheckBox ID="CheckBoxActivo" runat="server"  CssClass="bodyText"/>
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
            
        </asp:Table>
        </div>
        <div>
            <asp:Image ID="Image1" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
        </div>
        <div >
        <asp:Table ID="Table3" runat="server">
                 <asp:TableRow ID="TableRow1" runat="server" Width="742px">
                    <asp:TableCell ID="TableCell1" runat="server" Width="371px" HorizontalAlign="Left">
                        <asp:Button ID="btnBuscar" runat="server"  CssClass="botonGenerarReporte" Text="Buscar" onclick="btnBuscar_Click"  />              
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell2" runat="server" Width="371px" HorizontalAlign="Right">
                        <asp:Button ID="ButtonNuevo" runat="server" CssClass="botonGenerarReporte" Text="Nuevo" PostBackUrl="~/Maestros/NuevoCliente.aspx"/>
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
                    
                        <asp:GridView ID="gridViewClientes" runat="server"  CssClass="grid-view" AutoGenerateColumns ="false" CellSpacing="0" 
                            CellPadding="0" BorderStyle="None" onrowcreated="gridViewClientes_RowCreated" AllowPaging ="true"   
                            OnPageIndexChanging = "OnPaging" onrowediting="Editar"
                            onrowupdating="Modificar"  onrowcancelingedit="CancelEdit"
                            PageSize = "10" >
                            
                            <Columns>
                            <asp:TemplateField ItemStyle-Width = "25px"  HeaderText = "Id" Visible = "false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdCliente" runat="server"
                                    Text='<%# Eval("IdCliente")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                             <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Nombre">
                                <ItemTemplate>
                                    <asp:Label ID="lblNombre" runat="server"
                                            Text='<%# Eval("Nombre")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNombre" runat="server"
                                        Text='<%# Eval("Nombre")%>' Width="60px"></asp:TextBox>
                                </EditItemTemplate> 
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "250px"  HeaderText = "Tipo Documento">
                                <ItemTemplate>
                                    <asp:Label ID="lbltipoDocumento" runat="server"
                                            Text='<%# Eval("tipoDocumento")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                        <asp:Label ID="lbltipoDocumento" runat="server" Text='<%# Eval("tipoDocumento")%>' Visible = "false"></asp:Label>
                                <asp:DropDownList ID = "ddltipoDocumento" runat = "server" Width="60px"></asp:DropDownList>
                                </EditItemTemplate>  
                                
                            </asp:TemplateField>
                            
                            <asp:TemplateField ItemStyle-Width = "150px"  HeaderText = "Nro. Documento">
                                <ItemTemplate>
                                    <asp:Label ID="lblnroDoc" runat="server"
                                            Text='<%# Eval("nroDoc")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtnroDoc" runat="server"
                                        Text='<%# Eval("nroDoc")%>' Width="70px"></asp:TextBox>
                                </EditItemTemplate> 
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "250px"  HeaderText = "Pais Origen">
                                <ItemTemplate>
                                    <asp:Label ID="lblPais" runat="server"
                                            Text='<%# Eval("Pais")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                        <asp:Label ID="lblPais" runat="server" Text='<%# Eval("Pais")%>' Visible = "false"></asp:Label>
                                <asp:DropDownList ID = "ddlPais" runat = "server" Width="80px"></asp:DropDownList>
                                </EditItemTemplate>  
                                
                            </asp:TemplateField>
                            
                            <asp:TemplateField ItemStyle-Width = "150px"  HeaderText = "Direccion">
                                <ItemTemplate>
                                    <asp:Label ID="lblDireccion" runat="server"
                                            Text='<%# Eval("Direccion")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDireccion" runat="server"
                                        Text='<%# Eval("Direccion")%>' Width="100px"></asp:TextBox>
                                </EditItemTemplate> 
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "150px"  HeaderText = "Ciudad">
                                <ItemTemplate>
                                    <asp:Label ID="lblCiudad" runat="server"
                                            Text='<%# Eval("Ciudad")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCiudad" runat="server"
                                        Text='<%# Eval("Ciudad")%>' Width="80px"></asp:TextBox>
                                </EditItemTemplate> 
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "150px"  HeaderText = "Codigo Postal">
                                <ItemTemplate>
                                    <asp:Label ID="lblCodigoPostal" runat="server"
                                            Text='<%# Eval("CodigoPostal")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCodigoPostal" runat="server"
                                        Text='<%# Eval("CodigoPostal")%>' Width="60px"></asp:TextBox>
                                </EditItemTemplate> 
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "150px"  HeaderText = "Mail">
                                <ItemTemplate>
                                    <asp:Label ID="lblMail" runat="server"
                                            Text='<%# Eval("Mail")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtMail" runat="server"
                                        Text='<%# Eval("Mail")%>' Width="100px"></asp:TextBox>
                                </EditItemTemplate> 
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Tel">
                                <ItemTemplate>
                                    <asp:Label ID="lblTel" runat="server"
                                            Text='<%# Eval("Tel")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTel" runat="server"
                                        Text='<%# Eval("Tel")%>' Width="100px"></asp:TextBox>
                                </EditItemTemplate> 
                                
                            </asp:TemplateField>
                            
                            <asp:TemplateField ItemStyle-Width = "120px"  HeaderText = "Zona">
                                <ItemTemplate>
                                    <asp:Label ID="lblZona" runat="server"
                                            Text='<%# Eval("Zona")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                        <asp:Label ID="lblZona" runat="server" Text='<%# Eval("Zona")%>' Visible = "false"></asp:Label>
                                <asp:DropDownList ID = "ddlZonas" runat = "server" Width="80px"></asp:DropDownList>
                                </EditItemTemplate>  
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "150px"  HeaderText = "Vendedor">
                                <ItemTemplate>
                                    <asp:Label ID="lblVendedor" runat="server"
                                            Text='<%# Eval("Vendedor")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                        <asp:Label ID="lblVendedor" runat="server" Text='<%# Eval("Vendedor")%>' Visible = "false"></asp:Label>
                                <asp:DropDownList ID = "ddlVendedores" runat = "server" Width="80px"></asp:DropDownList>
                                </EditItemTemplate>  
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "50px"  HeaderText = "Activo">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckActivo" runat="server" Enabled = "false" 
                                    Checked='<%#Eval("Activo")%>'></asp:CheckBox>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "60px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRemove" runat="server" 
                                        CommandArgument = '<%# Eval("IdCliente")%>'
                                     OnClientClick = "return confirm('Desea eliminar el cliente?')"
                                    OnClick = "Eliminar"><img src="../Imagenes/cancelar.bmp" alt="delete group" /></asp:LinkButton>
                 
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:CommandField  ShowEditButton="True" ButtonType="Image" EditImageUrl="~/Imagenes/Modificar.ico" UpdateImageUrl="~/Imagenes/aceptar.bmp" CancelImageUrl="~/Imagenes/cancelar.bmp" CausesValidation="false" />
                            </Columns>
                            
                            </asp:GridView>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            </ContentTemplate>
        </asp:UpdatePanel>  
        
        
        
</asp:Content>



