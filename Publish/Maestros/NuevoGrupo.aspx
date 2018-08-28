<%@ Page Language="C#" MasterPageFile="~/MasterPages/Maestros.Master" AutoEventWireup="true" CodeBehind="NuevoGrupo.aspx.cs" Inherits="Interfaz_Web.Maestros.NuevoGrupo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
       
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css"/>
    
    <div class="divTitulo">
    <asp:Label ID="lblReportes" runat="server" Text="Nuevo Grupo"  Font-Bold="true"></asp:Label>
    </div>
    <div></div>
    <div>
            <asp:Image ID="Image5" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
        </div>
    <div>
    <div>
        <asp:Table ID="Table1" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="TableRow1" runat="server">
                    <asp:TableCell ID="TableCell2" runat="server" Width="200px">
                        <asp:Label ID="lblCodigo" runat="server" Text="Código *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell3" runat="server">
                        <asp:TextBox ID="txtCodigo" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell4" runat="server" HorizontalAlign="Left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCodigo" 
                       ErrorMessage="Debe ingresar el código" ForeColor="Red" ToolTip="Debe ingresar el código" ></asp:RequiredFieldValidator>
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
    </div>
    <div>
            <asp:Image ID="Image1" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>
    <div>
         <asp:Table ID="Table2" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="Fila2" runat="server">
                    <asp:TableCell ID="Celda21" runat="server" Width="200px">
                        <asp:Label ID="lblNombre" runat="server" Text="Descripción *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="Celda22" runat="server">
                        <asp:TextBox ID="txtNombre" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="Celda23" runat="server" HorizontalAlign="Left">
                    <asp:RequiredFieldValidator ID="NombreObligatorio" runat="server" ControlToValidate="txtNombre" 
                       ErrorMessage="Debe ingresar la descripción" ForeColor="Red" ToolTip="Debe ingresar la descripción" ></asp:RequiredFieldValidator>
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>

     <div>
            <asp:Image ID="Image3" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>
    
        </div>
        <div></div>
        <div >
            <asp:Table ID="Table5" runat="server">
                     <asp:TableRow ID="TableRow2" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell5" runat="server" Width="371px" HorizontalAlign="Right">
                            <asp:Button ID="btnBuscar" runat="server" CssClass="botonGenerarReporte" Text="Guardar" onclick="btnGuardar_Click" />              
                        </asp:TableCell>
                        
                    </asp:TableRow>
            </asp:Table>
                                    
        </div>
</asp:Content>

