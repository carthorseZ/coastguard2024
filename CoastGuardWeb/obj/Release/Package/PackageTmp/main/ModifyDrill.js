var DrillID;
var ModeAdd = false;
$(document).ready(function() {
//debugger;
DrillID = sessionID = $.getURLParam('DrillId');
if (typeof(DrillID) == 'undefined'||DrillID==null) ModeAdd = true;
//top.CBEvents.AddCallBack("GetDrills", "Modify Drills setup Form", setupForm);


setupForm();
});

function setupForm() {
var Drills=top.getDrills();
//debugger;
if (ModeAdd) return;
var DrillInfo = ArrayFind(Drills, DrillID);
$('input#Name').val(DrillInfo.Name);
$('input#Code').val(DrillInfo.Code);
$('input#Deleted').attr('checked', DrillInfo.Deleted);
$('input#Name').keyboard({ layout: "qwerty" });
$('input#Code').keyboard({ layout: "qwerty" });


}

function Cancel_onclick() {
    top.PopPage('FindDrills.htm');
}
function Save_onclick() {
//debugger;
var Drill;
Drill=top.BlankDrill;
if (typeof(DrillID) != 'undefined' && DrillID != null) Drill.id = DrillID;
Drill.Name = $('input#Name').val();
Drill.Code = $('input#Code').val();
Drill.Deleted = $('input#Deleted').is(':checked');
top.modifyDrill(Drill);
top.getDrills(true);
top.PopPage('FindDrills.htm');
}
