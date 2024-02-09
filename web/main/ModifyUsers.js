var MemberID;
var LogLevels = ["Debug", "Info", "Warn", "Error", "Fatal"];
var ModeAdd = false;
$(document).ready(function() {
//debugger;
MemberID = $.getURLParam('MemberId');
if (typeof(MemberID) == 'undefined'||MemberID==null) ModeAdd = true;
//formatPhone("+Abdc0381273789 123 'asd askmdh 123%$#");
//formatPhone("012345678");
setupForm();
//top.CBEvents.AddCallBack("GetMember","setupForm",setupForm);
//top.BEGetTrip(TripID);
});

function setupForm() {
var Crew=top.getCrew();
//debugger;
if (ModeAdd) return;
var MemberInfo = ArrayFind(Crew,MemberID);
$('input#UserID').val(MemberInfo.userid); 
$('input#FirstName').val(MemberInfo.First_Name);
$('input#LastName').val(MemberInfo.Last_Name);
$('input#Mobile').val(formatPhone(MemberInfo.Mobile));
$('input#Home').val(formatPhone(MemberInfo.Home));
$('input#Work').val(formatPhone(MemberInfo.Work));
$('input#Email').val(MemberInfo.email);
$('input#Skipper').attr('checked', MemberInfo.Skipper);
$('input#Operational').attr('checked', MemberInfo.Operational);
$('input#Trainee').attr('checked', MemberInfo.Trainee);
$('input#Deleted').attr('checked', MemberInfo.Deleted);
$('input#Senior').attr('checked', MemberInfo.Senior);
$('input#Password').val(MemberInfo.password);
populateDropdown($('#LogLevel'), LogLevels);
setDropdown($('#LogLevel'), LogLevels[MemberInfo.LogLevel]);
$('input#LogWindow').attr('checked', MemberInfo.LogEnabled);
}

function LastName_OnChange(newLName) {
    var Crew = top.getCrew();
    //debugger;
    var userID = $('input#UserID').val();
    if (typeof (userID) != 'undefined' && userID != null && userID != '') return;
    var id1 = newLName.substr(0, 1);
    var newId = 0;
    for (var i = 0; i < Crew.length; i++) {
        try {
            if (Crew[i].userid.substr(0, 1) == id1) {
                if (top.toInt(Crew[i].userid.substr(1)) > newId) newId = top.toInt(Crew[i].userid.substr(1));
            }
        } catch (e) {
            debugger;
        }
    }
    $('input#UserID').val(id1 + (newId + 1));
}


function formatPhone(inph)
{
inph = inph.replace(/[^0-9+]/g,'');
var retval;

//debugger;
if (inph.substring(0,1) == '+') {
    // International Number
    switch(inph.length) {
    }
}

switch (inph.length) {
    case 0,1,2,3,4,5,6:
        retval=inph;
    break;
    case 7:
        retval=inph.substring(0,3)+"-"+inph.substring(3);
    break;
    case 8,9:
        retval='(' + inph.substring(0,2) + ') '+inph.substring(2,5)+"-"+inph.substring(5);
    break;
    case 10:
        retval='(' + inph.substring(0,3) + ') '+inph.substring(3,6)+"-"+inph.substring(6);
        break;
    default:
        retval=inph;
    break;
}
return retval;
}


function phoneRemoveFormat(inph) {
return inph.replace(/[^0-9+]/g,'');
}


function Cancel_onclick() {
    top.PopPage('FindUsers.htm');
}

function findIndex(val, Array) {
    //debugger;
    var retval = -1;
    for (var i = 0; i < Array.length; i++) {
        if (Array[i] == val) return i;
    }
    return retval;

}
function UpdateStats_onclick() {
    top.BEUpdateStatistics();

}


function Save_onclick() {
//debugger;
var User;
User=top.BlankUser;
if (typeof(MemberID) != 'undefined' && MemberID != null) User.id = MemberID;
User.userid = $('input#UserID').val();
User.First_Name = $('input#FirstName').val();
User.Last_Name = $('input#LastName').val();
User.Mobile = phoneRemoveFormat($('input#Mobile').val());
User.Home = phoneRemoveFormat($('input#Home').val());
User.Work = phoneRemoveFormat($('input#Work').val());
User.email = $('input#Email').val();
User.password = $('input#Password').val();
User.Skipper = $('input#Skipper').is(':checked');
User.Operational = $('input#Operational').is(':checked');
User.Deleted = $('input#Deleted').is(':checked');
User.Trainee = $('input#Trainee').is(':checked');
User.Senior = $('input#Senior').is(':checked');
//debugger;
User.LogLevel = findIndex($("#LogLevel").val(), LogLevels);
User.LogEnabled = $('input#LogWindow').is(':checked');
top.modifyCoastGuardMember(User);
if (top.Session.Memberid == User.id) {
    if (User.LogEnabled) {
        top.$('#divLogger').show();
    } else {
        top.$('#divLogger').hide();
    }

}
if (ModeAdd) {
    top.getCrew();
    top.PopPage('FindUsers.htm')
    return
};
var memberInfo = ArrayFind(top.getCrew(),MemberID);
memberInfo.First_Name = User.First_Name;
memberInfo.Last_Name = User.Last_Name;
memberInfo.Mobile = User.Mobile;
memberInfo.Home = User.Home;
memberInfo.Work =User.Work;
memberInfo.email = User.email;
memberInfo.Skipper = User.Skipper;
memberInfo.Operational = User.Operational;
memberInfo.Senior = User.Senior;
memberInfo.Trainee = User.Trainee;
memberInfo.Deleted = User.Deleted;
memberInfo.LogLevel = User.LogLevel;
memberInfo.LogEnabled = User.LogEnabled;
top.getCrew();
top.PopPage('FindUsers.htm')
}
