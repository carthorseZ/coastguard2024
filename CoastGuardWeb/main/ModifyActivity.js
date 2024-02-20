var crewcount = 0;
var coursesSelected = new Array();
var crewSelected = new Array();
var Crew;
var Courses;
var dialogArray = ["Main", "Crew", "Tides", "Procedures", "Drills", "Scenarios","Destinations","Engines"];
var CrewUnselected;
var CoursesUnSelected;
var Skipper;
var WeatherType = ["Sun", "Cloud", "Rain", "Wind", "Night"];
var YesNo = ["Yes","No"];
var ActivityID;

//var ActivityType;
var ActivityDetails;
var OrigProcedures;
var OrigPassangers;
var OrigScenarios;
var OrigDestinations;
var OrigDrills;
var DrillsUnSelected;
var newActivityDrill;
var newActivityScenario;
var newActivityDestination;
var newActivityPassanger;
var ProcedureTypes = ["Weekly", "Monthly", "StartUp", "Maintenance", "Safety", "Radio"];
var ModeAdd = false;
var Cap;
var DrillCount = 0;
var PassangerCount = 0;
var ScenarioCount = 0;
var DestinationCount = 0;
var POB = 0;
var Vessels;
var curVessel;
$(document).ready(function () {

    //debugger;
    //top.updateFrameSize(this.outerHeight);
    ActivityID = $.getURLParam('ActivityId');
    if (typeof (ActivityID) == 'undefined' || ActivityID == null) ModeAdd = true;
    $("#ProcedureDiv").hide();
    $("#ScenarioDiv").hide();
    $("#DestinationDiv").hide();
    $("#DrillDiv").hide();
    $("#PassangerDiv").hide();
    $("#EngineDiv").hide();
    $(".DryOnly").hide();

    $(".WetOnly").hide();
    gotActivityDrills();
    gotActivityScenarios();
    gotActivityDestinations();
    gotActivityPassangers();
    top.CBEvents.AddCallBack("GetActivity", "setupForm", setupForm);
    top.CBEvents.AddCallBack("GetVesselList", "gotVessels", gotVessels);
    top.CBEvents.AddCallBack("NewActivityID", "ModifyActivityAdd", NewActivityId);
    top.CBEvents.AddCallBack("GetEngineStartHours", "ModifyActivity", GotStartHours);
    top.CBEvents.AddCallBack('GetActivityProcedures', 'GetActivityProcedures', gotActivityProcedures);
    top.CBEvents.AddCallBack('NewObject:ActivityDrillInfo', 'NewObject:ActivityDrillInfo', gotNewActivityDrillInfo);
    top.CBEvents.AddCallBack('NewObject:ActivityScenarioInfo', 'NewObject:ActivityScenarioInfo', gotNewActivityScenarioInfo);
    top.CBEvents.AddCallBack('NewObject:ActivityDestinationInfo', 'NewObject:ActivityDestinationInfo', gotNewActivityDestinationInfo);
    top.CBEvents.AddCallBack('NewObject:ActivityPassangerInfo', 'NewObject:ActivityPassangerInfo', gotNewActivityPassangerInfo);
    top.CBEvents.AddCallBack('GetObjects:ActivityDrillInfo', 'GetObjects:ActivityDrillInfo', gotActivityDrills);
    top.CBEvents.AddCallBack('GetObjects:ActivityScenarioInfo', 'GetObjects:ActivityScenarioInfo', gotActivityScenarios);
    top.CBEvents.AddCallBack('GetObjects:ActivityDestinationInfo', 'GetObjects:ActivityDestinationInfo', gotActivityDestinations);
    top.CBEvents.AddCallBack('GetObjects:ActivityPassangerInfo', 'GetObjects:ActivityPassangerInfo', gotActivityPassangers);
    top.BEGetActivityProcedures();
    top.BEGetVesselList();
    top.BENewObject("ActivityDrillInfo");
    top.BENewObject("ActivityPassangerInfo");
    top.BENewObject("ActivityDestinationInfo");
    top.BENewObject("ActivityScenarioInfo");
    //top.BEGetObjects("ActivityDrillInfo","ActivityId = '"+ActivityID+"'");
    top.BEGetObjects("ActivityDrillInfo", ActivityID);
    top.BEGetObjects("ActivityScenarioInfo", ActivityID);
    top.BEGetObjects("ActivityDestinationInfo", ActivityID);
    top.BEGetObjects("ActivityPassangerInfo", ActivityID);
    //top.BEGetEngineStartHours();
    if (ModeAdd) {

        setupForm();
    } else {

        top.BEGetActivity(ActivityID);

    }
});

function gotVessels(result) {
    //debugger;
    if (result.length == 1) {
        curVessel = result[0];
        $('#Vessel').val(result[0].id);
        $('#Vessel').hide();
        $('#VesselLabel').hide();
        if (ModeAdd) {
            top.BEGetEngineStartHours(curVessel.id, -1);
        
        } else {
            //debugger;
            
        }
    } else {
    if (result.length > 1) {
        curVessel = result[0];
        if (ModeAdd) {
            top.BEGetEngineStartHours(curVessel.id, -1);

        } else {
            
        }
    }
        $('#Vessel').html('');
        $('#VesselLabel').show();
        for (var i = 0; i < result.length; i++) {
            $('#Vessel').append($('<option value=' + result[i].id + '>' + result[i].name + '</option>'));
        }
        $('#Vessel').show();
    }


    
}

