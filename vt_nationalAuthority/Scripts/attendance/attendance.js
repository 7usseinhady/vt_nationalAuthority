var monthNames = ["يناير", "فبراير", "مارس", "ابريل", "مايو", "يونيو", "يوليو", "اغسطس", "سبتمبر", "اكتوبر", "نوفمبر", "ديسمبر"];
var dayNames = ["احد", "اثنين", "ثلاثاء", "اربعاء", "خميس", "جمعه", "سبت"];
var currentYear = new Date().getFullYear();
var currentMonth = new Date().getMonth();
var currDay = new Date().getDate();
var curDate = new Date(currentYear, currentMonth, currDay);

var dataScreen = $('#calenderContent').attr('data-screen');

createMonthsYear(currentYear, 'center');

function styleMonths() {
    var year = $('#btnYears').text();
    var activeMonth = $('.twelveMonth.active').attr('id');

    // الحضور الخاص بالعامل
    // سنين بعد السنه اللى انا فيها
    if (year > currentYear)// 2021 > 2020
        $('.twelveMonth').addClass('monthDisabled');
    else if (year < currentYear) // سنين سابقة
        $('.twelveMonth').addClass('monthPrevious');
    else { // السنه الحالية
        $('.twelveMonth.active').prevAll().addClass('monthPrevious');
        $('.twelveMonth.active').nextAll().addClass('monthDisabled');
    }
}

function styleTenYears() {

    // الحضور الخاص بالعامل
    if (dataScreen == "periodMonth") {

        $('.tenYears').each(function () {
            if ($(this).attr('id') > currentYear)
                $(this).addClass('monthDisabled');
            else if ($(this).attr('id') < currentYear)
                $(this).addClass('monthPrevious');
            else
                $(this).addClass('active');
        });
    }
        // التحضير
    else if (dataScreen == "attendance") {

        $('.tenYears').each(function () {
            if ($(this).attr('id') > currentYear)
                $(this).addClass('monthDisabled');
            else if ($(this).attr('id') < currentYear)
                $(this).addClass('monthPrevious');
            else
                $(this).addClass('active');
        });
    }
}

function createMonthsYear(year, animate) {

    var calendarList = document.createElement("div");

    // التحضير
    if (dataScreen == "attendance") {
        if (animate == "left")
            calendarList.className = 'col-12 col-lg-6 col-md-10 col-sm-12 container divMonthYear animated bounceInLeft';
        else if (animate == "right")
            calendarList.className = 'col-12 col-lg-6 col-md-10 col-sm-12 container divMonthYear animated bounceInRight';
        else // center
            calendarList.className = 'col-12 col-lg-6 col-md-10 col-sm-12 container divMonthYear animated zoomIn';
    }
    else if (dataScreen == "periodMonth") // الحضور الخاص بالعامل
    {
        if (animate == "left")
            calendarList.className = 'divMonthYear animated bounceInLeft';
        else if (animate == "right")
            calendarList.className = 'divMonthYear animated bounceInRight';
        else // center
            calendarList.className = 'divMonthYear animated zoomIn';
    }

    for (var i = 1; i <= 12 ; i++) {
        var yearBtn = document.createElement('button');
        yearBtn.id = i;
        yearBtn.setAttribute('type', 'button');

        if (year == currentYear) {
            if ((i - 1) == currentMonth)
                yearBtn.className = 'btn btn-defualt twelveMonth currentMonth active';
            else if ((i - 1) < currentMonth)
                yearBtn.className = 'btn btn-defualt twelveMonth monthPrevious';
            else
                yearBtn.className = 'btn btn-defualt twelveMonth monthDisabled';
        }
        else if (year > currentYear)
            yearBtn.className = 'btn btn-defualt twelveMonth monthDisabled';
        else
            yearBtn.className = 'btn btn-defualt twelveMonth monthPrevious';

        var yearOf = document.createTextNode(monthNames[i - 1]);

        // التحضير
        if (dataScreen == "attendance")
            yearBtn.setAttribute('onclick', 'specialMonth(' + i + ')');
        else if (dataScreen == "periodMonth") // الحضور الخاص بالعامل
            yearBtn.setAttribute('onclick', 'openDivWorkers(this,"#divAttendanceMonthDays","../previousPeriod/_vpWorkerDays", "year=' + year + ',Month=' + i + '", "#displayDaysForWorker")');

        yearBtn.appendChild(yearOf);
        calendarList.appendChild(yearBtn);
    }

    var divTwelveMonth = document.querySelector('#divTwelveMonth');
    divTwelveMonth.appendChild(calendarList);
}

