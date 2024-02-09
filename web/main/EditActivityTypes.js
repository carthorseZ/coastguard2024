var TableName = "ActivityType";
var Data;
var OrigData;
$(document).ready(function() {
//debugger;
	top.CBEvents.AddCallBack('EditTable:'+TableName, 'foundActivityTypes', foundActivityTypes);
	FindActivityTypes();
	top.setWidth(-1);
});

function FindActivityTypes() {
	top.editTable(TableName);
}


function foundActivityTypes(ActivityTypes)
{
//$('#Trips').dataTable();
//debugger;
Data = ActivityTypes.data;
OrigData = ArrayCopy(Data);
//var test = []
var cols = getColumns(ActivityTypes.titles);
var editcols = ["Text", "False"];
   // cols = [ {"sTitle":"Col1"},{"sTitle":"Col2"} ]
   //cols = [ {"sTitle":"Col1"}]
	//Trips = [[ "Trident" ]]
	//Trips = [[ {"Name":"c1"}]]; 
   
	var mainTable= $('#ActivityTypes').dataTable({
	    "bDestroy": true,
	    "fnDrawCallback": function () {
	        //debugger;
	        setFrameSize();
	        top.setWidth($('#ActivityTypes')[0].scrollWidth);
	    },
	"aaData":Data,
	"aoColumns":cols,
	"aoColumnDefs": [ 
						{ "bSearchable": false, "bVisible": false, "aTargets": [ 0 ] }]
	//	"bProcessing": true,
	//	"sAjaxSource": topen.serviceURL + 'GetTrips2'
	} );
	
	$('#ActivityTypes tbody tr').live('click', function () {
	//debugger;
	var aPos = mainTable.fnGetPosition( this ); 
	 
	var aData = mainTable.fnGetData( this );

	//var aData = Trips;
	//var iId = aData[(this.rowIndex -1)][0];
	var iId = aData[0];
	//ActivityType("poping page ModifyTrip.htm?TripId="+iId);
	//top.PopPage("ModifyActivityType.htm?ActivityTypeId="+iId);
});
$('td', mainTable.fnGetNodes()).editable(function(value, settings) {
//debugger;
var aPos = mainTable.fnGetPosition(this);

//    var nTr = this.parentNode.parentNode;
Data[aPos[0]][aPos[1] + 1] = value;
	return (value);
}, {
	"onblur": "submit"
});
//debugger;
}
function newActivityType() {
	//debugger;
	var ActivityType;
	ActivityType = top.BlankActivityType;
	//if (typeof (ActivityTypeID) != 'undefined' && DrillID != null) Drill.id = DrillID;
	ActivityType.Name = "";

	ActivityType.Deleted = false;
	top.modifyActivityType(ActivityType);
	//top.getDrills(true);
	top.PopPage('EditActivityTypes.htm');
}
function saveActivityType() {
	debugger;
	//Data[1][1]= "Testing";
	setTimeout(function() {
		for (var i = 0; i < OrigData.length; i++) {
			if (!ArrayCompare(OrigData[i], Data[i])) {
				top.modifyActivityType(Data[i]);
			}
		} 
	}, 500);
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