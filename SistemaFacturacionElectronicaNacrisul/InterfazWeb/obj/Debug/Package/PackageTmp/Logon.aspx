<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Login.Master" CodeBehind="Logon.aspx.cs" Inherits="InterfazWeb.Logon" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contenido" runat="server">
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css"/>
    <div>
                <asp:Image ID="Image4" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="960px"/>
        </div>
     <div>
             <asp:Table ID="Table3" runat="server" Width="960px" Height="20px">
                    <asp:TableRow ID="TableRow4" runat="server">
                        <asp:TableCell ID="TableCell6" runat="server" Width="960px" HorizontalAlign="Center">
                            <asp:Label ID="Label3" runat="server" Text="Ingrese el usuario y clave" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        
                    </asp:TableRow>
                    
                </asp:Table>
            </div>
        <div>
                <asp:Image ID="Image5" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="960px"/>
        </div>
        <div>
             <asp:Table ID="Table4" runat="server" Width="960px" Height="20px" >
                    <asp:TableRow ID="TableRow5" runat="server">
                        <asp:TableCell ID="TableCell7" runat="server" Width="100px" >
                            <asp:Label ID="Label4" runat="server" Text="Usuario: " CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        
                    
                        <asp:TableCell ID="TableCell8" runat="server" Width="350px" >
                                <asp:TextBox ID="txtUsuario" Width="300px"  runat="server" ></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell9" runat="server" HorizontalAlign="Left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUsuario" 
                               ErrorMessage="Debe ingresar el usuario" ForeColor="Red" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe ingresar el usuario" ></asp:RequiredFieldValidator>
                           
                            </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow6" runat="server">
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow7" runat="server">
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow8" runat="server">
                        <asp:TableCell ID="TableCell10" runat="server" Width="100px" >
                            <asp:Label ID="Label5" runat="server" Text="Clave: " CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        
                    
                        <asp:TableCell ID="TableCell11" runat="server" Width="350px" >
                                <asp:TextBox ID="txtClave" Width="300px"  runat="server" Enabled="true" TextMode="Password" ></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell12" runat="server" HorizontalAlign="Left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtClave" 
                               ErrorMessage="Debe ingresar la clave" ForeColor="Red" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe ingresar la clave" ></asp:RequiredFieldValidator>
                           
                            </asp:TableCell>
                    </asp:TableRow>
                         
                    
                </asp:Table>
            </div>
        <div>
                <asp:Image ID="Image6" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="960px"/>
        </div>
        <div>
             <asp:Table ID="Table5" runat="server" Width="960px" Height="20px">
                    <asp:TableRow ID="TableRow12" runat="server">
                        <asp:TableCell ID="TableCell16" runat="server" Width="960px" HorizontalAlign="Center">
                            <asp:Label ID="lbMensaje" runat="server" Text="" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        
                    </asp:TableRow>
                    
                </asp:Table>
            </div>
            <div>
             <asp:Table ID="Table10" runat="server" Width="960px" Height="20px">
                    <asp:TableRow ID="TableRow17" runat="server">
                        <asp:TableCell ID="TableCell39" runat="server" Width="960px" HorizontalAlign="Center">
                            <asp:Label ID="Label12" runat="server" Text="Seleccione la empresa con la cual va a trabajar" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        
                    </asp:TableRow>
                    
                </asp:Table>
            </div>
        <div>
                <asp:Image ID="Image2" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="960px"/>
        </div>
        <div>
             <asp:Table ID="Table1" runat="server" Width="960px" Height="20px" >
                    <asp:TableRow ID="TableRow1" runat="server">
                        <asp:TableCell ID="TableCell1" runat="server" Width="100px" >
                            <asp:Label ID="Label1" runat="server" Text="Emisor: " CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        
                    
                        <asp:TableCell ID="TableCell40" runat="server" Width="350px" >
                                <asp:DropDownList ID="ddlEmisores" Width="300px"  runat="server" ></asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell41" runat="server" HorizontalAlign="Left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlEmisores" 
                               ErrorMessage="Debe seleccionar el emisor" ForeColor="Red" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe seleccionar el emisor" ></asp:RequiredFieldValidator>
                           
                            </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow3" runat="server">
                        <asp:TableCell ID="TableCell2" runat="server" Width="100px" >
                            <asp:Label ID="Label2" runat="server" Text="RUT: " CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        
                    
                        <asp:TableCell ID="TableCell3" runat="server" Width="350px" >
                                <asp:TextBox ID="txtRUC" Width="300px"  runat="server" Enabled="false" ></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell4" runat="server" HorizontalAlign="Left">
                            
                            </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        <div>
                <asp:Image ID="Image1" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="960px"/>
        </div>
        <div>
                <asp:Image ID="Image7" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="960px"/>
        </div>   
     
        <div>
             <asp:Table ID="Table2" runat="server" Width="960px" Height="20px">
                    <asp:TableRow ID="TableRow2" runat="server">
                        <asp:TableCell ID="TableCell5" runat="server" Width="371px" HorizontalAlign="Center">
                        <asp:Button ID="btnBuscar" runat="server" CssClass="botonGenerarReporte" Text="Ingresar" onclick="btnGuardar_Click" />              
                    </asp:TableCell>
                    </asp:TableRow>
                    
                </asp:Table>
            </div>
        <div>
                <asp:Image ID="Image3" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="960px"/>
        </div>
</asp:Content>


