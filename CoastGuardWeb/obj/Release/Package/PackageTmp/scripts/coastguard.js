var sessionID;
var childNodes;
var proxy;
//var baseURL;
var serviceURL = 'CGBE.svc/'
var debug = true;
var Skippers;
var Crew;
var Courses;
var Drills;
var Destinations;
var CRVs;
var ActivityTypes;
var Alerts;
var AlertDetails;
var Procedures;
var Scenarios;
var CurrentActivity;
var BlankActivity;
var BlankUser;
var BlankCourse;
var BlankDrill;
var BlankActivityType;
var BlankAttachment;
var BlankAlert;
var BlankProcedure;
var BlankScenario;
var BlankEngine;
var BlankCRV;
var Reports;
var Session;
var curTideHeights;
var ServerLoggingDisabled = false;
var altVisible = true;
var mainPageAlerts = true;
var ports = ["PORT TARANAKI","Helensville","Shelly Beach","Pouto Point","Whakapirau","Dargaville"];
var CBEvents = new cb();
var curMenuOption = -1;
var Messages = new message();
function Login(UserID,Password,Tenant) {
// Currently trhis will send password in clear text
// Will Enhanse later
    if (typeof (Tenant) == "undefined") Tenant = "Kaipara";
    //alert('logging in username:' + UserID);
    LogMsg("Info", "Logging in user" + UserID);
FELogin(UserID,Password,Tenant);
}

$(document).ready(function() {Init();});

function Init(){
//if (debug) debugger;
    sessionID = $.getURLParam('SessionId');
    serviceURL = getPath() + serviceURL;
proxy = new serviceProxy(serviceURL);

setFrameSize();
//$('#ISideFrame').hide();
//$('#iHeader').hide();
try {
   //debugger;
    $('#divLogger').hide();
    Session = getSession();
    try {
        Skippers = getSkippers();
    } catch (ex) {
    Log("WARN", "Error [" + err + "] in get Skippers");
    }
Crew = getCrew();
Courses = getCourses();
Drills = getDrills();
ActivityTypes = getActivityTypes();
Destinations = getDestinations();

//Alerts = getAlerts();
AlertDetails = getAlertDetails();
Procedures = getProcedures();
Scenarios = getScenarios();

BlankActivity = getBlankActivity();
BlankDrill = getBlankDrill();
BlankActivityType = getBlankActivityType();
BlankAlert = getBlankAlert();
BlankAttachment = getBlankAttachment();
BlankProcedure = getBlankProcedure();
BlankScenario = getBlankScenario();
//debugger;
BlankUser = getBlankUser();
BlankCourse = getBlankCourse();
//debugger;
BlankEngine = getBlankEngine();
BlankCRV = getBlankCRV();
curTideHeights=getTideHeights(new Date(),ports);
} catch (err) {
    Log("WARN","Error [" + err +"] in get Skippers");
}
PopMain();
//PopPage('Index.htm');
//PopSidePage('Index.htm');
//StartEventPolling(10);
//CBEvents.AddCallBack('GetTideHeights',test);
// Init function called first
setTimeout("CheckAlerts(true)", 1000);

getMainMenu();
//$(window).resize(myResize());


$(window).bind('resize', function () { myResize(); });  
}

function myResize() {
    setFrameSize();
}

function getMainMenu() {
    CBEvents.AddCallBack('GetMenu', 'processMainMenu', processMainMenu);
    BEGetMenu();
}

function processMainMenu(menu) {
    var myHtml = '';
    var prevItemType = -1;
    for (var i = 0; i < menu.Options.length; i++) {
        switch (menu.Options[i].type) {
            case 1:
                if (prevItemType > 1) myHtml += "</div>";

                if (menu.Options[i].link != null) {
                    myHtml += '<p class="menu_head"  onclick="PopPage(';
                    myHtml += "'" + menu.Options[i].link + "'";
                    myHtml += ')">' + menu.Options[i].Name + '</p>';
                } else {
                    myHtml += '<p class="menu_head">' + menu.Options[i].Name + '</p>';
                }
                // Header Bar
                break;
            default:
                if (prevItemType < 2) {
                    if (menu.Options[i].link != null) {
                        myHtml += '<div class="menu_body"><a onclick="PopPage('
                        myHtml += "'" + menu.Options[i].link + "'";
                        myHtml += ')">' + menu.Options[i].Name + '</a>';
                    } else {
                        myHtml += '<div class="menu_body"><a>' + menu.Options[i].Name + '</a>';
                    }
                } else {
                    if (menu.Options[i].link != null) {
                        myHtml += '<a onclick="PopPage('
                        myHtml += "'" + menu.Options[i].link + "'";
                        myHtml += ')">' + menu.Options[i].Name + '</a>';
                    } else {
                        myHtml += '<a>' + menu.Options[i].Name + '</a>';
                    }
                }
                // All others Noprmal Link
                break;
        }
        prevItemType = menu.Options[i].type;
    }
    if (prevItemType > 2) myHtml += "</div>";

$("#firstpane").html(myHtml);

$("#firstpane p.menu_head").mouseover(function () {
    var myIndex = $(this).parent().children().index($(this));
    if (curMenuOption != myIndex) {
        $(this).addClass("menu_head_alt").siblings("div.menu_body").slideUp("slow");
        $(this).addClass("menu_head_alt").next("div.menu_body").slideDown(1000);
        curMenuOption = myIndex;
    } else {
        
    }
    // $(this).addClass("menu_head_alt").next("div.menu_body").slideDown(1000).siblings("div.menu_body").slideUp("slow");

    $(this).siblings().removeClass("menu_head_alt");
});
}


