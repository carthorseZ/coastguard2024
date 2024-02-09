//$(document).ready(function() {
//debugger;



//});

$(document).ready(function() {Init();});

function Init(){
Log("INFO", "Initilising Login Page");
//if (debug) debugger;
serviceURL = getPath() + serviceURL;
proxy = new serviceProxy(serviceURL);


// Init function called first 
}
function loginWeb(UserID, Password) {
    // alert("Test");
   // debugger;
    Login(UserID, Password);
}