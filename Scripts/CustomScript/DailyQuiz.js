$(document).ready(function () {

    $("#txtQuizName").focus();
    attachEvents();
    setupQuizList();
 
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
        saveQuiz();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });

    $("#CourelistId").change(function () {
        $("#SubjectlistId").empty();
        $("#SubjectlistId").append($('<option/>', { Value: "", text: "Please Select" }));
        $.ajax({
            url: URL.BINDSUBJECTS,
            datatype: 'json',
            data: { CourelistId: $(this).val() ? $(this).val() : 0 },
            method: 'Post',
            success: function (data) {

                $("#SubjectlistId").prop("disabled", false);
                $(data).each(function (index, item) {
                    $("#SubjectlistId").append($('<option/>', { Value: item.SubjectId, text: item.SubjectName }));
                });
            }
        });
    });
}

function setupQuizList() {
    $.getDataTableList(URL.DAILYQUIZLIST, $("#tblDailyQuizList")); 
}


function clearFields() {
    $("#hdfQuizId").val(""); 
    $("#CourelistId").val("");
    $("#SubjectlistId").val("");
    $("#txtQuizName").val("");
    $("#txtNoOfQuestion").val("");
    $("#txtTimeInMinutes").val("");
    $("#txtDescription").val("");
   
}

function saveQuiz() {
    var model = {
        DailyQuizId: $("#hdfQuizId").val(),
        DailyQuizCourseId: $("#CourelistId").val(),
        DailyQuizSubjectId: $("#SubjectlistId").val(),
        DailyQuizName: $("#txtQuizName").val(),
        NoOfQuestions: $("#txtNoOfQuestion").val(),
        Description: $("#txtDescription").val(),
        LanguageCode: $("#LanguageCode").val(),
        TimeInMinutes: $("#txtTimeInMinutes").val()
    };

    $.postForm(
        $("#frmDailyQuizMaster"),
        model,
        function (data) {
            clearFields();
            setupQuizList();
        });
}


function editQuiz(obj) {
    $("#hdfQuizId").val(obj.id);
    $("#CourelistId").val(obj.courseId);
    $("#CourelistId").change();
    setTimeout(function () { $("#SubjectlistId").val(obj.subjectId);  }, 500);
    $("#txtQuizName").val(obj.quizName);
    $("#txtNoOfQuestion").val(obj.noOfQuestion);
    $("#txtTimeInMinutes").val(obj.timeInMinutes);
    $("#txtDescription").val(obj.description);
}


function deleteQuiz(obj) {

    var QuizName = obj.QuizName;
    var QuizId = obj.id;

    $.confirmDelete(
        "Quiz",
        QuizName,
        QuizId,
        function () {
            $.postData(
                URL.DELETEDAILYQUIZ,
                { DailyQuizId: QuizId },
                function () {
                    clearFields();
                    setupQuizList();
                });
        });
}