function CheckAlerts(setup) {
    if (typeof (setup) == "undefined" || !setup) {
        getAlerts(true);
    } else {
        if (mainPageAlerts) {
            CBEvents.AddCallBack('GetAlerts', 'gotMainAlerts', gotMainAlerts);
            CBEvents.AddCallBack('GetMessages', 'gotMainMessages', gotMainMessages);
            getAlerts(true);
        } 
    }

}


function gotMainAlerts(myAlerts) {
    DisplayAlerts('#AlertTable', myAlerts);
}

function DisplayAlerts(alertTable, Alerts) {
    $(alertTable).find("tbody").html("");
    var tr = "";
    for (var i = 0; i < Alerts.length; i++) {
        var cssclass = ' class="Alert' + Alerts[i].Level + '1"';
        var cssclass2 = ' class="Alert' + Alerts[i].Level + '2"';
        tr += "<tr><td" + cssclass + ">" + Alerts[i].Level + "</td><td" + cssclass2 + ">" + Alerts[i].Text + "</td></tr>";
    }
    $(alertTable + ' > tbody:last').append(tr);
    //debugger;
}
function DisplayMessages(MsgTable, msgs) {
    //debugger;


    $(MsgTable).find("tbody").html("");
    var tr = "";
    for (var i = 0; i < msgs.length; i++) {
        var cssclass = ' class="Message';
        if (msgs[i].Complete >= 0 && msgs[i].Complete <= 100) {
            tr += "<tr><td" + cssclass + '1">' + msgs[i].Msg + "<td" + cssclass + '2">' + Math.round(msgs[i].Complete * 100) / 100 + "% Complete</td><td" + cssclass + '3">' + '<div id="pBar' + msgs[i].Name.replace(/[^a-zA-Z 0-9]+/g, '') + '"></div>' + "</td></tr>";
        } else {
            tr += "<tr><td" + cssclass + ' colspan="3">' + msgs[i].Msg + "</td></tr>";
        }
    }

    $(MsgTable + ' > tbody:last').append(tr);

    for (var j = 0; j < msgs.length; j++) {
        if (msgs[j].Complete >= 0 && msgs[j].Complete <= 100) {
            try {
                //          debugger;
                Log("Info", "progress bar[" + "#pBar" + msgs[j].Name + "]");
                $("#pBar" + msgs[j].Name.replace(/[^a-zA-Z 0-9]+/g, '')).progressbar({ value: msgs[j].Complete });
            } catch (e) {
                //debugger;
            }
        }
    }
    //debugger;


}


function gotMainMessages() {
    DisplayMessages('#MessageTable', myAlerts);
}

function getSkippers(reload){
    //if (debug) debugger;
    if (typeof(Skippers) == undefined || Skippers == null||(typeof(reload) != undefined && reload)) {
        BEGetSkippers();
    }
    //if (debug) debugger;
    return Skippers;
}
function getBlankActivity(reload){
//if (debug) debugger;
    if (typeof(BlankActivity) == undefined || BlankActivity == null||(typeof(reload) != undefined && reload)) {
        BENewActivity();
    }
    //if (debug) debugger;
    return BlankActivity;
}
function getBlankCourse(reload) {
    //if (debug) debugger;
    if (typeof (BlankCourse) == undefined || BlankCourse == null||(typeof(reload) != undefined && reload)) {
        BENewCourse();
    }
    //if (debug) debugger;
    return BlankCourse;
}
function getBlankDrill() {
    //if (debug) debugger;
    if (typeof (BlankDrill) == undefined || BlankDrill == null) {
        BENewDrill();
    }
    //if (debug) debugger;
    return BlankDrill;
}
function getBlankEngine() {
    //if (debug) debugger;
    CBEvents.AddCallBack('NewObject:Engine', 'NewObject:Engine', gotBlankEngine);
    if (typeof (BlankEngine) == undefined || BlankEngine == null) {
        BENewObject('Engine');
    }
    //if (debug) debugger;
    return BlankEngine;
}

