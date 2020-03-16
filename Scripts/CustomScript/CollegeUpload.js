$(document).ready(function () {

    $("#txtPaperName").focus();
    attachEvents();
    setupCollegeQuestionUploadList();
 
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


function setupCollegeQuestionUploadList() {
    $.getDataTableList(URL.COLLEGEUPLOADPAPERLIST, $("#dvUploadPaperList")); 
}


function clearFields() {
    $("#txtPaperName").val("");
    $("#txtPaperDescription").val("");
    $("#CourelistId").val("");
    $("#SubjectlistId").val("");
    $("#hdfCollegePaperId").val("");
    $("#hdfDataFileId").val("");
    $("#filename").text("");
    //$('#chkIsActive').attr('checked', false);
    //if ($("#chkIsActive").prop('checked')) {
    //    $('#chkIsActive').click();
    //}
   
}


function saveQuestionUpload() {
    var model = {
        CollegePaperId: $("#hdfCollegePaperId").val(),
        CourseId: $("#CourelistId").val(),
        SubjectId: $("#SubjectlistId").val(),
        PaperName: $("#txtPaperName").val(),
        Description: $("#txtPaperDescription").val(),
        LanguageCode: $("#LanguageCode").val(),
        DataFileId: $("#hdfDataFileId").val(),

        //IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmCollegeUploadPaper"),
        model,
        function (data) {
            clearFields();
            setupCollegeQuestionUploadList();
        });
}


function editPaper(obj) {
    $("#hdfCollegePaperId").val(obj.id);
    $("#CourelistId").val(obj.courseId);
    $("#SubjectlistId").val(obj.subjectId);
    $("#txtPaperName").val(obj.paperName);
    $("#txtPaperDescription").val(obj.description);
    $("#LanguageCode").val(obj.languagecode);
    $("#hdfDataFileId").val(obj.dataFileId);
    $("#filename").text(obj.dataFileName);
    //if (obj.isActive == true) {
    //    $('#chkIsActive').click();
    //}
}


function deletePaper(obj) {

    var paperName = obj.paperName;
    var paperId = obj.id;

    $.confirmDelete(
        "Section",
        paperName,
        paperId,
        function () {
            $.postData(
                URL.DELETECOLLEGEUPLOADPAPER,
                { paperId: paperId },
                function () {
                    clearFields();
                    setupCollegeQuestionUploadList();
                });
        });
}
