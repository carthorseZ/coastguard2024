/**
 * Copyright (C) 2004, CodeHouse.com. All rights reserved.
 * CodeHouse(TM) is a registered trademark.
 *
 * THIS SOURCE CODE MAY BE USED FREELY PROVIDED THAT
 * IT IS NOT MODIFIED OR DISTRIBUTED, AND IT IS USED
 * ON A PUBLICLY ACCESSIBLE INTERNET WEB SITE.
 *
 * Script Name: Script Loader
 *
 * You can obtain this script at http://www.codehouse.com
 */
var sl_browser;
var useLink = false;
var sl_scriptLoc ="scripts/";
var sl_lib=sl_scriptLoc+"lib/";
var sl_cssLoc ="css/";
var sl_BaseLoc;
function cssLoader(url,title) {
//alert("test "+url);
	if (typeof (title) == "undefined") {
		if (useLink) {
			document.write('link href="' + url + '" rel="stylesheet" type="text/css"/>');
			return;
		} else {

			document.write('<style type="text/css">');
		}
	} else {
	if (useLink) {
		document.write('link href="' + url + '" rel="stylesheet" type="text/css"/>');
		
	}else {
		document.write('<style type="text/css" title="' + title + '">');
		}
	}
	if (FileExists(url)) {
		document.write('@import "'+url+'";');
		//document.write('<link href="'+sl_cssLoc+url+'" rel="stylesheet" type="text/css"/>');
	} else {
	
		if (FileExists(sl_cssLoc + url)) {
			document.write('@import "'+sl_cssLoc + url+'";');
			//document.write('<link href="'+sl_cssLoc+url+'" rel="stylesheet" type="text/css"/>');
		} else {
	
			if (FileExists("../"+sl_cssLoc+url))
			{
				//document.write('<link href="'+url+'" rel="stylesheet" type="text/css"/>');
			  document.write('@import "../'+sl_cssLoc + url+'";');
			} else {
				  alert("File ["+url+"] does not exists");   
			}
		}
	}
	document.write('</style>');
}

function cssLoaderLink(url,title) {
//alert("test "+url);
	if (typeof(title) == "undefined") {
		document.write('<link href="'+url+'" rel="stylesheet" type="text/css"/>');
		//document.write('<style type="text/css"');
	} else {
		//document.write('<style type="text/css" title="'+title+'">');
		document.write('<link href="'+url+'" rel="stylesheet" type="text/css"/>');
	}
	if (FileExists(url)) {
		//document.write('@import "'+url+'";');
		document.write('<link href="'+sl_cssLoc+url+'" rel="stylesheet" type="text/css"/>');
	} else {
	
		if (FileExists(sl_cssLoc + url)) {
		//    document.write('@import "'+sl_cssLoc + url+'";');
			document.write('<link href="'+sl_cssLoc+url+'" rel="stylesheet" type="text/css"/>');
		} else {
	
			if (FileExists("../"+sl_cssLoc+url))
			{
				document.write('<link href="'+url+'" rel="stylesheet" type="text/css"/>');
			  //document.write('@import "../'+sl_cssLoc + url+'";');
			} else {
				  alert("File ["+url+"] does not exists");   
			}
		}
	}
	//document.write('</style>');
}


function scriptLoader(url,checkParent)
{
	if (typeof(checkParent) == 'undefined') checkParent = true;
	if (FileExists(url)) {
		document.write('<script src="', url, '" type="text/JavaScript"><\/script>');
	 } else {
		if(!checkParent) return;
		if (FileExists("../"+url))
		{
			document.write('<script src="../', url, '" type="text/JavaScript"><\/script>');
		} else {
		if (typeof(errorAlert) == 'undefined') { 
			alert("Can not find script ["+url+"]");
		}
		}
   }
	  
}
function slJQuery(){
//sl_lib = "scripts/lib/";
//scriptLoader(sl_lib + "IE9.js");
	//scriptLoader(sl_lib + "jquery-1.4.2.js");
	scriptLoader(sl_lib + "jquery-1.5.2.js");
scriptLoader(sl_lib + "jquery-ui-1.8.2.custom.min.js");
cssLoader("smoothness/jquery-ui-1.8.2.custom.css");
}

function slAjax() {
	scriptLoader(sl_scriptLoc + "message.js");
	scriptLoader(sl_lib + "json2.js");
	scriptLoader(sl_lib + "wwproxy.js");
	scriptLoader(sl_scriptLoc + "backendcallbacks.js");
	scriptLoader(sl_scriptLoc + "PeriodicTasks.js");
	scriptLoader(sl_scriptLoc + "gen/backendcall.js");
}

function slMisc() {
	GetMyBaseURL();
	scriptLoader(sl_lib + "browserDetect.js");
	scriptLoader(sl_lib + "cookies.js");
	scriptLoader(sl_lib + "jquery.json-2.2.js");
	scriptLoader(sl_lib + "jquery.json2xml.js");
	scriptLoader(sl_lib + "date.format.js");
	scriptLoader(sl_lib + "stacktrace.js");
	scriptLoader(sl_scriptLoc + "logger.js");
	scriptLoader(sl_lib + "URLParam.js");
}

function slJQueryExtras() {
	cssLoader("jquery.timeentry.css");
	cssLoader("demo_table.css","currentStyle");
	cssLoader("Find.css");

	scriptLoader(sl_lib + "jquery.dataTables.js");
	scriptLoader(sl_lib + "DataTablePlugIns.js");
	scriptLoader(sl_lib + "jquery.jeditable.js");
	scriptLoader(sl_lib +"jquery.timeentry.js");
	scriptLoader(sl_lib + "VirtualKeyboard.js");

}

function slAlerts() {
	cssLoader("Alerts.css");
}
function slMenu() {
	cssLoader("Menu.css");
}

function getPage() {
//debugger;
var href = window.location.href;
var page= "";
if (href.indexOf("?") > -1) {
	page = href.substr(0,href.indexOf("?"));
} else page=href;

if (page.lastIndexOf(".") > -1) {
	return page.substr(0,page.lastIndexOf("."));
} else {
return page;
}

}


function getPath() {
	//debugger;
	var href = window.location.href;
	var page = "";
	if (href.indexOf("?") > -1) {
		page = href.substr(0, href.indexOf("?"));
	} else page = href;

	if (page.lastIndexOf("/") > -1) {
		return page.substr(0, page.lastIndexOf("/")+1);
	} else {
	if (page.lastIndexOf("\\") > -1) {
		return page.substr(0, page.lastIndexOf("\\")+1);
	} else {

		return "";
	} 
}

}



function slMain() {

//debugger;
slJQuery();
slAjax();
slMisc();
scriptLoader(sl_scriptLoc + "cb.js");
scriptLoader(sl_scriptLoc + "coastguard.js");


scriptLoader(getPage() + '.js', false);

}
function slLogin() {

	//debugger;
	slJQuery();
	slAjax();
	slMisc();
	scriptLoader(sl_scriptLoc + "cb.js");
	scriptLoader(sl_scriptLoc + "login.js");


	scriptLoader(getPage() + '.js', false);

}


function slSubFrame() {
var temp = document;
//debugger;

slJQuery();
slMisc();
slJQueryExtras();
scriptLoader(sl_scriptLoc + "mainUtils.js");
scriptLoader(getPage()+'.js',false);
}


function FileExists(strURL)
{
oHttp = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP"); 
oHttp.open("HEAD", strURL, false);
oHttp.send();
return (oHttp.status==404) ? false : true;
}

function GetMyBaseURL() {
	sl_BaseLoc = window.location.protocol + "//" + window.location.host;
//    debugger;


}