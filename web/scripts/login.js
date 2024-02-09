var sessionID;
var childNodes;
var proxy;
var serviceURL = 'CGBE.svc/'
var debug = true;
var ServerLoggingDisabled = false;
function Login(UserID,Password,Tenant) {
// Currently trhis will send password in clear text
    // Will Enhanse later
    if (typeof (Tenant) == "undefined") Tenant = "Kaipara";
    //alert('logging in username:' + UserID);
    //alert("Logging in");
 //   debugger;
FELogin(UserID,Password,Tenant);
}



function GotoLogin()
{

    top.window.location.href = "Login.htm";
}

function GotoMain()
{
    if (typeof(sessionID)== "undefined") {
        Log("WARN","GotoLogin Called without session Id");
        return; 
    }
    top.window.location.href = "MainCSS.htm?SessionId="+sessionID;
}
function PopPage(url) {
LogMsg("Poping Main URL[" + url + "]");
$("#mainFrame").attr('src',url);    
}
function PopSidePage(url) {
LogMsg("Poping Side URL[" + url + "]");
$("#sideFrame").attr('src',url);    
}