


function FELogin(uid,pwd,unit)
{
	//var sessionIdVal = $("#sessionId").val();
		//var sessionIdVal = 1;
		//if (debug) debugger;
	proxy.invoke
	(
		"Login",
		{ Username: uid , Password: pwd, Unit: unit},
		CallbackLogin,
		onPageError,
		true
	);
}
function BELogoff() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"Logoff",
		{ SessionID: sessionID },
		CallbackLogoff,
		onPageError,
		true
	);
}

function BEBackupDataBase(filename) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"BackupDataBase",
		{ SessionID: sessionID, fname: filename },
		CallbackBackupDataBase,
		onPageError,
		true
	);
}

function BERestoreDataBase(filename) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"RestoreDataBase",
		{ SessionID: sessionID, fname: filename },
		CallbackRestoreDataBase,
		onPageError,
		true
	);
}
function BEAddActivity(newActivity)
{
	//var sessionIdVal = $("#sessionId").val();
		//var sessionIdVal = 1;
		//if (debug) debugger;
		
	proxy.invoke
	(
		"AddActivity",
		{ SessionID: sessionID , Activity: newActivity},
		CallbackAddActivity,
		onPageError,
		true
	);
}

function BEModifyCoastGuardMember(newMember)
{
	//var sessionIdVal = $("#sessionId").val();
		//var sessionIdVal = 1;
		//if (debug) debugger;
		
	proxy.invoke
	(
		"ModifyCoastGuardMember",
		{ SessionID: sessionID , Member: newMember},
		CallbackModifyCoastGuardMember,
		onPageError,
		true
	);
}

function BEModifyCoastguardRescueVessel(inVal) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"ModifyCoastguardRescueVessel",
		{ SessionID: sessionID, crv: inVal },
		CallbackModifyCoastguardRescueVessel,
		onPageError,
		true
	);
}

function BEModifyCourse(course) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"Modifycourse",
		{ SessionID: sessionID, Course: course },
		CallbackModifyCourse,
		onPageError,
		true
	);
}
function BEGetEngineStartHours(id,logNo) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
    if (!isGuid(id)) id = newGuid();
    if (!isInt(logNo)) logNo = -1;
	//if (!isGuid(middle)) middle = newGuid();
	//if (!isGuid(starboard)) starboard = newGuid();

	proxy.invoke
	(
		"GetEngineStartHours",
		{ SessionID: sessionID, Vessel: id, LogNo: logNo },
		CallbackGetEngineStartHours,
		onPageError,
		true
	);
}
function BEModifyTable(tablename, object) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"ModifyTable",
		{ SessionID: sessionID, TableName: tablename, Fields: object },
		CallbackModifyTable,
		onPageError,
		true
	);
}

function BEGetMenu(menuname) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	if (typeof(menuname) == "undefined") menuname = "main"

	proxy.invoke
	(
		"GetMenu",
		{ SessionID: sessionID, MenuName: menuname },
		CallbackGetMenu,
		onPageError,
		true
	);
}
function BEModifyDrill(drill) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"ModifyDrill",
		{ SessionID: sessionID, Drill: drill },
		CallbackModifyDrill,
		onPageError,
		true
	);
}
function BEModifyAttachment(attach) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"ModifyAttachment",
		{ SessionID: sessionID, attach: attach },
		CallbackModifyAttachment,
		onPageError,
		true
	);
}

function BEModifyActivityDrill(drill) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"ModifyActivityDrill",
		{ SessionID: sessionID, inDrillInfo: drill },
		CallbackModifyActivityDrill,
		onPageError,
		true
	);
}
function BEModifyActivityDestination(destination) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"ModifyActivityDestination",
		{ SessionID: sessionID, inDestinationInfo: destination },
		CallbackModifyActivityDestination,
		onPageError,
		true
	);
}
function BEModifyActivityScenario(scenario) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"ModifyActivityScenario",
		{ SessionID: sessionID, inScenarioInfo: scenario },
		CallbackModifyActivityScenario,
		onPageError,
		true
	);
}

function BEModifyActivityType(activityType) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"ModifyActivityType",
		{ SessionID: sessionID, ActivityType: activityType },
		CallbackModifyActivityType,
		onPageError,
		true
	);
}
function BEModifyAlert(alert) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"ModifyAlert",
		{ SessionID: sessionID, Alert: alert },
		CallbackModifyAlert,
		onPageError,
		true
	);
}
function BEModifyProcedure(procedure) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"ModifyProcedure",
		{ SessionID: sessionID, Procedure: procedure },
		CallbackModifyProcedure,
		onPageError,
		true
	);
}
function BEModifyProcedureInfo(procedure) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"ModifyProcedureInfo",
		{ SessionID: sessionID, Procedure: procedure },
		CallbackModifyProcedureInfo,
		onPageError,
		true
	);
}