function getBlankCRV() {
    //if (debug) debugger;
    CBEvents.AddCallBack('NewObject:CRV', 'NewObject:CRV', gotBlankCRV);
    if (typeof (BlankCRV) == undefined || BlankCRV == null) {
        BENewObject('CRV');
    }
    //if (debug) debugger;
    return BlankCRV;
}
function getBlankActivityType() {
    //if (debug) debugger;
    
    if (typeof (BlankActivityType) == undefined || BlankActivityType == null) {
        BENewActivityType();
    }
    //if (debug) debugger;
    return BlankActivityType;
}

function getBlankAttachment() {
    //if (debug) debugger;
    CBEvents.AddCallBack('NewObject:Attachment', 'NewObject:Attachment', gotBlankAttachment);
    if (typeof (BlankAttachment) == undefined || BlankAttachment == null) {
        BENewObject('Attachment');
    }
    //if (debug) debugger;
    return BlankAttachment;
}

function gotBlankAttachment(attachment)
{
    //debugger;
    
    BlankAttachment = attachment;
}

function gotBlankEngine(retval) {
    //debugger;

    BlankEngine = retval;
}

function gotBlankCRV(retval) {
    //debugger;

    BlankCRV = retval;
}



function getBlankAlert() {
    //if (debug) debugger;
    if (typeof (BlankAlert) == undefined || BlankAlert == null) {
        BENewAlert();
    }
    //if (debug) debugger;
    return BlankAlert;
}
function getBlankProcedure() {
   // if (debug) debugger;
    if (typeof (BlankProcedure) == undefined || BlankProcedure == null) {
        BENewProcedure();
    }
    //if (debug) debugger;
    return BlankProcedure;
}
function getBlankScenario() {
    //if (debug) debugger;
    if (typeof (BlankScenario) == undefined || BlankScenario == null) {
        BENewScenario();
    }
    //if (debug) debugger;
    return BlankScenario;
}
function getBlankUser(){
//if (debug) debugger;
    if (typeof(BlankUser) == undefined || BlankUser == null) {
        BENewUser();
    }
    //if (debug) debugger;
    return BlankUser;
}

function getCrew(){
//if (debug) debugger;
    if (typeof(Crew) == undefined || Crew == null) {
        BEGetCrew();
    }
    //if (debug) debugger;
    return Crew;
}

function getSession(reload) {
    //if (debug) debugger;
    if (typeof (Session) == undefined || Session == null||(typeof(reload) != undefined && reload)) {
        BEGetSession();
    }
    //if (debug) debugger;
    return Session;
}

function addMessage(Name, message, percentcomplete) {
    Messages.AddMessage(Name, message, percentcomplete);
}

function addActivity(newActivity){
//if (debug) debugger;
    //if (typeof(Crew) == undefined || Crew == null) {
    var activity = PrepareActivity(newActivity)
    //var jsonNewActivity = JSON2.stringify(Activity);
    Log("DEBUG","Saving Activity",activity);
        BEAddActivity(activity);
    //}
    //if (debug) debugger;
    //return Crew;
    }

    function modifyCourse(course) {
        Log("DEBUG", "Saving Course",course);
        BEModifyCourse(course);
    }
    function modifyDrill(drill) {
        Log("DEBUG", "Saving Drill", drill);
        BEModifyDrill(drill);
    }
    //function modifyCRV(crv) {
    //    Log("DEBUG", "Saving CRV", crv);
    //    modifyObject('CRV', crv);
    //}
    function modifyActivityType(activityType) {
        Log("DEBUG", "Saving ActivityType",activityType);
        //BEModifyActivityType(activityType);
        modifyTable("ActivityType",activityType);
    }
    function modifyAttachment(attachment) {
        Log("DEBUG", "Saving Attachment",attachment);
        //ActivityType(activityType);
        BEModifyAttachment( attachment);
    }
    function modifyRoleType(Type) {
        Log("DEBUG", "Saving RoleType [" + Type + "]");
        //BEModifyActivityType(activityType);
        modifyTable("RoleType", Type);
    }
    function modifyObject(objName,Obj) {
        //Log("DEBUG", "Saving RoleType [" + Type + "]");
        //BEModifyActivityType(activityType);
        //debugger;
        modifyTable(objName, Obj);
    }
    function modifyAlert(alert) {
        Log("DEBUG", "Saving Alert [" + alert + "]");
        BEModifyAlert(alert);
    }
    function modifyProcedure(procedure) {
        //debugger;
        Log("DEBUG", "Saving Procedure [" + procedure + "]");
        BEModifyProcedure(procedure);
    }
    function modifyProcedureInfo(procedure) {
     //   debugger;
        Log("DEBUG", "Saving Procedure [" + procedure + "]");
        procedure.DateCompleted = JSONDate(procedure.DateCompleted);
        BEModifyProcedureInfo(procedure);
    }
    function modifyPassangerInfo(passanger) {
        //debugger;
        Log("DEBUG", "Saving Passanger [" + passanger + "]");
        //procedure.DateCompleted = JSONDate(procedure.DateCompleted);
        BEModifyActivityPassanger(passanger);
    }
    function modifyScenario(scenario) {
        Log("DEBUG", "Saving Scenario [" + scenario + "]");
        BEModifyScenario(scenario);
    }
    function modifyTable(TableName,Object) {
        var jsonNewActivity = JSON2.stringify(Object);
        Log("DEBUG", "Saving " + TableName + " [" + jsonNewActivity + "]");
        BEModifyTable(TableName, Object);
    }

    function modifyEngine(inval) {
        modifyTable('Engine', inval);
    }
    function modifyCRV(inval) {
        //debugger;
        Log("DEBUG", "Saving Coastguard rescue vessel [" + inval + "]");
        //procedure.DateCompleted = JSONDate(procedure.DateCompleted);
        BEModifyCoastguardRescueVessel(inval);
    }

    function modifyCoastGuardMember(member){
    Log("DEBUG","Saving Member ["+member+"]");
        BEModifyCoastGuardMember(member);
}


