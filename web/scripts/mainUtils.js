function populateTideTable(){
//alert("test2");
//var table=$('Table#Tides');
Heights = top.curTideHeights;
$('#Tides').dataTable().fnClearTable();
try {
    if (Heights[0].H1 > 1.5) {
        $('#Tides').dataTable().fnAddData([TimeFormat(Heights[0].Time1), TimeFormat(Heights[0].Time2), TimeFormat(Heights[1].Time1), TimeFormat(Heights[1].Time2), TimeFormat(Heights[2].Time1), TimeFormat(Heights[2].Time2), TimeFormat(Heights[3].Time1), TimeFormat(Heights[3].Time2), TimeFormat(Heights[4].Time1), TimeFormat(Heights[4].Time2), TimeFormat(Heights[5].Time1), TimeFormat(Heights[5].Time2)]);
        $('#Tides').dataTable().fnAddData([TimeFormat(Heights[0].Time3), TimeFormat(Heights[0].Time4), TimeFormat(Heights[1].Time3), TimeFormat(Heights[1].Time4), TimeFormat(Heights[2].Time3), TimeFormat(Heights[2].Time4), TimeFormat(Heights[3].Time3), TimeFormat(Heights[3].Time4), TimeFormat(Heights[4].Time3), TimeFormat(Heights[4].Time4), TimeFormat(Heights[5].Time3), TimeFormat(Heights[5].Time4)]);
    } else {
        $('#Tides').dataTable().fnAddData(["", TimeFormat(Heights[0].Time1), "", TimeFormat(Heights[1].Time1), "", TimeFormat(Heights[2].Time1), "", TimeFormat(Heights[3].Time1), "", TimeFormat(Heights[4].Time1), "", TimeFormat(Heights[5].Time1)]);
        $('#Tides').dataTable().fnAddData([TimeFormat(Heights[0].Time2), TimeFormat(Heights[0].Time3), TimeFormat(Heights[1].Time2), TimeFormat(Heights[1].Time3), TimeFormat(Heights[2].Time2), TimeFormat(Heights[2].Time3), TimeFormat(Heights[3].Time2), TimeFormat(Heights[3].Time3), TimeFormat(Heights[4].Time2), TimeFormat(Heights[4].Time3), TimeFormat(Heights[5].Time2), TimeFormat(Heights[5].Time3)]);
        $('#Tides').dataTable().fnAddData([TimeFormat(Heights[0].Time4), "", TimeFormat(Heights[1].Time4), "", TimeFormat(Heights[2].Time4), "", TimeFormat(Heights[3].Time4), "", TimeFormat(Heights[4].Time4), "", TimeFormat(Heights[5].Time4), ""]);
    }
} catch (e) {
//debugger;
}
//if (top.debug) debugger;
//$('#Tides').dataTable().fnAddData( ["A","B","C","D","E","F","G","H","I","J","K","L"] );
}

function TimeFormat(dt) {
//debugger;
if (typeof(dt) == "Date"||typeof(dt) == "object") { 
//debugger;
try {
if (dt.getHours() == 0 && dt.getMinutes() ==0) return "12:00pm";
if (dt.getHours() == 0 ) return "12:"+formatnumber(dt.getMinutes())+"am";
if (dt.getHours() < 12) return dt.getHours()+":"+formatnumber(dt.getMinutes())+"am";
return (dt.getHours()-12)+":"+formatnumber(dt.getMinutes())+"pm";


} catch (err) {
top.LogMsg("Error in displaying time from date"+dt);
return dt;
}
return "HH:mm";
}else {
    return dt;
}
}

function formatnumber(inVal) {
if (inVal < 10) return "0"+inVal;
return inVal;
}
function ArrayCopy(inarr) {
    try {
        if (typeof(inarr.length) == "undefined" || inarr.length == 0 ) {
            return inarr;
        }
        var retval = new Array(inarr.length);
        for (var i = 0; i < inarr.length; i++) {
            try {
                var type = typeof (inarr[i]);
                if (type == "object") {
                    retval[i] = ArrayCopy(inarr[i]);
                } else {
                    retval[i] = inarr[i];
                }
            } catch (Err) {
                debugger;
                LogMsg(Err);
            }
        }
        return retval;
    } catch (err) {
    return inarr;
    }
}
function LogMsg(msg,level) {
    top.LogMsg(msg,level);
}