function BEModifyActivityPassanger(passanger) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"ModifyActivityPassanger",
		{ SessionID: sessionID, inPassangerInfo: passanger },
		CallbackModifyActivityPassanger,
		onPageError,
		true
	);
}
function BEGetEvents() {


	proxy.invoke
	(
		"GetEvents",
		{ SessionID: sessionID},
		CallbackGetEvents,
		onPageError,
		true
	);
}
function BEModifyScenario(scenario) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"ModifyScenario",
		{ SessionID: sessionID, Scenario: scenario },
		CallbackModifyScenario,
		onPageError,
		true
	);
}
function BEGetActivities(startDt,endDt,filter,maxCount,activityType,vessel)
{
	//var sessionIdVal = $("#sessionId").val();
		//var sessionIdVal = 1;
    //if (debug) debugger;
    if (!isGuid(activityType)) activityType = newGuid();
    if (!isGuid(vessel)) vessel = newGuid();	
	proxy.invoke
	(
		"GetActivities",
		{ SessionID: sessionID , StartDt: startDt , EndDt: endDt , Filter: "filter" , MaxCount: 50 ,ActivityType: activityType,Vessel: vessel },
		CallbackGetActivities,
		onPageError,
		true
	);
}
function BEGetRecentActivities( activityType,vessel,maxCount) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	if (isNaN(maxCount)) {
		maxCount = 10;
	}
if (!isGuid(activityType)) activityType = newGuid();
if (!isGuid(vessel)) vessel = newGuid();	
	proxy.invoke
	(
		"GetRecentActivities",
		{ SessionID: sessionID,  MaxCount: maxCount, ActivityType:activityType, Vessel:vessel },
		CallbackGetRecentActivities,
		onPageError,
		true
	);
}
function BEGetReports() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"GetReports",
		{ SessionID: sessionID },
		CallbackGetReports,
		onPageError,
		true
	);
}

function BEGetBackups() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"GetBackups",
		{ SessionID: sessionID },
		CallbackGetBackups,
		onPageError,
		true
	);
}


function BEGetObjects(objectName,where) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	if (typeof (where) == "undefined") { where = null; }
	proxy.invoke
	(
		"GetObjects",
		{ SessionID: sessionID, ObjectName: objectName,where:where },
		CallbackGetObjects,
		onPageError,
		true
	);
}

function BEGetActivity(ActivityId)
{
	//var sessionIdVal = $("#sessionId").val();
		//var sessionIdVal = 1;
		//if (debug) debugger;
		
	proxy.invoke
	(
		"getActivity",
		{ SessionID: sessionID, tripid:ActivityId },
		CallbackgetActivity,
		onPageError,
		true
	);
}

function BEGetUsers(filter,maxCount)
{
	//var sessionIdVal = $("#sessionId").val();
		//var sessionIdVal = 1;
		//if (debug) debugger;
		
	proxy.invoke
	(
		"GetUsers",
		{ SessionID: sessionID , Filter: "filter" , MaxCount: 50},
		CallbackGetUsers,
		onPageError,
		true
	);
}

function BEGetPeople(filter, maxCount) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"GetPeople",
		{ SessionID: sessionID, Filter: "filter", MaxCount: 50 },
		CallbackGetPeople,
		onPageError,
		true
	);
}


function BEGetVesselList(filter, maxCount) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"GetVesselList",
		{ SessionID: sessionID, Filter: "filter", MaxCount: 50 },
		CallbackGetVesselList,
		onPageError,
		true
	);
}

function BEFindCourses(filter, maxCount) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"FindCourses",
		{ SessionID: sessionID, Filter: "filter", MaxCount: 50 },
		CallbackFindCourses,
		onPageError,
		true
	);
}

function BEEditTable(TableName,filter, maxCount) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	if (typeof (filter) == "undefined" || filter == null) { filter = ""; };
	proxy.invoke
	(
		"GetEditGrid",
		{ SessionID: sessionID, TableName:TableName, Filter: filter, MaxCount: 50 },
		CallbackEditTable,
		onPageError,
		true
	);
}

