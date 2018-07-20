
//main page js

$(function () {
    //$('#side_menu').metisMenu();
});


function AjaxSend(url, data, successCallback) {
    $.ajax({
        type: "POST",
        data: data,
        url: url,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: successCallback,
        error: function (err) {
            alert(err.responseText);
        }
    });
}

function ShowSuccessDialog(message, replaceURL) {
    BootstrapDialog.alert(message, function () { window.location.replace(replaceURL); });    
}