function specialMonth(month) {

    var year = $('#btnYears').text();

    $('#btnYears').text('');

    var yearBtn = document.querySelector('#btnYears');

    var yearOf = document.createTextNode(year);
    var monthSup = document.createElement("sup");
    monthSup.className = "superMonth";
    var monthOf = document.createTextNode(monthNames[month - 1]);

    monthOf = document.createTextNode(monthNames[month - 1]);
    monthSup.appendChild(monthOf);
    yearBtn.appendChild(monthSup);
    yearBtn.appendChild(yearOf);

    bindDaysTable(month, year, 'center');
    $('#divTwelveMonth').css('display', 'none');
}

function addElem(elementType, elemID, elemClass, appendTarget) {
    appendTarget.innerHTML += "<" + elementType + " id= " + elemID + " class=" + elemClass + "> </" + elementType + ">";
}

function createElement(elementType, elemID, elemClass, text, appendTarget) {
    var elemCreate = document.createElement(elementType);
    elemCreate.id = elemID;
    elemCreate.className = elemClass;
    var elemText = document.createTextNode(text);
    elemCreate.appendChild(elemText);
    appendTarget.appendChild(elemCreate);
}

function bindDaysTable(month, year, animate) {

    // days month
    var theDate = new Date(year, month - 1);
    var currentDate = new DateObject(theDate);

    var calendarTable = document.createElement("table");
    calendarTable.id = "tblDays";

    // #tblDays
    if (animate == "left")
        calendarTable.className = 'month animated bounceInLeft';
    else if (animate == "right")
        calendarTable.className = 'month animated bounceInRight';
    else
        calendarTable.className = 'month animated zoomIn ';

    // Thead - tr
    var calendarTableThead = document.createElement("thead");
    var calendarTableTr = document.createElement("tr");
    calendarTableTr = document.createElement("tr");

    for (var i = 1; i <= 7 ; i++) {
        var calendarTableTh = document.createElement("th");
        calendarTableTh.className = "day-header";
        var headText = document.createTextNode(dayNames[i - 1]);
        calendarTableTh.appendChild(headText);
        calendarTableTr.appendChild(calendarTableTh);
    }

    calendarTableThead.appendChild(calendarTableTr);
    calendarTable.appendChild(calendarTableThead);

    var dayMonths = 1;

    // Tbody
    var calendarTableTbody = document.createElement("tbody");
    for (var i = 1; i <= 6 ; i++) {

        calendarTableTr = document.createElement("tr");

        for (var j = 0 ; j < 7 ; j++) {

            var calendarTableTd = document.createElement("td");
            calendarTableTd.className = "m-1 day";

            // first loop
            if (i == 1) {

                // ايام  تبع الشهر اللى فات
                if (currentDate.dayNameNumber > j) {
                    calendarTableTr.appendChild(calendarTableTd);
                }
                else {
                    var divRow = document.createElement("button");
                    divRow.type = "button";

                    var day = new Date(year, month - 1, dayMonths);

                    if (day > curDate) {// disabled
                        divRow.className = "btn day-content disabledDay";
                    }
                    else if (day < curDate) { // previousDay
                        divRow.className = "btn day-content previousDay";
                    }
                    else {
                        divRow.className = "btn day-content currentDay active";
                    }

                    divRow.setAttribute('onclick', 'openDivWorkers(this,"#divWorks","../Workers/_vpWorkerAttendance", "year=' + year + ',month=' + month + ',day=' + dayMonths + '", "#displayWorkers")');


                    if (dayMonths < 10)
                        dayMonths = "0" + dayMonths;

                    headText = document.createTextNode(dayMonths);
                    divRow.appendChild(headText);

                    calendarTableTd.appendChild(divRow);
                    calendarTableTr.appendChild(calendarTableTd);

                    dayMonths++;
                }
            }
            else {
                var divRow = document.createElement("button");
                divRow.type = "button";

                var day = new Date(year, month - 1, dayMonths);

                if (day > curDate) {// disabled
                    divRow.className = "btn day-content disabledDay";
                }
                else if (day < curDate) { // previousDay
                    divRow.className = "btn day-content previousDay";
                }
                else {
                    divRow.className = "btn day-content currentDay active";
                }

                divRow.setAttribute('onclick', 'openDivWorkers(this,"#divWorks","../Workers/_vpWorkerAttendance", "year=' + year + ',month=' + month + ',day=' + dayMonths + '", "#displayWorkers")');

                if (dayMonths < 10)
                    dayMonths = "0" + dayMonths;
                headText = document.createTextNode(dayMonths);
                divRow.appendChild(headText);

                calendarTableTd.appendChild(divRow);
                calendarTableTr.appendChild(calendarTableTd);

                dayMonths++;
            }

            if (dayMonths > currentDate.daysInMonth) {
                break;
            }
        }

        calendarTableTbody.appendChild(calendarTableTr);

        if (dayMonths > currentDate.daysInMonth) {
            break;
        }
    }

    calendarTable.appendChild(calendarTableTbody);

    var divDaysMonth = document.querySelector('#divDaysMonth');
    divDaysMonth.appendChild(calendarTable);
}

