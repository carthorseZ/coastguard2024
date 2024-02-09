<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="CoastGuardWeb.Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<script type="text/javascript">
    function FileUpload_OnChange(sender, e) {
        //debugger;
        parent.myFileName = sender.value;
        
    }
    function submitform() {
        top.addMessage("FileUpload:" + parent.myFileName, "Uploading File " + parent.myFileName, 0);
        top.checkuploadstatus();
        document.myform.submit();
        
    }
    function remove(control) {
        var who = document.getElementsByName(control)[0];
        var who2 = who.cloneNode(false);
        who2.onchange = who.onchange;
        who.parentNode.replaceChild(who2, who); 
    }


</script>
</head>
<body>
    <form id="myform" runat="server" enctype="multipart/form-data">
    <div>
        <asp:FileUpload ID="FileUpload" runat="server" Width="100%" onchange="FileUpload_OnChange(this,event);" />
    </div>
    <p></p>
    </form>
</body>
</html>
