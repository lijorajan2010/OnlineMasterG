$(document).ready(function () {

    $("#txtUpdateDescription").focus();
    attachEvents();
    setupLatesUpdatesList();
 
    $("input").bind("keydown", function (event) {
        // track enter key
        var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
        if (keycode == 13) { // keycode for enter key
            // force the 'Enter Key' to implicitly click the Update button
            $('#btnSearch').click();
            return false;
        } else {
            return true;
        }
    }); // end of function
});

function attachEvents() {

    $("#btnSave").click(function () {
        saveUpdates();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });
}


function setupLatesUpdatesList() {
    $.getDataTableList(URL.LATESTUPDATESLIST, $("#tblLatestUpdateList")); 
}


function clearFields() {
    $("#txtUpdateDescription").val("");
    $("#txtSequence").val("");
    $("#hdfUpdateId").val("");
    $('#chkIsActive').attr('checked', false);
    if ($("#chkIsActive").prop('checked')) {
        $('#chkIsActive').click();
    }
   
}


function saveUpdates() {
    var model = {
        UpdateId: $("#hdfUpdateId").val(),
        UpdateDescription: $("#txtUpdateDescription").val(),
        Sequence: $("#txtSequence").val(),
        LanguageCode: $("#LanguageCode").val(),
        IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmLatestUpdates"),
        model,
        function (data) {
            clearFields();
            setupLatesUpdatesList();
        });
}


function editUpdates(obj) {
    $("#hdfUpdateId").val(obj.id);
    $("#txtUpdateDescription").val(obj.updateDescription);
    $("#txtSequence").val(obj.sequence);
    $("#LanguageCode").val(obj.languagecode);
    if (obj.isActive == true) {
        $('#chkIsActive').click();
    }
}


function deleteUpdates(obj) {

    var updateDescription = obj.updateDescription;
    var UpdateId = obj.id;

    $.confirmDelete(
        "Latest Update",
        updateDescription,
        UpdateId,
        function (UpdateId) {
            $.postData(
                URL.DELETELATESTUPDATE,
                { updateId: UpdateId },
                function () {
                    clearFields();
                    setupLatesUpdatesList();
                });
        });
}
