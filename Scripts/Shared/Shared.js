// Setup modes
var SETUP_MODE_NEW = "NEW";
var SETUP_MODE_EDIT = "EDIT";

// Setup screen helpers
function clearValidations() {
    $("#divValidationMsg").each(function (key, value) {
        $(this).children(".alert").hide();
    });
}

// Confirms
$.confirmDelete = function (elementType, elementName, elementId, onDelete) {
    $("#divDeleteDialog").appendTo("body");
    $("#divDeleteDialog").find("#lblElementType").text(elementType);
    $("#divDeleteDialog").find("#lblElementName").text(elementName);
    $("#divDeleteDialog").modal("show");
  
   

    // No
    $("#btnConfirmDeleteNO").off("click");
    $("#btnConfirmDeleteNO").click(function () {
        $("#divDeleteDialog").modal("hide");
    });

    // Yes
    $("#btnConfirmDeleteYES").off("click");
    $("#btnConfirmDeleteYES").click(function () {
        onDelete(elementId);

        $("#divDeleteDialog").modal("hide");
    });
};

// Confirms
$.confirm = function (message, onYes, onNo) {
    $("#divConfirmDialog").appendTo("body");
    $("#divConfirmDialog").find("#lblMessage").html(message);
    $("#divConfirmDialog").modal("show");

    // No
    $("#btnConfirmNO").off("click");
    $("#btnConfirmNO").click(function () {
        $("#divConfirmDialog").modal("hide");
        if (onNo != null) onNo();
    });

    // Yes
    $("#btnConfirmYES").off("click");
    $("#btnConfirmYES").click(function () {
        $("#divConfirmDialog").modal("hide");
        if (onYes != null) onYes();
    });
};

// Confirm Backdrop Static
$.confirmStatic = function (message, onYes, onNo, onClosed) {
    $("#divConfirmDialog").find("#lblMessage").html(message);
    $("#divConfirmDialog").modal({ backdrop: 'static' });

    // No
    $("#btnConfirmNO").off("click");
    $("#btnConfirmNO").click(function () {
        $("#divConfirmDialog").modal("hide");
        if (onNo != null) onNo();
    });

    // Yes
    $("#btnConfirmYES").off("click");
    $("#btnConfirmYES").click(function () {
        $("#divConfirmDialog").modal("hide");
        if (onYes != null) onYes();
    });

    // Closed
    $("#btnClosedDialog").off("click");
    $("#btnClosedDialog").click(function () {
        $("#divConfirmDialog").modal("hide");
        if (onYes != null) onClosed();
    });
};

// Alert
$.alert = function (title, message, onOk) {
    $("#divAlert").find("#lblAlertTitle").text(title);
    $("#divAlert").find("#lblAlertMessage").html(message);
    $("#divAlert").modal("show");

    // OK
    $("#btnAlertOK").off("click");
    $("#btnAlertOK").click(function () {
        $("#divAlert").modal("hide");
        if (onOk != null) onOk();
    });
};

// Popup dialog
$.popUp = function (url, postData, title, onSave, onClose) {

    $.blockUI();

    // Post data using AJAX
    $.ajax({
        type: "POST",
        url: url,
        traditional: true,
        data: JSON.stringify(postData),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $.unblockUI();

            $("#divPopUpTitle").html(title);
            $("#divPopUpHandlerBody").html(data);
            $("#divPopUpHandler").modal("show");

            // Close
            $("#btnPopUpHandlerClose").click(function () {
                $("#divPopUpHandler").modal("hide");
                $("#btnPopUpHandlerSave").hide();
                if (onClose && typeof (onClose) == 'function') {
                    onClose();
                }
            });

            // Save
            if (onSave) {
                $("#btnPopUpHandlerSave").off().show().on('click', this, onSave);
            } else {
                $("#btnPopUpHandlerSave").hide();
            }
        }
    });
};


$.popUpNew = function (url, postData, title) {
    // Post data using AJAX
    $.ajax({
        type: "POST",
        url: url,
        traditional: true,
        data: JSON.stringify(postData),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#divPopUpTitleNew").html(title);
            $("#divPopUpHandlerBodyNew").html(data);
            $("#divPopUpHandlerNew").modal("show");

            $.unblockUI();
            global.initPlugins('#divPopUpHandlerBody');

            // Close
            $("#btnPopUpHandlerCloseNew").click(function () {
                $("#divPopUpHandlerNew").modal("hide");
            });
        }
    });
};

// Download Data File
$.downloadDataFile = function (dataFileId) {
    $("#hdnDownloadDataFileId").val(dataFileId);
    $("#frmDataFileDownload").submit();
}

