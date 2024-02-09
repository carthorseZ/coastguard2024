var tideHeightCallbacks = new Array();
var curActivities = new Array();
var curActivity;
var curUsers = new Array();
var curCourses = new Array();
var curDrills = new Array();
var curActivityTypes = new Array();
var curProcedures = new Array();
var curAlertDetails = new Array();
var curScenarios = new Array();
function ProcessLoginResult(result)
{
//if (debug) debugger;
LogMsg("Login result:"+result);
if (result > -1){ 
    sessionID = result;
    //top.window.location("Main.htm");
    GotoMain();
} else {
    alert("Login Failed please check user name and password and try again");
}
}

function ProcessGetSkippersResult(result)
{
//if (debug) debugger;
LogMsg("GetSkippers result:"+result);
Skippers = result;
}
function ProcessNewActivityResult(result)
{
//if (debug) debugger;
LogMsg("NewActivity result:"+result);
BlankActivity = result;
}

function ProcessNewUserResult(result)
{
//if (debug) debugger;
LogMsg("NewUser result:"+result);
BlankUser = result;
}

function ProcessNewCourseResult(result) {
    //if (debug) debugger;
    LogMsg("NewCourse result:" + result);
    BlankCourse = result;
}
function ProcessNewDrillResult(result) {
    //if (debug) debugger;
    LogMsg("NewCourse result:" + result);
    BlankDrill = result;
}
function ProcessNewActivityTypeResult(result) {
    //if (debug) debugger;
    LogMsg("NewActivity Type result:" + result);
    BlankActivityType = result;
}

function ProcessGetEngineStartHours(result) {
    //if (debug) debugger;
    LogMsg("GetEngineStartHours result:" + result);
    CBEvents.Evoke("GetEngineStartHours", result);
}

function ProcessNewObjectResult(result) {
    //debugger;
    try {
        var myObject = eval('(' + result.ObjectStr + ')');

    } catch (err) {
    }
    LogMsg("NewObject Type result:" + result);
    CBEvents.Evoke("NewObject:"+result.Name, myObject);
    
    //BlankActivityType = result;
}

function ProcessGetObjectsResult(result) {
    //debugger;
    var retval =  new Array();
    var retName = "";
    for (var i = 0; i < result.length; i++) {
        try {
            retName = result[i].Name;
            var myObject = eval('(' + result[i].ObjectStr + ')');
            retval.push(myObject);
            
        } catch (err) {
        LogMsg("An error " + err + " occured while processing object:" + retName);
        }    
    }

    LogMsg("GetObjects Type result:" + result);
    //debugger;
    CBEvents.Evoke("GetObjects:" + retName, retval);
    if (retName == "CoastguardRescueVessel") {
        CBEvents.Evoke("GetObjects:" + "CRVs", retval);
    }
    
    //BlankActivityType = result;
}
function ProcessNewObjectAsStringListResult(result) {
    //debugger;
    //try {
    //   var myObject = eval('(' + result.ObjectStr + ')');

    //} catch (err) {
    //}
    LogMsg("NewObject Type ["+result.Name+"] ",result.ObjectStr);
    CBEvents.Evoke("NewObject:" + result.Name, result.ObjectStr);

    //BlankActivityType = result;
}
function ProcessNewProcedureResult(result) {
    //if (debug) debugger;
    LogMsg("NewCourse result:" + result);
    BlankProcedure = result;
}
function ProcessNewAlertResult(result) {
    //if (debug) debugger;
    LogMsg("NewCourse result:" + result);
    BlankAlert = result;
}
function ProcessNewScenarioResult(result) {
    //if (debug) debugger;
    LogMsg("NewCourse result:" + result);
    BlankScenario = result;
}
function ProcessGetCrewResult(result)
{
//if (debug) debugger;
LogMsg("GetCrews result:"+result);
Crew = result;
}
function ProcessGetCoursesResult(result)
{
//if (debug) debugger;
LogMsg("GetCourses result:"+result);
Courses = result;
}
function ProcessGetDrillsResult(result) {
    //if (debug) debugger;
    LogMsg("GetDrills result:" + result);
    Drills = result;
}

