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

    $("#ClasslistId").change(function () {
        $('#SubjectlistId').empty();
        $('#SubjectlistId').append($('<option/>', { Value: "", text: "Please Select" }));
        $.ajax({
            url: URL.BINDSUBJECTS,
            datatype: 'json',
            data: { classId: $(this).val() ? $(this).val() : 0 },
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


function setupSectionList() {
    $.getDataTableList(URL.SECTIONLIST, $("#dvSectionList")); 
}


function clearFields() {
    $("#txtSectionName").val("");
    $("#txtSectionDescription").val("");
    $("#ClasslistId").val("");
    $("#SubjectlistId").val("");
    $("#hdfSectionId").val("");
    //$('#chkIsActive').attr('checked', false);
    //if ($("#chkIsActive").prop('checked')) {
    //    $('#chkIsActive').click();
    //}
   
}


function saveSection() {
    var model = {
        SectionId: $("#hdfSectionId").val(),
        ClassId: $("#ClasslistId").val(),
        SubjectId: $("#SubjectlistId").val(),
        SectionName: $("#txtSectionName").val(),
        Description: $("#txtSectionDescription").val(),
        LanguageCode: $("#LanguageCode").val(),
        //IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmSchoolSection"),
        model,
        function (data) {
            clearFields();
            setupSectionList();
        });
}


function editSection(obj) {
    $("#hdfSectionId").val(obj.id);
    $("#ClasslistId").val(obj.classId);
    $("#SubjectlistId").val(obj.subjectId);
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
