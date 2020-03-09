$(document).ready(function () {

    $("#txtCategoryName").focus();
    attachEvents();
    setupCategoryList();
 
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
        saveCategory();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });
}

function setupCategoryList() {
    $.getDataTableList(URL.CATEGORYLIST, $("#dvCategoryList")); 
}


function clearFields() {
    $("#txtCategoryName").val("");
    $("#txtSequence").val("");
    $("#CourelistId").val("");
    $("#hdfCategoryId").val("");
    $('#chkIsActive').attr('checked', false);
    if ($("#chkIsActive").prop('checked')) {
        $('#chkIsActive').click();
    }
   
}


function saveCategory() {
    var model = {
        CourseId: $("#CourelistId").val(),
        CategoryId: $("#hdfCategoryId").val(),
        CategoryName: $("#txtCategoryName").val(),
        Sequence: $("#txtSequence").val(),
        LanguageCode: $("#LanguageCode").val(),
        IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmCategoryMaster"),
        model,
        function (data) {
            clearFields();
            setupCategoryList();
        });
}


function editCourse(obj) {
    $("#hdfCategoryId").val(obj.id);
    $("#CourelistId").val(obj.courseId);
    $("#txtCategoryName").val(obj.categoryName);
    $("#txtSequence").val(obj.sequence);
    $("#LanguageCode").val(obj.languagecode);
    if (obj.isActive == true) {
        $('#chkIsActive').click();
    }
}


function deleteCourse(obj) {

    var categoryName = obj.categoryName;
    var categoryId = obj.id;

    $.confirmDelete(
        "Category",
        categoryName,
        categoryId,
        function () {
            $.postData(
                URL.DELETECATEGORY,
                { CategoryId: categoryId },
                function () {
                    clearFields();
                    setupCategoryList();
                });
        });
}