function bindDays(month, year, animate) {

    // days month
    var theDate = new Date(year, month - 1);
    var currentDate = new DateObject(theDate);

    var calendarTable = document.createElement("table");
    calendarTable.id = "tblDays";

    // #tblDays
    if (animate == "left")
        calendarTable.className = 'justify-content-lg-start row month animated bounceInLeft';
    else if (animate == "right")
        calendarTable.className = 'justify-content-lg-start row month animated bounceInRight';
    else
        calendarTable.className = 'justify-content-lg-start row month animated zoomIn ';

    var calendarTableThead = document.createElement("thead");
    var calendarTableTr = document.createElement("tr");
    var calendarTableTh = document.createElement("th");

    calendarTableTr.appendChild(calendarTableTh);
    calendarTableThead.appendChild(calendarTableTr);
    calendarTable.appendChild(calendarTableThead);

    var divDaysMonth = document.querySelector('#divDaysMonth');
    divDaysMonth.appendChild(calendarTable);
}

function bindLi(currentDate, liClass, i, tooltipclass, afterBefore, calendarList) {

    var calendarLi = document.createElement("li");
    calendarLi.id = "day_" + i;
    calendarLi.className = liClass;

    var toolTipText = "عدد العمال : 1200";

    createElementTooltip('span', tooltipclass, toolTipText, calendarLi, afterBefore);

    var dayData = new Date(currentDate.theYear, currentDate.theMonth - 1, i);
    dayData = new DateObject(dayData);

    var elemText;
    if (i < 10)
        elemText = document.createTextNode("0" + dayData.theDay); // 01 / 02 / 03 / ... / 09
    else
        elemText = document.createTextNode(dayData.theDay); // 10 / 11 / 12 / ... / 31

    calendarLi.appendChild(elemText);
    calendarList.appendChild(calendarLi);
}

function bindDays_(month, year, animate) {

    // days month
    var theDate = new Date(year, month - 1);
    var currentDate = new DateObject(theDate);

    var calendarList = document.createElement("ul");

    if (animate == "left")
        calendarList.className = 'justify-content-lg-start row ulDaysMonth animated bounceInLeft';
    else if (animate == "right")
        calendarList.className = 'justify-content-lg-start row ulDaysMonth animated bounceInRight';
    else
        calendarList.className = 'justify-content-lg-start row ulDaysMonth animated zoomIn ';

    for (var i = 1; i <= currentDate.daysInMonth ; i++)
        bindLi_(currentDate, 'calendarLi', i, '_tooltip', '._tooltip::after, ._tooltip::before', calendarList);

    var divDaysMonth = document.querySelector('#divDaysMonth');
    divDaysMonth.appendChild(calendarList);
}

