$(document).ready(function () {

    $("#txtClassName").focus();
    attachEvents();
    setupClassList();
 
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
        saveClasses();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });
}


function setupClassList() {
    $.getDataTableList(URL.CLASSLIST, $("#tblClassList")); 
}


function clearFields() {
    $("#txtClassName").val("");
    $("#txtSequence").val("");
    $("#hdfClassId").val("");
    $('#chkIsActive').attr('checked', false);
    if ($("#chkIsActive").prop('checked')) {
        $('#chkIsActive').click();
    }
   
}


function saveClasses() {
    var model = {
        ClassId: $("#hdfClassId").val(),
        ClassName: $("#txtClassName").val(),
        Sequence: $("#txtSequence").val(),
        LanguageCode: $("#LanguageCode").val(),
        IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmSchoolClass"),
        model,
        function (data) {
            clearFields();
            setupClassList();
        });
}


function editClass(obj) {

    $("#hdfClassId").val(obj.id);
    $("#txtClassName").val(obj.className);
    $("#txtSequence").val(obj.sequence);
    $("#LanguageCode").val(obj.languagecode);
    if (obj.isActive == true) {
        $('#chkIsActive').click();
    }
}


function deleteClass(obj) {

    var className = obj.className;
    var classId = obj.id;

    $.confirmDelete(
        "Class",
        className,
        classId,
        function (coursetId) {
            $.postData(
                URL.DELETESCHOOLCLASS,
                { classId: classId },
                function () {
                    clearFields();
                    setupClassList();
                });
        });
}
