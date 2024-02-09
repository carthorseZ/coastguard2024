var crewcount = 0;
var coursesSelected = new Array();
var crewSelected = new Array();
var Crew;
var Courses;
var dialogArray = ["Main", "Crew","Main2", "Tides", "Procedure", "Drill", "Scenario"];
var CrewUnselected;
var CoursesUnSelected;
var Skipper;
var WeatherType = ["Sun","Cloud","Rain","Wind"];
var ActivityID;
var ActivityType;
var ActivityDetails;
var OrigProcedures;
var OrigScenarios;
var OrigDrills;
var DrillsUnSelected;
var newActivityDrill;
var newActivityScenario;
var ProcedureTypes = ["Weekly", "Monthly", "StartUp", "Maintenance", "Safety", "Radio"];
var ModeAdd = false;
var Cap;
var DrillCount=0;
var ScenarioCount = 0;
$(document).ready(function() {

	//debugger;
	//top.updateFrameSize(this.outerHeight);
	ActivityID = $.getURLParam('ActivityId');
	if (typeof (ActivityID) == 'undefined' || ActivityID == null) ModeAdd = true;
//    $("#ProcedureDiv").hide();
//    $("#ScenarioDiv").hide();
//    $("#DrillDiv").hide();
	gotActivityDrills();
	gotActivityScenarios();
	top.CBEvents.AddCallBack("GetActivity", "setupForm", setupForm);
	top.CBEvents.AddCallBack('GetActivityProcedures', 'GetActivityProcedures', gotActivityProcedures);
	top.CBEvents.AddCallBack('NewObject:ActivityDrillInfo', 'NewObject:ActivityDrillInfo', gotNewActivityDrillInfo);
	top.CBEvents.AddCallBack('NewObject:ActivityScenarioInfo', 'NewObject:ActivityScenarioInfo', gotNewActivityScenarioInfo);
	top.CBEvents.AddCallBack('GetObjects:ActivityDrillInfo', 'GetObjects:ActivityDrillInfo', gotActivityDrills);
	top.CBEvents.AddCallBack('GetObjects:ActivityScenarioInfo', 'GetObjects:ActivityScenarioInfo', gotActivityScenarios);
	top.BEGetActivityProcedures();
	top.BENewObject("ActivityDrillInfo");
	top.BENewObject("ActivityScenarioInfo");
	//top.BEGetObjects("ActivityDrillInfo","ActivityId = '"+ActivityID+"'");
	top.BEGetObjects("ActivityDrillInfo", ActivityID);
	top.BEGetObjects("ActivityScenarioInfo", ActivityID);
	
	if (ModeAdd) {
		setupForm();
	} else {

		top.BEGetActivity(ActivityID);
	}
});

function StartWizard() {
	//debugger;
	for (var i = 0; i < dialogArray.length; i++) {
		try {
			$("#" + dialogArray[i]+"Div").hide();
		} catch (err) {
		}
	}
	$("#ButtonsDiv").hide();
	$("#ChecksDiv").hide();
	//showDialog("Main");
	showDialog(0);
}


function gotNewActivityScenarioInfo(result) {
	newActivityScenario = result;
}

function gotNewActivityDrillInfo(result) {
	newActivityDrill = result;
}