function bindLi_(currentDate, liClass, i, tooltipclass, afterBefore, calendarList) {

    var calendarLi = document.createElement("li");
    calendarLi.id = "day_" + i;
    calendarLi.className = liClass;

    var toolTipText = "عدد العمال : 1200";

    createElementTooltip('span', tooltipclass, toolTipText, calendarLi, afterBefore);

    var dayData = new Date(currentDate.theYear, currentDate.theMonth - 1, i);
    dayData = new DateObject(dayData);

    var elemText;
    if (i < 10)
        elemText = document.createTextNode("0" + dayData.theDay); // 01 / 02 / 03 / ... / 09
    else
        elemText = document.createTextNode(dayData.theDay); // 10 / 11 / 12 / ... / 31

    calendarLi.appendChild(elemText);
    calendarList.appendChild(calendarLi);
}

// بيانات اليوم 
function DateObject(theDate) {
    this.theDay = theDate.getDate();
    this.dayName = dayNames[theDate.getDay()];
    this.dayNameNumber = dayNames.indexOf(this.dayName);
    this.theMonth = theDate.getMonth() + 1;
    this.theMonthName = monthNames[theDate.getMonth()];;
    this.theYear = theDate.getFullYear();
    this.daysInMonth = new Date(theDate.getFullYear(), theDate.getMonth() + 1, 0).getDate();
    this.firstDayOfMonth = dayNames[new Date(theDate.getFullYear(), theDate.getMonth(), 1).getDay()];
};

function createElementTooltip(elementType, elemClass, text, appendTarget, afterBefore) {
    var elemCreate = document.createElement(elementType);
    elemCreate.className = elemClass;
    var elemText = document.createTextNode(text);
    elemCreate.appendChild(elemText);
    appendTarget.appendChild(elemCreate);

    $(afterBefore).css('border-bottom', '10px solid ' + 'yellow');
}

function createElement(elementType, elemID, elemClass, text, appendTarget) {
    var elemCreate = document.createElement(elementType);
    elemCreate.id = elemID;
    elemCreate.className = elemClass;
    var elemText = document.createTextNode(text);
    elemCreate.appendChild(elemText);
    appendTarget.appendChild(elemCreate);
}

// april 2018 => 2018 => 2010 - 2019
function changeYear(operation) {

    var btnYear = $('#btnYears').text();
    var btnYearsLength = $('#btnYears').text().length;
    var year = btnYear.substr(btnYearsLength - 4);

    // before - after
    if (operation != 10) {
        if (btnYearsLength == 4) { // 2019 => 2020

            $('.divMonthYear').remove();
            $('#divTwelveMonth').css('display', 'initial');

            var num = Number($('#btnYears').text()) + Number(operation);

            if (operation == -1) {
                $('#btnYears').text(num);
                createMonthsYear(num, 'right');
            }
            else {
                createMonthsYear(num, 'left');
                $('#btnYears').text(num);
            }

            $('#divTenYear').remove();
        }
        else {
            // change between months
            if (btnYear.indexOf('-') == -1) {// فبراير 2020 => مارس 2020

                $('#tblDays').remove();
                monthIndex = monthNames.indexOf($('.superMonth').text());

                $('#btnYears').text('');

                // after ==> فبراير 2020 => مارس 2020
                if (operation == 1) {

                    monthIndex++;

                    if ((monthIndex) > 11) {
                        month = monthNames[0];
                        monthIndex = 1;
                        year++;
                    }
                    else {
                        month = monthNames[monthIndex];
                        monthIndex++;
                    }

                    bindDaysTable(monthIndex, year, 'left');
                }
                else // before ==> فبراير 2020 => يناير 2020
                {
                    month = monthNames[monthIndex - 1];
                    if ((monthIndex - 1) < 0) {
                        month = monthNames[11];
                        monthIndex = 12;
                        year--;
                    }

                    bindDaysTable(monthIndex, year, 'right');
                }

                var yearBtn = document.querySelector('#btnYears');
                var yearOf = document.createTextNode(year);
                var monthSup = document.createElement("sup");
                monthSup.className = "superMonth";
                var monthOf = document.createTextNode(month);
                monthOf = document.createTextNode(month);
                monthSup.appendChild(monthOf);
                yearBtn.appendChild(monthSup);
                yearBtn.appendChild(yearOf);
            }
                // change between 10 years
            else { // 2020 - 2029 => 2030 - 2039

                var startYear = Number(btnYear.substring(0, 4));
                var endYear = Number(btnYear.substring(7, 11));

                // before ==> 2000 - 2009
                if (operation == -1) {
                    startYear = startYear - 10;
                    endYear = endYear - 10;
                    createTenYears('calenderContent', startYear, endYear, 'right');
                }
                else // after ==> 2020 - 2029
                {
                    startYear = startYear + 10;
                    endYear = endYear + 10;
                    createTenYears('calenderContent', startYear, endYear, 'left');
                }

                $('#btnYears').text(startYear + " - " + endYear);
                styleTenYears();
            }
        }
    }
    else {

        $('#tblDays').remove();
        $('.divMonthYear').remove()

        // 2020 => 2020 - 2029
        if (btnYearsLength == 4) {

            var reminder = btnYear % 10;
            var startYear = btnYear - reminder;
            var endYear = Number(btnYear) + (9 - reminder);
            $('#btnYears').text(startYear + " - " + endYear);

            $('#divTwelveMonth').css('display', 'none');
            $('#divAttendanceMonthDays').css('display', 'none');

            createTenYears('calenderContent', startYear, endYear, 'center');
            styleTenYears();
        }
        else if (btnYear.indexOf('-') == -1) { // فبراير 2020 => 2020
            createMonthsYear(year, 'center');
            $('#divTwelveMonth').css('display', 'initial');
            $('#btnYears').text(year);
        }
    }

    $('#divWorks').css('display', 'none');
}