function NewActivityId(ActID) {
    if (typeof (ActID) != "undefined" && ActID != null) {
        if (typeof (ActID.id) != "undefined" && ActID.id != null) ActivityID = ActID.id
        if (typeof (ActID.LogNo) != "undefined" && ActID.LogNo != null) $('#LogNo').text(ActID.LogNo);
        top.curActivity = top.cloneObject(top.BlankActivity);
        top.curActivity.id = ActID.id;
        top.curActivity.LogNo = ActID.LogNo;
        ModeAdd = false;
    }
}

function gotNewActivityScenarioInfo(result) {
    newActivityScenario = result;
}


function gotNewActivityDestinationInfo(result) {
    //debugger;
    newActivityDestination = result;
}

function gotNewActivityDrillInfo(result) {
    newActivityDrill = result;
}

function gotNewActivityPassangerInfo(result) {
    newActivityPassanger = result;
}


function setOtherPOB(inPOB) {
    try {
        $("#NamedPassangerCount").text(PassangerCount);
        var namedPOB = (top.toInt(PassangerCount) + top.toInt(crewcount));
        var tempskip = $("#Skippers").val();
        if (typeof (tempskip) != "undefined" && tempskip != "") {
            namedPOB++;
        }
        $("#OtherPeople").val(inPOB - namedPOB);
        $("#POB").text(inPOB);

    } catch (ex) {
        LogMsg("An error " + ex + " occured in setOtherPOB");
    }
}




function updatePOBCounts() {
    try {
        $("#NamedPassangerCount").text(PassangerCount);
        POB = (top.toInt(PassangerCount) + top.toInt(crewcount) + top.toInt($("#OtherPeople").val()));
        var tempskip = $("#Skippers").val();
        if (typeof (tempskip) != "undefined" && tempskip != "") {
            POB++;
        }
        $("#POB").text(POB);
        
    } catch (ex) {
    LogMsg("An error " + ex + " occured in updatePOBCounts");
    }
}

