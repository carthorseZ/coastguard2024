
$(document).ready(function() {
	top.setWidth(-1);
	top.CBEvents.AddCallBack('FindAlerts', 'foundAlerts', foundAlerts);
	FindAlerts();

});

function FindAlerts(){
	top.findAlerts();
}


function foundAlerts()
{
//$('#Trips').dataTable();
//debugger;
var Alerts = top.curAlertDetails.data;
//var test = [] 
var cols = getColumns(top.curAlertDetails.titles);
   // cols = [ {"sTitle":"Col1"},{"sTitle":"Col2"} ]
   //cols = [ {"sTitle":"Col1"}]
	//Trips = [[ "Trident" ]]
	//Trips = [[ {"Name":"c1"}]]; 
   
	var mainTable= $('#Alerts').dataTable({
		"bDestroy": true,
		"fnDrawCallback": function () {
			//debugger;
			setFrameSize();
			top.setWidth($('#Alerts')[0].scrollWidth);
		},
	"aaData":Alerts,
	"aoColumns":cols,
	"aoColumnDefs": [ 
						{ "bSearchable": false, "bVisible": false, "aTargets": [ 0 ] }]
	//	"bProcessing": true,
	//	"sAjaxSource": topen.serviceURL + 'GetTrips2'
	} );
	
	$('#Alerts tbody tr').live('click', function () {
	//debugger;
	var aPos = mainTable.fnGetPosition( this ); 
	 
	var aData = mainTable.fnGetData( this );

	//var aData = Trips;
	//var iId = aData[(this.rowIndex -1)][0];
	var iId = aData[0];
	//alert("poping page ModifyTrip.htm?TripId="+iId);
	top.PopPage("ModifyAlert.htm?AlertId="+iId);
	});

//debugger;
}
function newAlert() {
	top.PopPage("ModifyAlert.htm");
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