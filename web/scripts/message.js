function message(){
var messages=new Array();
function addMessage(Name, message, percentcomplete) {
    //debugger;
    var existing = null;
    for(var i=0;i < messages.length;i++) {
        if (messages[i].Name==Name){ 
            existing= messages[i];
        break;
        }
    }
    if (existing == null || typeof(existing) == "undefined") {
        
        if (typeof(messages.length) != "undefined") id = messages.length; 
        messages[id] = new Object();
        messages[id].Name = Name;
        messages[id].Msg = message;
        
        //if (percentcomplete == null || typeof (percentcomplete) == "undefined") {
        //    percentcomplete = 0;
        //}
        messages[id].Complete = 0;
    } else {


    existing.Msg = message;
        existing.Complete = percentcomplete;
    }
    //debugger;
    CBEvents.Evoke("GetMessages", messages)
}


function removeMessage(Name) {
    
    for (var i = 0; i < messages.length; i++) {
        if (messages[i].Name == Name) {
            messages.slice(i, 1);
            return;
        }
    }
    
}

function getMessages() {
return messages;
}
this.AddMessage = addMessage;
this.RemoveMessage = removeMessage;
//this.GetMessgaes = getMessages;
}