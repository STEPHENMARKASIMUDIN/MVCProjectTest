function helpers() {
    var txtdate = document.getElementById("txtdate").value;
    var DateSplit = txtdate.split('/');
    var month = DateSplit[0];
    var monthWord;
    if (month === '01') {
        monthWord = "January";
    }
    else if (month === '02') {
        monthWord = "Febuary";
    }
    else if (month === '03') {
        monthWord = "March";
    }
    else if (month === '04') {
        monthWord = "April";
    }
    else if (month === '05') {
        monthWord = "May";
    }
    else if (month === '06') {
        monthWord = "June";
    }
    else if (month === '07') {
        monthWord = "July";
    }
    else if (month === '08') {
        monthWord = "August";
    }
    else if (month === '09') {
        monthWord = "September";
    }
    else if (month === '10') {
        monthWord = "October";
    }
    else if (month === '11') {
        monthWord = "November";
    }
    else if (month === '12') {
        monthWord = "December";
    }

    var finalDate = monthWord + " " + DateSplit[1] + "," + DateSplit[2];
    document.getElementById("txtdate").value = finalDate;

    document.getElementById('fileDownload').onclick = function () {
        var a = document.createElement('a');
        var downlink = document.getElementById('fileDownload').value;
        var downName = document.getElementById('fileDownload').name;
        a.href = "data:application/pdf;base64," + downlink;
        a.download = downName;

        if (!downlink === "" || !downlink === null) {
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }
    }
}