/*

Rick Strahl
2008
http://www.west-wind.com/WebLog/posts/400990.aspx

*/

function onPageError(xhr,status)
{
    //if (debug) debugger;
    var msg = "Unknown Error";
    if (xhr.responseText)
    {
       try
       {
           var error = JSON2.parse(xhr.responseText);
       }
       catch(e)
       {
           msg = "Non-JSON Response returned.";
       }       
    }
    else
    {
        msg = xhr.statusText || 
              "An unknown error occurred: " + status;
    }
    
    alert(msg);
}

// *** Generic Service Proxy class that can be used to 
// *** call JSON Services generically using jQuery
// *** Depends on JSON2 modified for MS Ajax usage
function serviceProxy(serviceUrl)
{
    var _I = this;
    
    // *** The Url of the service with a backslash
    // *** Example: BasicWcfService.svc/
    this.serviceUrl = serviceUrl;
    
    // *** Call a wrapped object
    this.invoke = function(method,data,callback,error,bare)
    {
        // *** Convert input data into JSON - REQUIRES Json2.js
        var json = JSON2.stringify(data); 
//if(debug)debugger;
        // *** The service endpoint URL        
        var url = _I.serviceUrl + method;       
        
        $.ajax( { 
                    url: url,
                    data: json,
                    type: "POST",
                    processData: false,
                    contentType: "application/json",
                    timeout: 600000, // for testing purposes. TODO: production value should be 10000
                    dataType: "text",  // not "json" we'll parse
                    success: 
                    function(res) 
                    {                                    
                        //if (debug) debugger;
                        if (!callback) return;
                        
                        // *** Use json library so we can fix up MS AJAX dates
                        var result = JSON2.parse(res);
                        
                        // *** Bare message IS result
                        if (bare)
                        { callback(result); return; }
                        
                        // *** Wrapped message contains top level object node
                        // *** strip it off
                        for(var property in result)
                        {
                            callback( result[property] );
                            break;
                        }                    
                    },
                    error:  function(xhr) {
                    //alert("test");
                        if (debug) debugger;
                        if (!error) return;
                        if (xhr.responseText)
                        {
                            //Log("WARN","Webservice returned error ["+xhr+"]");
                            var err = JSON2.parse(xhr.responseText);
                            if (err)
                                error(err); 
                            else    
                                error( { Message: "Unknown server error." })
                        }
                        return;
                    }
                 });   
    }
}