function getTideHeights(dt,ports){
    BEGetTideHeights(JSONDateTime(dt, new Date()), ports);
        }

function getCourses(reload){
//if (debug) debugger;
    if (typeof(Courses) == undefined || Courses == null||(typeof(reload) != undefined && reload)) {
        BEGetCourses();
    }
    //if (debug) debugger;
    return Courses;
}
function getDrills(reload) {
    //if (debug) debugger;
    if (typeof (Drills) == undefined || Drills == null||(typeof(reload) != undefined && reload)) {
        BEGetDrills();
    }
    //if (debug) debugger;
    return Drills;
}
function getActivityTypes(reload) {
    //if (debug) debugger;
    if (typeof (ActivityTypes) == undefined || ActivityTypes == null || (typeof (reload) != undefined && reload)) {
        BEGetActivityTypes();
    }
    //if (debug) debugger;
    return ActivityTypes;
}

function getDestinations(reload) {
    //if (debug) debugger;
    if (typeof (Destinations) == undefined || Destinations == null || (typeof (reload) != undefined && reload)) {
        top.CBEvents.AddCallBack('GetObjects:Destinations', 'GetObjects:Destinations', gotDestinations);
        BEGetObjects("Destinations","1=1");
    }
    //if (debug) debugger;
    return Destinations;
}
function getCRVs(reload) {
    // debugger;
    if (typeof (CRVs) == undefined || CRVs == null || (typeof (reload) != undefined && reload)) {
        top.CBEvents.AddCallBack('GetObjects:CRVs', 'GetObjects:CRVs', gotCRVs);
        BEGetObjects("CRVs", "1=1");
    }
    //if (debug) debugger;
    return CRVs;
}
function gotCRVs(newVal) {
     //debugger;
    CRVs = newVal;
}


function gotDestinations(newVal) {
    //if (debug) debugger;
    Destinations = newVal;
    }

function getAlerts(reload) {
    //if (debug) debugger;
    if (typeof (Alerts) == undefined || Alerts == null||(typeof(reload) != undefined && reload)) {
        BEGetAlerts();
    }
    //if (debug) debugger;
    return Alerts;
}
function getAlertDetails(reload) {
    //if (debug) debugger;
    if (typeof (AlertDetails) == undefined || AlertDetails == null || (typeof (reload) != undefined && reload)) {
        BEGetAlertDetails();
    }
    //if (debug) debugger;
    return AlertDetails;
}
function getProcedures(reload) {
    //if (debug) debugger;
    if (typeof (Procedures) == undefined || Procedures == null||(typeof(reload) != undefined && reload)) {
        BEGetProcedures();
    }
    //if (debug) debugger;
    return Procedures;
}
function getScenarios(reload) {
    //if (debug) debugger;
    if (typeof (Scenarios) == undefined || Scenarios == null||(typeof(reload) != undefined && reload)) {
        BEGetScenarios();
    }
    //if (debug) debugger;
    return Scenarios;
}



function GotoLogin()
{
    top.window.location.href = "Login.htm";
}

function GotoMain()
{
    top.window.location.href = "MainCSS.htm";
}
function PopRootPage(url) {
    //url = "main\\" + url;
    //url = "main/" + url;
    LogMsg("Poping Root URL[" + url + "]");
    //$("#mainFrame").attr('src',url);    
    $("#IMainFrame").attr('src', url);
}

