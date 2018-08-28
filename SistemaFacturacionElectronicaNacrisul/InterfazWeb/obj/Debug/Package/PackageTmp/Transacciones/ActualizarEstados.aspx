<%@ Page Language="C#" MasterPageFile="~/MasterPages/Transacciones.Master" AutoEventWireup="true" CodeBehind="ActualizarEstados.aspx.cs" Inherits="InterfazWeb.Transacciones.ActualizarEstados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
       
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css"/>
    
    
    <div class="divTitulo">
    <asp:Label ID="lblReportes" runat="server" Text="Actualizar Estados Documentos"  Font-Bold="true"></asp:Label>
    </div>
    <div>
    </div>
    <div>
        <asp:Image ID="Image8" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div> 
    <div></div>
    <div >
        <asp:Table ID="Table5" runat="server">
                    <asp:TableRow ID="TableRow2" runat="server" Width="742px">
                    <asp:TableCell ID="TableCell5" runat="server" Width="371px" HorizontalAlign="Right">
                        <asp:Button ID="btnBuscar" runat="server" CssClass="botonGenerarReporte" Text="Actualizar Estados" onclick="btnGuardar_Click" />              
                    </asp:TableCell>
                        
                </asp:TableRow>
        </asp:Table>
                                    
    </div>
    
</asp:Content>



