
$(document).ready(function () {
	top.setWidth(-1);
	top.CBEvents.AddCallBack('FindScenarios', 'foundScenarios', foundScenarios);
	FindScenarios();
});

function FindScenarios(){
top.findScenarios()
}


function foundScenarios()
{
//$('#Trips').dataTable();
//debugger;
var Scenarios = top.curScenarios.data;
//var test = [] 
var cols = getColumns(top.curScenarios.titles);
   // cols = [ {"sTitle":"Col1"},{"sTitle":"Col2"} ]
   //cols = [ {"sTitle":"Col1"}]
	//Trips = [[ "Trident" ]]
	//Trips = [[ {"Name":"c1"}]]; 
   
	var mainTable= $('#Scenarios').dataTable({
		"bDestroy": true,
		"fnDrawCallback": function () {
			//debugger;
			setFrameSize();
			top.setWidth($('#Scenarios')[0].scrollWidth);
		},
	"aaData":Scenarios,
	"aoColumns":cols,
	"aoColumnDefs": [ 
						{ "bSearchable": false, "bVisible": false, "aTargets": [ 0 ] }]
	//	"bProcessing": true,
	//	"sAjaxSource": topen.serviceURL + 'GetTrips2'
	} );
	
	$('#Scenarios tbody tr').live('click', function () {
	//debugger;
	var aPos = mainTable.fnGetPosition( this ); 
	 
	var aData = mainTable.fnGetData( this );

	//var aData = Trips;
	//var iId = aData[(this.rowIndex -1)][0];
	var iId = aData[0];
	//alert("poping page ModifyTrip.htm?TripId="+iId);
	top.PopPage("ModifyScenario.htm?ScenarioId="+iId);
	});

//debugger;
}
function newScenario() {
	top.PopPage("ModifyScenario.htm");
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