function PopMain() {
    altVisible = true;
    $("#divMainFrame").hide();
    $("#divMainMenu").show();
    for (var i = 0; i < 1; i++) {
        $("#divAltFrame" + (i + 1)).hide();
    }

}
function PopPage(url) {
    if (url == "main") {
        PopMain();
        return;
    }

//url = "main\\" + url;
    url = "main/" + url;
LogMsg("Poping Main URL[" + url + "]");
//$("#mainFrame").attr('src',url);
$("#IMainFrame").attr('src', url);
if (altVisible) {
    showMain();
    altVisible = false;
    
}
}
function showMain() {
    //debugger;
    $("#divMainFrame").show();
    $("#divMainMenu").hide();
    for (var i = 0; i < 1; i++) {
        $("#divAltFrame" + (i+1)).hide();
    }
}

function PopSidePage(url) {
    //url = "sidebar\\" + url;
    url = "sidebar/" + url;
LogMsg("Poping Side URL[" + url + "]");
$("#ISideFrame").attr('src',url);
}

function PopAltPage(url, page) {
    //debugger;
    if (typeof(page) == "undefined") { page =1};
    //url = "main\\" + url;
    url = "Alt/" + url;
    LogMsg("Poping alt URL[" + url + "]");
    //$("#mainFrame").attr('src',url);
    $("#IAltFrame" + page).attr('src', url);
    $("#divAltFrame" + page).show();
    $("#divMainMenu").hide();
    $("#divMainFrame").hide();
    
    altVisible = true;
}

function PrepareActivity(Activity) {
   // if (debug) debugger;
    var tripDate;
    //if (Activity.date.length = 8) {
     //   debugger
     //   tripDate = toDate(inVal.substring(0, 6) + '20' + inVal.substring(6));
    //} else {
        tripDate = toDate(Activity.date);
        //}
Activity.POB = toDecimal(Activity.POB);
Activity.WaterTemp = toDecimal(Activity.WaterTemp);
Activity.AirTemp = toDecimal(Activity.AirTemp);
Activity.BarometricPreasure = toDecimal(Activity.BarometricPreasure);
//if (debug) debugger;
Activity.DepartTime = JSONDateTime(toDateTime(Activity.DepartTime, tripDate));
Activity.ReturnTime = JSONDateTime(toDateTime(Activity.ReturnTime, tripDate));
if (Activity.ReturnTime < Activity.DepartTime) {
    alert("In valid depart time");
}


Activity.date = JSONDate(tripDate);
Activity.SafetyCheck = toBool(Activity.SafetyCheck);
Activity.SafetyCheckComment = toString(Activity.SafetyCheckComment);
Activity.SafetyCheckInitial = toString(Activity.SafetyCheckInitial);

Activity.WeeklyChecks = toBool(Activity.WeeklyChecks);
Activity.WeeklyChecksComment = toString(Activity.WeeklyChecksComment);
Activity.WeeklyChecksInitial = toString(Activity.WeeklyChecksInitial);

Activity.MonthlyChecks = toBool(Activity.MonthlyChecks);
Activity.MonthlyChecksComment = toString(Activity.MonthlyChecksComment);
Activity.MonthlyChecksInitial = toString(Activity.MonthlyChecksInitial);

Activity.Maintenance = toBool(Activity.Maintenance);
Activity.MaintenanceComment = toString(Activity.MaintenanceComment);
Activity.MaintenanceInitial = toString(Activity.MaintenanceInitial);

Activity.RadioCheck = toBool(Activity.RadioCheck);
Activity.RadioCheckComment = toString(Activity.RadioCheckComment);
Activity.RadioCheckInitial = toString(Activity.RadioCheckInitial);

Activity.StartupCheck = toString(Activity.StartupCheck);

Activity.StartupProcedures = toBool(Activity.StartupProcedures);
Activity.StartupProceduresComment = toString(Activity.StartupProceduresComment);
Activity.StartupProceduresInitial = toString(Activity.StartupProceduresInitial);

Activity.Actions = toString(Activity.Actions);
Activity.CompletedBy = toString(Activity.CompletedBy);
Activity.SafetyOfficerAdvised = toBool(Activity.SafetyOfficerAdvised);
Activity.DateSafetyOfficerAdvised = JSONDateTime(Activity.DateSafetyOfficerAdvised);
Activity.EnginePortFuel = toDecimal(Activity.EnginePortFuel);
Activity.EngineFuelLitres = toDecimal(Activity.EngineFuelLitres);
Activity.EngineFuelDollars = toDecimal(Activity.EngineFuelDollars);
Activity.EnginePortOilAdded = toDecimal(Activity.EnginePortOilAdded);
Activity.EnginePortRunHours = toDecimal(Activity.EnginePortRunHours);
Activity.EnginePortTotalHours = toDecimal(Activity.EnginePortTotalHours);
Activity.EngineStarboardFuel = toDecimal(Activity.EngineStarboardFuel);
Activity.EngineStarboardOilAdded = toDecimal(Activity.EngineStarboardOilAdded);
Activity.EngineStarboardRunHours = toDecimal(Activity.EngineStarboardRunHours);
Activity.EngineStarboardTotalHours = toDecimal(Activity.EngineStarboardTotalHours);
Activity.FirepumpStarted = toBool(Activity.FirepumpStarted);
Activity.NavHazards = toInt(Activity.NavHazards);
Activity.NavHazardsDetails = toString(Activity.NavHazardsDetails);
Activity.PostOperationChecks = toBool(Activity.PostOperationChecks);
Activity.SecondaryWeather = toString(Activity.SecondaryWeather);
if (typeof (Activity.Skipper) == "undefined" || Activity.Skipper == "undefined") {
    Activity.Skipper = '00000000-0000-0000-0000-000000000000';
} 




return Activity;
}

