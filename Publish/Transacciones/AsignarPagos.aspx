<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Transacciones.Master"
    CodeBehind="AsignarPagos.aspx.cs" Inherits="InterfazWeb.Transacciones.AsignarPagos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css" />
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css" />
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloGridView.css" />
    <link rel="stylesheet" href="../Styles/chosen.css" />
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
	<script src="../Scripts/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true })
            $('#Contenido_ddlCliente_chzn_o_0').hide();
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#Contenido_txtSaldo').keyup(function () {
                var aux = $('#Contenido_txtSaldo').val().replace(".", ",");
                $('#Contenido_txtSaldo').val(aux);
            });
        });
    </script>

    <div class="divTitulo">
        <asp:Label ID="lblReportes" runat="server" Text="Asignar Pagos" Font-Bold="true"></asp:Label>
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
                        <asp:Label ID="Label12" runat="server" Text="Clientes *" CssClass="bodyText"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell40" runat="server" Width="350px">
                        <asp:DropDownList ID="ddlClientes" class="chzn-select" Width="300px" runat="server">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell41" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlClientes"
                            ErrorMessage="Debe seleccionar un cliente" ForeColor="Red" EnableClientScript="false"
                            Display="Dynamic" SetFocusOnError="true" ToolTip="Debe seleccionar un cliente"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div>
            <asp:Image ID="Image2" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px" />
        </div>
        <div>
            <asp:Table ID="Table1" runat="server" Width="742px" Height="20px">
                <asp:TableRow ID="TableRow1" runat="server">
                    <asp:TableCell ID="TableCell6" runat="server" Width="200px">
                        <asp:Label ID="Label2" runat="server" Text="Moneda *" CssClass="bodyText"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell7" runat="server" Width="350px">
                        <asp:DropDownList ID="ddlMoneda" Width="300px" runat="server" AutoPostBack="true">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell8" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlMoneda"
                            ErrorMessage="Debe seleccionar la moneda" ForeColor="Red" EnableClientScript="false"
                            Display="Dynamic" SetFocusOnError="true" ToolTip="Debe seleccionar la moneda"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div>
            <asp:Image ID="Image6" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px" />
        </div>
        <div>
            <asp:Table ID="Table5" runat="server">
                <asp:TableRow ID="TableRow2" runat="server" Width="742px">
                    <asp:TableCell ID="TableCell5" runat="server" Width="450px" HorizontalAlign="Right">
                        <asp:Button ID="btnBuscar" runat="server" CssClass="botonGenerarReporte" Text="Documentos pendientes"
                            OnClick="btnGuardar_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div>
            <asp:Image ID="Image3" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px" />
        </div>
    </asp:Panel>
    <asp:Panel ID="Pendientes" runat="server">
        <asp:Label ID="Label3" runat="server" Text="DOCUMENTOS" CssClass="bodyText"></asp:Label>
        <asp:Table ID="Table7" runat="server">
            <asp:TableRow ID="TableRow7" runat="server">
                <asp:TableCell ID="TableCell16" runat="server">
                    <asp:GridView ID="gridViewEstadoCuenta" runat="server" CssClass="grid-view" AutoGenerateColumns="false"
                        CellSpacing="0" CellPadding="0" BorderStyle="None" OnRowCreated="gridViewDocumentos_RowCreated"
                        AllowPaging="true" PageSize="10" OnPageIndexChanging="OnPaging">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="400px" HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdDocumento" runat="server" Text='<%# Eval("IdDocumento")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="400px" HeaderText="Pagar">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPagar" runat="server" OnCheckedChanged="chkPagar_checkedChanged" AutoPostBack="true"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="400px" HeaderText="Fecha">
                                <ItemTemplate>
                                    <asp:Label ID="lblFecha" runat="server" Text='<%# Eval("FechaEmision")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="400px" HeaderText="Fecha Vencimiento">
                                <ItemTemplate>
                                    <asp:Label ID="lblFechaVencimiento" runat="server" Text='<%# Eval("FechaVencimiento")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Detalle">
                                <ItemTemplate>
                                    <asp:Label ID="lblDetalle" runat="server" Text='<%# Eval("Detalle")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Moneda">
                                <ItemTemplate>
                                    <asp:Label ID="lblMoneda" runat="server" Text='<%# Eval("Moneda")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Monto Total">
                                <ItemTemplate>
                                    <asp:Label ID="lblMonto" runat="server" Text='<%# Eval("Total")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Restante">
                                <ItemTemplate>
                                    <asp:Label ID="lblRestante" runat="server" Text='<%# Eval("SaldoDocumento")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Saldo">
                                <ItemTemplate>
                                    <asp:Label ID="lblSaldo" runat="server" Text='<%# Eval("SaldoTotal")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <div>
            <asp:Image ID="Image7" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px" />
        </div>
        <asp:Label ID="Label4" runat="server" Text="RECIBOS" CssClass="bodyText"></asp:Label>
        <asp:Table ID="Table6" runat="server">
            <asp:TableRow ID="TableRow5" runat="server">
                <asp:TableCell ID="TableCell12" runat="server">
                    <asp:GridView ID="gridViewRecibos" runat="server" CssClass="grid-view" AutoGenerateColumns="false"
                        CellSpacing="0" CellPadding="0" BorderStyle="None" OnRowCreated="gridViewDocumentos_RowCreated"
                        AllowPaging="true" PageSize="10" OnPageIndexChanging="OnPaging">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="400px" HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdRecibo" runat="server" Text='<%# Eval("IdDocumento")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="400px" HeaderText="Pagar">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPagar" runat="server" OnCheckedChanged="chkPagar_checkedChanged" AutoPostBack="true"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="400px" HeaderText="Fecha">
                                <ItemTemplate>
                                    <asp:Label ID="lblFecha" runat="server" Text='<%# Eval("FechaEmision")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="400px" HeaderText="Fecha Vencimiento">
                                <ItemTemplate>
                                    <asp:Label ID="lblFechaVencimiento" runat="server" Text='<%# Eval("FechaVencimiento")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Detalle">
                                <ItemTemplate>
                                    <asp:Label ID="lblDetalle" runat="server" Text='<%# Eval("Detalle")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Moneda">
                                <ItemTemplate>
                                    <asp:Label ID="lblMoneda" runat="server" Text='<%# Eval("Moneda")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Monto Total">
                                <ItemTemplate>
                                    <asp:Label ID="lblMonto" runat="server" Text='<%# Eval("Total")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Restante">
                                <ItemTemplate>
                                    <asp:Label ID="lblRestante" runat="server" Text='<%# Eval("SaldoDocumento")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="100px" HeaderText="Saldo">
                                <ItemTemplate>
                                    <asp:Label ID="lblSaldo" runat="server" Text='<%# Eval("SaldoTotal")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
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
    </asp:Panel>
    <%--<asp:Panel ID= "MedioDePago" runat="server">
    <div>
         <asp:Table ID="Table12" runat="server" Width="742px" Height="20px">
                <asp:TableRow ID="TableRow11" runat="server">
                    <asp:TableCell ID="TableCell22" runat="server" Width="200px">
                        <asp:Label ID="Label9" runat="server" Text="Medio de Pago" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell23" runat="server">
                        <asp:DropDownList ID="ddlMedioPago" Width="300px" runat="server"></asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell24" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlMedioPago" 
                           ErrorMessage="Debe seleccionar el medio de pago" ForeColor="Red" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe seleccionar el medio de pago" ></asp:RequiredFieldValidator>
                           
                        </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div>
                <asp:Image ID="Image11" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>
    
     </asp:Panel> --%>
    <div>
        <asp:Table ID="Table3" runat="server" Width="742px" Height="20px">
            <asp:TableRow ID="TableRow4" runat="server">
                <asp:TableCell ID="TableCell9" runat="server" Width="500px" HorizontalAlign="Right">
                    <asp:Label ID="Label1" runat="server" Text="Saldo: " CssClass="bodyText"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCell10" runat="server" HorizontalAlign="Right">
                    <asp:TextBox ID="txtSaldoTotal" Width="150px" runat="server" Enabled="false"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell ID="TableCell11" runat="server" HorizontalAlign="Left">
                    
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <asp:Panel ID="Importe" runat="server">
        <div>
            <asp:Table ID="Table4" runat="server" Width="742px" Height="20px">
                <asp:TableRow ID="TableRow8" runat="server">
                    <asp:TableCell ID="TableCell1" runat="server" Width="200px">
                        <asp:Label ID="Label7" runat="server" Text="Diferencia entre Debe y Haber" CssClass="bodyText"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell2" runat="server">
                        <asp:TextBox ID="txtSaldo" Width="300px" runat="server" Enabled="false"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell3" runat="server" HorizontalAlign="Left">
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div>
            <asp:Image ID="Image4" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px" />
        </div>
    </asp:Panel>
    <asp:Panel ID="Panel1" runat="server">
        
        <div>
            <asp:Image ID="Image9" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px" />
        </div>
        <div>
            <asp:Table ID="Table2" runat="server">
                <asp:TableRow ID="TableRow3" runat="server" Width="742px">
                    <asp:TableCell ID="TableCell4" runat="server" Width="450px" HorizontalAlign="Center">
                        <asp:Button ID="btnCobrar" runat="server" CssClass="botonGenerarReporte" Text="Asignar Pagos"
                            OnClick="btnCobrar_Click" />
                    </asp:TableCell></asp:TableRow>
            </asp:Table>
        </div>
        <div>
            <asp:Image ID="Image5" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px" />
        </div>
    </asp:Panel>
</asp:Content>
