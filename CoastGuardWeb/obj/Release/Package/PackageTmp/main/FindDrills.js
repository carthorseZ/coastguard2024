
$(document).ready(function() {
	top.CBEvents.AddCallBack('FindDrills', 'foundDrills', foundDrills);
	FindDrills();
	top.setWidth(-1);
});

function FindDrills(){
top.findDrills()
}


function foundDrills()
{
//$('#Trips').dataTable();
//debugger;
var Drills = top.curDrills.data;
//var test = [] 
var cols = getColumns(top.curDrills.titles);
   // cols = [ {"sTitle":"Col1"},{"sTitle":"Col2"} ]
   //cols = [ {"sTitle":"Col1"}]
	//Trips = [[ "Trident" ]]
	//Trips = [[ {"Name":"c1"}]]; 
   
	var mainTable= $('#Drills').dataTable({
	    "bDestroy": true,
	    "fnDrawCallback": function () {
	        //debugger;
	        setFrameSize();
	        top.setWidth($('#Drills')[0].scrollWidth);
	    },
	"aaData":Drills,
	"aoColumns":cols,
	"aoColumnDefs": [ 
						{ "bSearchable": false, "bVisible": false, "aTargets": [ 0 ] }]
	//	"bProcessing": true,
	//	"sAjaxSource": topen.serviceURL + 'GetTrips2'
	} );
	
	$('#Drills tbody tr').live('click', function () {
	//debugger;
	var aPos = mainTable.fnGetPosition( this ); 
	 
	var aData = mainTable.fnGetData( this );

	//var aData = Trips;
	//var iId = aData[(this.rowIndex -1)][0];
	var iId = aData[0];
	//alert("poping page ModifyTrip.htm?TripId="+iId);
	top.PopPage("ModifyDrill.htm?DrillId="+iId);
	});

//debugger;
}
function newDrill() {
	top.PopPage("ModifyDrill.htm");
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