function newGuid() {
    return '00000000-0000-0000-0000-000000000000';
}

function isInt(value) {
    return !isNaN(value) && parseInt(value) == value;
}

function isGuid(inVal) {
    try {
        if (typeof (inVal) == "undefined" || inVal == null) return false;
        if (inVal.length == 36) return true;
        return false;
    } catch (e) {
        return false;
    }
}
function trimNumber(s) {
    while (s.substr(0, 1) == '0' && s.length > 1) { s = s.substr(1, 9999); }
    return s;
}

function toInt(inVal,Default){
    try {
        //debugger;
        if (typeof(Default) == "undefined") {
            Default = 0;
        }
        var retval;
        if (typeof (inVal) == "string") {
            retval = parseInt(trimNumber(inVal));
        } else {
        retval = inVal;
        }
        if (typeof (retval) == "undefined" || retval == "NaN" || isNaN(retval)) {

          //  debugger;
            retval = Default;
        }
        return retval;
    } catch (err) {
    //debugger;
    Log("WARN",'An error ['+err+'] Occured while processing to toInt for value ['+inVal+']');
        return Default;
    }
    
}

function toBool(inVal) {
if (inVal == "on") return true ;
return Boolean(inVal);

}

function toDecimal(inVal, Default) {
    try {
        //debugger;
        if (typeof (Default) == "undefined") {
            Default = 0;
        }
        var retval;
        if (typeof (inVal) == "string") {
            retval = parseFloat(trimNumber(inVal));
        } else {
            retval = inVal;
        }
        if (typeof (retval) == "undefined" || retval == "NaN" || isNaN(retval)) {

            //  debugger;
            retval = Default;
        }
        return retval;
    } catch (err) {
        //debugger;
        Log("WARN", 'An error [' + err + '] Occured while processing to toDecimal for value [' + inVal + ']');
        return Default;
    }

}


function JSONDateTime(inVal,Default) {
    //retval = new Date();
    //if (debug) debugger;
    if (inVal == null) {
        return null;
        //debugger;
        //var trace = printStackTrace();
        //throw "null passed to JSONDateTime as inVal by [" + arguments.callee.caller.toString() + "] Trace [" + trace+ "]";
        
    }
    
    if (typeof(Default) == "undefined") Default = new Date();
    if (typeof(inVal) == "undefined") 
    { 
        //return Default;
        inVal = Default;
    }
    try {
        var tmpSecs = (inVal.getTime() - (inVal.getTimezoneOffset() * 60 * 1000));
    return "\/Date(" + tmpSecs + ")\/";
    } catch (err) {
       Log("WARN","Unable to Seralise Date [" + inVal +"] in toDate Error ["+err+"]");
       return Default;
    }
}
function JSONDate(inVal) {
    //retval = new Date();
    //if (debug) debugger;

    var type = typeof (inVal);
    
    if (type == "undefined") {
        return new Date();

    }
    //if (inVal.length = 8) {
    //    debugger
    //    inVal = inVal.substring(0, 6) + '20' + inVal.substring(6);
    //}
    if (type == "string") {
        inVal = new Date(inVal);
    }
    var tmpSecs = (inVal.getTime() - (inVal.getTimezoneOffset() * 60*1000));
    
        return "\/Date(" + tmpSecs + ")\/";
    
}

function toDate(inVal) {
    //retval = new Date();
    //if (debug) debugger;

    var type = typeof (inVal);
    var tmpDate;
    switch (type) {
        case "undefined":
        tmpDate=new Date();
        break;
    case "string":
        if (inVal.indexOf("/") > 1) {
            var pos1 = inVal.indexOf("/");
            var pos2 = inVal.indexOf("/", pos1 + 1);
            var tempDay = inVal.substr(0, pos1);
            var tempMonth = inVal.substr(pos1 + 1, (pos2 - pos1 - 1));
            var tempYear = toInt(inVal.substr(pos2 + 1));
            if (tempYear > 0 && tempYear < 50) tempYear = tempYear + 2000;
            tmpDate = new Date(tempYear, tempMonth, tempDay);

        } else {
            tmpDate = new Date(inVal);
        }

        break;
    case "object":
        tmpDate = inVal;
        break;
        
    }



    return new Date(tmpDate.getFullYear(), tmpDate.getMonth() - 1, tmpDate.getDate()) ;

}