function setupForm() {
	//debugger;
	if (ModeAdd) {
		ActivityDetails = top.BlankActivity;
	} else {
		ActivityDetails = top.curActivity;
	}
//debugger;
try {
	
	Cap = top.getSkippers();
} catch (Err) { Cap = new Array(); }
//var ct = $('#CrewTable');
//$('#CrewTable > tbody:last').append('<tr><td><select id="Crew1" onchange="crewOnchange(this)"></select></td></tr>'); 
//var dt = $('#DepartTime').timeEntry();
//dt.setTime(ActivityDetails.DepartTime);
var teImage = 'css/images/spinnerDefault.png';
if (!FileExists(teImage)) {
	teImage = '../css/images/spinnerDefault.png';
}
$('#DepartTime').timeEntry({spinnerImage: teImage});
$('#ReturnTime').timeEntry({spinnerImage: teImage});
$('#ActivityDate').datepicker({ dateFormat: 'dd/mm/yy' });
populateDropdownCrew($('#Skippers'), Cap);
try {
	Crew = top.getCrew();
} catch (Err) { Crew = new Array(); }

//if (top.debug) debugger;
//CrewUnselected = ArrayCopy(Crew);
CrewUnselected = Crew;
ShowDropdowns('#CrewTable',1,4);
populateDropdownCrew($('#Crew1'), Crew);
try {
	Courses = top.getCourses();
} catch (Err) { Courses = new Array() }
populateDropdownCourse($('#Course1'), Courses);

populateDropdown($('#Weather'), WeatherType);
//populateDropdown($('#myDropdown'),WeatherType);
populateDropdown($('#SecondaryWeather'), WeatherType);
populateDropdownName($('#ActivityType'), top.ActivityTypes);
//debugger;
if (ModeAdd) {
	$('#Tides').dataTable({
		"bPaginate": false,
		"bLengthChange": true,
		"bFilter": false,
		"bSort": false,
		"bInfo": false,
		"bAutoWidth": true,
		"aLengthMenu": [-1]
	});
	$('#ActivityDate').datepicker('setDate', new Date());    
	populateTideTable();
	//alert("test:"+top.CBEvents.test);
	top.CBEvents.AddCallBack("GetTideHeights", "populateTideTable", populateTideTable);
	top.getActivityTypes();
	//debugger;
	//populateDropdownName($('#ActivityType'), top.ActivityTypes);
	top.CBEvents.AddCallBack("GetActivityTypes", "populateActivityType", populateActivityType);
	//debugger;
	var h = 0;
	try {
		h = this.outerHeight;
	} catch (err) {
	}
	if (typeof (h) == "undefined" || h == null || isNaN(h)) {
		h = parseInt(this.document.body.scrollHeight) + 5;
	}

	top.updateFrameSize(h);
	StartWizard();
	
	return;
}

//$('#Weather').val(TripDetails.Weather);

$('#UnusualWeather').val(ActivityDetails.UnusualWeather);
$('input#AirTemp').val(ActivityDetails.AirTemp);
$('input#WaterTemp').val(ActivityDetails.WaterTemp);
$('input#POB').val(ActivityDetails.POB);
//debugger;
//$('#ActivityDate').val(ActivityDetails.Date);
$('#ActivityDate').datepicker('setDate', ActivityDetails.date);    
// ToDO Find how to get text area text
$('#BarometricPreasure').val(ActivityDetails.BarometricPreasure);
$('#StartupProcedure').val(ActivityDetails.StartupProcedures);
$('#StartupProcedureComment').val(ActivityDetails.StartupProceduresComment);
$('#StartupProcedureInitial').val(ActivityDetails.StartupProceduresInitial);
$('#WeeklyCheck').val(ActivityDetails.WeeklyChecks);
$('#WeeklyCheckComment').val(ActivityDetails.WeeklyChecksComment);
$('#WeeklyCheckInitial').val(ActivityDetails.WeeklyChecksInitial);
$('#MonthlyCheck').val(ActivityDetails.MonthlyChecks);
$('#MonthlyCheckComment').val(ActivityDetails.MonthlyChecksComment);
$('#MonthlyCheckInitial').val(ActivityDetails.MonthlyChecksInitial);
$('#Maintenance').val(ActivityDetails.Maintenance);
$('#MaintenanceComment').val(ActivityDetails.MaintenanceComment);
$('#MaintenanceInitial').val(ActivityDetails.MaintenanceInitial);
$('#SafetyCheck').val(ActivityDetails.SafetyCheck);
$('#SafetyCheckComment').val(ActivityDetails.SafetyCheckComment);
$('#SafetyCheckInitial').val(ActivityDetails.SafetyCheckInitial);
$('#RadioCheck').val(ActivityDetails.RadioCheck);
$('#RadioCheckComment').val(ActivityDetails.RadioCheckComment);
$('#RadioCheckInitial').val(ActivityDetails.RadioCheckInitial);
$('#DepartTime').timeEntry('setTime', ActivityDetails.DepartTime);
$('#ReturnTime').timeEntry('setTime', ActivityDetails.ReturnTime);

//newActivity.Crew = getCrewIds(crewSelected);





setDropdown($('#Weather'),ActivityDetails.Weather);
setDropdown($('#SecondaryWeather'),ActivityDetails.SecondaryWeather);
var skip = ArrayFind(Crew,ActivityDetails.skipper);
var skipName= skip.First_Name+' '+skip.Last_Name;
SetSkipper(ActivityDetails.skipper,skipName);


for(var i=0;i<ActivityDetails.Crew.length;i++){

	CrewUnselected=ArrayRemoveItem(CrewUnselected,ActivityDetails.Crew[i]);
	crewSelected[i] = ArrayFind(Crew,ActivityDetails.Crew[i]);
	crewcount++;
	setDropdown($('#Crew'+(i)),ActivityDetails.Crew[i]);
	//$('#crewSpan'+(crewcount)).append('<p><select id="Crew'+(crewcount+1)+'" onchange="crewOnchange(this)"></select></p><span id="crewSpan'+ (crewcount+1) +'"></span>');    
	
//debugger;
}
try {
//var skip = ArrayFind(Crew,$('#Skippers').val());
//var skipName= skip.First_Name+' '+skip.Last_Name;
repopulateDropDowns(CrewUnselected,skip.id,skipName,crewSelected);
} catch (err) {
repopulateDropDowns(CrewUnselected);
}

setCallSign();
//debugger;
setDropdown($('#ActivityType'), ActivityDetails.ActivityType);
//if (top.debug) debugger;



$('#Tides').dataTable({
		"bPaginate": false,
		"bLengthChange": true,
		"bFilter": false,
		"bSort": false,
		"bInfo": false,
		"bAutoWidth": true, 
		"aLengthMenu": [-1]} );
populateTideTable();
//alert("test:"+top.CBEvents.test);
top.CBEvents.AddCallBack("GetTideHeights", "populateTideTable", populateTideTable);
//debugger;
var h3 = 0;
try {
	h3 = this.outerHeight;
} catch (err) {
}
if (typeof (h3) == "undefined" || h3 == null || isNaN(h3)) {
	h3 = parseInt(this.document.body.scrollHeight) + 5;
}

top.updateFrameSize(h3);
StartWizard();
}

