var CourseID;
var ModeAdd = false;
var MyPath;
var myFileName;
$(document).ready(function () {
    //debugger;
    // var Reports = top.getReports();
    top.CBEvents.AddCallBack('Restore', 'Restore', restoreComplete);
    top.CBEvents.AddCallBack('GetBackups', 'getBackups', setupForm);
    top.BEGetBackups();
    //setupForm();
});

function restoreComplete(result) {
    if (result) {
        alert("Restore was successful");
    } else {
        alert("Restore was unsuccessful");
    }
    //debugger;

}

function setupForm(backupList) {
    //debugger;
    populateDropdown($('#BackupName'), backupList);
}

function Cancel_onclick() {
    top.PopPage('Main');
}
function Restore_onclick() {
    //debugger;
    var myFname = $('#BackupName').val();
    if (typeof(myFname) != "undefined" && myFname != null && myFname != "") {
        top.BERestoreDataBase(myFname);
    }
}

function Upload_onclick() {
    //debugger;
    var myFname = $('#BackupName').val();
    if (typeof (myFname) != "undefined" && myFname != null && myFname != "") {
        uploadIframe.submitform();
    }
}

function Refresh_onclick() {
    top.BEGetBackups();
}