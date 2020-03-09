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
    $.getDataTableList(URL.SUBJECTLIST, $("#dvSubjectList")); 
}


function clearFields() {
    $("#txtSubjectName").val("");
    $("#txtSequence").val("");
    $("#CourelistId").val("");
    $("#CategorylistId").val("");
    $("#SectionlistId").val("");
    $("#TestlistId").val("");
    $("#hdfSubjectId").val("");
    //$('#chkIsActive').attr('checked', false);
    //if ($("#chkIsActive").prop('checked')) {
    //    $('#chkIsActive').click();
    //}
   
}


function saveSubject() {
    debugger
    var model = {
        SubjectId: $("#hdfSubjectId").val(),
        CourseId: $("#CourelistId").val(),
        CategoryId: $("#CategorylistId").val(),
        SectionId: $("#SectionlistId").val(),
        TestId: $("#TestlistId").val(),
        SubjectName: $("#txtSubjectName").val(),
        Sequence: $("#txtSequence").val(),
        LanguageCode: $("#LanguageCode").val(),
        //IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmSubjectMaster"),
        model,
        function (data) {
            clearFields();
            setupSubjectList();
        });
}


function editSubject(obj) {
    $("#hdfSubjectId").val(obj.id);
    $("#CourelistId").val(obj.courseId);
    $("#CategorylistId").val(obj.categoryId);
    $("#SectionlistId").val(obj.sectionId);
    $("#TestlistId").val(obj.testId);
    $("#txtSubjectName").val(obj.subjectName);
    $("#txtSequence").val(obj.sequence);
    $("#LanguageCode").val(obj.languagecode);
   
    //if (obj.isActive == true) {
    //    $('#chkIsActive').click();
    //}
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
                URL.DELETESSUBJECT,
                { SubjectId: subjectId },
                function () {
                    clearFields();
                    setupSubjectList();
                });
        });
}