function repopulateDropDowns(newList,id,value,CrewList){
 //if (top.debug) debugger;
	populateDropdownCrew($('#Skippers'), newList, true, id, value);
	ShowDropdowns('#CrewTable', crewcount + 1, 4);
 for (var i=0;i <= crewcount;i++) {
	var selectId = '#Crew'+(i+1);
	if (typeof(crewSelected[i])=='undefined'){
	populateDropdownCrew($(selectId),newList);   
	} else {
	var selected = crewSelected[i];
	if (typeof(CrewList) != 'undefined' ) {
		populateDropdownCrew($(selectId),newList,false,CrewList[i].id,CrewList[i].First_Name+' '+CrewList[i].Last_Name);   
	} else {
		populateDropdownCrew($(selectId),newList,false,selected.id,selected.First_Name+' '+selected.Last_Name);   
	}
	}
}
//debugger;
var h = 0;
try {
	h = this.outerHeight;
} catch (err) {
}
if (typeof (h) == "undefined" || h == null || isNaN(h)) {
	h = parseInt(this.document.body.scrollHeight) + 5;
}

top.updateFrameSize(h);
//top.updateFrameSize(this.outerHeight);
}

function populateActivityType(Types) {
	populateDropdownName($('#ActivityType'), Types);
}





function dateOnChange(t) {
top.getTideHeights($('#ActivityDate').datepicker( "getDate" ),top.ports);
}

function drillOnchange(t) {
	//debugger;
	var tr = '';
	if ((t.id.substring(11)-1)== DrillCount) {
		DrillCount = DrillCount + 1;
		tr += '<tr><td><select id="DrillSelect' + (DrillCount + 1) + '" name="DrillSelect' + (DrillCount + 1) + '" onchange="drillOnchange(this)"></select></td></tr>';
		$('#DrillTable > tbody:last').append(tr);
		//for (var i = 0; i <= DrillCount; i++) {
		//populateDropdown($('#DrillSelect' + (DrillCount + 1)), top.Drills, 'name');
		//}
	}
	rePopulateDrills(t);
	//debugger;
}

function rePopulateDrills() {
//    debugger;
	var tempDrills = ArrayCopy(top.Drills);
	//var newVal = t.options[t.selectedIndex].value;
	for (var i = 1; i <= (DrillCount+1); i++) {
		tempDrills = ArrayRemoveItem(tempDrills, $('#DrillSelect'+i).val());
	}
	for (var j = 1; i <= (DrillCount+1); j++) {
		populateDropdown($('#DrillSelect' + j), tempDrills, 'name');
	}
}

