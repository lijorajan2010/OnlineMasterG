$(document).ready(function () {

    attachEvents();
    setupQuizUploadList();
 
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
        saveQuizUpload();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });

    $("#CourelistId").change(function () {
        $('#SubjectlistId').empty();
        $('#SubjectlistId').append($('<option/>', { Value: "", text: "Please Select" }));
        $.ajax({
            url: URL.BINDSUBJECTS,
            datatype: 'json',
            data: { CourelistId: $(this).val() ? $(this).val() : 0 },
            method: 'Post',
            success: function (data) {

                $('#SubjectlistId').prop("disabled", false);
                $(data).each(function (index, item) {
                    $('#SubjectlistId').append($('<option/>', { Value: item.SubjectId, text: item.SubjectName }));
                });
            }
        });
    });

    $("#SubjectlistId").change(function () {
        $('#QuizlistId').empty();
        $('#QuizlistId').append($('<option/>', { Value: "", text: "Please Select" }));
        $.ajax({
            url: URL.BINDQUIZ,
            datatype: 'json',
            data: { SubjectListId: $(this).val() ? $(this).val() : 0 },
            method: 'Post',
            success: function (data) {

                $('#QuizlistId').prop("disabled", false);
                $(data).each(function (index, item) {
                    $('#QuizlistId').append($('<option/>', { Value: item.QuizId, text: item.QuizName }));
                });
            }
        });
    });

}


function setupQuizUploadList() {
    $.getDataTableList(URL.QUIZUPLOADLIST, $("#dvQuizUploadList")); 
}


function clearFields() {
  
    $("#CourelistId").val("");
    $("#SubjectlistId").val("");
    $("#QuizlistId").val("");
    $("#hdfQuizUploadId").val("");
    $("#uploadExcel").val("");
  
}


function saveQuizUpload() {

    $.submitForm(
        $("#frmDailyQuizUpload"),
        function (data) {
            clearFields();
            setupQuizUploadList();
        });

    $("#frmDailyQuizUpload").submit();
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


function deleteQuizUpload(obj) {

    var dailyQuizSubjectId = obj.id;

    $.confirmDelete(
        "Question",
        "",
        dailyQuizSubjectId,
        function () {
            $.postData(
                URL.DELETEQUIZUPLOAD,
                { QuizUploadId: dailyQuizSubjectId },
                function () {
                    clearFields();
                    setupQuizUploadList();
                });
        });
}

function fnApproval(obj) {
    var dailyQuizUploadId = obj.id;
    $.confirm("Do you want to approve these questions ? After approval, which will be available for the test",
        function () {
            $.postData(
                URL.APPROVEQUIZUPLOAD,
                { dailyQuizUploadId: dailyQuizUploadId },
                function () {
                    clearFields();
                    setupQuizUploadList();
                });

        },
        function () { }
    );
}
function fnDenyApproval(obj) {
    var dailyQuizUploadId = obj.id;
    $.confirm("Do you want to deny these questions ? After denial, which will not be available for the test",
        function () {
            $.postData(
                URL.DENYQUIZUPLOAD,
                { dailyQuizUploadId: dailyQuizUploadId },
                function () {
                    clearFields();
                    setupQuizUploadList();
                });

        },
        function () { }
    );
}
