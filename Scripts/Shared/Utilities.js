// Dropdowns
function fillList(listId, jsonData, dataTextField, dataValueField, firstItemText, firstItemValue) {
    var listItems = "";

    if (firstItemText != undefined && firstItemText != "")
        listItems += "<option value='" + firstItemValue + "'>" + firstItemText + "</option>";

    for (var i = 0; i < jsonData.length; i++)
        listItems += "<option value='" + jsonData[i][dataValueField] + "'>" + jsonData[i][dataTextField] + "</option>";

    $(listId).html(listItems);
    $(listId).enabled();
}

function appendList(listId, text, value)
{
	if (!optionExists(listId, value))
	{
		$(listId).append('<option value="' + value + '">' + text + '</option>');
	}
	else
	{
		$(listId).find("option[value="+ value +"]").text(text);
	}
};

function optionExists(listId, val)
{
	var exists = false;
	$(listId+' option').each(function ()
	{
		if (this.value === val.toString())
		{
			exists = true;
		}
	});

	return exists;
}

function clearList(listId) {
    $(listId).html("");
    $(listId).html("<option value''></option>");
    $(listId).disabled();
}

function updateList(listId, text, value) {
    $(listId).find('option[value="' + value + '"]').text(text);
};

// Mischelaneous
function reloadPage(block) {
    if (block == true)
        $.blockUI();

    window.location.href = window.location.href;
}

function redirect(url) {
    window.location.href = url;
}

// Formatting
function formatNumber(value) {
    var result = "";

    if (value != "") {
        result = $.format.number(value, "###,##0.00").toString();
    }
    else {
        result = "0.00";
    }

    return result;
}

function formatCurrency(value) {
    var result = "";

    if (value != "") {
        result = $.format.number(value, "###,##0.00").toString();
    }
    else {
        result = "0.00";
    }

    if (result.substring(0, 1) == "-")
        result = "($" + result.substring(1, result.length) + ")";
    else
        result = "$" + result;

    return result;
}

function formatPrct(value) {
    if (value != "")
        return $.format.number(value, "###,##0.00") + "%";
    else
        return "0%";
}

// Parsing numbers
function parseNumber(value) {
    return parseFloat(value.replace(",", ""));
}

function setHeartbeat() {
    setInterval(function () { heartbeat(); }, 10000);
};

function heartbeat() {
    $.api("/Global/HeartBeat");
}
function minDate()
{
	return "1/1/1900";
}

function maxDate()
{
	var d = new Date(Date.now() + 864e5);
	return (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
}

function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
}

function convertToValidLocaleCode(locale) {
    if (!locale) return;
    var localeArray = locale.split("-");
    localeArray[0] = localeArray[0].toLowerCase();
    localeArray[1] = localeArray[1].toUpperCase();
    var newLocale = localeArray.join("-");
    return localeArray.length === 2 ? newLocale : null;
}

function embedCountryCode(TextVal, CountryCode) {
    if (TextVal) {
        var outPaxPhone = "";
        var array = TextVal.split(",");
        $.each(array, function (i) {
            if (array[i] != null && array[i] != "") {
                if (array[i].charAt(0) != "+") {
                    outPaxPhone = $.trim(outPaxPhone.concat(',' + CountryCode.concat(array[i])));
                } else {
                    outPaxPhone = $.trim(outPaxPhone.concat(',' + array[i]));
                }
            }
        });
        if (outPaxPhone.charAt(0) == ",") {
            outPaxPhone = outPaxPhone.slice(1);
        }
        return outPaxPhone;
    }
}
function fileIsImage(mimeType) {
    var validImageTypes = ['image/gif', 'image/jpeg', 'image/png', 'image/jpg'];
    return $.inArray(mimeType, validImageTypes) > -1;
}