function scenarioOnchange(t) {
	debugger;
	var tr = '';
	if ((t.id.substring(14) - 1) == ScenarioCount) {
		ScenarioCount = ScenarioCount + 1;
		tr += '<tr><td><select id="ScenarioSelect' + (ScenarioCount + 1) + '" name="ScenarioSelect' + (ScenarioCount + 1) + '" onchange="scenarioOnchange(this)"></select></td></tr>';
		$('#ScenarioTable > tbody:last').append(tr);
		//for (var i = 0; i <= DrillCount; i++) {
		//populateDropdown($('#DrillSelect' + (DrillCount + 1)), top.Drills, 'name');
		//}
	}
	rePopulateScenarios(t);
	//debugger;
}

function rePopulateScenarios() {
	//    debugger;
	var tempScenarios = ArrayCopy(top.Scenarios);
	//var newVal = t.options[t.selectedIndex].value;
	for (var i = 1; i <= (ScenarioCount + 1); i++) {
		tempScenarios = ArrayRemoveItem(tempScenarios, $('#ScenarioSelect' + i).val());
	}
	for (var j = 1; j <= (ScenarioCount + 1); j++) {
		populateDropdown($('#ScenarioSelect' + j), tempScenarios, 'name');
	}
}

function crewOnchange(t) {
	//debugger;
var fid = t.id.substring(4) - 1;
var newVal = t.options[t.selectedIndex].value;
//crewSelected[fid] = t.outerText; 
if (crewSelected[fid] == ''||typeof(crewSelected[fid]) == 'undefined') {
	//if (top.debug) debugger;    
	CrewUnselected=ArrayRemoveItem(CrewUnselected,newVal);
	crewSelected[fid] = ArrayFind(Crew,newVal);
	crewcount++;
	//$('#crewSpan'+(crewcount)).append('<p><select id="Crew'+(crewcount+1)+'" onchange="crewOnchange(this)"></select></p><span id="crewSpan'+ (crewcount+1) +'"></span>');    
} else {
	//if (top.debug) debugger;
	if (newVal == '') {
		 CrewUnselected[CrewUnselected.length] = crewSelected[fid];
		 crewSelected = ArrayRemoveItem(crewSelected,crewSelected[fid].id);
		 crewcount = crewcount -1;
		 //$('#crewSpan'+(crewcount+1)).html('');
	} else {
	for (var i =0;i < CrewUnselected.length;i++) {
		if (CrewUnselected[i].id == newVal){
			CrewUnselected[i] = crewSelected[fid];        
		}
		
	}
	crewSelected[fid] = ArrayFind(Crew,newVal);

	}
	//CrewUnselected=
}

//CrewUnselected = ArrayRemoveItem(CrewUnselected,t.options[t.selectedIndex]); 

try {
var skip = ArrayFind(Crew,$('#Skippers').val());
var skipName= skip.First_Name+' '+skip.Last_Name;
repopulateDropDowns(CrewUnselected,skip.id,skipName);
} catch (err) {
repopulateDropDowns(CrewUnselected);
}
//if (top.debug) debugger;
setCallSign();
}

function setCallSign() {
	try {
//        debugger;
		var skip = ArrayFind(Crew, $('#Skippers').val());
		var cs = skip.userid;
		for (var i = 0; i < crewSelected.length; i++) {
			try {
				cs += " " + crewSelected[i].userid;
			} catch (err2) {
				top.LogMsg("Error [" + err2 + "] in adding crew member [" + i + "] to callsign");
			} 
		}
		$('#Callsign').html(cs);
	} catch (err) {
		top.LogMsg("Error ["+err+"] in setting call sign");
	}

}

function getCrewIds(inArr){
var retval = new Array();
for (var i=0;i<inArr.length;i++){
	retval[i] = inArr[i].id;
}
return retval;
}
function skipperOnchange(t) {
	var newVal = t.options[t.selectedIndex].value;
	//if (top.debug) debugger;
	SetSkipper(newVal, t.options[t.selectedIndex].text);
	setCallSign();
}