function OtherPeople_OnChange() {
    updatePOBCounts();
}
function setupForm() {
    //debugger;
    if (ModeAdd) {
        ActivityDetails = top.cloneObject(top.BlankActivity);
    } else {
        ActivityDetails = top.cloneObject(top.curActivity);
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
    $('#DepartTime').timeEntry({ spinnerImage: teImage, show24Hours:true});
    $('#ReturnTime').timeEntry({ spinnerImage: teImage, show24Hours:true });
    $('#ActivityDate').datepicker({ dateFormat: 'dd/mm/yy' });
    
    populateDropdownCrew($('#Skippers'), Cap);
    try {
        Crew = top.getCrew();
    } catch (Err) { Crew = new Array(); }

    //if (top.debug) debugger;
    //CrewUnselected = ArrayCopy(Crew);
    CrewUnselected = Crew;
    ShowDropdowns('#CrewTable', 1, 4);
    populateDropdownCrew($('#Crew1'), Crew);
    try {
        Courses = top.getCourses();
    } catch (Err) { Courses = new Array() }
    populateDropdownCourse($('#Course1'), Courses);
    populateDropdown($('#FirepumpStarted'), YesNo);
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
        
        top.CBEvents.AddCallBack("GetTideHeights", "populateTideTable", populateTideTable);
        top.getActivityTypes();
        //debugger;
        //populateDropdownName($('#ActivityType'), top.ActivityTypes);
        top.CBEvents.AddCallBack("GetActivityTypes", "populateActivityType", populateActivityType);
        if (ModeAdd) {
            //debugger;
            //setDropdown($("#ActivityType"), $("#ActivityType option:first").next().val());
            ActivityTypeOnChange(null,$("#ActivityType option:first").next().val());
            //ActivityTypeOnChange(null,$("#ActivityType option:first").next().val())
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
        //debugger;
        UpdateNewActivity();
        
        return;
    }

    //$('#Weather').val(TripDetails.Weather);
    $('#LogNo').text(ActivityDetails.LogNo);
    $('#UnusualWeather').val(ActivityDetails.UnusualWeather);
    $('input#AirTemp').val(ActivityDetails.AirTemp);
    $('input#WaterTemp').val(ActivityDetails.WaterTemp);
    //$('input#POB').val(ActivityDetails.POB);
    //debugger;
    $('#Notes').val(ActivityDetails.Notes);
    //debugger;
    //$('#ActivityDate').val(ActivityDetails.Date);
    $('#ActivityDate').datepicker('setDate', ActivityDetails.date);
    // ToDO Find how to get text area text
    $('#BarometricPreasure').val(ActivityDetails.BarometricPreasure);
    //$('#StartupProcedure').val(ActivityDetails.StartupProcedures);
    $('#StartupProcedure').attr('checked', ActivityDetails.StartupProcedures);
    $('#StartupProcedureComment').val(ActivityDetails.StartupProceduresComment);
    $('#StartupProcedureInitial').val(ActivityDetails.StartupProceduresInitial);
    //$('#WeeklyCheck').val(ActivityDetails.WeeklyChecks);
    $('#WeeklyCheck').attr('checked', ActivityDetails.WeeklyChecks);
    $('#WeeklyCheckComment').val(ActivityDetails.WeeklyChecksComment);
    $('#WeeklyCheckInitial').val(ActivityDetails.WeeklyChecksInitial);
    //$('#MonthlyCheck').val(ActivityDetails.MonthlyChecks);
    $('#MonthlyCheck').attr('checked', ActivityDetails.MonthlyChecks);
    $('#MonthlyCheckComment').val(ActivityDetails.MonthlyChecksComment);
    $('#MonthlyCheckInitial').val(ActivityDetails.MonthlyChecksInitial);
   // $('#Maintenance').val(ActivityDetails.Maintenance);
    $('#Maintenance').attr('checked', ActivityDetails.Maintenance);
    $('#MaintenanceComment').val(ActivityDetails.MaintenanceComment);
    $('#MaintenanceInitial').val(ActivityDetails.MaintenanceInitial);
    //$('#SafetyCheck').val(ActivityDetails.SafetyCheck);
    $('#SafetyCheck').attr('checked', ActivityDetails.SafetyCheck);
    $('#SafetyCheckComment').val(ActivityDetails.SafetyCheckComment);
    $('#SafetyCheckInitial').val(ActivityDetails.SafetyCheckInitial);
    //$('#RadioCheck').val(ActivityDetails.RadioCheck);
    $('#RadioCheck').attr('checked', ActivityDetails.RadioCheck);
    $('#RadioCheckComment').val(ActivityDetails.RadioCheckComment);
    $('#RadioCheckInitial').val(ActivityDetails.RadioCheckInitial);
    $('#DepartTime').timeEntry('setTime', ActivityDetails.DepartTime);
    $('#ReturnTime').timeEntry('setTime', ActivityDetails.ReturnTime);
    $('#EnginePortRunHours').val(ActivityDetails.EnginePortRunHours);
    $('#EngineFuelLitres').val(ActivityDetails.EngineFuelLitres);
    $('#EnginePortTotalHours').val(ActivityDetails.EnginePortTotalHours);
    $('#EnginePortOilAdded').val(ActivityDetails.EnginePortOilAdded);
    $('#EngineStarboardRunHours').val(ActivityDetails.EngineStarboardRunHours);
    $('#EngineFuelDollars').val(ActivityDetails.EngineFuelDollars);
    $('#EngineStarboardTotalHours').val(ActivityDetails.EngineStarboardTotalHours);
    $('#EngineStarboardOilAdded').val(ActivityDetails.EngineStarboardOilAdded);

    //newActivity.Crew = getCrewIds(crewSelected);
    //debugger;
    if (ActivityDetails.FirepumpStarted) $('#FirepumpStarted').val('Yes');



    setDropdown($('#Weather'), ActivityDetails.Weather);
    setDropdown($('#SecondaryWeather'), ActivityDetails.SecondaryWeather);
    var skip = ArrayFind(Crew, ActivityDetails.skipper);
    var skipName = '';
    if (skip != null && typeof (skip.First_Name) != "undefined" && typeof (skip.Last_Name) != "undefined") {
        try {
            skipName = skip.First_Name + ' ' + skip.Last_Name;
        } catch (e) {
        debugger;
        }
    }
    SetSkipper(ActivityDetails.skipper, skipName);


    for (var i = 0; i < ActivityDetails.Crew.length; i++) {

        CrewUnselected = ArrayRemoveItem(CrewUnselected, ActivityDetails.Crew[i]);
        crewSelected[i] = ArrayFind(Crew, ActivityDetails.Crew[i]);
        crewcount++;
        setDropdown($('#Crew' + (i)), ActivityDetails.Crew[i]);
        //$('#crewSpan'+(crewcount)).append('<p><select id="Crew'+(crewcount+1)+'" onchange="crewOnchange(this)"></select></p><span id="crewSpan'+ (crewcount+1) +'"></span>');    

        //debugger;
    }
    try {
        //var skip = ArrayFind(Crew,$('#Skippers').val());
        //var skipName= skip.First_Name+' '+skip.Last_Name;
        repopulateDropDowns(CrewUnselected, skip.id, skipName, crewSelected);
    } catch (err) {
        repopulateDropDowns(CrewUnselected);
    }

    setCallSign();
    setOtherPOB(ActivityDetails.POB);
    //debugger;
    setDropdown($('#ActivityType'), ActivityDetails.ActivityType);
    ActivityTypeOnChange(null, ActivityDetails.ActivityType);
    
    
    //if (top.debug) debugger;



    $('#Tides').dataTable({
        "bPaginate": false,
        "bLengthChange": true,
        "bFilter": false,
        "bSort": false,
        "bInfo": false,
        "bAutoWidth": true,
        "aLengthMenu": [-1]
    });
    populateTideTable();
    //alert("test:"+top.CBEvents.test);
    top.CBEvents.AddCallBack("GetTideHeights", "populateTideTable", populateTideTable);
    //debugger;
    top.BEGetEngineStartHours(ActivityDetails.CRV, ActivityDetails.LogNo);
    setFrameSize();

}

function setFrameSize() {
    var h2 = 0;
    try {
        h2 = this.outerHeight;
    } catch (err) {
    }
    if (typeof (h2) == "undefined" || h2 == null || isNaN(h2)) {
        try {
            h2 = parseInt(this.document.body.scrollHeight) + 5;
        } catch (e) {
            h2 = 660;
        }
    }

    top.updateFrameSize(h2);
}

function UpdateNewActivity() {
    //debugger;
    $('#LogNo').text(ActivityDetails.LogNo);
}

function repopulateDropDowns(newList, id, value, CrewList) {
    //if (top.debug) debugger;
    populateDropdownCrew($('#Skippers'), newList, true, id, value);
    ShowDropdowns('#CrewTable', crewcount + 1, 4);
    for (var i = 0; i <= crewcount; i++) {
        var selectId = '#Crew' + (i + 1);
        if (typeof (crewSelected[i]) == 'undefined') {
            populateDropdownCrew($(selectId), newList);
        } else {
            var selected = crewSelected[i];
            if (typeof (CrewList) != 'undefined') {
                populateDropdownCrew($(selectId), newList, false, CrewList[i].id, CrewList[i].First_Name + ' ' + CrewList[i].Last_Name);
            } else {
                populateDropdownCrew($(selectId), newList, false, selected.id, selected.First_Name + ' ' + selected.Last_Name);
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

    top.getTideHeights($('#ActivityDate').datepicker("getDate"), top.ports);
    if (typeof(ActivityID) == "undefined" || ActivityID == null || ActivityID ==0) {
        var LogType = $('#ActivityType').val();
        if (typeof(LogType) != "undefined" && LogType != null && LogType != "") {
            this.Save_onclick();
        }
    }

}

function drillOnchange(t) {
    //debugger;
    var tr = '';
    if ((t.id.substring(11) - 1) == DrillCount) {
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

function vesselOnchange(t) {
    //debugger;
    curVessel = t[t.selectedIndex];
    vesselID = t.value;
    top.BEGetEngineStartHours(t.value, -1);
    //BEGetEngineStartHours.(curVessel.id, -1);

}


function passangerOnchange(t) {
    //debugger;
    var tr = '';
    if ((t.id.substring(13) - 1) == PassangerCount) {
        PassangerCount = PassangerCount + 1;
        tr += '<tr><td><input id="PassangerName' + (1+top.toInt(PassangerCount)) + '" onchange="passangerOnchange(this)"></input></td></tr>';
        $('#PassangerTable > tbody:last').append(tr);
        //for (var i = 0; i <= DrillCount; i++) {
        //populateDropdown($('#DrillSelect' + (DrillCount + 1)), top.Drills, 'name');
        //}
    }
    updatePOBCounts();
    //rePopulateDrills(t);
    //debugger;
}
function rePopulateDrills() {
    //    debugger;
    var tempDrills = ArrayCopy(top.Drills);
    //var newVal = t.options[t.selectedIndex].value;
    for (var i = 1; i <= (DrillCount + 1); i++) {
        tempDrills = ArrayRemoveItem(tempDrills, $('#DrillSelect' + i).val());
    }
    for (var j = 1; j <= (DrillCount + 1); j++) {
        populateDropdown($('#DrillSelect' + j), tempDrills, 'name');
    }
}

function scenarioOnchange(t) {
    //debugger;
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

function destinationOnchange(t) {
    //debugger;
    var tr = '';
    if ((t.id.substring(17) - 1) == DestinationCount) {
        DestinationCount = DestinationCount + 1;
        tr += '<tr><td><select id="DestinationSelect' + (DestinationCount + 1) + '" name="DestinationSelect' + (DestinationCount + 1) + '" onchange="destinationOnchange(this)"></select></td></tr>';
        $('#DestinationTable > tbody:last').append(tr);
        //for (var i = 0; i <= DrillCount; i++) {
        //populateDropdown($('#DrillSelect' + (DrillCount + 1)), top.Drills, 'name');
        //}
    }
    rePopulateDestinations(t);
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

function rePopulateDestinations() {
       // debugger;
    var tempDestinations = ArrayCopy(top.Destinations);
    //var newVal = t.options[t.selectedIndex].value;
    for (var i = 1; i <= (DestinationCount + 1); i++) {
        tempDestinations = ArrayRemoveItem(tempDestinations, $('#DestinationSelect' + i).val());
    }
    for (var j = 1; j <= (DestinationCount + 1); j++) {
        populateDropdown($('#DestinationSelect' + j), tempDestinations, 'name');
    }
}

function crewOnchange(t) {
    //debugger;
    var fid = t.id.substring(4) - 1;
    var newVal = t.options[t.selectedIndex].value;
    //crewSelected[fid] = t.outerText; 
    if (crewSelected[fid] == '' || typeof (crewSelected[fid]) == 'undefined') {
        //if (top.debug) debugger;    
        CrewUnselected = ArrayRemoveItem(CrewUnselected, newVal);
        crewSelected[fid] = ArrayFind(Crew, newVal);
        crewcount++;
        //$('#crewSpan'+(crewcount)).append('<p><select id="Crew'+(crewcount+1)+'" onchange="crewOnchange(this)"></select></p><span id="crewSpan'+ (crewcount+1) +'"></span>');    
    } else {
        //if (top.debug) debugger;
        if (newVal == '') {
            CrewUnselected[CrewUnselected.length] = crewSelected[fid];
            crewSelected = ArrayRemoveItem(crewSelected, crewSelected[fid].id);
            crewcount = crewcount - 1;
            //$('#crewSpan'+(crewcount+1)).html('');
        } else {
            for (var i = 0; i < CrewUnselected.length; i++) {
                if (CrewUnselected[i].id == newVal) {
                    CrewUnselected[i] = crewSelected[fid];
                }

            }
            crewSelected[fid] = ArrayFind(Crew, newVal);

        }
        //CrewUnselected=
    }

    //CrewUnselected = ArrayRemoveItem(CrewUnselected,t.options[t.selectedIndex]); 

    try {
        var skip = ArrayFind(Crew, $('#Skippers').val());
        var skipName = skip.First_Name + ' ' + skip.Last_Name;
        repopulateDropDowns(CrewUnselected, skip.id, skipName);
    } catch (err) {
        repopulateDropDowns(CrewUnselected);
    }
    //if (top.debug) debugger;
    setCallSign();
    updatePOBCounts();
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
        top.LogMsg("Error [" + err + "] in setting call sign");
    }

}

function getCrewIds(inArr) {
    var retval = new Array();
    for (var i = 0; i < inArr.length; i++) {
        retval[i] = inArr[i].id;
    }
    return retval;
}
function skipperOnchange(t) {
    var newVal = t.options[t.selectedIndex].value;
    //if (top.debug) debugger;
    SetSkipper(newVal, t.options[t.selectedIndex].text);
    updatePOBCounts();
    setCallSign();
}

function SetSkipper(newVal, text) {
    //var newVal = t.options[t.selectedIndex].value;
    //if (top.debug) debugger;
    if (Skipper == '' || typeof (Skipper) == 'undefined') {
        CrewUnselected = ArrayRemoveItem(CrewUnselected, newVal);
    } else {
        //if (top.debug) debugger;
        if (newVal == '') {
            CrewUnselected[CrewUnselected.length] = ArrayFind(Crew, Skipper);
        } else {
            ArrayUpdateItem(CrewUnselected, newVal, ArrayFind(Crew, Skipper));
        }
    }
    repopulateDropDowns(CrewUnselected, newVal, text);

    Skipper = newVal;
}

function populateDropdownCrew(select, data, skip, id, value) {
    //if (top.debug) debugger;
    if (typeof (skip) == 'undefined') skip = false;
    select.html('');
    var addOptions = '';


    select.append($('<option></option>'));
    $.each(data, function(id) {
        if (data[id].Skipper) {
            addOptions = ' id="BoldOption"';
        } else { addOptions = '' };
        //addOptions = ' style="font-weight: bold"';
        if (!data[id].Deleted && data[id].Operational) {
            if (data[id].Skipper || !(skip)) {
                select.append($('<option value="' + data[id].id + '"' + addOptions + '><bold>' + data[id].First_Name + ' ' + data[id].Last_Name + '</bold></option>'));
            } 
        }
    });
    if (typeof (id) != 'undefined' && typeof (value) != 'undefined') {
        select.append($('<option value="' + id + '"' + addOptions + ' selected="selected">' + value + '</option>'));
        select.text = value;
    }
}
function populateDropdownCourse(select, data) {
    //if (top.debug) debugger;
    select.html('');
    $.each(data, function(id) {
        select.append($('<option val=' + data[id].Id + '>' + data[id].Name + '</option>'));
    });
}
function setTime(selectbox) {
    var curtime = new Date();

}
// ]]>
function Cancel_onclick() {
    top.PopPage('FindActivities.htm');
    //alert("1");
}

function SaveAndClose_onclick() {
Save_onclick();
top.PopPage('FindActivities.htm');
}





function Save_onclick() {
    var newActivity = top.curActivity;


    //debugger;
    
    
    if (ModeAdd || newActivity == null || typeof (newActivity) == "undefined") {
        newActivity = top.cloneObject(top.BlankActivity);
        newActivity.CRV = curVessel.id;
        //newActivity.PortEngine = Boat.LeftEngine;
        //newActivity.StarboardEngine = Boat.RightEngine;
     }


    newActivity.CRV = $('#Vessel').val();
    //if (top.debug) debugger;
    newActivity.ActivityType = $('#ActivityType').val();
    newActivity.AirTemp = $('input#AirTemp').val();
    newActivity.WaterTemp = $('input#WaterTemp').val();
    newActivity.skipper = Skipper;
    newActivity.POB = $('#POB').text();
    newActivity.DepartTime = $('#DepartTime').val();
    newActivity.ReturnTime = $('#ReturnTime').val();
    newActivity.Weather = $('#Weather').val();
    newActivity.SecondaryWeather = $('#SecondaryWeather').val();
    newActivity.UnusualWeather = $('textarea#UnusualWeather').val();
    newActivity.Notes = $('textarea#Notes').val();
    newActivity.date = $('#ActivityDate').val();

    var tripDate = top.toDate(newActivity.date);
    var DepartTime = top.toDateTime(newActivity.DepartTime, tripDate);
    var ReturnTime = top.toDateTime(newActivity.ReturnTime, tripDate);
    if (DepartTime > ReturnTime) {
      //  debugger;
    }

    // ToDO Find how to get text area text
    newActivity.BarometricPreasure = $('#BarometricPreasure').val();
    newActivity.StartupProcedures = $('#StartupProcedure').is(':checked');
    newActivity.StartupProceduresComment = $('#StartupProcedureComment').val();
    newActivity.StartupProceduresInitial = $('#StartupProcedureInitial').val();
    newActivity.WeeklyChecks = $('#WeeklyCheck').is(':checked');
    newActivity.WeeklyChecksComment = $('#WeeklyCheckComment').val();
    newActivity.WeeklyChecksInitial = $('#WeeklyCheckInitial').val();
    newActivity.MonthlyChecks = $('#MonthlyCheck').is(':checked');
    newActivity.MonthlyChecksComment = $('#MonthlyCheckComment').val();
    newActivity.MonthlyChecksInitial = $('#MonthlyCheckInitial').val();
    newActivity.Maintenance = $('#Maintenance').is(':checked');
    newActivity.MaintenanceComment = $('#MaintenanceComment').val();
    newActivity.MaintenanceInitial = $('#MaintenanceInitial').val();
    newActivity.SafetyCheck = $('#SafetyCheck').is(':checked') ;
    newActivity.SafetyCheckComment = $('#SafetyCheckComment').val();
    newActivity.SafetyCheckInitial = $('#SafetyCheckInitial').val();
    newActivity.RadioCheck = $('#RadioCheck').is(':checked');
    newActivity.RadioCheckComment = $('#RadioCheckComment').val();
    newActivity.RadioCheckInitial = $('#RadioCheckInitial').val();
    //newActivity.EnginePortFuel = $('#EnginePortFuel').val();
    newActivity.EnginePortOilAdded = $('#EnginePortOilAdded').val();
    newActivity.EnginePortRunHours = $('#EnginePortRunHours').val();
    newActivity.EnginePortTotalHours = $('#EnginePortTotalHours').val();
    //newActivity.EngineStarboardFuel = $('#EngineStarboardFuel').val();
    newActivity.EngineStarboardOilAdded = $('#EngineStarboardOilAdded').val();
    newActivity.EngineStarboardRunHours = $('#EngineStarboardRunHours').val();
    newActivity.EngineStarboardTotalHours = $('#EngineStarboardTotalHours').val();
    newActivity.EngineFuelLitres = $('#EngineFuelLitres').val();
    newActivity.EngineFuelDollars = $('#EngineFuelDollars').val();
    newActivity.FirepumpStarted = $('#FirepumpStarted').val();
    newActivity.Crew = getCrewIds(crewSelected);



    top.addActivity(newActivity);
    
}

function ShowDropdowns(table, dropdowncount, cols) {
    //debugger;
    $(table).find("tbody").html("");
    var noOfRows = Math.floor(dropdowncount / cols) + 1;
    var colno = 1;
    for (var i = 0; i < noOfRows; i++) {
        var td = "<tr>";
        for (var j = 0; j < cols; j++) {
            if (colno <= dropdowncount) {
                var temp = "Crew" + (colno);
                td = td + '<td><select id="' + temp + '" onchange="crewOnchange(this)"></select></td>';
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
    //var TripSel = '{Trip.id}="{' + ActivityID + '}"';

    var TripSel = "id='" + ActivityID + "'";
    top.PopRootPage('Report.aspx?SessionId=' + top.SessionId + '&filename=TripReport&SelectionCriteria=' + TripSel);
}

function PrintWord_onclick() {
       //debugger;
    //var TripSel = '{Trip.id}="{' + ActivityID + '}"';

   // var TripSel = "id='" + ActivityID + "'";
    top.PopRootPage('WordReport.aspx?SessionId=' + top.SessionId + '&filename=TripReport&LogNo=' + $('#LogNo').text());
}

function gotActivityProcedures(procedures) {

    //debugger;
    OrigProcedures = procedures;
    $('#ProcedureTable').find("tbody").html("");
    var tr = "";
    for (var i = 0; i < procedures.length; i++) {
        tr += '<tr><td>' + ProcedureTypes[procedures[i].Type] + '</td><td>' + procedures[i].Name + '</td><td><input id="ProcedureComments' + (i + 1) + '"></input></td><td><input id="ProcedureComplete' + (i + 1) + '" type="checkbox" /></td></tr>';
    }
    $('#ProcedureTable > tbody:last').append(tr);
}

function gotActivityPassangers(passangers) {

    //debugger;
    OrigPassangers = passangers;
    $('#PassangerTable').find("tbody").html("");
    var len = 0;
    if (typeof (passangers) != "undefined") {
        len = passangers.length;
    }
    PassangerCount = len;
    var tr = "";
    for (var i = 0; i < len; i++) {
        tr += '<tr><td><input id="PassangerName' + (i+1) + '"  onchange="passangerOnchange(this)" value="'+passangers[i].Comments+'"></input></td></tr>';
    }
    tr += '<tr><td><input id="PassangerName' + (len + 1) + '" onchange="passangerOnchange(this)"></input></td></tr>';
    $('#PassangerTable > tbody:last').append(tr);

    var oPob = top.toInt($('#OtherPeople').val());
    oPob = oPob - len;
    if (oPob < 0) oPob = 0;
    $('#OtherPeople').val(oPob);
    updatePOBCounts();
}



function gotActivityDrills(drills) {

    //debugger;
    //OrigProcedures = procedures;
    $('#DrillTable').find("tbody").html("");
    var tr = "";
    var len = 0;
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
    for (var j = 0; j <= len; j++) {
        populateDropdown($('#DrillSelect' + (j + 1)), top.Drills, 'name');
    }
    for (var k = 0; k < len; k++) {

        setDropdown($("#DrillSelect" + (k + 1)), drills[k].DrillId);
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
    for (var j = 0; j <= len; j++) {
        populateDropdown($('#ScenarioSelect' + (j + 1)), top.Scenarios, 'name');
    }
    for (var k = 0; k < len; k++) {

        setDropdown($("#ScenarioSelect" + (k + 1)), scenarios[k].ScenarioId);
    }
}

function gotActivityDestinations(destinations) {
    //debugger;
    $('#DestinationTable').find("tbody").html("");
    var tr = "";
    var len = 0;
    if (typeof (destinations) != "undefined") {
        len = destinations.length;
    }
    DestinationCount = len;
    tr = '<tr><td>Destinations</td></tr>';
    for (var i = 0; i < len; i++) {
        //tr += '<tr><td><select id="DrillSelect' + (i + 1) + '" name="DrillSelect' + (i + 1) + '" onchange="drillOnchange(this)"><option value="' + drills[i].DrillId + '">' +drills[i].DrillId+ '</option></select></td></tr>';
        tr += '<tr><td><select id="DestinationSelect' + (i + 1) + '" name="DestinationSelect' + (i + 1) + '" onchange="destinationOnchange(this)"></select></td></tr>';
    }
    tr += '<tr><td><select id="DestinationSelect' + (len + 1) + '" name="DestinationSelect' + (len + 1) + '" onchange="destinationOnchange(this)"></select></td></tr>';


    $('#DestinationTable > tbody:last').append(tr);
    for (var j = 0; j <= len; j++) {
        populateDropdown($('#DestinationSelect' + (j + 1)), top.Destinations, 'name');
    }
    for (var k = 0; k < len; k++) {

        setDropdown($("#DestinationSelect" + (k + 1)), destinations[k].DestinationId);
    }
    rePopulateDestinations();
}

function isProcedureUpdated(id) {
    var retval = false;
    try {
        if ($('#ProcedureComments' + (id + 1)).val() != OrigProcedures[id].Comments) {
            return true;
            //debugger;
        }
        if ($('#ProcedureComplete' + (id + 1)).is(':checked')) {
            return true;
        }
    } catch (err) { }
    return retval;
}

function isDestinationUpdated(id) {
    var retval = false;
    try {
        if ($('#DestinationSelect' + (id + 1)).val() != OrigDestinations[id].DestinationId) {
            return true;
            //debugger;
        }
        
    } catch (err) { }
    return retval;
}
function isScenarioUpdated(id) {
    var retval = false;
    try {
        if ($('#ScenarioSelect' + (id + 1)).val() != OrigScenarios[id].ScenarioId) {
            return true;
            //debugger;
        }

    } catch (err) { }
    return retval;
}
function isPassangerUpdated(id) {
    var retval = false;
    try {
        if ($('#PassangerName' + (id + 1)+'').val() != OrigPassanger[id].Comments) {
            return true;
            //debugger;
        }
    } catch (err) { }
    return retval;
}

function Engines_onclick() {
    //debugger;
    //top.BEGetEngineStartHours("6ddddca1-fe99-4fe7-8e78-b7922abf2c4a", -1);
    $("#EngineDiv").dialog({
        width: 'auto',

        buttons:
    {
        "Ok": function () { $(this).dialog("close"); }
        
    }
    });

}

function InvalidReturnTime() {
    //debugger;

    $("#MessageDiv").dialog({
        width: 'auto',

        buttons:
    {
        "Save": function () { },
        "Add Day": function () {  },
        "Cancel": function () { $(this).dialog("close"); }
    }
    });

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
function Destinations_onclick() {
    //debugger;
    if (ActivityID == null || ActivityID == "") {
     
    Save_onclick();
    }
    $("#DestinationDiv").dialog({
        width: 'auto',

        buttons:
    {
        "Ok": function() { SaveDestinations() },
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
function Passangers_onclick() {
    //debugger;

    $("#PassangerDiv").dialog({
        width: 'auto',

        buttons:
    {
        "Ok": function() { SavePassangers() },
        "Cancel": function() { $(this).dialog("close"); }
    }
    });

}
function showDialog(dialogName) {
    $("#DrillDiv").dialog({
        width: 800,
        height: 600,
        buttons:
    {
        "Ok": function() { SaveDrills() },
        "Cancel": function() { $(this).dialog("close"); }
    }
    });

}


function SaveProcedures() {

    //debugger;
    for (var i = 0; i < OrigProcedures.length; i++) {
        if (isProcedureUpdated(i)) {
            var curProcedure = OrigProcedures[i];
            curProcedure.Comments = $('#ProcedureComments' + (i + 1)).val()
            if ($('#ProcedureComplete' + (i + 1)).is(':checked')) {
                curProcedure.DateCompleted = new Date();
                curProcedure.CompletedBy = top.Session.Memberid;

            }
            curProcedure.ActivityId = ActivityID;
            top.modifyProcedureInfo(curProcedure);
        }
    }
    $("#ProcedureDiv").dialog("close");
}

function SavePassangers() {

    //debugger;
    var OrigPassangersCount = 0;
    var curPassanger;
    try {
        if (typeof (OrigPassangers) != "undefined" && OrigPassangers.length > 0) { OrigPassangersCount = OrigPassangers.length; }
    } catch (err) {
    }
    for (var i = 0; i < OrigPassangersCount; i++) {
        try {
            if (isPassangerUpdated(i)) {
                curPassanger.Activity = ActivityID;
                curPassanger = OrigPassanger[i];
                curPassanger.Comments = $('#PassangerName' + (i + 1)).val();
                //curProcedure.Comments = $('#ProcedureComments' + (i + 1)).val()
                //if ($('#ProcedureComplete' + (i + 1)).is(':checked')) {
                //    curProcedure.DateCompleted = new Date();
                //    curProcedure.CompletedBy = top.Session.Memberid;
                //}
                top.modifyPassangerInfo(curPassanger);
            }
        } catch (ex) {
        Log.Error("Error " + ex + " in Save Passangers");
        }
    }
    for (var j = OrigPassangersCount; j < (PassangerCount); j++) {
        curPassanger = newActivityPassanger;
        curPassanger.Comments = $('#PassangerName' + (j + 1)).val();
        curPassanger.Activity = ActivityID;
        top.modifyPassangerInfo(curPassanger);
    }
    $("#PassangerDiv").dialog("close");
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

    for (var j = OrigDrillsCount; j < (DrillCount); j++) {
        curDrill = newActivityDrill;
        curDrill.DrillId = $('#DrillSelect' + (j + 1)).val();
        curDrill.Activity = ActivityID;
        top.BEModifyActivityDrill(curDrill);
    }
    //debugger;
    $("#DrillDiv").dialog("close");
}



function SaveScenarios() {

    //debugger;
    var OrigScenariosCount = 0;
    var curScenario;
    try {
        if (typeof (OrigScenarios) != "undefined" && OrigScenarios.length > 0) { OrigScenariosCount = OrigScenarios.length; }
    } catch (err) {
    }
    for (var i = 0; i < (OrigScenariosCount); i++) {
        if (isScenarioUpdated(i)) {
            //debugger;
            curScenario = OrigScenarios[i];
            curScenario.ScenarioId = $('#ScenarioSelect' + (i + 1)).val();
            curScenario.Activity = ActivityID;
            top.BEModifyActivityScenario(curScenario);
            //top.modifyProcedureInfo(curProcedure);
        }
    }

    for (var j = OrigScenariosCount; j < (ScenarioCount); j++) {
        curScenario = newActivityScenario;
        curScenario.ScenarioId = $('#ScenarioSelect' + (j + 1)).val();
        curScenario.Activity = ActivityID;
        top.BEModifyActivityScenario(curScenario);
    }
    //debugger;
    $("#ScenarioDiv").dialog("close");
}

function SaveDestinations() {

   // debugger;
    var OrigDestinationsCount = 0;
    var curDestination;
    try {
        if (typeof (OrigDestinations) != "undefined" && OrigDestinations.length > 0) { OrigDestinationsCount = OrigDestinations.length; }
    } catch (err) {
    }
    for (var i = 0; i < (OrigDestinationsCount); i++) {
        if (isDestinationUpdated(i)) {
            //debugger;
            curDestination = OrigDestinations[i];
            curDestination.DestinationId = $('#DestinationSelect' + (i + 1)).val();
            curDestination.Activity = ActivityID;
            top.BEModifyActivityDestination(curDestination);
            //top.modifyProcedureInfo(curProcedure);
        }
    }

    for (var j = OrigDestinationsCount; j < (DestinationCount); j++) {
        curDestination = newActivityDestination;
        curDestination.DestinationId = $('#DestinationSelect' + (j + 1)).val();
        curDestination.Activity = ActivityID;
        top.BEModifyActivityDestination(curDestination);
    }
    //debugger;
    $("#DestinationDiv").dialog("close");
}

function ActivityTypeOnChange(me,newTypeId) {
    //debugger;
    if (typeof (me) != "undefined" && me != null && typeof (me.value) != "undefined" && me.value != null) {
        newTypeId = me.value;
    }
    var types = top.ActivityTypes;
    for (var i = 0; i < types.length; i++) {
        try {
            if (types[i].id == newTypeId) {
                if (types[i].IsWaterBased) {
                    // $("#TideTable").show();
                    $(".WetOnly").show();
                    $(".DryOnly").hide();
                } else {
                    //$("#TideTable").hide();
                    $(".WetOnly").hide();
                    $(".DryOnly").show();
                }
            }
        } catch (e) {
        debugger;
        }
}
setFrameSize();
}

function GotStartHours(Hours) {
    $('#EnginePortStartHours').val(Hours.PortEngineStart);
    $('#EngineStarboardStartHours').val(Hours.StarboardEngineStart);
}

//function EPTotHours_OnChange(myVal) {
//    var StarboardTotHours = $('#EngineStarboardTotalHours').val();
//    var runHours= (top.toDecimal(myVal) - top.toDecimal($('#EnginePortStartHours').val()));
//    if (typeof (StarboardTotHours) == "undefined" || StarboardTotHours == null || StarboardTotHours == "" || StarboardTotHours == 0) {
//        $('#EngineStarboardTotalHours').val(runHours + top.toDecimal($('#EngineStarboardStartHours').val()));
//    }

//    $('#EnginePortRunHours').val(runHours);
//    ESTotHours_OnChange(runHours + top.toDecimal($('#EngineStarboardStartHours').val()));
//}

function EPTotHours_OnChange(myVal) {
    $('#EnginePortRunHours').val(top.toDecimal(myVal) - top.toDecimal($('#EnginePortStartHours').val()));

}

function ESTotHours_OnChange(myVal) {
    $('#EngineStarboardRunHours').val(top.toDecimal(myVal) - top.toDecimal($('#EngineStarboardStartHours').val()));
}

function EPStartHours_OnChange(myVal) {
    var StarboardTotHours = $('#EngineStarboardTotalHours').val();
    var StartHours = top.toDecimal(myVal);
    var runHours = ( $('#EnginePortTotalHours').val() - StartHours);
    if (typeof (StarboardTotHours) == "undefined" || StarboardTotHours == null || StarboardTotHours == "" || StarboardTotHours == 0) {
        $('#EngineStarboardTotalHours').val(runHours + top.toDecimal($('#EngineStarboardStartHours').val()));
    }

    $('#EnginePortRunHours').val(runHours);
    ESTotHours_OnChange(runHours + top.toDecimal($('#EngineStarboardStartHours').val()));
}

function ESStartHours_OnChange(myVal) {
    $('#EngineStarboardRunHours').val(top.toDecimal(myVal) - top.toDecimal($('#EngineStarboardStartHours').val()));

}