function BEFindDrills(filter, maxCount) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"FindDrills",
		{ SessionID: sessionID, Filter: "filter", MaxCount: 50 },
		CallbackFindDrills,
		onPageError,
		true
	);
}
function BEFindActivityTypes(filter, maxCount) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"FindActivityTypes",
		{ SessionID: sessionID, Filter: "filter", MaxCount: 50 },
		CallbackFindActivityTypes,
		onPageError,
		true
	);
}
function BEFindProcedures(filter, maxCount) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"FindProcedures",
		{ SessionID: sessionID, Filter: "filter", MaxCount: 50 },
		CallbackFindProcedures,
		onPageError,
		true
	);
}
function BEFindAlerts(filter, maxCount) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"FindAlerts",
		{ SessionID: sessionID, Filter: "filter", MaxCount: 50 },
		CallbackFindAlerts,
		onPageError,
		true
	);
}
function BEFindScenarios(filter, maxCount) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;

	proxy.invoke
	(
		"FindScenarios",
		{ SessionID: sessionID, Filter: "filter", MaxCount: 50 },
		CallbackFindScenarios,
		onPageError,
		true
	);
}
function BEGetSkippers()
{
	//var sessionIdVal = $("#sessionId").val();
		//var sessionIdVal = 1;
		//if (debug) debugger;
	proxy.invoke
	(
		"GetSkippers",
		{ SessionID: sessionID},
		CallbackGetSkippers,
		onPageError,
		true
	);
}

function BEGetCrew()
{
	//var sessionIdVal = $("#sessionId").val();
		//var sessionIdVal = 1;
		//if (debug) debugger;
	proxy.invoke
	(
		"GetCrew",
		{ SessionID: sessionID},
		CallbackGetCrew,
		onPageError,
		true
	);
}

function BEGetSession() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"GetSession",
		{ SessionID: sessionID },
		CallbackGetSession,
		onPageError,
		true
	);
}

function BENewActivity()
{
	//var sessionIdVal = $("#sessionId").val();
		//var sessionIdVal = 1;
		//if (debug) debugger;
	proxy.invoke
	(
		"NewActivity",
		{ SessionID: sessionID},
		CallbackNewActivity,
		onPageError,
		true
	);
}

function BENewUser()
{
	//var sessionIdVal = $("#sessionId").val();
		//var sessionIdVal = 1;
		//if (debug) debugger;
	proxy.invoke
	(
		"NewUser",
		{ SessionID: sessionID},
		CallbackNewUser,
		onPageError,
		true
	);
}
function BENewCourse() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"NewCourse",
		{ SessionID: sessionID },
		CallbackNewCourse,
		onPageError,
		true
	);
}

function BENewDrill() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"NewDrill",
		{ SessionID: sessionID },
		CallbackNewDrill,
		onPageError,
		true
	);
}
function BENewActivityType() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"NewActivityType",
		{ SessionID: sessionID },
		CallbackNewActivityType,
		onPageError,
		true
	);
}

function BENewObject(name) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"NewObject",
		{ SessionID: sessionID,  ObjectName:name },
		CallbackNewObject,
		onPageError,
		true
	);
}
function BENewObjectAsStringList(name) {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"NewObjectAsStringList",
		{ SessionID: sessionID, ObjectName: name },
		CallbackNewObjectAsStringList,
		onPageError,
		true
	);
}
function BENewProcedure() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"NewProcedure",
		{ SessionID: sessionID },
		CallbackNewProcedure,
		onPageError,
		true
	);
}
function BENewAlert() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"NewAlert",
		{ SessionID: sessionID },
		CallbackNewAlert,
		onPageError,
		true
	);
}
function BENewScenario() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"NewScenario",
		{ SessionID: sessionID },
		CallbackNewScenario,
		onPageError,
		true
	);
}
function BEGetCourses()
{
	//var sessionIdVal = $("#sessionId").val();
		//var sessionIdVal = 1;
		//if (debug) debugger;
	proxy.invoke
	(
		"GetCourses",
		{ SessionID: sessionID},
		CallbackGetCourses,
		onPageError,
		true
	);
}
function BEGetDrills() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"GetDrills",
		{ SessionID: sessionID },
		CallbackGetDrills,
		onPageError,
		true
	);
}
function BEGetActivityTypes() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"GetActivityTypes",
		{ SessionID: sessionID },
		CallbackGetActivityTypes,
		onPageError,
		true
	);
}
function BEGetProcedures() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"GetProcedures",
		{ SessionID: sessionID },
		CallbackGetProcedures,
		onPageError,
		true
	);
}
function BEGetActivityProcedures() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"GetActivityProcedures",
		{ SessionID: sessionID },
		CallbackGetActivityProcedures,
		onPageError,
		true
	);
}
function BEGetAlerts() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"GetAlerts",
		{ SessionID: sessionID },
		CallbackGetAlerts,
		onPageError,
		true
	);
}
function BEGetAlertDetails() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"GetAlertDetails",
		{ SessionID: sessionID },
		CallbackGetAlertDetails,
		onPageError,
		true
	);
}
function BEGetScenarios() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"GetScenarios",
		{ SessionID: sessionID },
		CallbackGetScenarios,
		onPageError,
		true
	);
}
function BEGetTideHeights(tideDate,ports)
{
	//var sessionIdVal = $("#sessionId").val();
		//var sessionIdVal = 1;
		//if (debug) debugger;
	proxy.invoke
	(
		"getTideHeights",
		{ SessionID: sessionID , dt: tideDate , Ports: ports},
		CallbackGetTideHeights,
		onPageError,
		true
	);
}


