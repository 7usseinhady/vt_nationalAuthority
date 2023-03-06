function play() {
    var audio = new Audio("../soundNotification/sound.mp3");
    audio.play();
}
$(function () {
    // Declare a proxy to reference the hub.
    //connection to signal_R
    var notificaties = $.connection.notificationHub;
    //console.log('connection NotificationProcessStop.js = ', notificaties);
    //هنجيب عدد الناس الى لسه داخلين 
    notificaties.client.count_notificationProcessStop = function (count) { // اظهار عدد الاشعار
        //console.log('count_notificationProcess number', count);
        document.getElementById("idProcessStopNoifi").style.display = 'block';
        $('#idProcessStopNoifi').css({ 'visibility': 'visible', 'color': '#fff' }).text(count);
        play();
    }
    //اظهار الزائرين الى لسه داخلين
    $('#navbarDropdown').click(function () {
        //console.log('in notification_bell function');
        // اظهار الرساله بتاريخ فى الاشعار   
        $.get('/InsuranceEmployee/GetNotificationProcessStop', function (data) {
            //console.log('GetNotification data :- ', data);
        });
        $('#idProcessStopNoifi').css({ 'visibility': 'visible', 'color': 'WHITE' }).text('');
    });

    function getCookieValue(a) {
        var b = document.cookie.match('(^|;)\\s*' + a + '\\s*=\\s*([^;]+)');
        return b ? b.pop() : '';
    }
    $.connection.hub.start().done(function () {
        var kind = 'EmployeeProcessStop';
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