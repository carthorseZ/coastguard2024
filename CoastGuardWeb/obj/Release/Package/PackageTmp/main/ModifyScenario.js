var ScenarioID;
var ModeAdd = false;
$(document).ready(function() {
//debugger;
ScenarioID = sessionID = $.getURLParam('ScenarioId');
if (typeof(ScenarioID) == 'undefined'||ScenarioID==null) ModeAdd = true;
//top.CBEvents.AddCallBack("GetScenarios", "Modify Scenarios setup Form", setupForm);

setupForm();
});

function setupForm() {
var Scenarios=top.getScenarios();
//debugger;
if (ModeAdd) return;
var ScenarioInfo = ArrayFind(Scenarios, ScenarioID);
$('input#Name').val(ScenarioInfo.Name);
$('input#Code').val(ScenarioInfo.Code);
$('input#Deleted').attr('checked', ScenarioInfo.Deleted);

}

function Cancel_onclick() {
    top.PopPage('FindScenarios.htm');
}
function Save_onclick() {
//debugger;
var Scenario;
Scenario=top.BlankScenario;
if (typeof(ScenarioID) != 'undefined' && ScenarioID != null) Scenario.id = ScenarioID;
Scenario.Name = $('input#Name').val();
Scenario.Code = $('input#Code').val();
Scenario.Deleted = $('input#Deleted').is(':checked');
top.modifyScenario(Scenario);
top.getScenarios(true);
top.PopPage('FindScenarios.htm');
}
