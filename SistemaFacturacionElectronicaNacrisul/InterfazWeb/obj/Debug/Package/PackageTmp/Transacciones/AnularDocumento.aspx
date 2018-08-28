<%@ Page Language="C#" MasterPageFile="~/MasterPages/Transacciones.Master" AutoEventWireup="true" CodeBehind="AnularDocumento.aspx.cs" Inherits="InterfazWeb.Transacciones.AnularDocumento" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
       
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloMaster.css"/>
    <link rel="Stylesheet" type="text/css" media="all" href="../Styles/EstiloBotones.css"/>
    
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"/>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
            TargetControlID="txtFechaDesde" PopupButtonID="btnFechaDesde" 
            Format="dd/MM/yyyy" />
    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" 
            TargetControlID="txtFechaHasta" PopupButtonID="btnFechaHasta" 
            Format="dd/MM/yyyy" />
    <div class="divTitulo">
    <asp:Label ID="lblReportes" runat="server" Text="Anular Documento"  Font-Bold="true"></asp:Label>
    </div>
    <div>
    </div>
    <div>
    <asp:Table ID="Table1" runat="server" Width="742px">     
                <asp:TableRow ID="TableRow6" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell8" runat="server" Width="210px">
                            <asp:Label ID="lblFechaDesde" runat="server" Text="Fecha Desde *" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell9" runat="server" Width="350px">
                            <asp:ImageButton ID="btnFechaDesde" CausesValidation="false" runat="server" ImageUrl="~/Imagenes/datetime.png" Height="24px" Width="24px" ImageAlign="Right" />                
                            <asp:TextBox ID="txtFechaDesde" runat="server" Width = "300px"></asp:TextBox> 
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell10" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFechaDesde" 
                           ErrorMessage="Debe ingresar la fecha" ForeColor="Red" ToolTip="Debe ingresar la fecha" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </asp:TableCell>
               
                  </asp:TableRow> 
                  <asp:TableRow ID="TableRow7" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell11" runat="server" Width="210px">
                            <asp:Label ID="Label2" runat="server" Text="Fecha Hasta *" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell12" runat="server" Width="350px">
                            <asp:ImageButton ID="btnFechaHasta" CausesValidation="false" runat="server" ImageUrl="~/Imagenes/datetime.png" Height="24px" Width="24px" ImageAlign="Right" />                
                            <asp:TextBox ID="txtFechaHasta" runat="server" Width = "300px"></asp:TextBox> 
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell13" runat="server" HorizontalAlign="Left">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFechaHasta" 
                           ErrorMessage="Debe ingresar la fecha" ForeColor="Red" ToolTip="Debe ingresar la fecha" EnableClientScript="false" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </asp:TableCell>
               
                  </asp:TableRow>    
                  <asp:TableRow ID="TableRow2" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell3" runat="server" Width="210px">
                            <asp:Label ID="Label1" runat="server" Text="Cliente" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell4" runat="server" Width="350px">
                            
                            <asp:DropDownList ID="ddlCliente" Width="300px"  runat="server" ></asp:DropDownList>
                        
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell5" runat="server" HorizontalAlign="Left">
                         
                        </asp:TableCell>
               
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow3" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell7" runat="server" Width="210px">
                            <asp:Label ID="Label3" runat="server" Text="Tipo Documento" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell14" runat="server" Width="350px">
                            <asp:DropDownList ID="ddlTipoDocumento" Width="300px"  runat="server" ></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell16" runat="server" HorizontalAlign="Left">
                        <asp:Label ID="Label7" runat="server" Text="" ></asp:Label>  
                        </asp:TableCell>
               
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow8" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell17" runat="server" Width="210px">
                            <asp:Label ID="Label4" runat="server" Text="Serie" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell18" runat="server" Width="350px">
                            <asp:TextBox ID="txtSerie" Width="300px"  runat="server" ></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell20" runat="server" HorizontalAlign="Left">
                        <asp:Label ID="Label8" runat="server" Text="" ></asp:Label>  
                        </asp:TableCell>
               
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow9" runat="server" Width="742px">
                        <asp:TableCell ID="TableCell21" runat="server" Width="210px">
                            <asp:Label ID="Label5" runat="server" Text="Nro. Serie" CssClass="bodyText"></asp:Label>                
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell22" runat="server" Width="350px">
                            <asp:TextBox ID="txtNroSerie" Width="300px"  runat="server" ></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell24" runat="server" HorizontalAlign="Left">
                        <asp:Label ID="Label9" runat="server" Text="" ></asp:Label>  
                        </asp:TableCell>
               
                  </asp:TableRow> 
                  
        </asp:Table>
        </div>
        <div>
            <asp:Image ID="Image3" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
        </div>
        <div >
        <asp:Table ID="Table3" runat="server">
                 <asp:TableRow ID="TableRow4" runat="server" Width="742px">
                    <asp:TableCell ID="TableCell2" runat="server" Width="742px" HorizontalAlign="Center">
                        <asp:Button ID="btnBuscar" runat="server"  CssClass="botonGenerarReporte" Text="Buscar Documentos" onclick="btnBuscar_Click"  />              
                    </asp:TableCell>
                    
                </asp:TableRow>
        </asp:Table>
            
        </div>
        <div>
            <asp:Image ID="Image1" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
        </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>

            <asp:Table ID="Table10" runat="server">
                <asp:TableRow ID="TableRow20" runat="server">
                    <asp:TableCell ID="TableCell44" runat="server">
                    <div >
                        <asp:GridView ID="gridViewDocumentos" runat="server"  CssClass="grid-view" AutoGenerateColumns ="false" CellSpacing="0" 
                            CellPadding="0" BorderStyle="None" onrowcreated="gridViewDocumentos_RowCreated" AllowPaging ="true"   
                            OnPageIndexChanging="gvCustomersPageChanging"  PageSize="15"
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
                            <asp:TemplateField ItemStyle-Width = "60px"  HeaderText = "Estado">
                                <ItemTemplate>
                                    <asp:Label ID="lblEstado" runat="server"
                                            Text='<%# Eval("Estado")%>'></asp:Label>
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
            
            
        </ContentTemplate>
        </asp:UpdatePanel> 
    
        <div>
            <asp:Image ID="Image2" ImageUrl="~/Imagenes/botonesSep.png" runat="server" Width="742px"/>
        </div>
        <div></div>
    <div >
        <asp:Table ID="Table5" runat="server">
                    <asp:TableRow ID="TableRow1" runat="server" Width="742px">
                    <asp:TableCell ID="TableCell1" runat="server" Width="371px" HorizontalAlign="Right">
                        <asp:Button ID="btnGuardar" runat="server" CssClass="botonGenerarReporte" Text="Anular Documento" onclick="btnGuardar_Click" />              
                    </asp:TableCell>
                        
                </asp:TableRow>
        </asp:Table>
                                    
    </div>
</asp:Content>




