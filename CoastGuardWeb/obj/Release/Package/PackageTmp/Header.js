$(document).ready(function() {
    //debugger;

//    top.updateTopFrameSize(this.height);
top.CBEvents.AddCallBack('GetAlerts', 'gotAlerts', gotAlerts);
top.CBEvents.AddCallBack('GetMessages', 'gotMessages', gotMessages);
    top.getAlerts(true);
    
});

function gotAlerts(Alerts) {
    //debugger;
    DisplayAlerts('#AlertTable',Alerts);
}

function gotMessages(msgs) {
    //debugger;
    DisplayMessages('#MessageTable', msgs);
}


function DisplayAlerts(alertTable,Alerts) {
    $(alertTable).find("tbody").html("");
    var tr = "";
    for (var i = 0; i < Alerts.length; i++) {
        var cssclass = ' class="Alert'+Alerts[i].Level+'1"';
        var cssclass2 = ' class="Alert' + Alerts[i].Level + '2"';
        tr += "<tr><td"+cssclass+">"+Alerts[i].Level+"</td><td"+cssclass2+">"+Alerts[i].Text+"</td></tr>";
    }
    $(alertTable + ' > tbody:last').append(tr);
    //debugger;
    updateFrameHeight();
    
}
function DisplayMessages(MsgTable, msgs) {
    //debugger;


    $(MsgTable).find("tbody").html("");
    var tr = "";
    for (var i = 0; i < msgs.length; i++) {
        var cssclass = ' class="Message';
        if (msgs[i].Complete >= 0 && msgs[i].Complete <= 100) {
            tr += "<tr><td" + cssclass + '1">' + msgs[i].Msg + "<td" + cssclass + '2">' + Math.round(msgs[i].Complete * 100) / 100 + "% Complete</td><td" + cssclass + '3">' + '<div id="pBar' + msgs[i].Name.replace(/[^a-zA-Z 0-9]+/g, '') + '"></div>' + "</td></tr>";
        } else {
            tr += "<tr><td" + cssclass + ' colspan="3">' + msgs[i].Msg + "</td></tr>";
        }
    }
    
    $(MsgTable + ' > tbody:last').append(tr);

    for (var j = 0; j < msgs.length; j++) {
        if (msgs[j].Complete >= 0 && msgs[j].Complete <= 100) {
            try {
      //          debugger;
                Log("Info", "progress bar[" + "#pBar" + msgs[j].Name + "]");
                $("#pBar" + msgs[j].Name.replace(/[^a-zA-Z 0-9]+/g, '')).progressbar({ value: msgs[j].Complete });
            } catch (e) {
                //debugger;
            }       
        }
    }
    //debugger;
    updateFrameHeight();

}

function updateFrameHeight() {
    //var h = 0;
    //try {
    //    h = this.innerHeight + $('#AlertTable').height() + $('#MessageTable').height() + 5;
    //} catch (err) {
    //}
    //if (typeof (h) == "undefined" || h == null || isNaN(h)) {
    //    h = parseInt(this.frameElement.height) + $('#AlertTable').height() + $('#MessageTable').height() + 5;
    //}
    //debugger;
    var h = document.getElementById('MainDiv').offsetHeight + $('#AlertTable').height() + $('#MessageTable').height();
    if ($('#AlertTable').height() > 0) h += 5;
    if ($('#MessageTable').height() > 0) h += 5;
    top.updateTopFrameSize(h);
}
