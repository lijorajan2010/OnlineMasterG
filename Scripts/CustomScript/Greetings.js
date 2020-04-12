$(document).ready(function () {

    attachEvents();
    setupGreetingsList();
 
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
        saveGreetings();
        return false;
    });

    $("#btnClear").click(function () {
        clearFields();
        return false;
    });


}


function setupGreetingsList() {
    $.getDataTableList(URL.GREETINGSLIST, $("#dvGreetingsList")); 
}


function clearFields() {
    $("#uploadGreetings").val("");
}


function saveGreetings() {

    $.submitForm(
        $("#frmGreetings"),
        function (data) {
            clearFields();
            setupGreetingsList();
        });

    $("#frmGreetings").submit();
}



function deleteGreeting(obj) {

    var greetingId = obj.id;

    $.confirmDelete(
        "Greeting",
        "",
        greetingId,
        function () {
            $.postData(
                URL.DELETEGREETING,
                { GreetingId: greetingId },
                function () {
                    clearFields();
                    setupGreetingsList();
                });
        });
}