function ArrayFind(Arr,item){
    //if (top.debug) debugger;
    if (typeof(Arr) == "undefined") return;
    for (var i =0;i < Arr.length;i++) {
        if (Arr[i].id == item) { return Arr[i]; }
        if (Arr[i].Id == item) { return Arr[i]; }
    }
}

function ArrayCompare(Arr1, Arr2) {
   // if (top.debug) debugger;
    if (Arr1.length == Arr2.length) {
        for (var i = 0; i < Arr1.length; i++) {
            if (Arr1[i] != Arr2[i]) return false;
        }
        return true;
    } else {
    return false;
    }
}


function ArrayRemoveItem(Arr,item){
var retval = new Array();
var j=0;
//if (top.debug) debugger;
if (typeof (Arr) == "undefined" || Arr == null) return Arr;
for (var i =0;i < Arr.length;i++) {
    
    if (typeof(Arr[i]) == 'undefined' || Arr[i].id == item) {} else {
        retval[j] = Arr[i];
        j++;
    }
}
return retval;
}
function ArrayUpdateItem(Arr,oldItem,newItem){
    //if (top.debug) debugger;
    for (var i =0;i < Arr.length;i++) {
        if (Arr[i].id == oldItem) {Arr[i] = newItem;}
    }
}

function populateDropdown(select, data,table) {
  //  if (top.debug) debugger;
    var curval = select.val();
    if (typeof (data) == "undefined" || data == null) {
        return;
    }
    
    var curtext = "";
    try {
         curtext = select[0].options[select[0].options.selectedIndex].text;
    } catch (err) { }
    if (typeof (table) == "undefined") {
        select.html('');
        select.append($('<option></option>'));
        //$.each(data, function(id) {    
        //    select.append($('<option>'+ data[id]+'</option>'));    
        //});
        for (var i = 0; i < data.length; i++) {
            select.append($('<option>' + data[i] + '</option>'));
        };

    } else {
        switch (table) {
            case "name":
                select.html('');
                select.append($('<option></option>'));
                for (i = 0; i < data.length; i++) {
                    select.append($('<option value=' + data[i].id + '>' + data[i].Name + '</option>'));
                };
        }
        
    }
    try {
        if (curtext != "") {
            select.append($('<option value="' + curval + '" selected="selected">' + curtext + '</option>'));
            select.text = curtext;
        }
    } catch (err) { }
}
function populateDropdownName(select, data) {
    //if (top.debug) debugger;
    select.html('');
    select.append($('<option></option>'));
    //$.each(data, function(id) {    
    //    select.append($('<option>'+ data[id]+'</option>'));    
    //});
    for (var i = 0; i < data.length; i++) {
        select.append($('<option value=' + data[i].id+ '>' + data[i].Name + '</option>'));
    };
}

function setDropdown(select, val) {
//if (top.debug) debugger;
//alert("Val:"+val);
//document.getElementById('myDropdown').selectedIndex = 2;
//$(select+' option[value='+val+']').attr('selected', 'selected'); 
//document.getElementById('Weather').selectedIndex = 2;
//$('#Weather').attr('selectedindex',2);
//alert($("#Weather").attr("selectedIndex"));
select.val(val)
//$("#Weather option:eq(3)").attr("selected","selected");
//select.selectedIndex = 1;
    
    //if (top.debug) debugger;
    //select.html('');    
    //select.append($('<option></option>'));
    //$.each(data, function(id) {    
    //    select.append($('<option>'+ data[id]+'</option>'));    
    //});
    //for (var i =0;i < data.length;i++){
    //    select.append($('<option>'+ data[i]+'</option>'));    
    //};           
}
function getColumns(inArr) {
    if (inArr == null) throw new Error("Invalid argument");
    var retval = new Array();
    for (var i = 0; i < inArr.length; i++) {
        var temp = new Object;
        temp.sTitle = inArr[i];
        retval.push(temp);
    }
    return retval;
}

function isEmpty(inVal) {

    if (typeof (inVal) == "undefined" || inVal == null) return true;
    return false;
}


function TableAddColumn(Table, ColumnVal) {
    if (typeof (ColumnVal) == "undefined") ColumnVal = null;
    for (var i = 0; i < Table.length; i++) {
        Table[i].push(ColumnVal);
    }

}


function myAlert(msg) {
    alert(msg);
}