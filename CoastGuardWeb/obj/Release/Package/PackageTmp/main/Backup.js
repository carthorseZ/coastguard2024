var CourseID;
var ModeAdd = false;
$(document).ready(function () {
    //debugger;
    // var Reports = top.getReports();
    top.CBEvents.AddCallBack('Backup', 'Backup', setupForm);
    top.CBEvents.AddCallBack('EditTable:BackupFiles', 'foundTable:BackupFile', foundBackups);
    top.editTable('BackupFiles');
    //setupForm();
});

function setupForm(result) {
    if (result) {
        alert("Backup was successful");
    } else {
        alert("Backup was unsuccessful");
    }
    //debugger;

}


function Cancel_onclick() {
    top.PopPage('Main');
}
function Backup_onclick() {
    //debugger;
    var myFname = $('#FileName').val();
    if (typeof(myFname) != "undefined" && myFname != null && myFname != "") {
        top.BEBackupDataBase(myFname);
    }
}

function foundBackups(Backups) {
    //$('#Trips').dataTable();
    //debugger;
    //var Procedures = top.curProcedures.data;
    //var test = [] 
    var cols = getColumns(Backups.titles);
    // cols = [ {"sTitle":"Col1"},{"sTitle":"Col2"} ]
    //cols = [ {"sTitle":"Col1"}]
    //Trips = [[ "Trident" ]]
    //Trips = [[ {"Name":"c1"}]];

    var mainTable = $('#Backups').dataTable({
        "bDestroy": true,
        "aaData": Backups.data,
        "aoColumns": cols,
        "aoColumnDefs": [
						{ "bSearchable": false, "bVisible": false, "aTargets": [0]}]
        //	"bProcessing": true,
        //	"sAjaxSource": topen.serviceURL + 'GetTrips2'
    });


    $('#Backups tbody tr').live('click', function () {
        // debugger;
        var aPos = mainTable.fnGetPosition(this);

        var aData = mainTable.fnGetData(this);

        //var aData = Trips;
        //var iId = aData[(this.rowIndex -1)][0];
        //var iId = aData[0];
        //alert("poping page ModifyTrip.htm?TripId="+iId);
        top.ShowFile("Backups",aData[1]);

        //top.PopAltPage("ModifyAttachmnet.htm?AttachmentId=" + iId + '&ParentId="' + ProcedureID + '"&ParentEntity="Procedure"');
    });

    //debugger;
}
