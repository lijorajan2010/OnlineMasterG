// AJAX global variables
var m_ajaxLoadingStatus = false;
var m_ajaxLoadingHandler = null;

$(document).ready(function () {
    // AJAX global setup
    $.ajaxSetup({
        cache: false
    });

    $(document).ajaxStart(function () {
        m_ajaxLoadingStatus = true;
        m_ajaxLoadingHandler = setTimeout(function () {
            if (m_ajaxLoadingStatus)
                $("#divAjaxLoading").show();
        }, 3000);
    });

    // Error handling
    $(document).ajaxStop(function () {
        m_ajaxLoadingStatus = false;
        $("#divAjaxLoading").hide();
        clearTimeout(m_ajaxLoadingHandler);
    });

    $(document).ajaxError(function (event, jqxhr, settings, exception) {
        if (window.progressBarActive) {
            $("#mod-progress").modal("hide");
            window.progressBarActive = false;
        }

        if (jqxhr.responseText != undefined) {
            $.unblockUI();

            var msg = jqxhr.status === 500 ? exception + ", please contact the system administrator." : jqxhr.responseText;

            $("#divErrorHandlerBody").html(msg);
            $("#divErrorHandler").modal("show");

            // No
            $("#btnErrorHandlerClose").click(function () {
                $("#divErrorHandler").modal("hide");
            });
        }
    });

    // Timeout handling
    $(document).ajaxSuccess(function (event, jqxhr, settings, exception) {
        var loginHeader = jqxhr.getResponseHeader("X-LOGIN-PAGE");

        if (loginHeader == null)
            return;

        window.location.href = loginHeader;
    });

    // API call
    $.api = function (url, postData, callback) {
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(postData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: callback
        });
    }

    // Form post function
    $.postForm = function (form, postData, validationSuccess, validationError, callback) {

        // Perform client side alidations
        if (!form.valid()) {
            $('html, body').animate({
                scrollTop: $('.required.error:first').offset().top - 100
            });
            $('.required.error:first').focus();
            return;
        }

        $.blockUI();

        // Post form using AJAX
        $.ajax({
            type: "POST",
            url: form.attr("action"),
            traditional: true,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                // Redirect?
                if (data.Redirect != "" && data.Redirect != null)
                    window.location.replace(data.Redirect);

                $.unblockUI();

                // If validation message exist, display HTML
                if ($("#divValidationMsg") != null)
                    $("#divValidationMsg").html(data.ValidationHTML);

                if (data.Status) {
                    if (validationSuccess != null)
                        validationSuccess(data);
                } else {
                    if (validationError != null)
                        validationError(data);
                }

                if (callback != null)
                    callback(data);

            }
        });
    }

    // Data post function
    $.postData = function (url, postData, validationSuccess, validationError) {
        $.blockUI();

        // Post data using AJAX
        $.ajax({
            type: "POST",
            url: url,
            traditional: true,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                // Redirect?
                if (data.Redirect != "" && data.Redirect != null)
                    window.location.replace(data.Redirect);

                $.unblockUI();

                // If validation message exist, display HTML
                if ($("#divValidationMsg") != null)
                    $("#divValidationMsg").html(data.ValidationHTML);

                if (data.Status) {
                    if (validationSuccess != null)
                        validationSuccess(data);
                }
                else {
                    if (validationError != null)
                        validationError(data);
                }
            }
        });
    }

    // Quick post function (doesn't blocks UI, nor display validation messages)
    $.quickPost = function (url, postData, validationSuccess, validationError) {
        // Post data using AJAX
        $.ajax({
            type: "POST",
            url: url,
            traditional: true,
            data: JSON.stringify(postData),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.Status) {
                    if (validationSuccess != null) validationSuccess(data);
                }
                else {
                    if (validationError != null) validationError(data);
                }
            }
        });
    }

    $.getApi = function (url, data, callback) {
        $.ajax({
            url: url,
            type: 'GET',
            data: data,
            dataType: 'json',
            success: callback
        });
    };

    // Get ListDatatable function
    $.getDataTableList = function (url, divElement) {
        $.blockUI();
        $.get(url, function (data) {
            $(divElement).html('');
            $(divElement).html(data);
            $.unblockUI();
        });

        //// Post form using AJAX
        //$.ajax({
        //    type: "GET",
        //    url: url,
        //    traditional: true,
        //    //data: JSON.stringify(postData),
        //    contentType: 'application/json; charset=utf-8',
        //    success: function (data) {

        //    }
        //});
    };



    // Submit Form function
    $.submitForm = function (form, validationSuccess, validationError, callback, loadingElament,UploadbtnId) {

        // Perform client side alidations
        if (!form.valid()) {
            $('html, body').animate({
                scrollTop: $('.required.error:first').offset().top - 100
            });
            $('.required.error:first').focus();
            return;
        }
        if (UploadbtnId != null) {
            $(UploadbtnId).attr('disabled', true);
        }
        if (loadingElament != null) {
            $(loadingElament).show();
        }
        

        $.blockUI();

        $(form).ajaxForm(function (data) {
            // Redirect?
            if (data.Redirect != "" && data.Redirect != null)
                window.location.replace(data.Redirect);

            $.unblockUI();

            // If validation message exist, display HTML
            if ($("#divValidationMsg") != null)
                $("#divValidationMsg").html(data.ValidationHTML);

            if (data.Status) {
                if (validationSuccess != null)
                    validationSuccess(data);
            } else {
                if (validationError != null)
                    validationError(data);
            }

            if (callback != null)
                callback(data);

        });
    };
});