function ProcessGetActivityTypesResult(result) {
    //if (debug) debugger;
    LogMsg("GetActivityTypes result:" + result);
    ActivityTypes = result;
    CBEvents.Evoke("GetActivityTypes", result);
}
function ProcessGetProceduresResult(result) {
    //if (debug) debugger;
    LogMsg("GetProcedures result:" + result);
    Procedures = result;
}
function ProcessGetActivityProceduresResult(result) {
    //if (debug) debugger;
    LogMsg("GetActivityProcedures result:" + result);
    CBEvents.Evoke("GetActivityProcedures", result);
    //Procedures = result;
}
function ProcessGetAlertsResult(result) {
    //if (debug) debugger;
    LogMsg("GetAlerts result:",result);
    CBEvents.Evoke("GetAlerts",result);
    Alerts = result;
}
function ProcessGetAlertDetailsResult(result) {
    //if (debug) debugger;
    LogMsg("GetAlertDetails result:" + result);
    Alerts = result;
}
function ProcessGetScenariosResult(result) {
    //if (debug) debugger;
    LogMsg("GetScenarios result:" + result);
    Scenarios = result;
}
function ProcessAddActivityResult(result)
{
    //debugger;
    if (typeof (result) == "undefined" || result == null) {
        LogMsg("Activity add was unsuccessful");
    } else {
        LogMsg("ActivityAdd result:" + result);
        CBEvents.Evoke("NewActivityID", result);
    }
    try {
        if (toInt(top.BlankActivity.LogNo.substr(1)) < result.LogNo) {
            top.BlankActivity.LogNo = "T" + (toInt(result.LogNo) + 1);
        }  
    } catch (e) {
    debugger;
    }
}

function ProcessModifyCoastGuardMemberResult(result)
{
//if (debug) debugger;
if (result) {
LogMsg("Member add was successful");

} else { 
Logmsg("Member add was unsuccessful");
}


}
function ProcessModifyCoastguardRescueVesselResult(result) {
    //if (debug) debugger;
    if (result) {
        LogMsg("coastguard rescue vessel add was successful");

    } else {
        Logmsg("coastguard rescue vessel add was unsuccessful");
    }


}
function ProcessModifyResult(resultName,result) {
    //if (debug) debugger;
    if (result) {
        LogMsg(resultName + " was successful");

    } else {
        LogMsg(resultName + " was unsuccessful");
    }


}


function ProcessModifyCourseResult(result) {
    //if (debug) debugger;
    if (result) {
        LogMsg("Course add was successful");

    } else {
        Logmsg("Course add was unsuccessful");
    }
  

}
function ProcessModifyDrillResult(result) {
    //if (debug) debugger;
    if (result) {
        LogMsg("Drill add was successful");

    } else {
        LogMsg("Drill add was unsuccessful");
    }


}
function ProcessModifyActivityDrillResult(result) {
    //if (debug) debugger;
    if (result) {
        LogMsg("ModifyActivityDrill add was successful");

    } else {
    LogMsg("ModifyActivityDrill add was unsuccessful");
    }


}
function ProcessModifyActivityScenarioResult(result) {
    //if (debug) debugger;
    if (result) {
        LogMsg("ModifyActivityScenario add was successful");

    } else {
    Logmsg("ModifyActivityScenario add was unsuccessful");
    }


}
function ProcessModifyActivityDestinationResult(result) {
    //if (debug) debugger;
    if (result) {
        LogMsg("ModifyActivityDestination add was successful");

    } else {
        LogMsg("ModifyActivityDestination add was unsuccessful");
    }


}
function ProcessModifyActivityTypeResult(result) {
    //if (debug) debugger;
    if (result) {
        LogMsg("ActivityType add was successful");

    } else {
    LogMsg("ActivityType add was unsuccessful");
    }


}
function ProcessModifyProcedureResult(result) {
    //if (debug) debugger;
    if (result) {
        LogMsg("Procedure add was successful");

    } else {
        LogMsg("Procedure add was unsuccessful");
    }

}
function ProcessModifyProcedureInfoResult(result) {
    //if (debug) debugger;
    if (result) {
        LogMsg("ProcedureInfo add was successful");

    } else {
        LogMsg("ProcedureInfo add was unsuccessful");
    }

}
function ProcessModifyActivityPassangerResult(result) {
    //if (debug) debugger;
    if (result) {
        LogMsg("PassangerInfo add was successful");

    } else {
        LogMsg("PassangerInfo add was unsuccessful");
    }

}
 function ProcessModifyAlertResult(result) {
    //if (debug) debugger;
    if (result) {
        LogMsg("Alert add was successful");

    } else {
        LogMsg("Alert add was unsuccessful");
    }


}
function ProcessModifyTableResult(result) {
    //if (debug) debugger;
    if (result) {
        LogMsg("Table Modification was successful");

    } else {
    LogMsg("Table Modification add was unsuccessful");
    }


}

