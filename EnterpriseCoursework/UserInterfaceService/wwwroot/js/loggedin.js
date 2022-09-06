function onLogin() {
    $.ajax(
        {
            type: "GET",
            url: "/User/GetUser",
            async: true,
            success: function (usermsg) {

                var userobj = JSON.parse(JSON.stringify(usermsg));
                if (userobj.status) {
                    $.ajax(
                        {
                            type: "GET",
                            url: "/ShareAlert/GetAlerts",
                            async: true,
                            data: { userId: userobj.msg },
                            success: function (alertmsg) {

                                var alertobj = JSON.parse(JSON.stringify(alertmsg));
                                if (alertobj.status) {
                                    document.getElementById("alertBody").innerHTML = "";
                                    for (var i = 0; i < alertobj.msgs.length; i++) {
                                        document.getElementById("alertBody").innerHTML += alertobj.msgs[i];
                                    }
                                    $('#alert-modal').modal('show');
                                }

                            }
                        });
                }

            }
        });
}

$(document).ready(function () {

    onLogin();

    //$('#alert-modal').on('click', '.alert-view-button', function (e) {

    //    $('#alert-modal').modal('hide');
    //});
});