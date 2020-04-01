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
        saveCurrentAffairsCategory();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });
}


function setupCategoryList() {
    $.getDataTableList(URL.CURRENTAFFAIRSCATEGORYLIST, $("#tblCurrentAffairsCategoryList")); 
}


function clearFields() {
    $("#txtCategoryName").val("");
    $("#txtSequence").val("");
    $("#hdfCategoryId").val("");
    $('#chkIsActive').attr('checked', false);
    if ($("#chkIsActive").prop('checked')) {
        $('#chkIsActive').click();
    }
   
}


function saveCurrentAffairsCategory() {
    var model = {
        CurrentAffairsCategoryId: $("#hdfCategoryId").val(),
        AffairsCategoryName: $("#txtCategoryName").val(),
        Sequence: $("#txtSequence").val(),
        LanguageCode: $("#LanguageCode").val(),
        IsActive: $("#chkIsActive").prop('checked')
    };

    $.postForm(
        $("#frmCurrentAffairsCategory"),
        model,
        function (data) {
            clearFields();
            setupCategoryList();
        });
}


function editCategory(obj) {

    $("#hdfCategoryId").val(obj.id);
    $("#txtCategoryName").val(obj.categoryName);
    $("#txtSequence").val(obj.sequence);
    $("#LanguageCode").val(obj.languagecode);
    if (obj.isActive == true) {
        $('#chkIsActive').click();
    }
}


function deleteCategory(obj) {

    var categoryName = obj.categoryName;
    var categoryId = obj.id;

    $.confirmDelete(
        "Category",
        categoryName,
        categoryId,
        function (categoryId) {
            $.postData(
                URL.DELETECURRENTAFFAIRSCATEGORY,
                { categoryId: categoryId },
                function () {
                    clearFields();
                    setupCategoryList();
                });
        });
}
