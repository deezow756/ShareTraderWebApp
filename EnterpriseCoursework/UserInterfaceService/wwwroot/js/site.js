
function pollChecks() {

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
                            url: "/Monitor/GetMonitors",
                            async: true,
                            data: { userId: userobj.msg },
                            success: function (monitormsg) {

                                var monitorobj = JSON.parse(JSON.stringify(monitormsg));
                                if (monitorobj.status) {
                                    document.getElementById("monitorBody").innerHTML = "";
                                    for (var i = 0; i < monitorobj.msgs.length; i++) {
                                        document.getElementById("monitorBody").innerHTML += monitorobj.msgs[i];
                                    }
                                    $('#monitor-modal').modal('show');
                                }

                            }
                        });
                }

            }
        });    

    setTimeout(pollChecks, 20000);
}

$(document).ready(function () {    

    pollChecks();

    //$('#monitor-modal').on('click', '.monitor-view-button', function (e) {

    //    $('#monitor-modal').modal('hide');
    //});
});