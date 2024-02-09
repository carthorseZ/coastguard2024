var CRVID;
var ModeAdd = false;
var Engines = [[0, "Weekly"], [1, "Monthly"], [2, "StartUp"], [3, "Maintenance"], [4, "Safety"], [5, "Radio"]];
var pe;
var se;
var me;

$(document).ready(function() {
//debugger;
CRVID = sessionID = $.getURLParam('CRVId');
if (typeof(CRVID) == 'undefined'||CRVID==null) ModeAdd = true;
top.CBEvents.AddCallBack('GetObjects:CRVs', 'GetObjects:CRVs2', gotCRVs);
top.CBEvents.AddCallBack('GetObjects:Engine', 'GetObjects:Engine2', gotEngines);
setupForm();
});

function populateDropdownSerial(select, data) {
    //if (top.debug) debugger;
    select.html('');
    select.append($('<option></option>'));
    //$.each(data, function(id) {    
    //    select.append($('<option>'+ data[id]+'</option>'));    
    //});
    for (var i = 0; i < data.length; i++) {
        select.append($('<option value=' + data[i].id + '>' + data[i].SerialNumber + '</option>'));
    };
}
function gotEngines(Engines) {
    //debugger;

    populateDropdownSerial($('#PortEngine'), Engines);
    populateDropdownSerial($('#StarboardEngine'), Engines);
    populateDropdownSerial($('#MiddleEngine'), Engines);

    if (!ModeAdd) {
        setDropdown($('#PortEngine'), pe);
        setDropdown($('#StarboardEngine'), se);
        setDropdown($('#MiddleEngine'), me);
    }

}

function setupForm() {
    var CRVs = top.getCRVs(true);
    top.BEGetObjects("Engine", "1=1");
//debugger;
if (ModeAdd) return;
//var CRVInfo = ArrayFind(CRVs, CRVID);
//$('input#Name').val(CRVInfo.Name);

}

function gotCRVs(CRVs) {
    //debugger;
    var CRVInfo = ArrayFind(CRVs, CRVID);
    //debugger;
    $('input#Name').val(CRVInfo.name);
    pe = CRVInfo.PortEngine;
    se = CRVInfo.StarboardEngine;
    me = CRVInfo.MiddleEngine;
    if (!ModeAdd) {
        setDropdown($('#PortEngine'), pe);
        setDropdown($('#StarboardEngine'), se);
        setDropdown($('#MiddleEngine'), me);
    }
    //$('input#Name').val(CRVs.name);
}

function Cancel_onclick() {
    top.PopPage('FindCRV.htm');
}
function Save_onclick() {
//debugger;
var CRV;
CRV=top.BlankCRV;
if (typeof(CRV) != 'undefined' && CRVID != null) CRV.id = CRVID;
CRV.name = $('input#Name').val();
CRV.PortEngine = $('#PortEngine').val();
CRV.MiddleEngine = $('#MiddleEngine').val();
CRV.StarboardEngine = $('#StarboardEngine').val();
if (CRV.PortEngine == null || CRV.PortEngine == "") CRV.PortEngine = top.newGuid();
if (CRV.StarboardEngine == null || CRV.StarboardEngine == "") CRV.StarboardEngine = top.newGuid();
if (CRV.MiddleEngine == null || CRV.MiddleEngine == "") CRV.MiddleEngine = top.newGuid();

top.modifyCRV(CRV);
//top.getCRV(true);
top.PopPage('FindCRV.htm');
}
