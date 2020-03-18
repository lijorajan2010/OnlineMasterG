$(document).ready(function () {

    $("#txtSectionName").focus();
    attachEvents();
    setupSectionList();
 
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
        saveSections();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });
}


function setupSectionList() {
    $.getDataTableList(URL.EXAMSECTIONLIST, $("#tblSectionNameList")); 
}


function clearFields() {
    $("#txtSectionName").val("");
    $("#txtSequence").val("");
    $("#hdfSectionId").val("");
    $('#chkIsActive').attr('checked', false);
    if ($("#chkIsActive").prop('checked')) {
        $('#chkIsActive').click();
    }
   
}


function saveSections() {
    var model = {
        SectionId: $("#hdfSectionId").val(),
        SectionName: $("#txtSectionName").val(),
        Sequence: $("#txtSequence").val(),
        LanguageCode: $("#LanguageCode").val(),
        IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmExamUpdateSection"),
        model,
        function (data) {
            clearFields();
            setupSectionList();
        });
}


function editSection(obj) {

    $("#hdfSectionId").val(obj.id);
    $("#txtSectionName").val(obj.sectionName);
    $("#txtSequence").val(obj.sequence);
    $("#LanguageCode").val(obj.languagecode);
    if (obj.isActive == true) {
        $('#chkIsActive').click();
    }
}


function deleteSection(obj) {

    var sectionName = obj.sectionName;
    var sectionId = obj.id;

    $.confirmDelete(
        "Exam Update Section",
        sectionName,
        sectionId,
        function (sectionId) {
            $.postData(
                URL.DELETEEXAMUPDATESECTION,
                { SectionId: sectionId },
                function () {
                    clearFields();
                    setupSectionList();
                });
        });
}
