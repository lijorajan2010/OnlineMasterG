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

    $("#CourelistId").change(function () {
        $('#CategorylistId').empty();
        $('#CategorylistId').append($('<option/>', { Value: "", text: "Please Select" }));
        $.ajax({
            url: URL.BINDCATEGORY,
            datatype: 'json',
            data: { courseId: $(this).val() ? $(this).val() : 0 },
            method: 'Post',
            success: function (data) {

                $('#CategorylistId').prop("disabled", false);
                $(data).each(function (index, item) {
                    $('#CategorylistId').append($('<option/>', { Value: item.CategoryId, text: item.CategoryName }));
                });
            }
        });
    });

    $("#CategorylistId").change(function () {
        $('#SectionlistId').empty();
        $('#SectionlistId').append($('<option/>', { Value: "", text: "Please Select" }));
        $.ajax({
            url: URL.BINDSECTIONS,
            datatype: 'json',
            data: { categoryId: $(this).val() ? $(this).val() : 0 },
            method: 'Post',
            success: function (data) {

                $('#SectionlistId').prop("disabled", false);
                $(data).each(function (index, item) {
                    $('#SectionlistId').append($('<option/>', { Value: item.SectionId, text: item.SectionName }));
                });
            }
        });
    });

   

    $("#SectionlistId").change(function () {
        $('#TestlistId').empty();
        $('#TestlistId').append($('<option/>', { Value: "", text: "Please Select" }));
        $.ajax({
            url: URL.BINDTESTS,
            datatype: 'json',
            data: { sectionId: $(this).val() ? $(this).val() : 0 },
            method: 'Post',
            success: function (data) {

                $('#TestlistId').prop("disabled", false);
                $(data).each(function (index, item) {
                    $('#TestlistId').append($('<option/>', { Value: item.TestId, text: item.TestName }));
                });
            }
        });
    });

    $("#TestlistId").change(function () {
        $('#SubjectlistId').empty();
        $('#SubjectlistId').append($('<option/>', { Value: "", text: "Please Select" }));
        $.ajax({
            url: URL.BINDSUBJECTS,
            datatype: 'json',
            data: { testId: $(this).val() ? $(this).val() : 0 },
            method: 'Post',
            success: function (data) {

                $('#SubjectlistId').prop("disabled", false);
                $(data).each(function (index, item) {
                    $('#SubjectlistId').append($('<option/>', { Value: item.SubjectId, text: item.SubjectName }));
                });
            }
        });
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
            setupQuestionUploadList();
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

function fnApproval(obj) {
    var questionUploadId = obj.id;
    $.confirm("Do you want to approve these questions ? After approval, which will be available for the test",
        function () {
            $.postData(
                URL.APPROVEQUESTIONUPLOAD,
                { QuestionUploadId: questionUploadId },
                function () {
                    clearFields();
                    setupQuestionUploadList();
                });

        },
        function () { }
    );
}
function fnDenyApproval(obj) {
    var questionUploadId = obj.id;
    $.confirm("Do you want to deny these questions ? After denial, which will not be available for the test",
        function () {
            $.postData(
                URL.DENYQUESTIONUPLOAD,
                { QuestionUploadId: questionUploadId },
                function () {
                    clearFields();
                    setupQuestionUploadList();
                });

        },
        function () { }
    );
}