function toString(inVal,Default){
if (typeof(Default) == "undefined") Default = "";
if (typeof(inVal) == "undefined"||inVal == null) {
    return Default
} else {
    return inVal;
}

}

function DateCopy(inDat) {
    var inDate = new Date();
    return new Date(inDate.getTime());
}

function toDateTime(inVal,inDate,Default)
{
//if (debug) debugger;
var retval = new Date(); 
// Expecting Date Time in Format HH:mmTT
if (typeof(inDate) != "undefined") {
    retval = DateCopy(inDate);
    
}
if (typeof(Default) == "undefined") {
    Default = new Date();
}
try {
    if (typeof(inVal) == "string" && inVal.length == 7) {
       var Hours = toInt(inVal.substring(0,2));
       var Minutes = toInt(inVal.substring(3,5));
       var PM = inVal.substring(5,7);
       if (PM == "PM" || PM == "pm") {
       if (Hours != 12) Hours += 12;  
       } else {
         if (Hours == 12) Hours = 0;
       }
       //if (debug) debugger;
       retval.setHours(Hours);
       retval.setMinutes(Minutes);
       retval.setSeconds(0);
       retval.setMilliseconds(0);
       return retval;
   } else {
   if (typeof (inVal) == "string" && inVal.length == 5) {
       Hours = toInt(inVal.substring(0, 2));
       Minutes = toInt(inVal.substring(3, 5));
       //if (debug) debugger;
       retval.setHours(Hours);
       retval.setMinutes(Minutes);
       retval.setSeconds(0);
       retval.setMilliseconds(0);
       return retval;
   
   } else {
   Log("WARN", "Unable to parse date [" + inVal + "] in toDate");
   return Default;
   }
    }
} catch (err) {
    //if (debug) debugger;
    Log("WARN","Error [" + err +"] in toDate");
    return Default;
}
}

function test(){
alert("test");
}

function setFrameSize(){
var curheight = $("body").height();
var curwidth = $("body").width();
var headerheight = 200;
var sidewidth = 200;
//$('#iHeader').css("width", sidewidth + 'px');
//$('#iHeader').css("height", (headerheight) + 'px');

//$('#ISideFrame').css("width", sidewidth + 'px');
//$('#ISideFrame').css("height", (curheight - headerheight) + 'px');
$('#IMainFrame').css("left", (sidewidth) + 'px');
$('#IMainFrame').css("width", (curwidth-sidewidth) + 'px');
$('#IMainFrame').css("height", (curheight - headerheight) + 'px');

}

function setWidth(newWidth) {
    //var bodyWidth = newWidth();
    //debugger;

    if (newWidth < 0) {
        $("body").width(WindowWidth());
    } else {
        $("body").width(newWidth+250);
    }
    setFrameSize();
}


function updateTopFrameSize(newHeight) {
    try {
        //debugger;
        //var oBody = $('#IMainFrame').document.body;
        //var oFrame = $('#IMainFrame');
        //oFrame.style.height = newHeight;
        //$('#iHeader').css("height", (newHeight) + 'px');
        //$('#IMainFrame').css("height", (newHeight) + 'px');
        //oFrame.style.height = oBody.scrollHeight + (oBody.offsetHeight - oBody.clientHeight);
        //oFrame.style.width = oBody.scrollWidth + (oBody.offsetWidth - oBody.clientWidth);
    }
    //An error is raised if the IFrame domain != its container's domain
    catch (e) {
        window.status = 'Error: ' + e.number + '; ' + e.description;
    }
}


function updateFrameSize(newHeight) {
    try {
        //debugger;
        //var oBody = $('#IMainFrame').document.body;
        //var oFrame = $('#IMainFrame');
        //oFrame.style.height = newHeight;
        //$('#ISideFrame').css("height", (newHeight) + 'px');
        $('#IMainFrame').css("height", (newHeight+50) + 'px');
        $('#divMainFrame').css("height", (newHeight+50) + 'px');
        if (newHeight < 400) {
            $('#divMainFrame').css("top", (200) + 'px');
        } else {
            $('#divMainFrame').css("top", (200) + 'px');
        }


        //$('#Body').css("height", (newHeight + 200) + 'px');

        //oFrame.style.height = oBody.scrollHeight + (oBody.offsetHeight - oBody.clientHeight);
        //oFrame.style.width = oBody.scrollWidth + (oBody.offsetWidth - oBody.clientWidth);
    }
    //An error is raised if the IFrame domain != its container's domain
    catch (e) {
        window.status = 'Error: ' + e.number + '; ' + e.description;
    }
}
function getActivities(startDt,endDt,filter,maxCount){
BEGetActivities(JSONDate(startDt),JSONDate(endDt),filter,maxCount);

}

