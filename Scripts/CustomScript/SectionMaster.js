$(document).ready(function () {

    $("#txtSectionName").focus();
    attachEvents();
    setupSectionList();
 
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
        saveSection();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });


    $("#CourelistId").change(function () {
        $('#CategorylistId').empty();
        $('#CategorylistId').append($('<option/>', { Value: "", text: "Please Select"}));
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
}


function setupSectionList() {
    $.getDataTableList(URL.SECTIONLIST, $("#dvSectionList")); 
}


function clearFields() {
    $("#txtSectionName").val("");
    $("#txtSectionDescription").val("");
    $("#CourelistId").val("");
    $("#CategorylistId").val("");
    $("#hdfSectionId").val("");
    //$('#chkIsActive').attr('checked', false);
    //if ($("#chkIsActive").prop('checked')) {
    //    $('#chkIsActive').click();
    //}
   
}


function saveSection() {
    var model = {
        SectionId: $("#hdfSectionId").val(),
        CourseId: $("#CourelistId").val(),
        CategoryId: $("#CategorylistId").val(),
        SectionName: $("#txtSectionName").val(),
        Description: $("#txtSectionDescription").val(),
        LanguageCode: $("#LanguageCode").val(),
        //IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmSectionMaster"),
        model,
        function (data) {
            clearFields();
            setupSectionList();
        });
}


function editSection(obj) {
    $("#hdfSectionId").val(obj.id);
    $("#CourelistId").val(obj.courseId);
    $("#CategorylistId").val(obj.categoryId);
    $("#txtSectionName").val(obj.sectionName);
    $("#txtSectionDescription").val(obj.description);
    $("#LanguageCode").val(obj.languagecode);
    //if (obj.isActive == true) {
    //    $('#chkIsActive').click();
    //}
}


function deleteSection(obj) {

    var sectionName = obj.sectionName;
    var sectionId = obj.id;

    $.confirmDelete(
        "Section",
        sectionName,
        sectionId,
        function () {
            $.postData(
                URL.DELETESECTION,
                { SectionId: sectionId },
                function () {
                    clearFields();
                    setupSectionList();
                });
        });
}
