function onCreateBegin() {
    $(".loader").modal('show');
}

function onCreateSuccess(result) {
    if (result.status == 200) {
        $(".loader").modal('hide');
        $('#respmessage').html(result.msg);
        $('#respmodal').modal('show');

        $('#txtTransactionDate1').prop('disabled', true);
        $('#txtrefno1').prop('disabled', true);
        $('#txtTransactionDate2').prop('disabled', true);
        $('#txtrefno2').prop('disabled', true);
        $('#txtTransactionDate3').prop('disabled', true);
        $('#txtrefno3').prop('disabled', true);

        $('#form0')[0].reset();
    } else {
        $(".loader").modal("hide");
        $('#respmessage').html(result.msg);
        $('#respmodal').modal('show');
    }
}

function onCreateFail(jqXHR, exception) {
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
    $('.loader').modal('hide');
    $('#respmessage').html(errmsg);
    $('#respmodal').modal('show');
}

function checkBoxRequired() {
    var el = document.getElementsByClassName("checkingCheckBox");
    var atLeastOneChecked = false;
    for (var i = 0; i < el.length; i++) {
        if (el[i].checked === true) {
            atLeastOneChecked = true;
        }
    }

    if (atLeastOneChecked === true) {
        for (var i = 0; i < el.length; i++) {
            el[i].required = false;
        }
    }
    else {
        for (var i = 0; i < el.length; i++) {
            el[i].required = true;
        }
    }
}

$(document).ready(function () {
    $("#WrongAmount").on("click", function () {
        if ($('input#WrongAmount').is(':checked')) {
            $('#txtTransactionDate1').prop('disabled', false);
            $('#txtrefno1').prop('disabled', false);
        }
        else {
            $('#txtTransactionDate1').prop('disabled', true);
            $('#txtrefno1').prop('disabled', true);
            $('#txtTransactionDate1').val("");
            $('#txtrefno1').val("");
        }
    });

    $("#Erroneous").on("click", function () {
        if ($('input#Erroneous').is(':checked')) {
            $('#txtTransactionDate2').prop('disabled', false);
            $('#txtrefno2').prop('disabled', false);
        }
        else {
            $('#txtTransactionDate2').prop('disabled', true);
            $('#txtrefno2').prop('disabled', true);
            $('#txtTransactionDate2').val("");
            $('#txtrefno2').val("");
        }
    });

    $("#SystemError").on("click", function () {
        if ($('input#SystemError').is(':checked')) {
            $('#txtTransactionDate3').prop('disabled', false);
            $('#txtrefno3').prop('disabled', false);
        }
        else {
            $('#txtTransactionDate3').prop('disabled', true);
            $('#txtrefno3').prop('disabled', true);
            $('#txtTransactionDate3').val("");
            $('#txtrefno3').val("");
        }
    });

    $("#txtTransactionDate1").datepicker({
        format: 'mm/dd/yyyy',
        autoclose: true,
        endDate: "today"
    });
    $("#txtTransactionDate2").datepicker({
        format: 'mm/dd/yyyy',
        autoclose: true,
        endDate: "today"
    });
    $("#txtTransactionDate3").datepicker({
        format: 'mm/dd/yyyy',
        autoclose: true,
        endDate: "today"
    });

    var myfile = "";
    $('#attachment').on('change', function (event) {
        myfile = $(this).val();
        var myfilee = $(this).get(0).files[0];
        if (myfilee.type == 'application/pdf' || myfilee.type == 'application/msword' || myfilee.type == 'application/vnd.openxmlformats-officedocument.wordprocessingml.document') {
            var holder = $('#fileBase64');
            var base64;
            var reader = new FileReader();
            reader.onload = function (fileloadedevent) {
                base64 = fileloadedevent.target.result;
                console.log(base64);
                holder.val(base64);
            };

            reader.readAsDataURL(myfilee);

            console.log($('#fileBase64').val());
        }
        else {
            $(this).val('');
            alert("Invalid file. Attach pdf/doc file only.")
        }
    });

});

    