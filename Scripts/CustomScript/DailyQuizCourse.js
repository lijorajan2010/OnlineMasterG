$(document).ready(function () {

    $("#txtCourseName").focus();
    attachEvents();
    setupCourseList();
 
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
        saveCourse();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });
}


function setupCourseList() {
    $.getDataTableList(URL.DAILYQUIZCOURSELIST, $("#tblDailyQuizSubjectList")); 
}


function clearFields() {
    $("#DailyQuizCourseName").val("");
    $("#txtSequence").val("");
    $("#hdfcourseId").val("");
    $('#chkIsActive').attr('checked', false);
    if ($("#chkIsActive").prop('checked')) {
        $('#chkIsActive').click();
    }
   
}


function saveCourse() {
    var model = {
        DailyQuizCourseId: $("#hdfcourseId").val(),
        DailyQuizCourseName: $("#DailyQuizCourseName").val(),
        Sequence: $("#txtSequence").val(),
        LanguageCode: $("#LanguageCode").val(),
        IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmDailyQuizCourseMaster"),
        model,
        function (data) {
            clearFields();
            setupCourseList();
        });
}


function editCourse(obj) {

    $("#hdfcourseId").val(obj.id);
    $("#DailyQuizCourseName").val(obj.courseName);
    $("#txtSequence").val(obj.sequence);
    $("#LanguageCode").val(obj.languagecode);
    if (obj.isActive == true) {
        $('#chkIsActive').click();
    }
}


function deleteCourse(obj) {

    var courseName = obj.courseName;
    var courseId = obj.id;

    $.confirmDelete(
        "Course",
        courseName,
        courseId,
        function () {
            $.postData(
                URL.DELETEDAILYQUIZCOURSE,
                { courseId: courseId },
                function () {
                    clearFields();
                    setupCourseList();
                });
        });
}
