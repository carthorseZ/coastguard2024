var AttachmentID;
var ParentId;
var ParentEntity;
var ModeAdd = false;
var MyPath;
var myFileName;
$(document).ready(function() {
    //debugger;
    AttachmentID = $.getURLParam('AttachmentId');
    ParentId = $.getURLParam('ParentId');
    ParentEntity = $.getURLParam('ParentEntity');
    if (typeof (AttachmentID) == 'undefined' || AttachmentID == null) ModeAdd = true;
    top.CBEvents.AddCallBack("GetAttachments", "Modify Attachments setup Form", setupForm);

    setupForm();
});

function setupForm(AttachmentInfo) {
    
    //debugger;
    if (typeof (AttachmentInfo) == 'undefined' || AttachmentInfo == null) return;
    if (ModeAdd) return;
    MyPath = AttachmentInfo.Path;
    $('input#Name').val(AttachmentInfo.Name);

    $('#Description').val(AttachmentInfo.Description);

    $('input#Deleted').attr('checked', AttachmentInfo.Deleted);

}

function Cancel_onclick() {
    top.showMain();
}
function Save_onclick() {
    //debugger;
    if (isEmpty(MyPath) && isEmpty(myFileName)) {
        myAlert("a path is mandatory for an attachment");
        return;
    }
    var Attachment;
    Attachment = top.BlankAttachment;
    if (typeof (AttachmentID) != 'undefined' && AttachmentID != null) Attachment.id = AttachmentID;
    Attachment.Name = $('input#Name').val();
    //Attachment.SQL = $('input#SQL').val();
    if (!isEmpty(myFileName)) {
        Attachment.Path = myFileName;
    }
    Attachment.Description = $('#Decription').text();
    Attachment.ParentId = ParentId;
    Attachment.ParentType = ParentEntity;
    Attachment.Deleted = $('input#Deleted').is(':checked');
    uploadIframe.submitform();
    
    top.modifyAttachment(Attachment);
    top.showMain();
    
}
