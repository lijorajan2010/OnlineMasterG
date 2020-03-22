$(document).ready(function () {

    $("#TestlistId").focus();
    attachEvents();
    setupInstructionList();
 
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
        saveInstructions();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });

    $("#TestlistId").change(function () {
        $.post(URL.LOADINSTRUCTIONLIST, { testId: $(this).val() ? $(this).val():0 }, function (data) {
            $("#lstSubjects").html('');
            $("#lstSubjects").html(data);
        });
    });
}


function setupInstructionList() {
    $.getDataTableList(URL.INSTRUCTIONLIST, $("#tblInstructionList")); 
}


function clearFields() {
    $("#TestlistId").val('');
    $("#TestlistId").change();
   
}


function saveInstructions() {

    $.submitForm(
        $("#frmGeneralInstructionMaster"),
        function (data) {
            clearFields();
            setupInstructionList();
        });

    $("#frmGeneralInstructionMaster").submit();
    
}


function deleteInstruction(obj) {

    var SubjectName = obj.SubjectName;
    var instructionId = obj.id;

    $.confirmDelete(
        "Instruction/Marks",
        SubjectName,
        instructionId,
        function (instructionId) {
            $.postData(
                URL.DELETEINSTRUCTION,
                { instructionId: instructionId },
                function () {
                    clearFields();
                    setupInstructionList();
                });
        });
}
