$(document).ready(function () {

    $("#CategorylistId").focus();
    attachEvents();
    setupCurrentAffairsUploadList();
 
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
        saveCurrentAffairsUpload();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });

}


function setupCurrentAffairsUploadList() {
    $.getDataTableList(URL.CURRENTAFFAIRSUPLOADLIST, $("#dvCurrentAffairsUploadList")); 
}


function clearFields() {
    $("#CategorylistId").val("");
    $("#txtDate").val("");
    $("#hdfCurrentAffairsUploadId").val("");
    $("#hdfDataFileId").val("");
    $("#filename").text("");
}


function saveCurrentAffairsUpload() {
    var model = {
        CurrentAffairsUploadId: $("#hdfCurrentAffairsUploadId").val(),
        CurrentAffairsCategoryId: $("#CategorylistId").val(),  
        UploadDate: $("#txtDate").val(),
        LanguageCode: $("#LanguageCode").val(),
        DataFileId: $("#hdfDataFileId").val()
    };

    $.postForm(
        $("#frmCurrentAffairsUpload"),
        model,
        function (data) {
            clearFields();
            setupCurrentAffairsUploadList();
        });
}


function editCurrentAffairsUpload(obj) {
    $("#hdfCurrentAffairsUploadId").val(obj.id);
    $("#CategorylistId").val(obj.categoryId);
    $("#txtDate").val(obj.UploadDate);
    $("#LanguageCode").val(obj.languagecode);
    $("#hdfDataFileId").val(obj.dataFileId);
    $("#filename").text(obj.dataFileName);
}


function deleteCurrentAffairsUpload(obj) {

    var categoryName = obj.categoryName;
    var uploadId = obj.id;

    $.confirmDelete(
        "Current Affairs PDF",
        categoryName,
        uploadId,
        function () {
            $.postData(
                URL.DELETECURRENTAFFAIRSUPLOAD,
                { UploadId: uploadId },
                function () {
                    clearFields();
                    setupCurrentAffairsUploadList();
                });
        });
}
