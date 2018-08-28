<%@ Page Language="C#" MasterPageFile="~/MasterPages/Maestros.Master" AutoEventWireup="true" CodeBehind="NuevoCliente.aspx.cs" Inherits="InterfazWeb.Maestros.NuevoCliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
       
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css"/>
    
    <div class="divTitulo">
    <asp:Label ID="lblReportes" runat="server" Text="Nuevo Cliente"  Font-Bold="true"></asp:Label>
    </div>
    <div></div>
    
    <div>
            <asp:Image ID="Image1" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
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
            <asp:Image ID="Image2" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
     </div>
     <div>
             <asp:Table ID="Table1" runat="server" Width="742px" Height="20px">
                    <asp:TableRow ID="TableRow3" runat="server">
                        <asp:TableCell ID="TableCell2" runat="server" Width="200px">
                            <asp:Label ID="Label2" runat="server" Text="Tipo Documento *" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell6" runat="server" Width="350px">
                            <asp:DropDownList ID="ddlTipoDocumento" Width="300px"  runat="server" ></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell7" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlTipoDocumento" 
                           ErrorMessage="Debe seleccionar el tipo de documento" ForeColor="Red" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe seleccionar el tipo de documento" ></asp:RequiredFieldValidator>
                           
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
                        <asp:Label ID="Label1" runat="server" Text="Nro. Documento *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell4" runat="server">
                        <asp:TextBox ID="txtNroDoc" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell8" runat="server" HorizontalAlign="Left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNroDoc" 
                       ErrorMessage="Debe ingresar el nro. de documento" ForeColor="Red" ToolTip="Debe ingresar el nro. de documento" ></asp:RequiredFieldValidator>
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>
            <div>
                <asp:Image ID="Image4" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
            </div>  
      <div>
         <asp:Table ID="Table4" runat="server" Width="742px" Height="20px">
                <asp:TableRow ID="Fila4" runat="server">
                    <asp:TableCell ID="Celda41" runat="server" Width="200px">
                        <asp:Label ID="lblPais" runat="server" Text="País Origen *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell10" runat="server">
                        <asp:DropDownList ID="ddlPais" Width="300px" runat="server"></asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="Left">
                    <asp:RequiredFieldValidator ID="ZonaObligatorio" runat="server" ControlToValidate="ddlPais" 
                       ErrorMessage="Debe seleccionar el país de origen" ForeColor="Red" ToolTip="Debe seleccionar el país de origen" ></asp:RequiredFieldValidator>
                
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        
        <div>
                <asp:Image ID="Image3" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
            </div> 
            <div>
         <asp:Table ID="Table6" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="TableRow4" runat="server">
                    <asp:TableCell ID="TableCell9" runat="server" Width="200px">
                        <asp:Label ID="Label3" runat="server" Text="Dirección" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell11" runat="server">
                        <asp:TextBox ID="txtDireccion" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell12" runat="server" HorizontalAlign="Left">
                    
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>
            <div>
                <asp:Image ID="Image6" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
            </div> 
        <div>
         <asp:Table ID="Table7" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="TableRow5" runat="server">
                    <asp:TableCell ID="TableCell13" runat="server" Width="200px">
                        <asp:Label ID="Label4" runat="server" Text="Ciudad" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell14" runat="server">
                        <asp:TextBox ID="txtCiudad" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell15" runat="server" HorizontalAlign="Left">
                    
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>
            <div>
                <asp:Image ID="Image7" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
            </div> 
            <div>
         <asp:Table ID="Table8" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="TableRow6" runat="server">
                    <asp:TableCell ID="TableCell16" runat="server" Width="200px">
                        <asp:Label ID="Label5" runat="server" Text="Código Postal" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell17" runat="server">
                        <asp:TextBox ID="txtCodigoPostal" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell18" runat="server" HorizontalAlign="Left">
                    
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>
     <div>
                <asp:Image ID="Image9" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
            </div> 
            <div>
         <asp:Table ID="Table9" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="TableRow7" runat="server">
                    <asp:TableCell ID="TableCell19" runat="server" Width="200px">
                        <asp:Label ID="Label6" runat="server" Text="Mail" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell20" runat="server">
                        <asp:TextBox ID="txtMail" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell21" runat="server" HorizontalAlign="Left">
                    
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>
     <div>
                <asp:Image ID="Image12" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
            </div> 
            <div>
         <asp:Table ID="Table12" runat="server" Width="742px" Height="20px">
              <asp:TableRow ID="TableRow10" runat="server">
                    <asp:TableCell ID="TableCell28" runat="server" Width="200px">
                        <asp:Label ID="Label9" runat="server" Text="Tel" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell29" runat="server">
                        <asp:TextBox ID="txtTel" Width="300px" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell30" runat="server" HorizontalAlign="Left">
                    
                    </asp:TableCell>
               
              </asp:TableRow>
           </asp:Table>
     </div>
            <div>
                <asp:Image ID="Image8" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
            </div> 
            <div>
         <asp:Table ID="Table10" runat="server" Width="742px" Height="20px">
                <asp:TableRow ID="TableRow8" runat="server">
                    <asp:TableCell ID="TableCell22" runat="server" Width="200px">
                        <asp:Label ID="Label7" runat="server" Text="Zona *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell23" runat="server">
                        <asp:DropDownList ID="ddlZonas" Width="300px" runat="server"></asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell24" runat="server" HorizontalAlign="Left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlZonas" 
                       ErrorMessage="Debe seleccionar la zona" ForeColor="Red" ToolTip="Debe seleccionar la zona" ></asp:RequiredFieldValidator>
                
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        
        <div>
                <asp:Image ID="Image10" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
            </div>
            <div>
         <asp:Table ID="Table11" runat="server" Width="742px" Height="20px">
                <asp:TableRow ID="TableRow9" runat="server">
                    <asp:TableCell ID="TableCell25" runat="server" Width="200px">
                        <asp:Label ID="Label8" runat="server" Text="Vendedor *" CssClass="bodyText"></asp:Label>                
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell26" runat="server">
                        <asp:DropDownList ID="ddlVendedores" Width="300px" runat="server"></asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell27" runat="server" HorizontalAlign="Left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlVendedores" 
                       ErrorMessage="Debe seleccionar el vendedor" ForeColor="Red" ToolTip="Debe seleccionar el vendedor" ></asp:RequiredFieldValidator>
                
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        
        <div>
                <asp:Image ID="Image11" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
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