function BELogMsg(msg,level)
{
	//var sessionIdVal = $("#sessionId").val();
		//var sessionIdVal = 1;
		//if (debug) debugger;
	//var sessionID = 0;
	//debugger;
	if (ServerLoggingDisabled) return;
	if (typeof (level) == "undefined") level = 1;
	if (typeof (sessionID) == "undefined"||sessionID == null) return;
	
	proxy.invoke
	(
		"LogMessage",
		{ SessionID: sessionID , Level: level , Message: msg},
		CallbackLogMessage,
		CallbackLogMessageError,
		true
	);
}

function BEUpdateStatistics() {
	//var sessionIdVal = $("#sessionId").val();
	//var sessionIdVal = 1;
	//if (debug) debugger;
	proxy.invoke
	(
		"UpdateStatistics",
		{ SessionID: sessionID },
		CallbackUpdateStatistics,
		onPageError,
		true
	);
}



function CallbackLogMessageError(retval) {

	ServerLoggingDisabled = true;
}


function CallbackLogin(result, status, xhr){
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
		ProcessLoginResult(result.LoginResult);
	//}
}
function CallbackGetSkippers(result, status, xhr){
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
		ProcessGetSkippersResult(result.GetSkippersResult);
	//}
}

function CallbackGetCrew(result, status, xhr){
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
		ProcessGetCrewResult(result.GetCrewResult);
	//}
}
function CallbackUpdateStatistics(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessUpdateStatisticsResult(result.GetCrewResult);
	//}
}


function CallbackBackupDataBase(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessBackupDataBaseResult(result.BackupDataBaseResult);
	//}
}

function CallbackRestoreDataBase(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessRestoreDataBaseResult(result.BackupDataBaseResult);
	//}
}
function CallbackGetSession(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetSessionResult(result.GetSessionResult);
	//}
}
function CallbackGetEngineStartHours(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetEngineStartHours(result.GetEngineStartHoursResult);
	//}
}
function CallbackGetCourses(result, status, xhr){
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
		ProcessGetCoursesResult(result.GetCoursesResult);
	//}
}
function CallbackGetDrills(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetDrillsResult(result.GetDrillsResult);
	//}

}

function CallbackGetVesselList(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetVesselListResult(result.GetVesselListResult);
	//}
}

function CallbackGetActivityTypes(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetActivityTypesResult(result.GetActivityTypesResult);
	//}
}
function CallbackGetProcedures(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetProceduresResult(result.GetProceduresResult);
	//}
}
function CallbackGetActivityProcedures(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetActivityProceduresResult(result.GetActivityProceduresResult);
	//}
}
function CallbackGetAlerts(result, status, xhr) {
   //if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetAlertsResult(result.GetAlertsResult);
	//}
}
function CallbackGetAlertDetails(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetAlertDetailsResult(result.GetAlertDetailsResult);
	//}
}
function CallbackGetScenarios(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetScenariosResult(result.GetScenariosResult);
	//}
}
function CallbackGetReports(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetReportsResult(result.GetReportsResult);
	//}
}

function CallbackGetBackups(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetBackupsResult(result.GetBackupsResult);
	//}
}
function CallbackGetObjects(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetObjectsResult(result.GetObjectsResult);
	//}
}
function CallbackAddActivity(result, status, xhr){
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
		ProcessAddActivityResult(result.AddActivityResult);
	//}
}


function CallbackModifyCoastGuardMember(result, status, xhr){
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
		ProcessModifyCoastGuardMemberResult(result.ModifyCoastGuardMemberResult);
	//}
}
function CallbackModifyCoastguardRescueVessel(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyCoastguardRescueVesselResult(result.ModifyCoastguardRescueVesselResult);
	//}
}
function CallbackModifyAttachment(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyResult("Attachment",result.ModifyAttachmentResult);
	//}
}
function CallbackModifyCourse(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyCourseResult(result.ModifyCourseResult);
	//}
}
function CallbackModifyDrill(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyDrillResult(result.ModifyDrillResult);
	//}
}
function CallbackModifyActivityDrill(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyActivityDrillResult(result.ModifyDrillResult);
	//}
}