// Post form
jQuery(function ($) {
    $.extend({
        form: function (url, data, method) {
            if (method == null) method = 'POST';
            if (data == null) data = {};

            var form = $('<form>').attr({
                method: method,
                action: url
            }).css({
                display: 'none'
            });

            var addData = function (name, data) {
                if ($.isArray(data)) {
                    for (var i = 0; i < data.length; i++) {
                        var value = data[i];
                        addData(name + '[]', value);
                    }
                } else if (typeof data === 'object') {
                    for (var key in data) {
                        if (data.hasOwnProperty(key)) {
                            addData(name + '[' + key + ']', data[key]);
                        }
                    }
                } else if (data != null) {
                    form.append($('<input>').attr({
                        type: 'hidden',
                        name: String(name),
                        value: String(data)
                    }));
                }
            };

            for (var key in data) {
                if (data.hasOwnProperty(key)) {
                    addData(key, data[key]);
                }
            }

            return form.appendTo('body');
        }
    });
});

function insertText($txt, txtToAdd) {
    var caretPos = $txt[0].selectionStart;
    var textAreaTxt = $txt.val();
    if (txtToAdd) {
        $txt.val(textAreaTxt.substring(0, caretPos) + txtToAdd + textAreaTxt.substring(caretPos));
    }
    return false;
}

function getBUAgentModel(url) {
    $.blockUI();
    $.ajax({
        type: 'POST',
        url: url,
        traditional: true,
        contentType: 'application/json; charset=utf-8',
        async: true,
        success: function (data) {
            $.unblockUI();
            if (data !== null) {
                document.body.style.cursor = 'default';
                _globalConstant.agentBUModel = null;
                _globalConstant.agentBUModel = data;
                var dataBU = [];
                var dataAgent = [];
                var nullAgent = [];
                var nullBU = [];
                var uniqueBUNames = [];
                $('#ddlBusniessUnitAgent').multiselect('dataprovider', nullAgent);
                $('#ddlBusniessUnit').multiselect('dataprovider', nullBU);
                data.map(function (item) {
                    dataBU.push({ label: item.BUs, value: item.BUs });
                    dataAgent.push({ label: item.Agents, value: item.Agents });
                });
                var UniqueBUFilter = dataBU.map(item => item.value).filter((value, index, self) => self.indexOf(value) === index);
                $.each(UniqueBUFilter, function (i, el) {
                    uniqueBUNames.push({ label: el, value: el });
                });
                $('#ddlBusniessUnit').multiselect('dataprovider', uniqueBUNames);
                $('#ddlBusniessUnitAgent').multiselect('dataprovider', dataAgent);
                $('.input-group-btn').hide();
            }
            document.body.style.cursor = "default";
        }
    });
}

function setAgentByBU(lstBusinessUnits, agentBUModel) {
    var dataAgent = [];
    var model = agentBUModel;
    if (lstBusinessUnits) {
        lstBusinessUnits.forEach((item) => {
            var result = model.filter((e) => e.BUs === item);
            result.forEach((itemNew) => {
                dataAgent.push({ label: itemNew.Agents, value: itemNew.Agents });
            });
        });
    } else {
        model.forEach((itemNew) => {
            dataAgent.push({ label: itemNew.Agents, value: itemNew.Agents });
        });
    }
    $('#ddlBusniessUnitAgent').multiselect('dataprovider', dataAgent);
    $('.input-group-btn').hide();
}

function showTemplatePreview(modal, html, htmlPar, subjectText = null, subjectHolder = null) {
    htmlPar.html(html);
    modal.modal();
    if (subjectText && subjectHolder) {       
        subjectHolder.html('');
        subjectHolder.html(subjectText);
    }
    htmlPar.children('a').each(function (i, item) {
        item.setAttribute('style', 'pointer-events:none');
    });
}

function printTemplate(subject, htmlText) {
    var xCords = screen.width / 2 - 700 / 2;
    var yCords = screen.height / 2 - 600 / 2;
    var mywindow = window.open(window.location.href, 'PRINT', 'height=600,width=800,left=' + xCords + ',top=' + yCords);
    mywindow.document.write('<html><head><title>' + subject + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write('<h1>' + subject + '</h1><hr>');
    mywindow.document.write(htmlText);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    mywindow.print();
    mywindow.close();

    return true;
}

function datePickerYearSettings(e) {
    var keynum;
    if (window.event) { // IE                    
        keynum = e.keyCode;
    } else if (e.which) { // Netscape/Firefox/Opera                   
        keynum = e.which;
    }
    if (keynum !== 8) {
        var dt = $(e.target).val();
        if (dt !== "" && dt !== null && dt !== undefined) {
            // checking mm && dd && yy is only two characters which is yy is not yyyy
            var dateReg = /^\d{2}([./-])\d{2}\1\d{2}$/;
            if (dateReg.test(dt)) {
                var date = new Date(dt);
                var day = date.getDate();
                var month = date.getMonth();
                var year = date.getFullYear();
                if (!isNaN(day) && !isNaN(month) && !isNaN(year)) {
                    date = new Date(year, month, day, 0, 0, 0);
                    $(e.target).val(convertDate(date));
                }
            }
        }
    }
};

function convertDate(inputFormat) {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = new Date(inputFormat);
    return [pad(d.getMonth() + 1), pad(d.getDate()), d.getFullYear()].join('/');
}