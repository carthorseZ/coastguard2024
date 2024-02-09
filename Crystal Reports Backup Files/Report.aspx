<%@ Page Language="C#" AutoEventWireup="true"  Inherits="CoastGuardWeb.Reporting.Report" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Reporting</title>
</head>
<body>
Reporting
    <form id="form1" runat="server">
 	<CR:CrystalReportViewer ID="reportViewer" runat="server" AutoDataBind="False" ReuseParameterValuesOnRefresh="True" EnableDatabaseLogonPrompt="True" EnableParameterPrompt="True" BorderColor="#333333" BorderStyle="None" BorderWidth="1px" HasSearchButton="False" HasToggleGroupTreeButton="False" HasZoomFactorList="False" HyperlinkTarget="" BestFitPage="true"/>
    </form>
    </body>
</html>
