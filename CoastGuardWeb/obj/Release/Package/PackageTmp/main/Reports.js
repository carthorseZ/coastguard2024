var CourseID;
var ModeAdd = false;
$(document).ready(function() {
    //debugger;
   // var Reports = top.getReports();
    top.CBEvents.AddCallBack('GetReports', 'getReports', setupForm);
    top.getReports();
    //setupForm();
});

function setupForm() {
//debugger;
    populateDropdown($('#ReportName'), top.Reports);
}

function Cancel_onclick() {
    top.PopPage('main');
}
function Print_onclick() {
    //debugger;
    var Rptname = $('#ReportName').val();
    //if (typeof(Rptname) == "undefined") return;
    var SelCriteria = "";
    if (Rptname == "Fuel") SelCriteria = "vFuelReport";
    if (Rptname == "Drills") SelCriteria = "vDrills";
    if (Rptname == "UserBasic") SelCriteria = "MemberHours";
    if (SelCriteria == "") {
        //top.PopRootPage('Report.aspx?SessionId=' + top.SessionId + '&filename=' + $('#ReportName').val());
        window.open(top.getBaseURL() + 'Report.aspx?SessionId=' + top.SessionId + '&filename=' + $('#ReportName').val());
    } else {
        //top.PopRootPage('Report.aspx?SessionId=' + top.SessionId + '&filename=' + $('#ReportName').val()+'&Views='+SelCriteria);
        window.open(top.getBaseURL() + 'Report.aspx?SessionId=' + top.SessionId + '&filename=' + $('#ReportName').val() + '&Views=' + SelCriteria);
    }
}
