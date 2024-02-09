var TableName = "Engine";
var Data;
var OrigData;
$(document).ready(function () {
	//debugger;
	top.setWidth(-1);
	top.CBEvents.AddCallBack('EditTable:' + TableName, 'foundTable:' + TableName, foundTable);
	top.CBEvents.AddCallBack('NewObject:' + TableName, 'NewObject:' + TableName, newObjectCallback);
	Find();

	$('input[type=text], input[type=password], textarea')
.keyboard({
	layout: "qwerty",
	customLayout:
[["q w e r t y {bksp}", "Q W E R T Y {bksp}"],
["s a m p l e {shift}", "S A M P L E {shift}"],
["{accept} {space} {cancel}", "{accept} {space} {cancel}"]]
});

});

function Find() {
	top.editTable(TableName);
}


function foundTable(table) {
	//$('#Trips').dataTable();
	//debugger;
	Data = table.data;
	OrigData = ArrayCopy(Data);
	//var test = []
	var cols = getColumns(table.titles);
	var editcols = ["Text", "False"];
	// cols = [ {"sTitle":"Col1"},{"sTitle":"Col2"} ]
	//cols = [ {"sTitle":"Col1"}]
	//Trips = [[ "Trident" ]]
	//Trips = [[ {"Name":"c1"}]]; 

	var mainTable = $('#editTable').dataTable({
		"bDestroy": true,
		"fnDrawCallback": function () {
			//debugger;
			setFrameSize();
			top.setWidth($('#editTable')[0].scrollWidth);
		},
		"aaData": Data,
		"aoColumns": cols,
		"aoColumnDefs": [
						{ "bSearchable": false, "bVisible": false, "aTargets": [0]}]
		//	"bProcessing": true,
		//	"sAjaxSource": topen.serviceURL + 'GetTrips2'
	});

	$('#editTable tbody tr').live('click', function () {
		//debugger;
		var aPos = mainTable.fnGetPosition(this);

		var aData = mainTable.fnGetData(this);

		//var aData = Trips;
		//var iId = aData[(this.rowIndex -1)][0];
		var iId = aData[0];
		//ActivityType("poping page ModifyTrip.htm?TripId="+iId);
		//top.PopPage("ModifyActivityType.htm?ActivityTypeId="+iId);
	});
	$('td', mainTable.fnGetNodes()).editable(function (value, settings) {
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

function newObjectCallback(tempObject) {
	//debugger;
	//var tempObject;

	//    tempObject = "";
	//if (typeof (ActivityTypeID) != 'undefined' && DrillID != null) Drill.id = DrillID;
	//   tempObject.Name = "";

	//tempObject.Deleted = false;
	top.modifyObject(TableName, tempObject);
	setTimeout('Find()', 750);
}

function newObject() {
	//debugger;
	top.BENewObjectAsStringList(TableName);
	//top.getDrills(true);
	//top.PopPage('EditRoleTypes.htm');
}
function saveObject() {
	//debugger;
	//Data[1][1]= "Testing";
	setTimeout(function () {
		for (var i = 0; i < OrigData.length; i++) {
			if (!ArrayCompare(OrigData[i], Data[i])) {
				//debugger;
				top.modifyObject(TableName, Data[i]);
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