<%@ Page Language="C#" MasterPageFile="~/MasterPages/Maestros.Master" AutoEventWireup="true" CodeBehind="MaestrosZonas.aspx.cs" Inherits="InterfazWeb.Maestros.MaestrosZonas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloGridView.css"/>
    <div class="divTitulo">
    <asp:Label ID="lblReportes" runat="server" Text="Zonas"  Font-Bold="true"></asp:Label>
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
            <asp:TableRow ID="Fila2" runat="server">
                <asp:TableCell ID="Celda21" runat="server">
                    <asp:Label ID="Label1" runat="server" Text="Código" CssClass="bodyText"></asp:Label>                
                </asp:TableCell>
                <asp:TableCell ID="Celda22" runat="server">
                    <asp:TextBox ID="txbCodigo" Width="300px" runat="server" CssClass="bodyText"></asp:TextBox>
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
                        <asp:Button ID="ButtonNuevo" runat="server" CssClass="botonGenerarReporte" Text="Nuevo" PostBackUrl="~/Maestros/NuevaZona.aspx"/>
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
                    
                        <asp:GridView ID="gridViewZonas" runat="server"  CssClass="grid-view" AutoGenerateColumns ="false" CellSpacing="0" 
                            CellPadding="0" BorderStyle="None" onrowcreated="gridViewZonas_RowCreated" AllowPaging ="true"   
                            OnPageIndexChanging = "OnPaging" onrowediting="Editar"
                            onrowupdating="Modificar"  onrowcancelingedit="CancelEdit"
                            PageSize = "10" >
                            
                            <Columns>
                            <asp:TemplateField ItemStyle-Width = "25px"  HeaderText = "Id" Visible = "false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdZona" runat="server"
                                    Text='<%# Eval("IdZona")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                             <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Código">
                                <ItemTemplate>
                                    <asp:Label ID="lblCodigo" runat="server"
                                            Text='<%# Eval("Codigo")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCodigo" runat="server"
                                        Text='<%# Eval("Codigo")%>' Width="60px"></asp:TextBox>
                                </EditItemTemplate> 
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "150px"  HeaderText = "Nombre">
                                <ItemTemplate>
                                    <asp:Label ID="lblNombre" runat="server"
                                            Text='<%# Eval("Nombre")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNombre" runat="server"
                                        Text='<%# Eval("Nombre")%>' Width="150px"></asp:TextBox>
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
                                        CommandArgument = '<%# Eval("IdZona")%>'
                                     OnClientClick = "return confirm('Desea eliminar la zona?')"
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




