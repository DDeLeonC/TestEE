<%@ Page Language="C#" MasterPageFile="~/MasterPages/Transacciones.Master" AutoEventWireup="true" CodeBehind="Resguardo.aspx.cs" Inherits="InterfazWeb.Transacciones.Resguardo" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
       
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css"/>
    
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"/>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
        TargetControlID="txtFecha" PopupButtonID="btnFecha" 
        Format="dd/MM/yyyy" />
    <div class="divTitulo">
    <asp:Label ID="lblReportes" runat="server" Text="Resguardo"  Font-Bold="true"></asp:Label>
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
            <asp:Image ID="Image5" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>
    
      <div>
             <asp:Table ID="Table9" runat="server" Width="742px" Height="20px">
                    <asp:TableRow ID="TableRow13" runat="server">
                        <asp:TableCell ID="TableCell29" runat="server" Width="200px">
                            <asp:Label ID="Label9" runat="server" Text="Cliente *" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell30" runat="server" Width="350px">
                            <asp:DropDownList ID="ddlCliente" Width="300px"  runat="server" ></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell31" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlCliente" 
                           ErrorMessage="Debe seleccionar el cliente" ForeColor="Red" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true" ToolTip="Debe seleccionar el cliente" ></asp:RequiredFieldValidator>
                           
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div>
            <asp:Image ID="Image9" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div>
    <div>
        <asp:Table ID="Table4" runat="server" Width="742px" Height="20px">
            <asp:TableRow ID="TableRow6" runat="server">
                <asp:TableCell ID="TableCell14" runat="server" Width="200px">
                    <asp:Label ID="Label5" runat="server" Text="Seleccionar Retención-Percepción" CssClass="bodyText"></asp:Label>                
                </asp:TableCell>
                
            </asp:TableRow>
            <asp:TableRow ID="TableRow7" runat="server">
                <asp:TableCell ID="TableCell15" runat="server" Width="200px">
                    
                </asp:TableCell>
                
            </asp:TableRow>
            <asp:TableRow ID="TableRow14" runat="server">
                <asp:TableCell ID="TableCell32" runat="server" Width="200px">
                    
                </asp:TableCell>
                
            </asp:TableRow>
            
            <asp:TableRow ID="TableRow5" runat="server">
                <asp:TableCell ID="TableCell1" runat="server" Width="200px">
                    <asp:Label ID="Label4" runat="server" Text="Retención-Percepción:" CssClass="bodyText"></asp:Label>                
                </asp:TableCell>
                <asp:TableCell ID="TableCell10" runat="server">
                    <asp:DropDownList ID="ddlRetencionPercepcion" Width="300px"  runat="server" ></asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell ID="TableCell13" runat="server" HorizontalAlign="Left">
                    <asp:ImageButton ID="Nuevo" ImageUrl="~/Imagenes/Agregar.png" OnClick="btnAgregar_Click" runat="server"></asp:ImageButton>
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
                    
                        <asp:GridView ID="gridViewRetenciones" runat="server"  CssClass="grid-view" AutoGenerateColumns ="false" CellSpacing="0" 
                            CellPadding="0" BorderStyle="None" onrowcreated="gridViewFacturas_RowCreated" AllowPaging ="false"   
                             onrowediting="Editar" 
                            onrowupdating="Modificar" HeaderStyle-BackColor="#464646" HeaderStyle-HorizontalAlign ="Center" HeaderStyle-Height = "25px"
                             >
                            
                            <Columns>
                            <asp:TemplateField ItemStyle-Width = "25px"  HeaderText = "Id" Visible = "false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIdCodigoRetencionPercepcion" runat="server"
                                    Text='<%# Eval("IdCodigoRetencionPercepcion")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "100px"  HeaderText = "Retención-Percepción">
                                <ItemTemplate>
                                    <asp:Label ID="lblCodigoPercepcionRetencion" runat="server"
                                            Text='<%# Eval("CodigoPercepcionRetencion")%>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>
                             <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Tasa">
                                <ItemTemplate>
                                    <asp:Label ID="lblTasa" runat="server"
                                            Text='<%# Eval("tasa")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTasa" runat="server"
                                        Text='<%# Eval("tasa")%>' Width="60px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Monto">
                                <ItemTemplate>
                                    <asp:Label ID="lblMonto" runat="server"
                                            Text='<%# Eval("monto")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtMonto" runat="server"
                                        Text='<%# Eval("monto")%>' Width="60px"></asp:TextBox>
                                </EditItemTemplate> 
                                
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Valor">
                                <ItemTemplate>
                                    <asp:Label ID="lblValor" runat="server"
                                            Text='<%# Eval("valor")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtValor" runat="server"
                                        Text='<%# Eval("valor")%>' Width="60px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField ItemStyle-Width = "60px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRemove" runat="server" 
                                        CommandArgument = '<%# Eval("IdCodigoRetencionPercepcion")%>'
                                     OnClientClick = "return confirm('Desea quitar la retención-percepción?')"
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
        <asp:Image ID="Image11" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
    </div> 
    <div></div>
    <div >
        <asp:Table ID="Table19" runat="server">
                    <asp:TableRow ID="TableRow25" runat="server" Width="742px">
                    <asp:TableCell ID="TableCell2" runat="server" Width="300px" >
                         <asp:Label ID="Label20" runat="server" Text="Adenda:" CssClass="bodyText"></asp:Label>              
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell65" runat="server" >
                         <asp:TextBox ID="txtAdenda" Width="600px" Height="150px" TextMode="MultiLine" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    
                </asp:TableRow>
        </asp:Table>
                                    
    </div>
    <div>
        <asp:Image ID="Image7" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
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
