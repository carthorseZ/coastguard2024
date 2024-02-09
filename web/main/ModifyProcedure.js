var ProcedureID;
var Types = ["Weekly", "Monthly", "StartUp", "Maintenance", "Safety","Radio"];
var ModeAdd = false;
$(document).ready(function() {
//debugger;
ProcedureID = sessionID = $.getURLParam('ProcedureId');
if (typeof(ProcedureID) == 'undefined'||ProcedureID==null) ModeAdd = true;
top.CBEvents.AddCallBack("GetProcedures", "Modify Procedures setup Form", setupForm);

setupForm();
});

function setupForm() {
	//debugger;
	var Procedures = top.getProcedures();
	populateDropdown($('#Type'), Types);
//debugger;
	if (ModeAdd) return;
	if (typeof (Procedures) == "undefined" || Procedures == null) return;
var ProcedureInfo = ArrayFind(Procedures, ProcedureID);
setDropdown($('#Type'), Types[ProcedureInfo.Type]);
$('#Name').val(ProcedureInfo.Name);
$('#Description').val(ProcedureInfo.Description);
$('#Deleted').attr('checked', ProcedureInfo.Deleted);
// Attcahments
top.CBEvents.AddCallBack('EditTable:Attachment', 'foundTable:Attachment', foundAttachments);

//top.CBEvents.AddCallBack('FindAttachments', 'foundAttachments', foundAttachments);
//Call found attachments to display even if none are found
//foundAttachments();
top.getAttachments('ParentType == Procedure and ParentId == '+ProcedureID);
}

function Cancel_onclick() {
	//debugger;
	top.PopPage('FindProcedures.htm');
}

function Attach_onclick() {
	//debugger;
	top.PopAltPage('ModifyAttachment.htm?ParentId=' + ProcedureID + '&ParentEntity=Procedure');
}



function ShowLog_onclick() {
	//debugger;
	top.PopPage('ProcedureLog.htm');
}
function Save_onclick() {
//debugger;
var Procedure;
Procedure=top.BlankProcedure;
if (typeof(ProcedureID) != 'undefined' && ProcedureID != null) Procedure.id = ProcedureID;
Procedure.Name = $('#Name').val();
Procedure.Type = findIndex($("#Type").val(), Types);
Procedure.Deleted = $('#Deleted').is(':checked');
Procedure.Description = $('#Description').val();
top.modifyProcedure(Procedure);
top.getProcedures(true);
top.PopPage('FindProcedures.htm');
}
function findIndex(val, Array) {
	//debugger;
	var retval = -1;
	for (var i = 0; i < Array.length; i++) {
		if (Array[i] == val) return i;
	}
	return retval;

}

function foundAttachments(Attachments) {
	//$('#Trips').dataTable();
	//debugger;
	//var Procedures = top.curProcedures.data;
	//var test = [] 
	var cols = getColumns(Attachments.titles);
	// cols = [ {"sTitle":"Col1"},{"sTitle":"Col2"} ]
	//cols = [ {"sTitle":"Col1"}]
	//Trips = [[ "Trident" ]]
	//Trips = [[ {"Name":"c1"}]]; 

	var mainTable = $('#Attachments').dataTable({
	"bDestroy": true,
		"aaData": Attachments.data,
		"aoColumns": cols,
		"aoColumnDefs": [
						{ "bSearchable": false, "bVisible": false, "aTargets": [0]}]
		//	"bProcessing": true,
		//	"sAjaxSource": topen.serviceURL + 'GetTrips2'
	});

	$('#Attachments tbody tr').live('click', function() {
	   // debugger;
		var aPos = mainTable.fnGetPosition(this);

		var aData = mainTable.fnGetData(this);

		//var aData = Trips;
		//var iId = aData[(this.rowIndex -1)][0];
		var iId = aData[0];
		//alert("poping page ModifyTrip.htm?TripId="+iId);
		top.ShowAttachment(aData[2]);

		//top.PopAltPage("ModifyAttachmnet.htm?AttachmentId=" + iId + '&ParentId="' + ProcedureID + '"&ParentEntity="Procedure"');
	});

	//debugger;
}
