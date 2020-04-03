$(document).ready(function () {

    $("#txtCategoryName").focus();
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
    $.getDataTableList(URL.SUBJECTLIST, $("#dvSubjectList")); 
}


function clearFields() {
    $("#txtDailyQuizSubjectName").val(""); 
    $("#CourelistId").val("");
    $("#hdfDailyQuizSubjectId").val("");
    $('#chkIsActive').attr('checked', false);
    if ($("#chkIsActive").prop('checked')) {
        $('#chkIsActive').click();
    }
   
}

function saveSubject() {
    var model = {
        DailyQuizCourseId: $("#CourelistId").val(),
        DailyQuizSubjectId: $("#hdfDailyQuizSubjectId").val(),
        DailyQuizSubjectName: $("#txtDailyQuizSubjectName").val(),
        LanguageCode: $("#LanguageCode").val(),
        IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmDailyQuizSubjectMaster"),
        model,
        function (data) {
            clearFields();
            setupSubjectList();
        });
}


function editSubject(obj) {
    $("#hdfDailyQuizSubjectId").val(obj.id);
    $("#CourelistId").val(obj.courseId);
    $("#txtDailyQuizSubjectName").val(obj.subjectName);
    $("#LanguageCode").val(obj.languagecode);
    if (obj.isActive == true) {
        $('#chkIsActive').click();
    }
}


function deleteSubject(obj) {

    var subjectName = obj.subjectName;
    var subjectId = obj.id;

    $.confirmDelete(
        "Subject",
        subjectName,
        subjectId,
        function () {
            $.postData(
                URL.DELETESUBJECT,
                { SubjectId: subjectId },
                function () {
                    clearFields();
                    setupSubjectList();
                });
        });
}
