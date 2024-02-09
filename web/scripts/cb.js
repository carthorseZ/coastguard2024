function cb(){
this.test="testing";
var callbacks=new Array();
function addCallBack(Name,evalName,eval)
{
    var existing = null;
    for(var i=0;i < callbacks.length;i++) {
        if (callbacks[i].Name==Name){ 
            existing= callbacks[i];
        break;
        }
    }
    if (existing == null || typeof(existing) == "undefined") {
        
        if (typeof(callbacks.length) != "undefined") id = callbacks.length; 
        callbacks[id] = new Object();
        callbacks[id].Name = Name;
        callbacks[id].eval = new Array();
        callbacks[id].eval[0] = new Object();
        callbacks[id].eval[0].name = evalName;
        callbacks[id].eval[0].eval = eval;
    } else {
        for (var j=0;j<existing.eval.length;j++) {
            if (existing.eval[j].name == evalName) {
                existing.eval[j].eval = eval; 
                return;
            } 
        }
        
        if (typeof(existing.eval)=="undefined"||existing.eval == null) {
            existing.eval = new Array();
        }
        var evalcount = existing.eval.length;        
        existing.eval[evalcount] = new Object();
        existing.eval[evalcount].name = evalName;
        existing.eval[evalcount].eval = eval;        
    }
}
function evoke(Name,param1) {
   for(var i=0;i < callbacks.length;i++) {
        if (Name==callbacks[i].Name){
            for (var j=0;j< callbacks[i].eval.length;j++) {            
                if (typeof(callbacks[i].eval[j]) == "undefined") {
                    
                } else {
                try {
                    //debugger;
                    if (typeof (param1) != undefined) {
                        callbacks[i].eval[j].eval(param1);
                    } else {
                        callbacks[i].eval[j].eval();
                    }
                } catch (err) {
                
                }
                }
            }
        }
   } 
}
this.AddCallBack = addCallBack;
this.Evoke = evoke;


}