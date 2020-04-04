$(document).ready(function () {

    $("#txtSubjectName").focus();
    attachEvents();
    setupSubjectList();
 
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
        saveSubject();
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


}


function setupSubjectList() {
    $.getDataTableList(URL.SUBJECTLIST, $("#dvSubjectList")); 
}


function clearFields() {
    $("#txtSubjectName").val("");
    $("#txtSequence").val("");
    $("#CourelistId").val("");
    $("#CategorylistId").val("");
    $("#SectionlistId").val("");
    $("#TestlistId").val("");
    $("#hdfSubjectId").val("");
    //$('#chkIsActive').attr('checked', false);
    //if ($("#chkIsActive").prop('checked')) {
    //    $('#chkIsActive').click();
    //}
   
}


function saveSubject() {
    debugger
    var model = {
        SubjectId: $("#hdfSubjectId").val(),
        CourseId: $("#CourelistId").val(),
        CategoryId: $("#CategorylistId").val(),
        SectionId: $("#SectionlistId").val(),
        TestId: $("#TestlistId").val(),
        SubjectName: $("#txtSubjectName").val(),
        Sequence: $("#txtSequence").val(),
        LanguageCode: $("#LanguageCode").val(),
        //IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmSubjectMaster"),
        model,
        function (data) {
            clearFields();
            setupSubjectList();
        });
}


function editSubject(obj) {
    $("#hdfSubjectId").val(obj.id);
    $("#CourelistId").val(obj.courseId);
    $("#CourelistId").change();
    setTimeout(function () { $("#CategorylistId").val(obj.categoryId); $("#CategorylistId").change(); }, 500);
    setTimeout(function () { $("#SectionlistId").val(obj.sectionId); $("#SectionlistId").change(); }, 1500);
    setTimeout(function () { $("#TestlistId").val(obj.testId); }, 3000);
    $("#txtSubjectName").val(obj.subjectName);
    $("#txtSequence").val(obj.sequence);
    $("#LanguageCode").val(obj.languagecode);
   
    //if (obj.isActive == true) {
    //    $('#chkIsActive').click();
    //}
}


function deleteSubject(obj) {

    var subjectName = obj.subjectName;
    var subjectId = obj.id;

    $.confirmDelete(
        "Subject",
        subjectName,
        subjectId,
        function () {
            $.postData(
                URL.DELETESSUBJECT,
                { SubjectId: subjectId },
                function () {
                    clearFields();
                    setupSubjectList();
                });
        });
}
