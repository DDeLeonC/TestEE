<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Transacciones.Master"
    CodeBehind="AnularRecibo.aspx.cs" Inherits="InterfazWeb.Transacciones.AnularRecibo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css" />
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css" />
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloGridView.css" />
    <link rel="stylesheet" href="../Styles/chosen.css" />
    <style>
        .red_font
        {
            color: Red;
        }
    </style>
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../Scripts/chosen.jquery.js" type="text/javascript"></script>
    
    <div class="divTitulo">
        <asp:Label ID="lblReportes" runat="server" Text="Anular Recibo" Font-Bold="true"></asp:Label>
    </div>
    <div>
    </div>
    <asp:Panel ID="NroPeticion" runat="server">
        <div>
            <asp:Image ID="Image1" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px" />
        </div>
        <div>
            <asp:Table ID="Table10" runat="server" Width="742px" Height="20px">
                <asp:TableRow ID="TableRow17" runat="server">
                    <asp:TableCell ID="TableCell39" runat="server" Width="200px">
                        <asp:Label ID="Label12" runat="server" Text="Nro. Recibo *" CssClass="bodyText"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell40" runat="server" Width="350px">
                        <asp:TextBox ID="txtNroRecibo" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell41" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtNroRecibo"
                            ErrorMessage="Debe ingresar un Nro Recibo" ForeColor="Red" EnableClientScript="false"
                            Display="Dynamic" SetFocusOnError="true" ToolTip="Debe ingresar un Nro Recibo"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtNroRecibo"
                                ErrorMessage="Debe ingresar un Nro" ForeColor="Red" Type="Integer" Operator="DataTypeCheck"
                                EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe ingresar un Nro"></asp:CompareValidator>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div>
            <asp:Table ID="Table5" runat="server">
                <asp:TableRow ID="TableRow2" runat="server" Width="742px">
                    <asp:TableCell ID="TableCell5" runat="server" Width="450px" HorizontalAlign="Right">
                        <asp:Button ID="btnBuscar" runat="server" CssClass="botonGenerarReporte" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div>
            <asp:Image ID="Image3" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px" />
        </div>
    </asp:Panel>
    <asp:Panel ID="Pendientes" runat="server" Visible="false">
        <asp:Table ID="Table7" runat="server">
            <asp:TableRow ID="TableRow5" runat="server">
                <asp:TableCell>
                    <asp:Label ID="Label1" runat="server">Nro: </asp:Label>
                    <asp:Label ID="lblNroRecibo" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow7" runat="server">
                <asp:TableCell>
                    <asp:Label ID="lblCliente" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow1" runat="server">
                <asp:TableCell>
                    <asp:Label ID="lblFecha" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow4" runat="server">
                <asp:TableCell>
                    <asp:Label ID="lblMonto" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow6" runat="server">
                <asp:TableCell>
                    <asp:Label ID="lblEstadoRecibo" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <div>
        </div>
        <div>
        </div>
        <div>
            <asp:Image ID="Image8" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px" />
        </div>
        <div>
            <asp:Table ID="Table19" runat="server">
                <asp:TableRow ID="TableRow25" runat="server" Width="742px">
                    <asp:TableCell ID="TableCell31" runat="server" Width="300px">
                        <asp:Label ID="Label20" runat="server" Text="Observaciones:" CssClass="bodyText"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell65" runat="server">
                        <asp:TextBox ID="txtObservaciones" Width="600px" Height="150px" TextMode="MultiLine" runat="server"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div>
            <asp:Image ID="Image10" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlAnular" runat="server" Visible="false">
        <div>
            <asp:Table ID="Table2" runat="server">
                <asp:TableRow ID="TableRow3" runat="server" Width="742px">
                    <asp:TableCell ID="TableCell4" runat="server" Width="450px" HorizontalAlign="Right">
                        <asp:Button ID="btnAnular" runat="server" CssClass="botonGenerarReporte" Text="Anular"
                            OnClientClick = "return confirm('Confirma que desea anular el recibo?')" OnClick="btnAnular_Click" />
                    </asp:TableCell></asp:TableRow>
            </asp:Table>
        </div>
        <div>
            <asp:Image ID="Image5" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px" />
        </div>
    </asp:Panel>
</asp:Content>
