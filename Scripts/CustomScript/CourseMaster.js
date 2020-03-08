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

function clearControls() {
    $("#ddlAirportName").val('');
    $("#ddlCountry").val('');
    $("#ddlState").val('');
}

function setupCourseList() {
    $.getDataTableList(URL.COURSELIST, $("#tblCourseList")); 
}


function clearFields() {
    $("#txtCourseName").val("");
    $("#txtSequence").val("");
    $("#hdfcourseId").val("");
    $('#chkIsActive').attr('checked', false);
    if ($("#chkIsActive").prop('checked')) {
        $('#chkIsActive').click();
    }
   
}


function saveCourse() {
    var model = {
        CourseId: $("#hdfcourseId").val(),
        CourseName: $("#txtCourseName").val(),
        Sequence: $("#txtSequence").val(),
        LanguageCode: $("#LanguageCode").val(),
        IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmCourseMaster"),
        model,
        function (data) {
            clearFields();
            setupCourseList();
        });
}


function editCourse(obj) {
    debugger
    $("#hdfcourseId").val(obj.id);
    $("#txtCourseName").val(obj.courseName);
    $("#txtSequence").val(obj.sequence);
    $("#LanguageCode").val(obj.languagecode);
    if (obj.isActive == true) {
        $('#chkIsActive').click();
    }
}


function deleteCourse(obj) {
    debugger

    var courseName = obj.courseName;
    var coursetId = obj.id;

    $.confirmDelete(
        "Course",
        courseName,
        coursetId,
        function (coursetId) {
            $.postData(
                URL.DELETECOURSE,
                { coursetId: coursetId },
                function () {
                    clearFields();
                    setupCourseList();
                });
        });
}
