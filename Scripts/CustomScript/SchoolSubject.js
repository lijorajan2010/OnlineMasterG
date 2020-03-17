$(document).ready(function () {

    $("#txtSubjectName").focus();
    attachEvents();
    setupSubjectList();
 
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
        saveSubject();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });
}

function setupSubjectList() {
    $.getDataTableList(URL.SCHOOLSUBJECTLIST, $("#dvSchoolSubjectList")); 
}


function clearFields() {
    $("#txtSubjectName").val("");
    $("#txtSequence").val("");
    $("#ClasslistId").val("");
    $("#hdfSubjectId").val("");
    $('#chkIsActive').attr('checked', false);
    if ($("#chkIsActive").prop('checked')) {
        $('#chkIsActive').click();
    }
   
}


function saveSubject() {
    var model = {
        ClassId: $("#ClasslistId").val(),
        SubjectId: $("#hdfSubjectId").val(),
        SubjectName: $("#txtSubjectName").val(),
        SequenceSub: $("#txtSequence").val(),
        LanguageCode: $("#LanguageCode").val(),
        IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmSchoolSubjectMaster"),
        model,
        function (data) {
            clearFields();
            setupSubjectList();
        });
}


function editSubject(obj) {
    $("#hdfSubjectId").val(obj.id);
    $("#ClasslistId").val(obj.classId);
    $("#txtSubjectName").val(obj.subjectName);
    $("#txtSequence").val(obj.sequence);
    $("#LanguageCode").val(obj.languagecode);
    if (obj.isActive == true) {
        $('#chkIsActive').click();
    }
}


function deleteSubject(obj) {

    var SubjectName = obj.subjectName;
    var SubjectId = obj.id;

    $.confirmDelete(
        "Subject",
        SubjectName,
        SubjectId,
        function () {
            $.postData(
                URL.DELETESCHOOLSUBJECT,
                { SubjectId: SubjectId },
                function () {
                    clearFields();
                    setupSubjectList();
                });
        });
}