function SetSkipper(newVal,text) {
	//var newVal = t.options[t.selectedIndex].value;
	//if (top.debug) debugger;
	if (Skipper == '' || typeof(Skipper) == 'undefined') {
		 CrewUnselected=ArrayRemoveItem(CrewUnselected,newVal);         
	} else {
		//if (top.debug) debugger;
		if (newVal == '') {
			CrewUnselected[CrewUnselected.length] = ArrayFind(Crew,Skipper);
		} else {
		 ArrayUpdateItem(CrewUnselected,newVal,ArrayFind(Crew,Skipper));
		 }
	}
		repopulateDropDowns(CrewUnselected,newVal,text);
	
	Skipper = newVal;
}

  function populateDropdownCrew(select, data, skip,id,value) {    
	//if (top.debug) debugger;
	if (typeof(skip) == 'undefined') skip = false;
	select.html('');
	var addOptions = '';
	
		
	select.append($('<option></option>'));
	$.each(data, function(id) {    
			if (data[id].Skipper) { addOptions = ' id="BoldOption"';
			 } else { addOptions = ''};
			 //addOptions = ' style="font-weight: bold"';
			 if (!data[id].Deleted && data[id].Operational) {
			if (data[id].Skipper || !(skip)) {
			select.append($('<option value="'+ data[id].id + '"'+addOptions+'><bold>'+ data[id].First_Name +' ' +data[id].Last_Name+'</bold></option>'));    
			}}
	});  
	if (typeof(id) != 'undefined' && typeof(value) != 'undefined'){
	  select.append($('<option value="'+ id + '"'+addOptions+' selected="selected">'+ value+'</option>'));
	  select.text = value;
	}
}    
function populateDropdownCourse(select, data) {    
	//if (top.debug) debugger;
	select.html('');    
	$.each(data, function(id) {    
		select.append($('<option val='+ data[id].Id + '>'+ data[id].Name+'</option>'));    
	});  
}
function setTime(selectbox){
var curtime = new Date();

}    
// ]]>
function Cancel_onclick() {
	top.PopPage('FindActivities.htm');
//alert("1");
}
function Save_onclick() {
	var newActivity = top.curActivity;
	if (ModeAdd) {newActivity = top.BlankActivity; }
	//if (top.debug) debugger;
	newActivity.ActivityType = $('#ActivityType').val();
newActivity.AirTemp = $('input#AirTemp').val();
newActivity.WaterTemp = $('input#WaterTemp').val();
newActivity.skipper = Skipper;
newActivity.POB =  $('input#POB').val();
newActivity.DepartTime = $('#DepartTime').val();
newActivity.ReturnTime = $('#ReturnTime').val();
newActivity.Weather = $('#Weather').val();
newActivity.SecondaryWeather = $('#SecondaryWeather').val();
newActivity.UnusualWeather = $('textarea#UnusualWeather').val();
newActivity.date = $('#ActivityDate').val();
// ToDO Find how to get text area text
newActivity.BarometricPreasure = $('#BarometricPreasure').val();
newActivity.StartupProcedures = $('#StartupProcedure').val();
newActivity.StartupProceduresComment = $('#StartupProcedureComment').val();
newActivity.StartupProceduresInitial = $('#StartupProcedureInitial').val();
newActivity.WeeklyChecks = $('#WeeklyCheck').val();
newActivity.WeeklyChecksComment = $('#WeeklyCheckComment').val();
newActivity.WeeklyChecksInitial = $('#WeeklyCheckInitial').val();
newActivity.MonthlyChecks = $('#MonthlyCheck').val();
newActivity.MonthlyChecksComment = $('#MonthlyCheckComment').val();
newActivity.MonthlyChecksInitial = $('#MonthlyCheckInitial').val();
newActivity.Maintenance = $('#Maintenance').val();
newActivity.MaintenanceComment = $('#MaintenanceComment').val();
newActivity.MaintenanceInitial = $('#MaintenanceInitial').val();
newActivity.SafetyCheck = $('#SafetyCheck').val();
newActivity.SafetyCheckComment = $('#SafetyCheckComment').val();
newActivity.SafetyCheckInitial = $('#SafetyCheckInitial').val();
newActivity.RadioCheck = $('#RadioCheck').val();
newActivity.RadioCheckComment = $('#RadioCheckComment').val();
newActivity.RadioCheckInitial = $('#RadioCheckInitial').val();
newActivity.Crew = getCrewIds(crewSelected);
top.addActivity(newActivity);
	top.PopPage('FindActivities.htm');
}

