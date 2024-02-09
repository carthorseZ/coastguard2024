var loggerShouldLog = true;
var debugColour = "#aaa";
var infoColour = "black";
var warnColour = "orange";
var errorColour = "red";
var fatalColour = "red";


function LogMsg(msg,myObject) {
	//debugger;
		Log("DEBUG",msg,myObject);
	
}

function Log(level,msg,myObject) {
	switch (typeof(myObject)) {
		case "undefined":
			WriteLog(level, msg);
			break;
		case "object":
			//debugger;
			var options = { formatOutput: true };
			var myMsgObject = $.json2xml(myObject, options);
			WriteLog(level, msg + myMsgObject);
			break;
		default:
		WriteLog(level,msg + " Object logging of type "+typeof(myObject)+ " is not currently supported");
		break;
	}

}

function WriteLog(level, msg)
{
	//if (debug) debugger;
	if (!loggerShouldLog) return;
	if (typeof(msg)== "undefined") {
		msg = level;
		level = "DEBUG";
	}
	var colour = debugColour;
	var weight = "normal";
	switch (level)
	{
		case "INFO":
			colour = infoColour;
			level = level + "&nbsp;";
			BELogMsg(level + ":" + msg,1)
			break;
		case "WARN":
			colour = warnColour;
			level = level + "&nbsp;";
			BELogMsg(level + ":" + msg,2)
			break;

case "ERROR":

	colour = errorColour;

	try {

		BELogMsg(level + ":" + msg, 3)

	} catch (e) {

		debugger;

	}

	break;
		case "FATAL":
			colour = fatalColour;
			weight = "bold";
			BELogMsg(level + ":" + msg,4)
			break;
		case "DEBUG":
			BELogMsg(level + ":" + msg)
			break;	
	}

	$("#logArea").prepend("<div style='font-family:Consolas'><span style='color:"+colour+";font-weight:"+weight+"'>" + level + "&nbsp;&nbsp;&nbsp;&nbsp;" + msg + "</span></div>");
}
function Debug(msg)
{
	Log("DEBUG", msg);
}
function Info(msg)
{
	Log("INFO", msg);
}
function Warn(msg)
{
	Log("WARN", msg);
}
function Error(msg)
{
	Log("ERROR", msg);
}
function Fatal(msg)
{
	Log("FATAL", msg);
}