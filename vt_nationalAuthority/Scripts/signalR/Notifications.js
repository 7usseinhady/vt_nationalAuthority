var screen = ".." + location.pathname;

$(function () {
    // Declare a proxy to reference the hub.
    //connection to signal_R
    var notificaties = $.connection.notificationHub;
    notificaties.client.count_notification = function (count) { // اظهار عدد الاشعار

        $('#notificationNumber').css({ 'visibility': 'visible', 'color': '#fff', 'display': 'block' }).text(count);
        play();
    }
    $('#navbarDropdown').click(function () {
        // اظهار الرساله بتاريخ فى الاشعار   
        $("#notification_div1").empty();
        $.get('/jsonFunctions/GetNotification', function (data) {

            if (data.length != 0) {
                $.each(data, function (index) {


                    if (data[index].seen == "False") {
                        $("#notification_div1").empty();
                    }
                    else {
                        //TypeNotfication
                        var insert_date = convert_date_in_Jason(data[index].dateInsert);
                        if (data[index].TypeNotfication == 0) //Notes for employee
                        {
                            $("#notification_div1").append("<a class='card w-100 unread_color' href='../Process/_vpProcessIndexShow?cp=1&type=3&pCode=" + data[index].processCode + "'><div class='card-body col-12 d-flex'><div class='col-12  px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'> لقد تم اضافة ملحوظه : </span> <br><span style='font-weight: bold; color:#027902d1'> " + data[index].Message + "</span><span style='color:black;font-weight: bold;'> , في عمليه : </span><span style='font-weight: bold; color:#027902d1'>" +
                                  data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp;<span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div> </div></div></a>");
                        }
                        else if (data[index].TypeNotfication == 1) //contractors main
                        {
                            $("#notification_div1").append(" <a class='card w-100 unread_color' href='../Process/_vpProcessIndexShow?cp=1&type=4&pCode=" + data[index].processCode + "'><div class='card-body col-12 d-flex'><div class='col-12  px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'>لقد تم اضافتك كمقاول رئيسي في العمليه :  </span><br>"
                                + "<span style='font-weight: bold;color: cadetblue'>" + data[index].Message + " </span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span> </span> </div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div></div></div></a>");
                        }
                        else if (data[index].TypeNotfication == 8) //contractors sub
                        {
                            $("#notification_div1").append(" <a class='card w-100 unread_color' href='../Process/_vpProcessIndexShow?cp=1&type=4&pCode=" + data[index].processCode + "'><div class='card-body col-12 d-flex'><div class='col-12  px-1  scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'>لقد تم اضافتك كمقاول من الباطن في العمليه :  </span><br>"
                                + "<span style='font-weight: bold;color: #d58b17f2'>" + data[index].Message + " </span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span> </span> </div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div></div></div></a>");
                        }
                        else if (data[index].TypeNotfication == 10) //accept sub contractor
                        {
                            $("#notification_div1").append("<a class='card w-100 unread_color' href='../Process/_vpProcessIndexShow?cp=1&type=2&pCode=" + data[index].processCode + "'><div class='card-body col-12 d-flex'><div class='col-12  px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'> لقد تم الموافقه على المقاول من الباطن : </span><br> <span style='font-weight: bold; color:rgb(0 123 255 / 97%)'> " + data[index].Message + "</span><span style='color:black;font-weight: bold;'> , في عمليه : </span> <span style='font-weight: bold; color:#027902d1'>" +
                                 data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div> </div></div></a>");

                        }
                        else if (data[index].TypeNotfication == 2)// refernce side
                        {
                            $("#notification_div1").append(" <a class='card w-100 unread_color' href='../Process/_vpProcessIndexShow?cp=1&type=5&pCode=" + data[index].processCode + "'><div class='card-body col-12 d-flex'><div class='col-12  px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'>لقد تم اضافتك  كجهة اسناد فى العمليه : </span><br>"
                                + "<span style='font-weight: bold;color: #012fb1bd'>" + data[index].Message + " </span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div>    </div></div></a>");
                        }
                        else if (data[index].TypeNotfication == 3) //accept process
                        {
                            $("#notification_div1").append(" <a class='card w-100 unread_color' href='../Process/_vpProcessIndexShow?cp=1&type=6&pCode=" + data[index].processCode + "'><div class='card-body col-12 d-flex'><div class='col-12  px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'> " + data[index].Message + "  : </span><br><span  style='font-weight: bold;color: #cc1b34e0'>" + data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div>    </div></div></a>");
                        }
                        else if (data[index].TypeNotfication == 7)// add process user
                        {
                            $("#notification_div1").append(" <a class='card w-100 unread_color' href='../Process/_vpProcessIndexShow?cp=1&type=7&pCode=" + data[index].processCode + "'><div class='card-body col-12 d-flex'><div class='col-12  px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'> " + data[index].Message + "  : </span><br><span  style='font-weight: bold;color: #712db9'>" + data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div>    </div></div></a>");
                        }
                        else if (data[index].TypeNotfication == 11) //attachments
                        {
                            $("#notification_div1").append(" <a class='card w-100 unread_color' href='../Process/_vpProcessIndexShow?cp=1&type=1&pCode=" + data[index].processCode + "'><div class='card-body col-12 d-flex'><div class='col-12  px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'> " + data[index].Message + "  : </span><br><span  style='font-weight: bold;color: rgb(16 48 83 / 89%)'>" + data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div>    </div></div></a>");
                        }
                        else if (data[index].TypeNotfication == 17) // Change Password
                        {
                            $("#notification_div1 ").append('<a class="card text-right w-100 unread_color" href="#"><button type="button" name="submitType" style="COLOR: red;font-weight: bold;border: none;outline:none;" class="font_size_s fontar text_color102 w-100" data-toggle="modal" data-target="#sharedModal" ' +
                                                               'onclick="openModel___ss(\'#titleSharedModal\', \'تغيير كلمه المرور \', \'#btnChangePassword\',\' تغيير كلمه المرور\', \'far fa-edit\', \'btn btn-outline-warning font-weight-bold btnsStyle\', \'../Home/_ChangePassword\', \'?screen=' + screen + '\', \'#sharedModalBody\')"  data-toggle="modal" data-target="#sharedModal"><div class="card-body col-12 d-flex"><div class="col-12 px-1 scroll-max" style="margin-bottom:-10px !important;"><div class="col-12 text-center">' + data[index].Message + '</div><div><span class="text_color102 fontar w-100 text-muted" style="font-size:11px;"></span></div></div></div></button></a>');

                        }
                        else {
                            $("#notification_div1").append(" <a class='card w-100 unread_color'><div class='card-body col-12 d-flex'><div class='col-12  px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100'>ليس ليدك اشعار في الوقت الحالى</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E'></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div></div></div></a>");
                            //$("#notification_div1").append("<div class='alert alert-info'>ليس ليدك اشعار في الوقت الحالى: </div>");
                        }
                    }
                })
                document.getElementById("notificationNumber").style.display = 'none';
            }
        });
        $('#notificationNumber').css({ 'visibility': 'visible', 'color': 'WHITE' }).text('');
    });
    function getCookieValue(a) {
        var b = document.cookie.match('(^|;)\\s*' + a + '\\s*=\\s*([^;]+)');
        return b ? b.pop() : '';
    }
    $.connection.hub.start().done(function () {
        var kind = 'Contractor';
        var un = getCookieValue("uc");
        notificaties.server.sendNotification(un, kind);
    }).fail(function (e) {
        alert(e);
    });
});
function convert_date_in_Jason(str) {
    if (str != null) {
        var x = [{ "id": 1, "start": str }];
        var myDate = new Date(x[0].start.match(/\d+/)[0] * 1);
        var date = new Date(myDate), mnth = ("0" + (date.getMonth() + 1)).slice(-2), day = ("0" + date.getDate()).slice(-2);
        return [date.getFullYear(), mnth, day].join("-");
    }
    else
        return 'لم يتم ادخال الوقت';
}