function getUsers(){
BEGetUsers();

}

function findCourses() {
    //debugger;
    BEFindCourses();

}
function editTable(TableName) {
    //debugger;
    BEEditTable(TableName);

}

function getAttachments(where) {
    //debugger;
    BEEditTable("Attachment", where)
    

}



function findDrills() {
    //debugger;
    BEFindDrills();

}
 function findActivityTypes() {
    //debugger;
    BEFindActivityTypes();

}



function findAlerts() {
    //debugger;
    BEFindAlerts();

}
function findProcedures() {
    //debugger;
    BEFindProcedures();

}
function findScenarios() {
    //debugger;
    BEFindScenarios();

}
function Logoff() {
    //debugger;
    BELogoff();
}

function getReports() {
    //debugger;
    if (typeof (Reports) == undefined || Reports == null) {
        BEGetReports();
    } else {
        CBEvents.Evoke("GetReports");
    }
    
    //return Reports;
}

function checkuploadstatus() {
    StartEventPolling(5000);
}

function uploadcomplete() {
    StopEventPolling();
}

//function getEvents() {
//    BEGetEvents();
//}
function ProcessEvent(Name, Content) {
    //debugger;
    switch (Name.toLowerCase()) {
        case "fileupload":
            addMessage("FileUpload:" + Content.FileName, "Uploading File " + Content.FileName, (Content.UploadedLength / Content.ContentLength)*100);
            break;
        default:
            Log("Error","Invalid Event "+Name);
            break;
    }
}
function ShowAttachment(Attachmentid, newWindow) {
    //debugger;
    if (typeof (newWindow) == "undefined" || newWindow == null || newWindow) {
        window.open("../Download.aspx?file=" + Attachmentid);
    } else {
    window.open("../Download.aspx?file=" + Attachmentid);
    }


}

function ShowFile(Path,Attachmentid, newWindow) {
    //debugger;
    if (typeof (newWindow) == "undefined" || newWindow == null || newWindow) {
        window.open("../Download.aspx?path="+Path+"&file=" + Attachmentid);
    } else {
        window.open("../Download.aspx?path=" + Path + "&file=" + Attachmentid);
    }


}

function cloneObject(inObject) {
    var newObject = {};
    jQuery.extend(newObject, inObject);
    return newObject;

}

function WindowWidth() {
    var myWidth = 0, myHeight = 0;
    if (typeof (window.innerWidth) == 'number') {
        //Non-IE
        myWidth = window.innerWidth;
        myHeight = window.innerHeight;
    } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        myWidth = document.documentElement.clientWidth;
        myHeight = document.documentElement.clientHeight;
    } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        //IE 4 compatible
        myWidth = document.body.clientWidth;
        myHeight = document.body.clientHeight;
    }
    //window.alert('Width = ' + myWidth);
    //window.alert('Height = ' + myHeight);
    return myWidth;
}

function WindowHeight() {
    var myWidth = 0, myHeight = 0;
    if (typeof (window.innerWidth) == 'number') {
        //Non-IE
        myWidth = window.innerWidth;
        myHeight = window.innerHeight;
    } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        myWidth = document.documentElement.clientWidth;
        myHeight = document.documentElement.clientHeight;
    } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        //IE 4 compatible
        myWidth = document.body.clientWidth;
        myHeight = document.body.clientHeight;
    }
    //window.alert('Width = ' + myWidth);
    //window.alert('Height = ' + myHeight);
    return myHeight;
}

function getBaseURL() {
    var url = location.href;  // entire url including querystring - also: window.location.href;
    var baseURL = url.substring(0, url.indexOf('/', 14));


    if (baseURL.indexOf('http://localhost') != -1) {
        // Base Url for localhost
        var url = location.href;  // window.location.href;
        var pathname = location.pathname;  // window.location.pathname;
        var index1 = url.indexOf(pathname);
        var index2 = url.indexOf("/", index1 + 1);
        var baseLocalUrl = url.substr(0, index2);

        return baseLocalUrl + "/";
    }
    else {
        // Root Url for domain name
        return baseURL + "/";
    }

}
function FormatUkDate(dateStr) {
    dateStr = dateStr.split("/");
    return new Date(dateStr[2], dateStr[1] - 1, dateStr[0]);
}

function IsUkDate(dateStr) {
    var retval = false;
    try {
        var t1 = FormatUkDate(dateStr);
        if (isNaN(t1)) return false;
        if (typeof(t1) == "object") return true;
    } catch (e) {
    }

    return retval;
}
