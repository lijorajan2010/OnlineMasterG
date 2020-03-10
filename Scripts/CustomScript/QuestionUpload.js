$(document).ready(function () {

    attachEvents();
    setupQuestionUploadList();
 
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
        saveQuestionUpload();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });
}


function setupQuestionUploadList() {
    $.getDataTableList(URL.QUESTIONUPLOADLIST, $("#dvQuestionUploadList")); 
}


function clearFields() {
  
    $("#CourelistId").val("");
    $("#CategorylistId").val("");
    $("#SectionlistId").val("");
    $("#TestlistId").val("");
    $("#SubjectlistId").val("");
    $("#hdfQuestionUploadId").val("");
    $("#uploadExcel").val("");
    //$('#chkIsActive').attr('checked', false);
    //if ($("#chkIsActive").prop('checked')) {
    //    $('#chkIsActive').click();
    //}
   
}


function saveQuestionUpload() {

    $.submitForm(
        $("#frmQuestionUpload"),
        function (data) {
            clearFields();
            setupSubjectList();
        });

    $("#frmQuestionUpload").submit();
}


function editQuestionUpload(obj) {
    $("#hdfQuestionUploadId").val(obj.id);
    $("#CourelistId").val(obj.courseId);
    $("#CategorylistId").val(obj.categoryId);
    $("#SectionlistId").val(obj.sectionId);
    $("#TestlistId").val(obj.testId);
    $("#SubjectlistId").val(obj.subjectId);
    $("#LanguageCode").val(obj.languagecode);
}


function deleteQuestionUpload(obj) {

    var questionUploadId = obj.id;

    $.confirmDelete(
        "Question",
        "",
        questionUploadId,
        function () {
            $.postData(
                URL.DELETESQUESTIONUPLOAD,
                { QuestionUploadId: questionUploadId },
                function () {
                    clearFields();
                    setupQuestionUploadList();
                });
        });
}
