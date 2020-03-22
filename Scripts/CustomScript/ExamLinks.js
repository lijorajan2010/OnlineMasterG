$(document).ready(function () {

    $("#txtLink").focus();
    attachEvents();
    setupLinkList();
 
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
        saveLinks();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });
}

function setupLinkList() {
    $.getDataTableList(URL.EXAMLINKLIST, $("#dvLinkList")); 
}


function clearFields() {
    $("#txtLink").val("");
    $("#txtLinkDescription").val("");
    $("#txtSequence").val("");
    $("#SectionlistId").val("");
    $("#hdfLinkId").val("");
    $('#chkIsActive').attr('checked', false);
    if ($("#chkIsActive").prop('checked')) {
        $('#chkIsActive').click();
    }
   
}


function saveLinks() {
    var model = {
        SectionLinkId: $("#SectionlistId").val(),
        LinkId: $("#hdfLinkId").val(),
        Link: $("#txtLink").val(),
        LinkDescription: $("#txtLinkDescription").val(),
        Sequence: $("#txtSequence").val(),
        LanguageCode: $("#LanguageCode").val(),
        IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmExamUpdateLinks"),
        model,
        function (data) {
            clearFields();
            setupLinkList();
        });
}


function editLinks(obj) {
    $("#hdfLinkId").val(obj.id);
    $("#SectionlistId").val(obj.sectionId);
    $("#txtLink").val(obj.link);
    $("#txtLinkDescription").val(obj.linkDescription);
    $("#txtSequence").val(obj.sequence);
    $("#LanguageCode").val(obj.languagecode);
    if (obj.isActive == true) {
        $('#chkIsActive').click();
    }
}


function deleteLinks(obj) {

    var link = obj.Link;
    var linkId = obj.id;

    $.confirmDelete(
        "Link",
        link,
        linkId,
        function () {
            $.postData(
                URL.DELETEEXAMLINK,
                { LinkId: linkId },
                function () {
                    clearFields();
                    setupLinkList();
                });
        });
}
