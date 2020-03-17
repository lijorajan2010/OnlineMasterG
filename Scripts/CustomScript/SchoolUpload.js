﻿$(document).ready(function () {

    $("#txtPaperName").focus();
    attachEvents();
    setupSchoolQuestionUploadList();
 
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


function setupSchoolQuestionUploadList() {
    $.getDataTableList(URL.SCHOOLUPLOADPAPERLIST, $("#dvUploadPaperList")); 
}


function clearFields() {
    $("#txtPaperName").val("");
    $("#txtPaperDescription").val("");
    $("#ClasslistId").val("");
    $("#SubjectlistId").val("");
    $("#SectionlistId").val("");
    $("#hdfSchoolPaperId").val("");
    $("#hdfDataFileId").val("");
    $("#filename").text("");
    //$('#chkIsActive').attr('checked', false);
    //if ($("#chkIsActive").prop('checked')) {
    //    $('#chkIsActive').click();
    //}
   
}


function saveQuestionUpload() {
    var model = {
        PaperId: $("#hdfSchoolPaperId").val(),
        ClassId: $("#ClasslistId").val(),
        SubjectId: $("#SubjectlistId").val(),
        SectionId: $("#SectionlistId").val(),
        PaperName: $("#txtPaperName").val(),
        Description: $("#txtPaperDescription").val(),
        LanguageCode: $("#LanguageCode").val(),
        DataFileId: $("#hdfDataFileId").val(),

        //IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmSchoolUploadPaper"),
        model,
        function (data) {
            clearFields();
            setupSchoolQuestionUploadList();
        });
}


function editPaper(obj) {
    $("#hdfSchoolPaperId").val(obj.id);
    $("#ClasslistId").val(obj.classId);
    $("#SubjectlistId").val(obj.subjectId);
    $("#SectionlistId").val(obj.sectionId);
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
                URL.DELETESCHOOLPAPER,
                { paperId: paperId },
                function () {
                    clearFields();
                    setupSchoolQuestionUploadList();
                });
        });
}
