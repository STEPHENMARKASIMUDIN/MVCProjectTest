function onSubmitSaveBegin() {
    $(".my-modal").modal("show");
}

function onSubmitSaveSuccess(result) {
    console.log(result)
}

function onSubmitSaveFail(jqXHR, exception) {
    $(".my-modal").modal("hide");
    var errmsg = '';
    if (jqXHR.status === 0) {
        errmsg = 'Not connect.\n Verify Network.';
        $("#webErrorMSG").html(errmsg);
    } else if (jqXHR.status == 404) {
        errmsg = 'Requested page not found. [404]';
        $("#webErrorMSG").html(errmsg);
    } else if (jqXHR.status == 500) {
        errmsg = 'Internal Server Error [500].';
        $("#webErrorMSG").html(errmsg);
    } else if (exception === 'parsererror') {
        errmsg = 'Requested JSON parse failed.';
        $("#webErrorMSG").html(errmsg);
    } else if (exception === 'timeout') {
        errmsg = 'Time out error.';
        $("#webErrorMSG").html(errmsg);
    } else if (exception === 'abort') {
        errmsg = 'Ajax request aborted.';
        $("#webErrorMSG").html(errmsg);
    } else {
        errmsg = 'Uncaught Error.\n' + jqXHR.responseText;
        $("#webErrorMSG").html(errmsg);
    }
    $(".modal-Notification").modal("show");
}