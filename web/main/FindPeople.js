
$(document).ready(function () {
	top.setWidth(-1);
	top.CBEvents.AddCallBack('GetPeople','gotPeople',gotPeople);
	getPeople();
});

function getPeople(){
	top.BEGetPeople();
//top.getTrips($('#StartDate').datepicker( "getDate" ),$('#EndDate').datepicker( "getDate" ),"",5)
}


function newUser(){
top.PopPage("ModifyPerson.htm");
}

function gotPeople()
{
//$('#Trips').dataTable();
//debugger;
var People = top.curUsers.data;
//var test = [] 
var cols = getColumns(top.curUsers.titles);
   // cols = [ {"sTitle":"Col1"},{"sTitle":"Col2"} ]
   //cols = [ {"sTitle":"Col1"}]
	//Trips = [[ "Trident" ]]
	//Trips = [[ {"Name":"c1"}]];

var mainTable = $('#Users').dataTable({
	"aaSorting": [[3, 'desc']],
	"fnDrawCallback": function () {
		//debugger;
		setFrameSize();
		top.setWidth($('#Users')[0].scrollWidth);
	},
	"bDestroy": true,
	"aaData": Users,
	"aoColumns": cols,
	"aoColumnDefs": [
						{ "bSearchable": false, "bVisible": false, "aTargets": [0]}]

	//	"bProcessing": true,
	//	"sAjaxSource": topen.serviceURL + 'GetTrips2'
});
	
	$('#Users tbody tr').live('click', function () {
	//debugger;
	var aPos = mainTable.fnGetPosition( this ); 
	 
	var aData = mainTable.fnGetData( this );

	//var aData = Trips;
	//var iId = aData[(this.rowIndex -1)][0];
	var iId = aData[0];
	//alert("poping page ModifyTrip.htm?TripId="+iId);
	top.PopPage("ModifyUsers.htm?MemberId="+iId);
});
//debugger;
top.setWidth($('#Users')[0].scrollWidth);

	

//debugger;
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