function ShowDropdowns(table, dropdowncount, cols) {
	//debugger;
	$(table).find("tbody").html("");
	var noOfRows = Math.floor(dropdowncount / cols) + 1;
	var colno = 1;
	for (var i = 0; i < noOfRows; i++) {
		var td="<tr>";
		for (var j=0;j < cols;j++){
			if (colno <= dropdowncount) {
				var temp = "Crew"+(colno);
			td = td + '<td><select id="'+temp+'" onchange="crewOnchange(this)"></select></td>';
			} else {
			td = td + '<td></td>';
			}
			colno++;
		}
		td += '</tr>';
		//debugger;
		$(table + ' > tbody:last').append(td);
	}


}
function Print_onclick() {
 //   debugger;
	var TripSel = '{Trip.id}="{'+ActivityID+'}"';
	top.PopPage('..\\Reporting\\Report.aspx?SessionId=' + top.SessionId + '&filename=TripReport&SelectionCriteria='+TripSel);
}

function gotActivityProcedures(procedures) {

	//debugger;
	OrigProcedures = procedures;
	$('#ProcedureTable').find("tbody").html("");
	var tr = "";
	for (var i = 0; i < procedures.length; i++) {
		tr += '<tr><td>'+ProcedureTypes[procedures[i].Type]+'</td><td>' + procedures[i].Name + '</td><td><input id="ProcedureComments' + (i + 1) + '"></input></td><td><input id="ProcedureComplete'+(i+1)+'" type="checkbox" /></td></tr>';
	}
	$('#ProcedureTable > tbody:last').append(tr);
}
function activityOnChange() {
}

function gotActivityDrills(drills) {
	
	//debugger;
	//OrigProcedures = procedures;
	$('#DrillTable').find("tbody").html("");
	var tr = "";
	var len =0;
	if (typeof (drills) != "undefined") {
		len = drills.length;
	}
	DrillCount = len;
	tr = '<tr><td>Drills</td></tr>';
	for (var i = 0; i < len; i++) {
		//tr += '<tr><td><select id="DrillSelect' + (i + 1) + '" name="DrillSelect' + (i + 1) + '" onchange="drillOnchange(this)"><option value="' + drills[i].DrillId + '">' +drills[i].DrillId+ '</option></select></td></tr>';
		tr += '<tr><td><select id="DrillSelect' + (i + 1) + '" name="DrillSelect' + (i + 1) + '" onchange="drillOnchange(this)"></select></td></tr>'; 
	}
	tr += '<tr><td><select id="DrillSelect' + (len + 1) + '" name="DrillSelect' + (len + 1) + '" onchange="drillOnchange(this)"></select></td></tr>'; 
	
	
	$('#DrillTable > tbody:last').append(tr);
	for (var k = 0; k <= len; k++) {
		populateDropdown($('#DrillSelect' + (k+1)), top.Drills,'name');
	}
	for (var j = 0; j < len; j++) {

		setDropdown($("#DrillSelect" + (i + 1)), drills[i].DrillId);
	} 
}
function gotActivityScenarios(scenarios) {

	$('#ScenarioTable').find("tbody").html("");
	var tr = "";
	var len = 0;
	if (typeof (scenarios) != "undefined") {
		len = scenarios.length;
	}
	ScenarioCount = len;
	tr = '<tr><td>Scenarios</td></tr>';
	for (var i = 0; i < len; i++) {
		//tr += '<tr><td><select id="DrillSelect' + (i + 1) + '" name="DrillSelect' + (i + 1) + '" onchange="drillOnchange(this)"><option value="' + drills[i].DrillId + '">' +drills[i].DrillId+ '</option></select></td></tr>';
		tr += '<tr><td><select id="ScenarioSelect' + (i + 1) + '" name="ScenarioSelect' + (i + 1) + '" onchange="scenarioOnchange(this)"></select></td></tr>';
	}
	tr += '<tr><td><select id="ScenarioSelect' + (len + 1) + '" name="ScenarioSelect' + (len + 1) + '" onchange="scenarioOnchange(this)"></select></td></tr>';


	$('#ScenarioTable > tbody:last').append(tr);
	for (var i = 0; i <= len; i++) {
		populateDropdown($('#ScenarioSelect' + (i + 1)), top.Scenarios, 'name');
	}
	for (var i = 0; i < len; i++) {

		setDropdown($("#ScenarioSelect" + (i + 1)), scenarios[i].ScenarioId);
	} 
}
function isProcedureUpdated(id) {
	var retval = false;
	try {
		if ($('#ProcedureComments' + (id + 1)).val() != OrigProcedures[id].Comments) {
			return true;
			//debugger;
		} 
		if ($('#ProcedureComplete'+(id+1)).is(':checked')) {
			return true;
		}
	} catch (err) { }
	return retval;
}


