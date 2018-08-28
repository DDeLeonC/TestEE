<%@ Page Language="C#" MasterPageFile="~/MasterPages/Reportes.Master" AutoEventWireup="true" CodeBehind="VisorExcel.aspx.cs" Inherits="InterfazWeb.Reportes.VisorExcel" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#dialog").dialog();
    });
  </script>

<div id="dialog" title="Dialog Title">
 <iframe src="webformExcel.aspx" width="100%" height = "400px">
    </iframe>
</div>
</asp:Content>
