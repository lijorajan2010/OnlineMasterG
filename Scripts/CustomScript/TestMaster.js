$(document).ready(function () {

    $("#txtTestName").focus();
    attachEvents();
    setupTestList();
 
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
        saveTest();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });
}


function setupTestList() {
    $.getDataTableList(URL.TESTLIST, $("#dvTestList")); 
}


function clearFields() {
    $("#txtTestName").val("");
    $("#txtTestDescription").val("");
    $("#CourelistId").val("");
    $("#CategorylistId").val("");
    $("#SectionlistId").val("");
    $("#txtTestTime").val("");
    $("#hdfTestId").val("");
    //$('#chkIsActive').attr('checked', false);
    //if ($("#chkIsActive").prop('checked')) {
    //    $('#chkIsActive').click();
    //}
   
}


function saveTest() {
    var model = {
        TestId: $("#hdfTestId").val(),
        CourseId: $("#CourelistId").val(),
        CategoryId: $("#CategorylistId").val(),
        SectionId: $("#SectionlistId").val(),
        TestName: $("#txtTestName").val(),
        Description: $("#txtTestDescription").val(),
        LanguageCode: $("#LanguageCode").val(),
        TimeInMinutes: $("#txtTestTime").val(),
        //IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmTestMaster"),
        model,
        function (data) {
            clearFields();
            setupTestList();
        });
}


function editTest(obj) {
    $("#hdfTestId").val(obj.id);
    $("#CourelistId").val(obj.courseId);
    $("#CategorylistId").val(obj.categoryId);
    $("#SectionlistId").val(obj.sectionId);
    $("#txtTestName").val(obj.testName);
    $("#txtTestDescription").val(obj.description);
    $("#LanguageCode").val(obj.languagecode);
    $("#txtTestTime").val(obj.timeInMinutes);
    //if (obj.isActive == true) {
    //    $('#chkIsActive').click();
    //}
}


function deleteTest(obj) {

    var testName = obj.testName;
    var testId = obj.id;

    $.confirmDelete(
        "Test",
        testName,
        testId,
        function () {
            $.postData(
                URL.DELETESTEST,
                { TestId: testId },
                function () {
                    clearFields();
                    setupTestList();
                });
        });
}
