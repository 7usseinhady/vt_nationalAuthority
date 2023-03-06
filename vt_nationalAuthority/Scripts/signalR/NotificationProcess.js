const urlParams = new URLSearchParams(window.location.search).toString();
var screen = ".." + location.pathname + "?" + urlParams;

$(function () {

    // Declare a proxy to reference the hub.
    //connection to signal_R
    var notificaties = $.connection.notificationHub;
    notificaties.client.count_notificationProcess = function (count) { // اظهار عدد الاشعار
        $('#notificationNumberEmployee').css({ 'visibility': 'visible', 'color': '#fff', 'display': 'block' }).text(count);
        play();
    }

    //// هنا ظهور الاشعارات لما يدوس على الجرس 
    $('#navbarDropdownEmp').click(function () {
        // اظهار الرساله بتاريخ فى الاشعار   
        $("#notification_divEmployee").empty();
        $.get('/jsonFunctions/GetNotification', function (data) {
            if (data.length != 0) {
                $.each(data, function (index) {
                    if (data[index].seen == "False") {
                        $("#notification_divEmployee").empty();
                    }
                    else {
                        //TypeNotfication Notes for request process
                        var insert_date = convert_date_in_Jason(data[index].dateInsert);
                        //request process
                        if (data[index].TypeNotfication == 4) {
                            //$("#notification_divEmployee").append("<a class='card text-right w-100 unread_color'  href='/Process/vProcessRequest?pCode=45549' ><div class='card-body d-flex' ><div class='col-12 px-1 scroll-max' style='margin-bottom:-10px !important;'><div><span class='text_color102 font_size_s fontar w-100'></span> <span style='font-weight: bold;color: #9E9E9E !important; display: flex'> " + data[index].Message + "</span>  <span style='font-weight: bold; color:#027902d1'>" + data[index].processName + "</span><span class='text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + insert_date + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div> </div></div></a>");
                            $("#notification_divEmployee").append("<a class='card text-right w-100 unread_color' href='../Process/vProcessRequest?cp=1&cd=3&type=9&pCode=" + data[index].processCode + "' ><div class='card-body col-12 d-flex' ><div class='col-12 px-1 scroll-max' style='margin-bottom:-10px !important;' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100'></span> <span style='font-weight: bold;color:BLACK !important; display: flex'> " + data[index].Message + "</span><span style='font-weight: bold; color:#027902d1'>" + data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div> </div></div></a>");
                        }
                            // process Opposition
                        else if (data[index].TypeNotfication == 5) {
                            $("#notification_divEmployee").append("<a class='card text-right w-100 unread_color'  href='../Process/vProcessShowIndex?cp=1&cd=2&type=10&pCode=" + data[index].processCode + "' ><div class='card-body col-12 d-flex' ><div class='col-12 px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100'></span> <span style='font-weight: bold;color: BLACK !important; display: flex'> " + data[index].Message + "</span><span style='font-weight: bold; color:#dc3545'>" + data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div> </div></div></a>");
                        }
                            //process Stop
                        else if (data[index].TypeNotfication == 6) {
                            $("#notification_divEmployee").append("<a class='card text-right w-100 unread_color' href='../ExpireProcess/vExpireProcess?cp=1&cd=4&type=12&pCode=" + data[index].processCode + "' ><div class='card-body col-12 d-flex' ><div class='col-12 px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100'></span> <span style='font-weight: bold;color: BLACK !important; display: flex'> " + data[index].Message + "</span><span style='font-weight: bold; color:#012fb1bd'>" + data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div> </div></div></a>");
                        }
                            //Add subContractor
                        else if (data[index].TypeNotfication == 9) {
                            $("#notification_divEmployee").append("<a class='card text-right w-100 unread_color'  href='../Process/vProcessShowIndex?cp=1&cd=2&type=11&pCode=" + data[index].processCode + "'><div class='card-body col-12 d-flex'><div class='col-12 px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'> لقد تم اضافة المقاول من الباطن : </span> <span style='font-weight: bold; color:rgb(223 72 154 / 95%)'> " + data[index].Message + "</span><br><span style='color:black;font-weight: bold;'> , في عمليه : </span><span style='font-weight: bold; color:#027902d1'>" +
                               data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div class='col-12 text-right'><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div> </div></div></a>");
                        }
                        else if (data[index].TypeNotfication == 12) //attachments , process
                        {
                            $("#notification_divEmployee").append(" <a class='card text-right w-100 unread_color' href='../Process/vProcessShowIndex?cp=1&cd=2&type=8&pCode=" + data[index].processCode + "'><div class='card-body col-12 d-flex'><div class='col-12 px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'> " + data[index].Message + "  : </span><span  style='font-weight: bold;color: rgb(16 48 83 / 89%)'>" + data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div>    </div></div></a>");
                        }
                        else if (data[index].TypeNotfication == 13) //attachments , process request
                        {
                            $("#notification_divEmployee").append(" <a class='card text-right w-100 unread_color' href='../Process/vProcessRequest?cp=1&cd=3&type=13&pCode=" + data[index].processCode + "'><div class='card-body col-12 d-flex'><div class='col-12 px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'> " + data[index].Message + "  : </span><span  style='font-weight: bold;color: rgb(16 48 83 / 89%)'>" + data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div>    </div></div></a>");
                        }
                        else if (data[index].TypeNotfication == 14) //attachments , process stop
                        {
                            $("#notification_divEmployee").append(" <a class='card text-right w-100 unread_color' href='../ExpireProcess/vExpireProcess?cp=1&cd=4&type=14&pCode=" + data[index].processCode + "'><div class='card-body col-12 d-flex'><div class='col-12 px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'> " + data[index].Message + "  : </span><span  style='font-weight: bold;color: rgb(16 48 83 / 89%)'>" + data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div>    </div></div></a>");
                        }
                        else if (data[index].TypeNotfication == 15) //attachments , worker
                        {
                            $("#notification_divEmployee").append(" <a class='card text-right w-100 unread_color' href='../InsuranceEmployee/vpWorkers?cp=1&cd=5&workerCode=" + data[index].processCode + "'><div class='card-body col-12 d-flex'><div class='col-12 px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'> " + data[index].Message + "  : </span><span  style='font-weight: bold;color: rgb(16 48 83 / 89%)'>" + data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div>    </div></div></a>");
                        }
                        else if (data[index].TypeNotfication == 16) //attachments , card request
                        {
                            $("#notification_divEmployee").append(" <a class='card text-right w-100 unread_color' href='#'><div class='card-body col-12 d-flex'><div class='col-12 px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100' style='COLOR: BLACK; font-weight: bold;'> " + data[index].Message + "  : </span><span  style='font-weight: bold;color: rgb(16 48 83 / 89%)'>" + data[index].processName + "</span><br><span class='col-12 text-left text_color104 px-2' style='font-size:12px; color: #9E9E9E;display: inline-block;'>تاريخ الارسال: " + data[index].time + "&nbsp; <span>" + data[index].dateInsert + "</span></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div>    </div></div></a>");
                        }
                        else if (data[index].TypeNotfication == 17) // Change Password
                        {

                            $("#notification_divEmployee").append('<a class="card text-right w-100 unread_color" href="#"><button type="button" name="submitType" style="COLOR: red;font-weight: bold;border: none;outline:none;" class="font_size_s fontar text_color102 w-100" data-toggle="modal" data-target="#sharedModal" ' +
                                                          'onclick="openModel___ss(\'#titleSharedModal\', \'تغيير كلمه المرور \', \'#btnChangePassword\',\' تغيير كلمه المرور\', \'far fa-edit\', \'btn btn-outline-warning font-weight-bold btnsStyle\', \'../Home/_ChangePassword\', \'?screen=' + screen + '\', \'#sharedModalBody\')"  data-toggle="modal" data-target="#sharedModal"><div class="card-body col-12 d-flex"><div class="col-12 px-1 scroll-max" style="margin-bottom:-10px !important;"><div class="col-12 text-center">' + data[index].Message + '</div><div><span class="text_color102 fontar w-100 text-muted" style="font-size:11px;"></span></div></div></div></button></a>');
                        }
                        else {
                            $("#notification_divEmployee").append(" <a class='card text-right w-100 unread_color'><div class='card-body col-12 d-flex'><div class='col-12 px-1 scroll-max' style='margin-bottom:-10px !important;'><div class='col-12 text-right'><span class='text_color102 font_size_s fontar w-100'>ليس ليدك اشعار في الوقت الحالى</span><span class='text_color104 px-2' style='font-size:12px; color: #9E9E9E'></span></div><div><span class='text_color102 fontar w-100 text-muted' style='font-size:11px;'></span></div></div></div></a>");
                            //$("#notification_div1").append("<div class='alert alert-info'>ليس ليدك اشعار في الوقت الحالى: </div>");
                        }
                    }
                })
                document.getElementById("notificationNumberEmployee").style.display = 'none';
            }
        });
        $('#notificationNumberEmployee').css({ 'visibility': 'visible', 'color': 'WHITE' }).text('');
    });

    $.connection.hub.start().done(function () {
        var kind = 'EmployeeProcess';
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