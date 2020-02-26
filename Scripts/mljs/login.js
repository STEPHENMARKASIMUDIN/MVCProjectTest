function onSubmitLoginBegin() {
    $(".loader").modal("show");
}

function onSubmitLoginSuccess(result) {
    $(".loader").modal("hide");
}

function onSubmitLoginFail(jqXHR, exception) {
    $(".loader").modal("hide");
    var errmsg = '';
    if (jqXHR.status === 0) {
        errmsg = 'Not connect.\n Verify Network.';
        $("#respmessage").html(errmsg);
    } else if (jqXHR.status == 404) {
        errmsg = 'Requested page not found. [404]';
        $("#respmessage").html(errmsg);
    } else if (jqXHR.status == 500) {
        errmsg = 'Internal Server Error [500].';
        $("#respmessage").html(errmsg);
    } else if (exception === 'parsererror') {
        errmsg = 'Requested JSON parse failed.';
        $("#respmessage").html(errmsg);
    } else if (exception === 'timeout') {
        errmsg = 'Time out error.';
        $("#respmessage").html(errmsg);
    } else if (exception === 'abort') {
        errmsg = 'Ajax request aborted.';
        $("#respmessage").html(errmsg);
    } else {
        errmsg = 'Uncaught Error.\n' + jqXHR.responseText;
        $("#respmessage").html(errmsg);
    }
    $(".respmodal").modal("show");
}