function createTenYears(appendTarget, startLoop, endLoop, animate) {

    $('#divTenYear').remove();

    var appendTarget = document.getElementById(appendTarget);

    addElem('div', 'divTenYear', '', appendTarget);
    var divTenYear = document.querySelector('#divTenYear');

    // التحضير
    if (dataScreen == "attendance") {
        if (animate == "left")
            divTenYear.className = 'col-12 col-lg-6 col-md-10 col-sm-12 container divTenYear animated bounceInLeft';
        else if (animate == "right")
            divTenYear.className = 'col-12 col-lg-6 col-md-10 col-sm-12 container divTenYear animated bounceInRight';
        else // center
            divTenYear.className = 'col-12 col-lg-6 col-md-10 col-sm-12 container divTenYear animated zoomIn';
    }
    else if (dataScreen == "periodMonth") // الحضور الخاص بالعامل
    {
        if (animate == "left")
            divTenYear.className = 'divTenYear animated bounceInLeft';
        else if (animate == "right")
            divTenYear.className = 'divTenYear animated bounceInRight';
        else // center
            divTenYear.className = 'divTenYear animated zoomIn';
    }

    for (var i = startLoop; i <= endLoop; i++) {
        var elemCreate = document.createElement('button');
        elemCreate.id = i;
        elemCreate.setAttribute('type', 'button');

        if (i == currentYear)
            elemCreate.className = 'btn btn-defualt tenYears active';
        else
            elemCreate.className = 'btn btn-defualt tenYears';

        var elemText = document.createTextNode(i);
        elemCreate.setAttribute('onclick', 'specialYear(' + i + ');');
        elemCreate.appendChild(elemText);
        divTenYear.appendChild(elemCreate);
    }

    styleTenYears();
}

function specialYear(yearText) {
    $('#btnYears').text(yearText);
    $('#divTwelveMonth').css('display', 'initial');
    $('.divMonthYear').remove();
    createMonthsYear(yearText, 'center');
    $('#divTenYear').remove();
    $('#divAttendanceMonthDays').css('display', 'none');
    styleMonths();
}

function openDivWorkers(btnClick, openDivWorker, url, queryStrings, bodyModel) {

    $('.day-content , .twelveMonth').removeClass('active');

    $(btnClick).addClass('active');

    if ($(btnClick).hasClass('day-content'))
        $(openDivWorker).fadeOut("slow");
    else 
        $(openDivWorker).slideUp("slow");

    if (queryStrings != "") {
        var qs = queryStrings.split(',');

        url = url + "?";

        for (var i = 0 ; i < qs.length ; i++) {
            if (i != 0)
                url = url + "&";

            url = url + qs[i];
        }
    }

    $(bodyModel).load(url, function () {
        if ($(btnClick).hasClass('day-content'))
            $(openDivWorker).fadeIn("slow");
        else
            $(openDivWorker).slideDown("slow");
    });
}