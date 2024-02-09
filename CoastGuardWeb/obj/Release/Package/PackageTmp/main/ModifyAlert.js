var AlertID;
var ModeAdd = false;
$(document).ready(function() {
//debugger;
AlertID = sessionID = $.getURLParam('AlertId');
if (typeof(AlertID) == 'undefined'||AlertID==null) ModeAdd = true;
top.CBEvents.AddCallBack("GetAlerts", "Modify Alerts setup Form", setupForm);
top.getAlertDetails();
//setupForm();
});

function setupForm(Alerts) {

//debugger;
if (ModeAdd) return;
var AlertInfo = ArrayFind(Alerts, AlertID);
$('input#Name').val(AlertInfo.Name);
$('input#SQL').val(AlertInfo.SQL);
$('input#AlertText').val(AlertInfo.Text);

$('input#Deleted').attr('checked', AlertInfo.Deleted);

}

function Cancel_onclick() {
    top.PopPage('FindAlerts.htm');
}
function Save_onclick() {
//debugger;
var Alert;
Alert=top.BlankAlert;
if (typeof(AlertID) != 'undefined' && AlertID != null) Alert.id = AlertID;
Alert.Name = $('input#Name').val();
//Alert.SQL = $('input#SQL').val();
Alert.Text = $('input#AlertText').val();
Alert.Deleted = $('input#Deleted').is(':checked');
Alert.Fields = $('input#Fields').val();
Alert.Where = $('input#Where').is(':checked');
Alert.Tables = $('input#Tables').val();
top.modifyAlert(Alert);
top.getAlerts(true);
top.PopPage('FindAlerts.htm');
}
