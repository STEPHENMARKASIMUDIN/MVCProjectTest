function onUpdateBegin() {
    $('.loader').modal('show');
}
function onUpdateSuccess(result) {
    $(".loader").modal("hide");
    if (result.status == 200) {
        $('#respmessage').html(result.msg);
        $('#respmodal').modal('show');
        $('#btnApprove').attr('disabled', 'disabled');
    }
    else {
        $('#respmessage').html(result.msg);
        $('#respmodal').modal('show');
    }
}
function onUpdateFail(jqXHR, exception) {
    var errmsg = '';
    if (jqXHR.status === 0) {
        errmsg = 'Not connected. \n Please verify your network connection.';
    } else if (jqXHR.status == 404) {
        errmsg = 'Requested page not found. [404]';
    } else if (jqXHR.status == 500) {
        errmsg = 'Internal Server Error [500].';
    } else if (exception === 'parsererror') {
        errmsg = 'Requested JSON parse failed.';
    } else if (exception === 'timeout') {
        errmsg = 'Time out error.';
    } else if (exception === 'abort') {
        errmsg = 'Ajax request aborted.';
    } else {
        errmsg = 'Uncaught Error.\n' + jqXHR.responseText;
    }
    $(".loader").modal('hide');
    $('#respmessage').html(errmsg);
    $('#respmodal').modal('show');
}