function CallbackGetMenu(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetMenuResult(result.GetMenuResult);
	//}
}

function CallbackModifyActivityScenario(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyActivityScenarioResult(result.ModifyActivityScenarioResult);
	//}
}
function CallbackModifyActivityDestination(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyActivityDestinationResult(result.ModifyActivityDestinationResult);
	//}
}
function CallbackModifyTable(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyTableResult(result.ModifyTableResult);
	//}
}
function CallbackModifyActivityType(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyActivityTypeResult(result.ModifyActivityTypeResult);
	//}
}
function CallbackModifyProcedure(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyProcedureResult(result.ModifyProcedureResult);
	//}
}
function CallbackModifyProcedureInfo(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyProcedureInfoResult(result.ModifyProcedureInfoResult);
	//}
}
function CallbackModifyActivityPassanger(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyActivityPassangerResult(result.ModifyActivityPassangerResult);
	//}
}
function CallbackModifyAlert(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyAlertResult(result.ModifyAlertResult);
	//}
}
function CallbackModifyScenario(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessModifyScenarioResult(result.ModifyScenarioResult);
	//}
}
function CallbackNewActivity(result, status, xhr){
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
		ProcessNewActivityResult(result.NewActivityResult);
	//}
}
function CallbackNewUser(result, status, xhr){
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
		ProcessNewUserResult(result.NewUserResult);
	//}
}


function CallbackGetEvents(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetEventsResult(result.GetEventsResult);
	//}
}
function CallbackNewCourse(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessNewCourseResult(result.NewCourseResult);
	//}
}
function CallbackNewDrill(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessNewDrillResult(result.NewDrillResult);
	//}
}
function CallbackNewActivityType(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessNewActivityTypeResult(result.NewActivityTypeResult);
	//}
}
function CallbackNewObject(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessNewObjectResult(result.NewObjectResult);
	//}
}
function CallbackNewObjectAsStringList(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessNewObjectAsStringListResult(result.NewObjectAsStringListResult);
	//}
}
function CallbackNewProcedure(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessNewProcedureResult(result.NewProcedureResult);
	//}
}
function CallbackNewAlert(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessNewAlertResult(result.NewAlertResult);
	//}
}
function CallbackNewScenario(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessNewScenarioResult(result.NewScenarioResult);
	//}
}
function CallbackLogMessage(result, status, xhr){
	//if (debug) debugger;
	//if (self.ProcessLogMessageResult)
	//{
		ProcessLogMessageResult(result.LogMessageResult);
	//}

}

function CallbackGetTideHeights(result, status, xhr){
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
		ProcessGetTideHeightsResult(result.getTideHeightsResult);
	//}
}

function CallbackGetActivities(result, status, xhr){
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
		ProcessGetActivitiesResult(result.GetActivitiesResult);
	//}
}
function CallbackGetRecentActivities(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetActivitiesResult(result.GetRecentActivitiesResult);
	//}
}
function CallbackgetActivity(result, status, xhr){
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
		ProcessgetActivityResult(result.getActivityResult);
	//}
}

function CallbackGetUsers(result, status, xhr){
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
		ProcessGetUsersResult(result.GetUsersResult);
	//}
}

function CallbackGetPeople(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessGetPeopleResult(result.GetPeopleResult);
	//}
}

function CallbackFindCourses(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessFindCoursesResult(result.FindCoursesResult);
	//}
}
function CallbackEditTable(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessEditTableResult(result.GetEditGridResult);
	//}
}

function CallbackFindDrills(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessFindDrillsResult(result.FindDrillsResult);
	//}
}
function CallbackFindActivityTypes(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessFindActivityTypesResult(result.FindActivityTypesResult);
	//}
}
function CallbackFindProcedures(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessFindProceduresResult(result.FindProceduresResult);
	//}
}
function CallbackFindAlerts(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessFindAlertsResult(result.FindAlertsResult);
	//}
}
function CallbackFindScenarios(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessFindScenariosResult(result.FindScenariosResult);
	//}
}
function CallbackLogoff(result, status, xhr) {
	//if (debug) debugger;
	//if (self.ProcessLoginResult)
	//{
	ProcessLogoffResult(result.LogoffResult);
	//}
}