function ProcessModifyScenarioResult(result) {
    //if (debug) debugger;
    if (result) {
        LogMsg("Scenario add was successful");

    } else {
        Logmsg("Scenario add was unsuccessful");
    }


}

function ProcessGetTideHeightsResult(result)
{
//if (debug) debugger;
curTideHeights=result;
CBEvents.Evoke("GetTideHeights");
}


function ProcessGetActivitiesResult(result)
{
//if (debug) debugger;
curActivities=result;
CBEvents.Evoke("GetActivities");
}

function ProcessGetMenuResult(result) {
    //if (debug) debugger;
    if (result.Name == "main") {
    CBEvents.Evoke("GetMenu",result);
    } else {
    CBEvents.Evoke("GetMenu:"+result.Name,result);
    }
}


function ProcessgetActivityResult(result)
{
//if (debug) debugger;
curActivity=result;
CBEvents.Evoke("GetActivity");
}

function ProcessGetUsersResult(result)
{
//if (debug) debugger;
curUsers=result;
CBEvents.Evoke("GetUsers");
}

function ProcessGetPeopleResult(result) {
    //if (debug) debugger;
    curUsers = result;
    CBEvents.Evoke("GetPeople");
}


function ProcessFindCoursesResult(result) {
    //if (debug) debugger;
    curCourses = result;
    CBEvents.Evoke("FindCourses");
}

function ProcessEditTableResult(result) {
   // if (debug) debugger;
    //curCourses = result;
    CBEvents.Evoke("EditTable:"+result.Name,result);
}
function ProcessFindDrillsResult(result) {
    //if (debug) debugger;
    curDrills = result;
    CBEvents.Evoke("FindDrills");
}
function ProcessFindActivityTypesResult(result) {
    //if (debug) debugger;
    curActivityTypes = result;
    CBEvents.Evoke("FindActivityTypes");
}
function ProcessFindProceduresResult(result) {
    //if (debug) debugger;
    curProcedures = result;
    CBEvents.Evoke("FindProcedures");
}
 function ProcessFindAlertsResult(result) {
    //if (debug) debugger;
    curAlertDetails = result;
    CBEvents.Evoke("FindAlerts");
}
function ProcessFindScenariosResult(result) {
    //if (debug) debugger;
    curScenarios = result;
    CBEvents.Evoke("FindScenarios");
}
function ProcessGetSessionResult(result) {
    //if (debug) debugger;
    Session = result;
    if (Session.LogEnabled) {
        $('#divLogger').show();

    } else {
    $('#divLogger').hide();
    }
    CBEvents.Evoke("GetSession");
}

function ProcessLogMessageResult(result) {
    ProcessGetEventsResult(result.Events);
}

function ProcessLogoffResult(result) {
    //if (debug) debugger;
    top.window.location = "Login.htm";
}

function ProcessBackupDataBaseResult(result) {
    //if (debug) debugger;
    CBEvents.Evoke("Backup", result);
}

function ProcessRestoreDataBaseResult(result) {
    //if (debug) debugger;
    CBEvents.Evoke("Restore", result);
}
function ProcessGetReportsResult(result) {
    //if (debug) debugger;
    Reports = result;
    CBEvents.Evoke("GetReports");
}
function ProcessGetBackupsResult(result) {
    //if (debug) debugger;
    
    CBEvents.Evoke("GetBackups",result);
}

function ProcessGetVesselListResult(result) {
    //if (debug) debugger;

    CBEvents.Evoke("GetVesselList", result);
}



function ProcessUpdateStatisticsResult(result) {
    //if (debug) debugger;

    CBEvents.Evoke("UpdateStatistics", result);
}

function ProcessGetEventsResult(result) {
    //debugger;
    var retval = new Array();
    var retName = "";
    for (var i = 0; i < result.length; i++) {
        try {
            retName = result[i].name;
            var myObject = eval('(' + result[i].objectstr + ')');
            ProcessEvent(retName, myObject);
            //retval.push(myObject);

        } catch (err) {
            LogMsg("An error " + err + " occured while processing event:" + retName);
        }
    }

 //   LogMsg("GetObjects Type result:" + result);
    //debugger;
 //   CBEvents.Evoke("GetObjects:" + retName, retval);

  
}