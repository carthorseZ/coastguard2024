
$(document).ready(function () {
	$('#StartDate').datepicker({ dateFormat: 'dd/mm/yy' });
	$('#StartDate').datepicker('setDate', new Date());
	$('#EndDate').datepicker({ dateFormat: 'dd/mm/yy' });
	$('#EndDate').datepicker('setDate', new Date());
	top.CBEvents.AddCallBack('GetActivities', 'gotActivities', gotActivities);
	top.CBEvents.AddCallBack("GetVesselList", "gotVessels", gotVessels);
	top.CBEvents.AddCallBack("GetActivityTypes", "populateActivityType", populateActivityType);
	top.$('html, body').animate({ scrollTop: 0 }, 'fast');
	if (typeof (top.curActivities.data) != "undefined" && top.curActivities.data != null) {
		gotActivities();


	}
	top.getActivityTypes();
	populateDropdownName($('#ActivityType'), top.ActivityTypes);
	top.BEGetVesselList();
	getRecentActivities();

	top.setWidth(-1);

});

function populateActivityType(Types) {
    populateDropdownName($('#ActivityType'), Types);


}

function getActivities(){
top.getActivities($('#StartDate').datepicker( "getDate" ),$('#EndDate').datepicker( "getDate" ),"",5)
}
function getRecentActivities() {
	top.BEGetRecentActivities(10);
}

function gotVessels(result) {
        $('#Vessel').html('');
        $('#VesselLabel').show();
        for (var i = 0; i < result.length; i++) {
            $('#Vessel').append($('<option value=' + result[i] + '>' + result[i].name + '</option>'));
        }
        $('#Vessel').show();
    }

function newActivity() {
	top.PopPage("ModifyActivity.htm");
}


function gotActivities()
{
//$('#Activities').dataTable();
//debugger;
var Activities = top.curActivities.data;
//var test = [] 
var cols = getColumns(top.curActivities.titles);
   // cols = [ {"sTitle":"Col1"},{"sTitle":"Col2"} ]
   //cols = [ {"sTitle":"Col1"}]
	//Activities = [[ "Trident" ]]
	//Activities = [[ {"Name":"c1"}]]; 
   
	var mainTable= $('#Activities').dataTable({
	"bDestroy":true,
	"aaData":Activities,
	"aoColumns": cols,
    "aaSorting": [[1,"desc"]],
	"fnDrawCallback": function () {
		//debugger;
		setFrameSize();
		top.setWidth($('#Activities')[0].scrollWidth);
	},
	"aoColumnDefs": [
						{ "bSearchable": false, "bVisible": false, "aTargets": [0] },
						{ "bSearchable": true, "bVisible": true, "aTargets": [4] },
						{ "bSearchable": true, "bVisible": true, "aTargets": [5] },
						]
	//	"bProcessing": true,
	//	"sAjaxSource": topen.serviceURL + 'GetActivities2'
	} );

	$('#Activities tbody tr').live('click', function () {
		var aPos = mainTable.fnGetPosition(this);

		var aData = mainTable.fnGetData(this);

		//var aData = Activities;
		//var iId = aData[(this.rowIndex -1)][0];
		var iId = aData[0];
		//alert("poping page ModifyActivity.htm?ActivityId="+iId);
		top.PopPage("ModifyActivity.htm?ActivityId=" + iId);
	});

//debugger;
}

function TableUpdate() {
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
