<%@ Page Language="C#" MasterPageFile="~/MasterPages/Transacciones.Master" AutoEventWireup="true" CodeBehind="NotaDebito.aspx.cs" Inherits="InterfazWeb.Transacciones.NotaDebito" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css"/>
    <style>
		a img{border: none;}
		ol li{list-style: decimal outside;}
		div#container{width: 780px;margin: 0 auto;padding: 1em 0;}
		div.side-by-side{width: 100%;margin-bottom: 1em;}
		div.side-by-side > div{float: left;width: 50%;}
		div.side-by-side > div > em{margin-bottom: 10px;display: block;}
		.clearfix:after{content: "\0020";display: block;height: 0;clear: both;overflow: hidden;visibility: hidden;}
	</style>
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
	        $('#Contenido_ddlCliente_chzn_o_0').hide();

	        $('#Contenido_txtTipoCambio').change(function () {
	            var aux = $('#Contenido_txtTipoCambio').val().replace(".", ",");
	            $('#Contenido_txtTipoCambio').val(aux);
	        });
	        $('.decimal_input').keyup(function () {
	            this.value = this.value.replace(".", ",");
	        });
	    });
    </script>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"/>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
        TargetControlID="txtFecha" PopupButtonID="btnFecha" 
        Format="dd/MM/yyyy" />
    <div class="divTitulo">
    <asp:Label ID="lblReportes" runat="server" Text="Nota Débito"  Font-Bold="true"></asp:Label>
    </div>
    <div>
    </div>
    
    <div>
            <asp:Image ID="Image1" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>
    <div>
            <asp:Table ID="Table2" runat="server" Width="742px" Height="20px">
                  <asp:TableRow ID="TableRow8" runat="server">
                        <asp:TableCell ID="TableCell22" runat="server" Width="200px">
                            <asp:Label ID="lblFecha" runat="server" Text="Fecha *" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell23" runat="server" Width="350px">
                            <asp:ImageButton ID="btnFecha" CausesValidation="false" runat="server" ImageUrl="~/Imagenes/datetime.png" Height="24px" Width="24px" ImageAlign="Right" />                
                            <asp:TextBox ID="txtFecha" runat="server" Width = "300px"></asp:TextBox> 
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell24" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFecha" 
                           ErrorMessage="Debe ingresar la fecha" ForeColor="Red" ToolTip="Debe ingresar la fecha" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </asp:TableCell>
               
                  </asp:TableRow>
               </asp:Table>
        </div>
        <div>
                <asp:Image ID="Image2" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
        </div>
     <div>
             <asp:Table ID="Table1" runat="server" Width="742px" Height="20px">
                    <asp:TableRow ID="TableRow3" runat="server">
                        <asp:TableCell ID="TableCell2" runat="server" Width="200px">
                            <asp:Label ID="Label2" runat="server" Text="Moneda *" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell6" runat="server" Width="350px">
                            <asp:DropDownList ID="ddlMoneda" Width="300px"  runat="server" OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell7" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlMoneda" 
                           ErrorMessage="Debe seleccionar la moneda" ForeColor="Red" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe seleccionar la moneda" ></asp:RequiredFieldValidator>
                           
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div>
                <asp:Image ID="Image10" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
        </div>
     <div>
             <asp:Table ID="Table12" runat="server" Width="742px" Height="20px">
                    <asp:TableRow ID="TableRow21" runat="server">
                        <asp:TableCell ID="TableCell45" runat="server" Width="200px">
                            <asp:Label ID="Label14" runat="server" Text="Forma de Pago *" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell46" runat="server" Width="350px">
                            <asp:DropDownList ID="ddlFormaPago" Width="300px"  runat="server" ></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell47" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlFormaPago" 
                           ErrorMessage="Debe seleccionar la forma de pago" ForeColor="Red" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe seleccionar la forma de pago" ></asp:RequiredFieldValidator>
                           
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div>
            <asp:Image ID="Image5" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>
    <asp:Panel ID= "TipoCambio" runat="server">
    <div>
         <asp:Table ID="Table3" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="TableRow1" runat="server">
                    <asp:TableCell ID="TableCell3" runat="server" Width="200px">
                        <asp:Label ID="Label1" runat="server" Text="Tipo de Cambio *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell4" runat="server">
                        <asp:TextBox ID="txtTipoCambio" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell8" runat="server" HorizontalAlign="Left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTipoCambio" 
                       ErrorMessage="Debe ingresar el tipo de cambio" ForeColor="Red" ToolTip="Debe ingresar el tipo de cambio" ></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtTipoCambio" 
                            ErrorMessage="Debe ingresar un decimal" ForeColor="Red" Type="Currency" Operator="DataTypeCheck" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe ingresar un decimal" ></asp:CompareValidator>
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>
    <div>
        <asp:Image ID="Image4" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>  
    </asp:Panel>
      <div>
            <asp:Table ID="Table9" runat="server" Width="742px" Height="20px">
                <asp:TableRow ID="TableRow13" runat="server">
                    <asp:TableCell ID="TableCell29" runat="server" Width="200px">
                        <asp:Label ID="Label9" runat="server" Text="Cliente *" CssClass="bodyText"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell30" runat="server" Width="320px">
                        <asp:DropDownList ID="ddlCliente" Width="300px" class="chzn-select"  runat="server">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell31" runat="server" HorizontalAlign="Left">
                    </asp:TableCell>
                    <%--<asp:TableCell ID="TableCell31" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlCliente" 
                           ErrorMessage="Debe seleccionar el cliente" ForeColor="Red" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe seleccionar el cliente" ></asp:RequiredFieldValidator>
                           
                        </asp:TableCell>--%>
                </asp:TableRow>
            </asp:Table>
        </div>
            <div>
            <asp:Image ID="Image9" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>
    <div>
        <asp:Table ID="Table6" runat="server" Width="742px" Height="20px">
            <asp:TableRow ID="TableRow4" runat="server">
                <asp:TableCell ID="TableCell9" runat="server" Width="200px">
                    <asp:Label ID="Label3" runat="server" Text="Asociar Documentos" CssClass="bodyText"></asp:Label>                
                </asp:TableCell>
                <asp:TableCell ID="TableCell11" runat="server">
                    <asp:DropDownList ID="ddlAsociar" Width="300px"  runat="server" OnSelectedIndexChanged="ddlAsociar_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow17" runat="server">
                <asp:TableCell ID="TableCell12" runat="server" Width="200px">
                    
                </asp:TableCell>
                
            </asp:TableRow>
            
        </asp:Table>
        <asp:Panel ID= "AsociarDocumentos" runat="server">
        <div>
            <asp:Image ID="Image11" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
        </div>
        <asp:Table ID="Table11" runat="server" Width="742px" Height="20px">
            <asp:TableRow ID="TableRow18" runat="server">
                <asp:TableCell ID="TableCell42" runat="server" Width="100px">
                    <asp:Label ID="Label13" runat="server" Text="Tipo Documento" CssClass="bodyText"></asp:Label>                
                </asp:TableCell>
                <asp:TableCell ID="TableCell43" runat="server">
                    <asp:DropDownList ID="ddlTipoDocumento" Width="200px"  runat="server" OnSelectedIndexChanged="ddlSeleccionarCliente_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell ID="TableCell39" runat="server" Width="100px">
                                 
                </asp:TableCell>
                <asp:TableCell ID="TableCell40" runat="server">
                    
                </asp:TableCell>
                
            </asp:TableRow>
            <asp:TableRow ID="TableRow19" runat="server">
                <asp:TableCell ID="TableCell41" runat="server" HorizontalAlign="Left">
                         
                        </asp:TableCell>
                
            </asp:TableRow>
            
        </asp:Table>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>

            <asp:Table ID="Table10" runat="server">
                <asp:TableRow ID="TableRow20" runat="server">
                    <asp:TableCell ID="TableCell44" runat="server">
                    <div >
                        <asp:GridView ID="gridViewDocumentos" runat="server"  CssClass="grid-view" AutoGenerateColumns ="false" CellSpacing="0" 
                            CellPadding="0" BorderStyle="None" onrowcreated="gridViewFacturas_RowCreated" AllowPaging ="true"   
                            OnPageIndexChanging="gvCustomersPageChanging"  PageSize="10"
                            HeaderStyle-BackColor="#464646" HeaderStyle-HorizontalAlign ="Center" HeaderStyle-Height = "25px" Width="742px">
                            
                            <Columns>
                            <asp:TemplateField ItemStyle-Width = "25px"  HeaderText = "Id" Visible = "false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdDocumento" runat="server"
                                    Text='<%# Eval("IdDocumento")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Fecha">
                                <ItemTemplate>
                                    <asp:Label ID="lblFecha" runat="server"
                                            Text='<%# Eval("Fecha", "{0:d}")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Tipo Documento">
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
                           
                            <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Nro. Serie">
                                <ItemTemplate>
                                    <asp:Label ID="lblNroSerie" runat="server"
                                            Text='<%# Eval("NroSerie")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "50px"  HeaderText = "Asociar">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckActivo" runat="server" Enabled = "true" 
                                    Checked='<%#Eval("Asociar")%>'></asp:CheckBox>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RowIndex" Visible="false">
                            <ItemTemplate>
                            <%# Container.DisplayIndex  %>
                            </ItemTemplate>
                            </asp:TemplateField>
    
                              <asp:TemplateField HeaderText="DataItemIndex" Visible="false">
                            <ItemTemplate>
                            <%# Container.DataItemIndex  %>
                            </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                            
                            </asp:GridView>
                            </div>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            
            <asp:Table ID="Table13" runat="server">
                        <asp:TableRow ID="TableRow22" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell48" runat="server" Width="742px" HorizontalAlign="Right">
                            <asp:Button ID="btnObtenerDetalle" runat="server" CssClass="botonGenerarReporte" Text="Obtener Detalle" onclick="btnObtenerDetalle_Click" />              
                        </asp:TableCell>
                        
                    </asp:TableRow>
            </asp:Table>
                                    
  
        </ContentTemplate>
        </asp:UpdatePanel> 
        </asp:Panel>
     </div>
     
    <div>
        <asp:Image ID="Image6" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div> 
    <div>
            <asp:Table ID="Table4" runat="server" Width="742px" Height="20px">
                <asp:TableRow ID="TableRow6" runat="server">
                    <asp:TableCell ID="TableCell50" runat="server" Width="200px">
                        <asp:Label ID="Label5" runat="server" Text="Seleccionar" CssClass="bodyText"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell32" runat="server" HorizontalAlign="Left">
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="Left">
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow7" runat="server">
                    <asp:TableCell ID="TableCell15" runat="server" Width="200px">
                    
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow15" runat="server" Visible="false">
                    <asp:TableCell ID="TableCell33" runat="server" Width="200px">
                        <asp:Label ID="Label10" runat="server" Text="Grupo" CssClass="bodyText"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell34" runat="server">
                        <asp:DropDownList ID="ddlGrupo" Width="300px" runat="server" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell35" runat="server" HorizontalAlign="Left">
                        
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow16" runat="server" Visible="false">
                    <asp:TableCell ID="TableCell36" runat="server" Width="200px">
                        <asp:Label ID="Label11" runat="server" Text="SubGrupo" CssClass="bodyText"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell37" runat="server">
                        <asp:DropDownList ID="ddlSubGrupo" Width="300px" runat="server" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell38" runat="server" HorizontalAlign="Left">
                    
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow5" runat="server">
                    <asp:TableCell ID="TableCell10" runat="server" Width="200px">
                        <asp:Label ID="Label4" runat="server" Text="Producto" CssClass="bodyText"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell13" runat="server" Width="320px">
                        <asp:DropDownList ID="ddlProductos" Width="300px" class="chzn-select" runat="server">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell14" runat="server" HorizontalAlign="Left">
                        <asp:ImageButton ID="Nuevo" ImageUrl="~/Imagenes/nuevo.png" OnClick="btnAgregar_Click"
                            runat="server"></asp:ImageButton>
                        <asp:Label ID="lblNuevaLinea" runat="server" style="margin: 5px; position: absolute;">Agregar Línea</asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    <div>
        <asp:Image ID="Image3" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>    
   
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:Table ID="Table7" runat="server">
                
                <asp:TableRow ID="TableRow9" runat="server">
                    <asp:TableCell ID="TableCell16" runat="server">
                    
                        <asp:GridView ID="gridViewFacturas" runat="server"  CssClass="grid-view" AutoGenerateColumns ="false" CellSpacing="0" 
                            CellPadding="0" BorderStyle="None" onrowcreated="gridViewFacturas_RowCreated" AllowPaging ="false"   
                            onrowediting="Editar" 
                            onrowupdating="Modificar" HeaderStyle-BackColor="#464646" HeaderStyle-HorizontalAlign ="Center" HeaderStyle-Height = "25px" >
                            
                            <Columns>
                            <asp:TemplateField ItemStyle-Width = "25px"  HeaderText = "Id" Visible = "false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdProducto" runat="server"
                                    Text='<%# Eval("IdProducto")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Producto">
                                <ItemTemplate>
                                    <asp:Label ID="lblProducto" runat="server"
                                            Text='<%# Eval("Producto")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                             <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Unidad Medida">
                                <ItemTemplate>
                                    <asp:Label ID="lblUnidad" runat="server"
                                            Text='<%# Eval("UnidadMedida")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Cantidad">
                                <ItemTemplate>
                                    <asp:Label ID="lblCantidad" runat="server"
                                            Text='<%# Eval("Cantidad")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCantidad" runat="server"
                                        Text='<%# Eval("Cantidad")%>' Width="60px"></asp:TextBox>
                                </EditItemTemplate> 
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Kilos">
                                <ItemTemplate>
                                    <asp:Label ID="lblKilos" runat="server"
                                            Text='<%# Eval("Kilos")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtKilos" runat="server"
                                        Text='<%# Eval("Kilos")%>' Width="60px" CssClass="decimal_input"></asp:TextBox>
                                </EditItemTemplate> 
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Precio Unitario">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrecio" runat="server"
                                            Text='<%# Eval("Precio")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPrecio" runat="server"
                                        Text='<%# Eval("Precio")%>' Width="60px"></asp:TextBox>
                                </EditItemTemplate> 
                            </asp:TemplateField>
                            
                            <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Impuesto">
                                <ItemTemplate>
                                    <asp:Label ID="lblImpuesto" runat="server"
                                            Text='<%# Eval("Impuesto")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Descuento %">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescuento" runat="server"
                                            Text='<%# Eval("Descuento")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescuento" runat="server"
                                        Text='<%# Eval("Descuento")%>' Width="60px" ></asp:TextBox>
                                </EditItemTemplate> 
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Recargo %">
                                <ItemTemplate>
                                    <asp:Label ID="lblRecargo" runat="server"
                                            Text='<%# Eval("Recargo")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRecargo" runat="server"
                                        Text='<%# Eval("Recargo")%>' Width="60px" ></asp:TextBox>
                                </EditItemTemplate> 
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "SubTotal">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubTotal" runat="server"
                                            Text='<%# Eval("SubTotal")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Total">
                                <ItemTemplate>
                                    <asp:Label ID="lblMonto" runat="server"
                                            Text='<%# Eval("MontoTotal")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "60px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRemove" runat="server" 
                                        CommandArgument = '<%# Eval("IdProducto")%>'
                                     OnClientClick = "return confirm('Desea quitar el producto?')"
                                    OnClick = "Eliminar"><img src="../Imagenes/cancelar.bmp" alt="delete group" /></asp:LinkButton>
                 
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:CommandField  ShowEditButton="True" ShowCancelButton="false" ButtonType="Image" EditImageUrl="~/Imagenes/Modificar.ico" UpdateImageUrl="~/Imagenes/aceptar.bmp" CausesValidation="false" />
                            </Columns>
                            
                            </asp:GridView>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            </ContentTemplate>
           </asp:UpdatePanel> 
    <div>
        <asp:Image ID="Image7" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div> 
      
    <div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
        <asp:Table ID="Table8" runat="server" Width="742px" Height="20px">
            <asp:TableRow ID="TableRow10" runat="server">
                <asp:TableCell ID="TableCell17" runat="server" Width="200px">
                                   
                </asp:TableCell>
                <asp:TableCell ID="TableCell18" runat="server" HorizontalAlign="Right">
                    <asp:Label ID="Label6" runat="server" Text="SubTotal:" CssClass="bodyText"></asp:Label> 
                    
                </asp:TableCell>
                <asp:TableCell ID="TableCell19" runat="server" HorizontalAlign="Right">
                    <asp:TextBox ID="txtSubTotal" Width="300px" runat="server"></asp:TextBox>
                </asp:TableCell>
               
            </asp:TableRow>
            <asp:TableRow ID="TableRow11" runat="server">
                <asp:TableCell ID="TableCell20" runat="server" Width="200px">
                                   
                </asp:TableCell>
                <asp:TableCell ID="TableCell21" runat="server" HorizontalAlign="Right">
                    <asp:Label ID="Label7" runat="server" Text="Impuestos:" CssClass="bodyText"></asp:Label> 
                    
                </asp:TableCell>
                <asp:TableCell ID="TableCell25" runat="server" HorizontalAlign="Right">
                    <asp:TextBox ID="txtImpuestos" Width="300px" runat="server"></asp:TextBox>
                </asp:TableCell>
               
            </asp:TableRow>
            <asp:TableRow ID="TableRow12" runat="server">
                <asp:TableCell ID="TableCell26" runat="server" Width="200px">
                                   
                </asp:TableCell>
                <asp:TableCell ID="TableCell27" runat="server" HorizontalAlign="Right">
                    <asp:Label ID="Label8" runat="server" Text="Total:" CssClass="bodyText"></asp:Label> 
                    
                </asp:TableCell>
                <asp:TableCell ID="TableCell28" runat="server" HorizontalAlign="Right">
                    <asp:TextBox ID="txtTotal" Width="300px" runat="server"></asp:TextBox>
                </asp:TableCell>
               
            </asp:TableRow>
        </asp:Table>
        </ContentTemplate>
           </asp:UpdatePanel> 
     </div>
     <div>
        <asp:Image ID="Image12" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div> 
    <div></div>
    <div >
        <asp:Table ID="Table19" runat="server">
                    <asp:TableRow ID="TableRow25" runat="server" Width="742px">
                    <asp:TableCell ID="TableCell49" runat="server" Width="300px" >
                         <asp:Label ID="Label20" runat="server" Text="Adenda:" CssClass="bodyText"></asp:Label>              
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell65" runat="server" >
                         <asp:TextBox ID="txtAdenda" Width="600px" Height="150px" TextMode="MultiLine" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    
                </asp:TableRow>
        </asp:Table>
                                    
    </div>
    <div>
        <asp:Image ID="Image8" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
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