function Procedures_onclick() {
	//debugger;

	$("#ProcedureDiv").dialog({
		width: 'auto',

		buttons:
	{
		"Ok": function() { SaveProcedures() },
		"Cancel": function() { $(this).dialog("close"); }
	}
	});

}
function Scenarios_onclick() {
	//debugger;

	$("#ScenarioDiv").dialog({
		width: 'auto',

		buttons:
	{
		"Ok": function() { SaveScenarios() },
		"Cancel": function() { $(this).dialog("close"); }
	}
	});

}
function Drills_onclick() {
	//debugger;

	$("#DrillDiv").dialog({
		width: 'auto',

		buttons:
	{
		"Ok": function() { SaveDrills() },
		"Cancel": function() { $(this).dialog("close"); }
	}
	});

}

function showDialog(did) {
	//debugger;
	dialogName = dialogArray[did];
	
	$("#"+dialogName+"Div").dialog({
		width: 800,
		height: 600,
		buttons:
	{
		"Previous": function() { WizardPrev(did) },           
		"Save": function() { WizardSave(did) },
		"Cancel": function() { $(this).dialog("close")},
		"Next": function() { WizardNext(did) } 
	   
	}
	});

}

function WizardSave(did) {
	Save_onclick();
}

function WizardPrev(did) {
	if (did > 0) {
		$("#" + dialogArray[did] + "Div").dialog("close");
		showDialog(did-1);
	}
}

function WizardNext(did) {
	if (did < dialogArray.length) {
		$("#" + dialogArray[did] + "Div").dialog("close");
		showDialog(did + 1);
	}

}


function SaveProcedures() {

	//debugger;
	for (var i = 0; i < OrigProcedures.length; i++) {
		if (isProcedureUpdated(i)) {
			var curProcedure = OrigProcedures[i];
			curProcedure.Comments = $('#ProcedureComments'+(i+1)).val()
			if ($('#ProcedureComplete'+(i+1)).is(':checked')) {
				curProcedure.DateCompleted = new Date();
				curProcedure.CompletedBy = top.Session.Memberid;
			}
			top.modifyProcedureInfo(curProcedure);
		}
	}
	$("#ProcedureDiv").dialog("close");
}


function SaveDrills() {

	//debugger;
	var OrigDrillsCount = 0;
	var curDrill;
	try {
		if (typeof (OrigDrills) != "undefined" && OrigDrills.length > 0) { OrigDrillsCount = OrigDrills.length; }
	} catch (err) {
	}
	for (var i = 0; i < (OrigDrillsCount); i++) {
		if (isDrillUpdated(i)) {
			//debugger;
			curDrill = OrigDrills[i];
			curDrill.DrillId = $('#DrillSelect' + (i + 1)).val();
			curDrill.Activity = ActivityID;
			top.BEModifyActivityDrill(curDrill);    
			//top.modifyProcedureInfo(curProcedure);
		}
	}

	for (var i = OrigDrillsCount; i < (DrillCount); i++) {
		curDrill = newActivityDrill;
		curDrill.DrillId = $('#DrillSelect' + (i + 1)).val();
		curDrill.Activity = ActivityID;
		top.BEModifyActivityDrill(curDrill);
	}
	//debugger;
	$("#DrillDiv").dialog("close");
}



function SaveScenarios() {

	//debugger;
	var OrigScenariosCount = 0;
	try {
		if (typeof (OrigScenarios) != "undefined" && OrigScenarios.length > 0) { OrigScenariosCount = Origscenarios.length; }
	} catch (err) {
	}
	for (var i = 0; i < (OrigScenariosCount); i++) {
		if (isScenarioUpdated(i)) {
			//debugger;
			var curScenario = OrigScenarios[i];
			curScenario.ScenarioId = $('#ScenarioSelect' + (i + 1)).val();
			curScenario.Activity = ActivityID;
			top.BEModifyActivityScenario(curScenario);
			//top.modifyProcedureInfo(curProcedure);
		}
	}

	for (var i = OrigScenariosCount; i < (ScenarioCount); i++) {
		var curScenario = newActivityScenario;
		curScenario.ScenarioId = $('#ScenarioSelect' + (i + 1)).val();
		curScenario.Activity = ActivityID;
		top.BEModifyActivityScenario(curScenario);
	}
	//debugger;
	$("#ScenarioDiv").dialog("close");
}