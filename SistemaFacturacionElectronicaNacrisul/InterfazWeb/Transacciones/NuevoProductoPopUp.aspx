<%@ Page Language="C#" MasterPageFile="~/MasterPages/PopUpMaster.Master" AutoEventWireup="true" CodeBehind="~/Transacciones/NuevoProductoPopUp.aspx.cs" Inherits="InterfazWeb.Transacciones.NuevoProductoPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
       
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css"/>
    <script type="text/javascript"">
        function cancelPopUpProducto() {
            window.parent.document.getElementById('btnCancelProducto').click();
        }
    </script>
    <div class="divTitulo">
    <asp:Label ID="lblReportes" runat="server" Text="Nuevo Producto"  Font-Bold="true"></asp:Label>
    </div>
    <div></div>
    
    <div>
            <asp:Image ID="Image1" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>
    <div>
         <asp:Table ID="Table1" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="TableRow3" runat="server">
                    <asp:TableCell ID="TableCell2" runat="server" Width="200px">
                        <asp:Label ID="Label2" runat="server" Text="Codigo *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell6" runat="server">
                        <asp:TextBox ID="txtCodigo" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell7" runat="server" HorizontalAlign="Left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCodigo" 
                       ErrorMessage="Debe ingresar el codigo" ForeColor="Red" ToolTip="Debe ingresar el codigo" ></asp:RequiredFieldValidator>
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>

     <div>
            <asp:Image ID="Image2" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
     </div>
     <div>
         <asp:Table ID="Table2" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="Fila2" runat="server">
                    <asp:TableCell ID="Celda21" runat="server" Width="200px">
                        <asp:Label ID="lblNombre" runat="server" Text="Nombre *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="Celda22" runat="server">
                        <asp:TextBox ID="txtNombre" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="Celda23" runat="server" HorizontalAlign="Left">
                    <asp:RequiredFieldValidator ID="NombreObligatorio" runat="server" ControlToValidate="txtNombre" 
                       ErrorMessage="Debe ingresar el nombre" ForeColor="Red" ToolTip="Debe ingresar el nombre" ></asp:RequiredFieldValidator>
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>
            <div>
            <asp:Image ID="Image5" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>
    <div>
         <asp:Table ID="Table3" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="TableRow1" runat="server">
                    <asp:TableCell ID="TableCell3" runat="server" Width="200px">
                        <asp:Label ID="Label1" runat="server" Text="Descripcion" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell4" runat="server">
                        <asp:TextBox ID="txtDescripcion" Width="300px"  runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell8" runat="server" HorizontalAlign="Left">
                    
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>
     <div>
            <asp:Image ID="Image10" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>
    <div>
         <asp:Table ID="Table4" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="TableRow8" runat="server">
                    <asp:TableCell ID="TableCell22" runat="server" Width="200px">
                        <asp:Label ID="Label7" runat="server" Text="Precio *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell23" runat="server">
                        <asp:TextBox ID="txtPrecio" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell24" runat="server" HorizontalAlign="Left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPrecio" 
                       ErrorMessage="Debe ingresar el precio" ForeColor="Red" ToolTip="Debe ingresar el precio" ></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtPrecio" 
                            ErrorMessage="Debe ingresar un decimal" ForeColor="Red" Type="Currency" Operator="DataTypeCheck" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe ingresar un decimal" ></asp:CompareValidator>
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>
     <div>
            <asp:Image ID="Image8" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>
    
      <div>
         <asp:Table ID="Table5" runat="server" Width="742px" Height="20px">
                <asp:TableRow ID="Fila4" runat="server">
                    <asp:TableCell ID="Celda41" runat="server" Width="200px">
                        <asp:Label ID="lblGrupo" runat="server" Text="Grupo *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell51" runat="server">
                        <asp:DropDownList ID="ddlGrupo" Width="300px" runat="server" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell52" runat="server" HorizontalAlign="Left">
                    
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div>
            <asp:Image ID="Image11" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
      </div>
      <div>
         <asp:Table ID="Table6" runat="server" Width="742px" Height="20px">
                <asp:TableRow ID="TableRow9" runat="server">
                    <asp:TableCell ID="TableCell1" runat="server" Width="200px">
                        <asp:Label ID="Label8" runat="server" Text="SubGrupo *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell10" runat="server">
                        <asp:DropDownList ID="ddlSubGrupo" Width="300px" runat="server"></asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell25" runat="server" HorizontalAlign="Left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlSubGrupo" 
                       ErrorMessage="Debe seleccionar el subgrupo" ForeColor="Red" ToolTip="Debe seleccionar el subgrupo" ></asp:RequiredFieldValidator>
                
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div>
            <asp:Image ID="Image6" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
      </div>
      <div>
         <asp:Table ID="Table8" runat="server" Width="742px" Height="20px">
                <asp:TableRow ID="TableRow4" runat="server">
                    <asp:TableCell ID="TableCell9" runat="server" Width="200px">
                        <asp:Label ID="Label3" runat="server" Text="Indicador de Facturacion *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell11" runat="server">
                        <asp:DropDownList ID="ddlIndicador" Width="300px" runat="server"></asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell12" runat="server" HorizontalAlign="Left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlIndicador" 
                       ErrorMessage="Debe seleccionar el indicador" ForeColor="Red" ToolTip="Debe seleccionar el indicador" ></asp:RequiredFieldValidator>
                
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div>
                <asp:Image ID="Image3" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
            </div> 
            <div>
         <asp:Table ID="Table9" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="TableRow5" runat="server">
                    <asp:TableCell ID="TableCell13" runat="server" Width="200px">
                        <asp:Label ID="Label4" runat="server" Text="Unidad de Medida" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell14" runat="server">
                        <asp:TextBox ID="txtUnidad" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell15" runat="server" HorizontalAlign="Left">
                    
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>

     <div>
            <asp:Image ID="Image7" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
     </div>
        <div></div>
        <div >
            <asp:Table ID="Table7" runat="server">
                     <asp:TableRow ID="TableRow2" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell5" runat="server" Width="500px" HorizontalAlign="Right">
                            <asp:Button ID="btnBuscar" runat="server" CssClass="botonGenerarReporte" Text="Guardar" onclick="btnGuardar_Click" />              
                            <Button id="btnSalirProducto" runat="server" class="botonGenerarReporte" style="margin-left: 15px;" onclick="cancelPopUpProducto();">Salir</Button>
                        </asp:TableCell>
                        
                    </asp:TableRow>
            </asp:Table>
                                    
        </div>
</asp:Content>