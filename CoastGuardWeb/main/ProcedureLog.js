
$(document).ready(function() {
	top.CBEvents.AddCallBack('FindProcedureLogs', 'foundProcedureLogs', foundProcedureLogs);
	FindProcedureLogs();
});

function FindProcedureLogs(){
top.findProcedureLogs()
}


function foundProcedureLogs()
{
//$('#Trips').dataTable();
//debugger;
var ProcedureLogs = top.curProcedureLogs.data;
//var test = [] 
var cols = getColumns(top.curProcedureLogs.titles);
   // cols = [ {"sTitle":"Col1"},{"sTitle":"Col2"} ]
   //cols = [ {"sTitle":"Col1"}]
	//Trips = [[ "Trident" ]]
	//Trips = [[ {"Name":"c1"}]]; 
   
	var mainTable= $('#ProcedureLogs').dataTable({
	"bDestroy": true,
	"aaData":ProcedureLogs,
	"aoColumns":cols,
	"aoColumnDefs": [ 
						{ "bSearchable": false, "bVisible": false, "aTargets": [ 0 ] }]
	//	"bProcessing": true,
	//	"sAjaxSource": topen.serviceURL + 'GetTrips2'
	} );
	
	$('#ProcedureLogs tbody tr').live('click', function () {
	//debugger;
	var aPos = mainTable.fnGetPosition( this ); 
	 
	var aData = mainTable.fnGetData( this );

	//var aData = Trips;
	//var iId = aData[(this.rowIndex -1)][0];
	var iId = aData[0];
	//alert("poping page ModifyTrip.htm?TripId="+iId);
	top.PopPage("ModifyProcedureLog.htm?ProcedureLogId="+iId);
	});

//debugger;
}
function newProcedureLog() {
	top.PopPage("ModifyProcedureLog.htm");
}