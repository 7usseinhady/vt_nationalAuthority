$(document).ready(function () {

    $('#arLanguage').click(function () {
        displayButtons('inline-block', 'none');
    });

    $('#enLanguage').click(function () {
        displayButtons('none', 'inline-block');
    });

});

var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December",
                   "يناير", "فبراير", "مارس", "ابريل", "مايو", "يونيو", "يوليو", "اغسطس", "سبتمبر", "اكتوبر", "نوفمبر", "ديسمبر"];
var dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday",
                "سبت", "جمعه", "خميس", "اربعاء", "ثلاثاء", "اثنين", "احد"];

var dayType, dayTypeNum, attendanceTime, lateTime, extraTime, leaveTime, month, year, theDate, currentDate, forAnyTab;

function createCalenderEnglish(dayType, dayTypeNum, attendanceTime, lateTime, extraTime, leaveTime, month, year, forAnyTab, daysColor, searchSpecialDay) {
    'use strict';

    //console.log("forAnyTab createCalenderEnglish :", forAnyTab);

    dayType = dayType.split(','); // نوع اليوم  => اجازه / غياب / ماموريه / اذن
    attendanceTime = attendanceTime.split(','); // وقت الحضور 
    leaveTime = leaveTime.split(','); // وقت الانصراف
    lateTime = lateTime.split(','); // وقت التاخير
    extraTime = extraTime.split(','); // وقت اضافى 
    dayTypeNum = dayTypeNum.split(','); // نوع اليوم

    var divCalender = document.getElementById('calenderContent');

    daysColor = daysColor.split(',');

    // دليل الالوان " مفتاح الخريطه" 
    var divEnDefinitions = document.querySelector('#enDefinitions');

    definitions(divEnDefinitions, 'div', '', daysColor[0], "-12px", 'img', daysColor[1]);

    for (var i = 2; i < daysColor.length; i += 2) {
        definitions(divEnDefinitions, 'div', '', daysColor[i], "", 'img', daysColor[i + 1]);
    }

    var divSearch = document.querySelector('#divSearch');
    createTextSearch('input', 'search', 'button', 'search', '', 'btn btn-success btn-sm search', 'javascript: searchBtn("' + forAnyTab + '");', divSearch);
    divCalender.appendChild(divSearch);

    // ex: April 2018 
    createMonthYear('divMonthYear', month, year, forAnyTab);

    // week days
    bindDaysNames('divWeekDays', 0, 7, 3); // 3 => عشان اخد اول 3 حروف من الايام بالانجلش

    // days month
    theDate = new Date(year, month - 1);
    currentDate = new DateObject(theDate);
    bindDaysNumbers(currentDate, 'ulDaysMonthEnglish', 'divDaysMonth', dayType, dayTypeNum, attendanceTime, lateTime, extraTime, leaveTime, forAnyTab, daysColor, searchSpecialDay, 'left');
}

function createTextSearch(control, elemID, elemType, text, placeholder, elemClasses, funJS, appendTarget) {
    var elemCreate = document.createElement(control);
    elemCreate.id = elemID;
    elemCreate.setAttribute('type', elemType);
    elemCreate.setAttribute('placeholder', placeholder);
    elemCreate.setAttribute('value', text);
    elemCreate.className = elemClasses;
    elemCreate.setAttribute('onclick', funJS);

    appendTarget.appendChild(elemCreate);
}

// دليل الالوان " مفتاح الخريطه" 
function definitions(targetElem, createElem, elemClass, elemText, elemStyle, insideElem, insideElemClass) {

    var createElem = document.createElement(createElem);
    createElem.className = elemClass;
    createElem.style.marginTop = elemStyle;

    var createElemText = document.createTextNode(elemText);

    var insideElem = document.createElement(insideElem);

    insideElem.style.backgroundColor = insideElemClass;

    createElem.appendChild(insideElem);
    createElem.appendChild(createElemText);
    targetElem.appendChild(createElem);
}

// april 2018 => 2018 => 2010 - 2019
function changeSuperYear(forAnyTab) {

    var btnText = $('#years').text();
    var btnLength = $('#years').text().length;
    var btnMonth = $('.superMonth').text();
    var btnYear = btnText.substring(btnLength - 4, btnLength);

    $('.yearBefore').css('display', 'inline-block');
    $('.yearAfter').css('display', 'inline-block');

    // april 2018 => 2018
    if (monthNames.includes(btnMonth)) {
        $('#years').text(btnYear);
        $('#divWeekDays').remove();
        $('#divDaysMonth').remove();
        $('#divTwelveMonth').remove();

        $('#enDefinitions').css('display', 'none');
        $('#arDefinitions').css('display', 'none');
        create12Month('calenderContent', forAnyTab);
    }
    else {  // 2018 => 2010 - 2019 
        $('#divWeekDays').remove();
        $('#divDaysMonth').remove();
        $('#divTenYear').remove();
        $('#divTwelveMonth').remove();
        $('#enDefinitions').css('display', 'none');
        $('#arDefinitions').css('display', 'none');

        var reminder = btnYear % 10;
        var startYear = btnYear - reminder;
        var endYear = Number(btnYear) + (9 - reminder);
        $('#years').text(startYear + " - " + endYear);

        createYears('calenderContent', startYear, endYear, forAnyTab);
    }
}

// << OR >> 'before OR After'
function changeYear(displayYear, forAnyTab) { // displayYear >> OR <<

    var btnText = $('#years').text();
    var btnLength = $('#years').text().length;

    // 2018
    if (btnLength == 4) {

        // before ==> 2017
        if (displayYear == "<<")
            btnText = Number(btnText) - 1;

        else // after ==> 2019
            btnText = Number(btnText) + 1;

        $('#years').text(btnText);

        $('#divTwelveMonth').children().each(function () {
            $(this).attr('onclick', 'javascript: specialMonth("' + $(this).text() + '","' + btnText + '","","' + forAnyTab + '");');
        });
    }
        // 2010 - 2019
    else {

        var startYear = Number(btnText.substring(0, 4));
        var endYear = Number(btnText.substring(7, 11));

        // before ==> 2000 - 2009
        if (displayYear == "<<") {
            startYear = startYear - 10;
            endYear = endYear - 10;
        }
        else // after ==> 2020 - 2029
        {
            startYear = startYear + 10;
            endYear = endYear + 10;
        }

        $('#years').text(startYear + " - " + endYear);

        $('#divTenYear').children().each(function () {
            $(this).attr('onclick', 'javascript: specialYear("' + startYear + '","' + forAnyTab + '");');
            $(this).text(startYear);
            startYear++;
        });
    }
}

function createCalenderArabic(dayType, dayTypeNum, attendanceTime, lateTime, extraTime, leaveTime, month, year, forAnyTab, daysColor, searchSpecialDay) {
    'use strict';

    dayType = dayType.split(','); // نوع اليوم 
    attendanceTime = attendanceTime.split(','); // وقت الحضور 
    leaveTime = leaveTime.split(','); // وقت الانصراف
    lateTime = lateTime.split(','); // وقت التاخير
    extraTime = extraTime.split(','); // وقت اضافى 
    dayTypeNum = dayTypeNum.split(','); // نوع اليوم

    var divCalender = document.getElementById('calenderContent');

    // arabic OR english button
    addElem('div', 'divLanguage', '', divCalender);
    var divLanguage = document.querySelector('#divLanguage');

    var elemCreate = document.createElement('button');
    elemCreate.id = 'enLanguage';
    elemCreate.setAttribute('type', 'button');
    elemCreate.className = 'btn btn-defualt';
    elemCreate.setAttribute('onclick', 'javascript: enBtnClick("' + dayType + '","' + dayTypeNum + '","' + attendanceTime + '","' + lateTime + '","' + extraTime + '","' + leaveTime + '","' + month + '","' + year + '","' + forAnyTab + '","' + searchSpecialDay + '");');
    var elemText = document.createTextNode('English');
    elemCreate.appendChild(elemText);
    divLanguage.appendChild(elemCreate);

    //دليل الالوان " مفتاح الخريطه" 
    createElement('div', 'arDefinitions', 'arDefinitions', '', divLanguage);
    var divEnDefinitions = document.querySelector('#arDefinitions');

    definitions(divEnDefinitions, 'div', 'arNormalDay', ' يوم عادى', 'img', 'imgNormalDay');
    definitions(divEnDefinitions, 'div', 'arHoliday', ' اجازه', 'img', 'imgHoliday');
    definitions(divEnDefinitions, 'div', 'arAbsence', ' غياب', 'img', 'imgAbsence');
    definitions(divEnDefinitions, 'div', 'arPermission', ' اذن', 'img', 'imgPremission');
    definitions(divEnDefinitions, 'div', 'arDelayTime', ' وقت تاخير', 'img', 'imgDelayTime');
    definitions(divEnDefinitions, 'div', 'arExtraTime', ' وقت اضافى', 'img', 'imgExtraTime');
    definitions(divEnDefinitions, 'div', 'arMamorya', ' مأموريه', 'img', 'imgMaamoria');

    // ex: April 2018 
    addElem('div', 'divMonthYear', '', divCalender);
    createMonthYear('divMonthYear', Number(month) + 12, year, forAnyTab); // +12 => عشان اجيب الشهور بالعربى

    // week days
    addElem('div', 'divWeekDays', '', divCalender);
    bindDaysNames('divWeekDays', 7, 14, 6); // 6 => اجيب اسم اليوم بالكامل

    // days month
    addElem('div', 'divDaysMonth', '', divCalender);

    theDate = new Date(year, month - 1);
    currentDate = new DateObject(theDate);

    bindDaysNumbers(currentDate, 'ulDaysMonthArabic', 'divDaysMonth', dayType, dayTypeNum, attendanceTime, lateTime, extraTime, leaveTime, forAnyTab, daysColor, searchSpecialDay, 'right');
}
// بيانات اليوم 
function DateObject(theDate) {
    this.theDay = theDate.getDate();
    this.dayName = dayNames[theDate.getDay()];
    this.theMonth = theDate.getMonth() + 1;
    this.theMonthName = monthNames[theDate.getMonth()];;
    this.theYear = theDate.getFullYear();
    this.daysInMonth = new Date(theDate.getFullYear(), theDate.getMonth() + 1, 0).getDate();
    this.firstDayOfMonth = dayNames[new Date(theDate.getFullYear(), theDate.getMonth(), 1).getDay()];
};

// ايام الاسبوع
function bindDaysNames(divWeekDays, startLoop, endLoop, subStringLength, daysColor) {
    var divWeekDays = document.getElementById(divWeekDays);

    for (var i = startLoop; i < endLoop; i++)
        createElement('h3', 'day_' + i + i + i, 'day-of-week', dayNames[i].substr(0, subStringLength), divWeekDays);
}

// ايام الشهر
function bindDaysNumbers(currentDate, className, appendTarget, dayType, dayTypeNum, attendanceTime, lateTime, extraTime, leaveTime, forAnyTab, daysColor, searchSpecialDay, margin) { // margin => for ar OR en language
    var appendTarget = document.getElementById(appendTarget);
    var calendarList = document.createElement("ul");
    calendarList.className = className;

    for (var i = 1; i <= currentDate.daysInMonth ; i++) {

        var calendarLi = document.createElement("li");
        calendarLi.id = "day_" + i;

        var toolTipText;
        if (margin == 'left') // english
            toolTipText = (dayType[i] == undefined ? "No Registration Yet" : (dayType[i] == "" ? "No Registration Yet" : dayType[i])) + '\n' + 'Attendance Time : ' + (attendanceTime[i] == undefined ? "ــــــــ" : (attendanceTime[i] == "" ? "ــــــــ" : attendanceTime[i])) + '\n' + 'Leave Time : ' + (leaveTime[i] == undefined ? "ــــــــ" : (leaveTime[i] == "" ? "ــــــــ" : leaveTime[i])) + '\n' + 'Delay Time : ' + (lateTime[i] == undefined ? "ــــــــ" : (lateTime[i] == "" ? "ــــــــ" : lateTime[i])) + '\n' + 'Extra Time : ' + (extraTime[i] == undefined ? "ــــــــ" : (extraTime[i] == "" ? "ــــــــ" : extraTime[i]));
        else
            toolTipText = (dayType[i] == undefined ? "لم  يتم التسجيل بعد" : (dayType[i] == "" ? "لم  يتم التسجيل بعد" : dayType[i])) + '\n' + 'وقت الحضــور : ' + (attendanceTime[i] == undefined ? "ــــــــ" : (attendanceTime[i] == "" ? "ــــــــ" : attendanceTime[i])) + '\n' + 'وقت الانصراف : ' + (leaveTime[i] == undefined ? "ــــــــ" : (leaveTime[i] == "" ? "ــــــــ" : leaveTime[i])) + '\n' + 'وقت التاخيـــــر : ' + (lateTime[i] == undefined ? "ــــــــ" : (lateTime[i] == "" ? "ــــــــ" : lateTime[i])) + '\n' + 'وقت اضافــــى : ' + (extraTime[i] == undefined ? "ــــــــ" : (extraTime[i] == "" ? "ــــــــ" : extraTime[i]));

        if (forAnyTab == "forHoliday") { //اجازه
            if (dayTypeNum[i] == "1") {
                calendarLi.className = "holiday";
                calendarLi.style.backgroundColor = daysColor[1];
                createElementTooltip('span', 'toolTip_' + i, 'tooltipHolidy', daysColor[1], toolTipText, calendarLi);
                $('.tooltipHolidy::after, .tooltipHolidy::before').css('border-bottom', '10px solid ' + daysColor[1]);
            } else {
                calendarLi.className = "calendarLi";

                createElement('span', 'toolTip_' + i, '_tooltip', toolTipText, calendarLi);
            }
        }
        else if (forAnyTab == "forAbsence") { // غياب
            if (dayTypeNum[i] == "2") {
                calendarLi.className = "absence";
                calendarLi.style.backgroundColor = daysColor[1];
                createElementTooltip('span', 'toolTip_' + i, 'tooltipAbsence', daysColor[1], toolTipText, calendarLi);
                $('.tooltipAbsence::after, .tooltipAbsence::before ').css('border-bottom', '10px solid ' + daysColor[1]);
            } else {
                calendarLi.className = "calendarLi";
                createElement('span', 'toolTip_' + i, '_tooltip', toolTipText, calendarLi);
            }
        }
        else if (forAnyTab == "forMission") { // ماموريه
            if (dayTypeNum[i] == "4") {
                calendarLi.className = "mamoryia";
                calendarLi.style.backgroundColor = daysColor[1];
                createElementTooltip('span', 'toolTip_' + i, 'tooltipMamoryia', daysColor[1], toolTipText, calendarLi);
                $('.tooltipMamoryia::after, .tooltipMamoryia::before').css('border-bottom', '10px solid ' + daysColor[1]);
            } else {
                calendarLi.className = "calendarLi";
                createElement('span', 'toolTip_' + i, '_tooltip', toolTipText, calendarLi);
            }
        }
        else if (forAnyTab == "forPremission") { // اذن
            if (dayTypeNum[i] == "3") {
                calendarLi.className = "permission";
                calendarLi.style.backgroundColor = daysColor[1];// " linear-gradient(to left, " + daysColor + " 0%," + daysColor + " 50%, #EEF0F2 51%,#EEF0F2 100%);";
                createElementTooltip('span', 'toolTip_' + i, 'tooltipPermission', daysColor[1], toolTipText, calendarLi);
                $('.tooltipPermission::after, .tooltipPermission::before').css('border-bottom', '10px solid ' + daysColor[1]);
            } else {
                calendarLi.className = "calendarLi";
                createElement('span', 'toolTip_' + i, '_tooltip', toolTipText, calendarLi);
            }
        }
        else if (forAnyTab == "forAttendance") { // للحضور

            if (dayTypeNum[i] == "0") {
                calendarLi.className = "attendance";
                calendarLi.style.backgroundColor = daysColor[1];
                createElementTooltip('span', 'toolTip_' + i, 'tooltipAttendance', daysColor[1], toolTipText, calendarLi);
                $('.tooltipAttendance::after, .tooltipAttendance::before').css('border-bottom', '10px solid ' + daysColor[1]);
            } else {
                calendarLi.className = "calendarLi";
                createElement('span', 'toolTip_' + i, '_tooltip', toolTipText, calendarLi);
            }
        }
        else {
            if (dayTypeNum[i] == "1") // اجازه
            {
                calendarLi.className = "holiday";
                calendarLi.style.backgroundColor = daysColor[1];
                createElementTooltip('span', 'toolTip_' + i, 'tooltipHolidy', daysColor[1], toolTipText, calendarLi);
                $('.tooltipHolidy::after, .tooltipHolidy::before').css('border-bottom', '10px solid ' + daysColor[1]);
            }
            if (dayTypeNum[i] == "2") // غياب
            {
                calendarLi.className = "absence";
                calendarLi.style.backgroundColor = daysColor[3];
                createElementTooltip('span', 'toolTip_' + i, 'tooltipAbsence', daysColor[3], toolTipText, calendarLi);
                $('.tooltipAbsence::after, .tooltipAbsence::before ').css('border-bottom', '10px solid ' + daysColor[3]);
            }
            if (dayTypeNum[i] == "3") // اذن
            {
                calendarLi.className = "permission";
                calendarLi.style.backgroundColor = daysColor[5];
                createElementTooltip('span', 'toolTip_' + i, 'tooltipPermission', daysColor[5], toolTipText, calendarLi);
                $('.tooltipPermission::after, .tooltipPermission::before').css('border-bottom', '10px solid ' + daysColor[5]);
            }
            if (dayTypeNum[i] == "4") // ماموريه
            {
                calendarLi.className = "mamoryia";
                calendarLi.style.backgroundColor = daysColor[7];
                createElementTooltip('span', 'toolTip_' + i, 'tooltipMamoryia', daysColor[7], toolTipText, calendarLi);
                $('.tooltipMamoryia::after, .tooltipMamoryia::before').css('border-bottom', '10px solid ' + daysColor[7]);
            }

            if (dayTypeNum[i] == "0") // يوم عادى
            {
                calendarLi.className = "attendance";
                calendarLi.style.backgroundColor = daysColor[9];
                createElementTooltip('span', 'toolTip_' + i, 'tooltipAttendance', daysColor[9], toolTipText, calendarLi);
                $('.tooltipAttendance::after, .tooltipAttendance::before').css('border-bottom', '10px solid ' + daysColor[9]);
            }
            if (dayTypeNum[i] != "0" && dayTypeNum[i] != "1" && dayTypeNum[i] != "2" && dayTypeNum[i] != "3" && dayTypeNum[i] != "4") // يوم عادى
            {
                calendarLi.className = "calendarLi";
                createElement('span', 'toolTip_' + i, '_tooltip', toolTipText, calendarLi);
            }
        }

        var dayData = new Date(currentDate.theYear, currentDate.theMonth - 1, i);
        dayData = new DateObject(dayData);

        var elemText;
        if (i < 10)
            elemText = document.createTextNode("0" + dayData.theDay); // 01 / 02 / 03 / ... / 09
        else
            elemText = document.createTextNode(dayData.theDay); // 10 / 11 / 12 / ... / 31

        calendarLi.appendChild(elemText);

        if (margin == 'left') // english
        {
            createElement('p', 'pAttend_' + i, 'calendarPragraph', attendanceTime[i] == undefined ? 'ـــــــــــــــــــــ' : (attendanceTime[i] == "" ? "ـــــــــــــــــــــ" : (attendanceTime[i] + " AM")), calendarLi);
            createElement('p', 'pLeave_' + i, 'calendarPragraph', leaveTime[i] == undefined ? 'ـــــــــــــــــــــ' : (leaveTime[i] == "" ? "ـــــــــــــــــــــ" : (leaveTime[i] + " PM")), calendarLi);
        }
        else {
            createElement('p', 'pAttend_' + i, 'calendarPragraph', attendanceTime[i] == undefined ? 'ـــــــــــــــــــــ' : (attendanceTime[i] == "" ? "ـــــــــــــــــــــ" : (attendanceTime[i] + " AM")), calendarLi); // ص => صباحا
            createElement('p', 'pLeave_' + i, 'calendarPragraph', leaveTime[i] == undefined ? 'ـــــــــــــــــــــ' : (leaveTime[i] == "" ? "ـــــــــــــــــــــ" : (leaveTime[i] + " PM")), calendarLi);  // م => مساءاً
        }
        calendarList.appendChild(calendarLi);
    }
    appendTarget.appendChild(calendarList);

    var dayOne = document.getElementById('day_1');

    if (margin == 'left') {
        if (currentDate.firstDayOfMonth == "Monday")
            dayOne.style.marginLeft = "13%";
        else if (currentDate.firstDayOfMonth == "Tuesday")
            dayOne.style.marginLeft = "26.3%";
        else if (currentDate.firstDayOfMonth == "Wednesday")
            dayOne.style.marginLeft = "39.6%";
        else if (currentDate.firstDayOfMonth == "Thursday")
            dayOne.style.marginLeft = "53.4%";
        else if (currentDate.firstDayOfMonth == "Friday")
            dayOne.style.marginLeft = "66.7%";
        else if (currentDate.firstDayOfMonth == "Saturday")
            dayOne.style.marginLeft = "80.3%";
    } else {
        if (currentDate.firstDayOfMonth == "Monday")
            dayOne.style.marginRight = "13%";
        else if (currentDate.firstDayOfMonth == "Tuesday")
            dayOne.style.marginRight = "26.3%";
        else if (currentDate.firstDayOfMonth == "Wednesday")
            dayOne.style.marginRight = "39.6%";
        else if (currentDate.firstDayOfMonth == "Thursday")
            dayOne.style.marginRight = "53.4%";
        else if (currentDate.firstDayOfMonth == "Friday")
            dayOne.style.marginRight = "66.7%";
        else if (currentDate.firstDayOfMonth == "Saturday")
            dayOne.style.marginRight = "80.3%";
    }
}

function createElementTooltip(elementType, elemID, elemClass, daysColor, text, appendTarget) {
    var elemCreate = document.createElement(elementType);
    elemCreate.id = elemID;
    elemCreate.className = elemClass;
    elemCreate.style.backgroundColor = daysColor;
    var elemText = document.createTextNode(text);
    elemCreate.appendChild(elemText);
    appendTarget.appendChild(elemCreate);
}

// when click on arabic button
function arBtnClick(dayType, dayTypeNum, attendanceTime, lateTime, extraTime, leaveTime, month, year, forAnyTab, searchSpecialDay) {

    var renderTarget = document.getElementById('calenderContent');
    renderTarget.remove();

    var divCalender = document.getElementsByClassName('modal-content')[0];
    createElement('div', 'calenderContent', 'modal-body', '', divCalender);

    createCalenderArabic(dayType, dayTypeNum, attendanceTime, lateTime, extraTime, leaveTime, month, year, forAnyTab, searchSpecialDay);
}

//when click on english button
function enBtnClick(dayType, dayTypeNum, attendanceTime, lateTime, extraTime, leaveTime, month, year, forAnyTab, searchSpecialDay) {

    var renderTarget = document.getElementById('calenderContent');
    renderTarget.remove();

    var divCalender = document.getElementsByClassName('modal-content')[0];
    createElement('div', 'calenderContent', 'modal-body', '', divCalender);

    createCalenderEnglish(dayType, dayTypeNum, attendanceTime, lateTime, extraTime, leaveTime, month, year, forAnyTab, searchSpecialDay);
}

// april 2018
function createMonthYear(divMonthYear, month, year, forAnyTab) {

    var divMonthYear = document.getElementById(divMonthYear);

    var yearBtn = document.createElement('button');
    yearBtn.id = 'years';
    yearBtn.setAttribute('type', 'button');
    yearBtn.className = 'btn btn-defualt superYear';
    var yearOf = document.createTextNode(year);
    yearBtn.setAttribute('onclick', 'javascript: changeSuperYear("' + forAnyTab + '");');
    var monthSup = document.createElement("sup");
    monthSup.className = "superMonth";
    var monthOf = document.createTextNode(monthNames[month - 1]);
    monthSup.appendChild(monthOf);
    yearBtn.appendChild(monthSup);
    yearBtn.appendChild(yearOf);
    divMonthYear.appendChild(yearBtn);

    var yearBefore = document.createElement('button');
    yearBefore.setAttribute('type', 'button');
    yearBefore.className = 'yearBefore';
    var yearBeforeOf = document.createTextNode("<<");
    yearBefore.setAttribute('onclick', 'javascript: changeYear("<<","' + forAnyTab + '");');//"' + monthNames[i] + '","' + btnText + '","' + forAnyTab + '","");')
    yearBefore.appendChild(yearBeforeOf);
    divMonthYear.appendChild(yearBefore);
    var yearAfter = document.createElement('button');
    yearAfter.setAttribute('type', 'button');
    yearAfter.className = 'yearAfter';
    var yearAfterOf = document.createTextNode(">>");
    yearAfter.setAttribute('onclick', 'javascript: changeYear(">>","' + forAnyTab + '");');
    yearAfter.appendChild(yearAfterOf);
    divMonthYear.appendChild(yearAfter);

    $('.yearBefore').css('display', 'none');
    $('.yearAfter').css('display', 'none');
}

function createElement(elementType, elemID, elemClass, text, appendTarget) {
    var elemCreate = document.createElement(elementType);
    elemCreate.id = elemID;
    elemCreate.className = elemClass;
    var elemText = document.createTextNode(text);
    elemCreate.appendChild(elemText);
    appendTarget.appendChild(elemCreate);
}

function addElem(elementType, elemID, elemClass, appendTarget) {
    appendTarget.innerHTML += "<" + elementType + " id= " + elemID + " class=" + elemClass + "> </" + elementType + ">";
}

function displayButtons(arBtns, enBtns) {
    $('#arMonthAfter').css('display', arBtns);
    $('#arMonthBefore').css('display', arBtns);
    $('#enMonthAfter').css('display', enBtns);
    $('#enMonthBefore').css('display', enBtns);
}

// 2010 - 2011 - 2012 - 2013 -2014 -2015 - 2016 - 2017 - 2018 - 2019 
function createYears(appendTarget, startLoop, endLoop, forAnyTab) {
    var appendTarget = document.getElementById(appendTarget);

    addElem('div', 'divTenYear', '', appendTarget);
    var divTenYear = document.querySelector('#divTenYear');

    for (var i = startLoop; i <= endLoop; i++) {
        var elemCreate = document.createElement('button');
        elemCreate.setAttribute('type', 'button');
        elemCreate.className = 'btn btn-defualt tenYears';
        var elemText = document.createTextNode(i);
        elemCreate.setAttribute('onclick', 'javascript: specialYear("' + i + '","' + forAnyTab + '");');
        elemCreate.appendChild(elemText);
        divTenYear.appendChild(elemCreate);
    }
}

function create12Month(appendTarget, forAnyTab) {
    var appendTarget = document.getElementById(appendTarget);
    var btnText = $('#years').text();

    addElem('div', 'divTwelveMonth', '', appendTarget);
    var divTenYear = document.querySelector('#divTwelveMonth');

    for (var i = 0; i < 12; i++) {
        var elemCreate = document.createElement('button');
        elemCreate.id = 'month_' + i;
        elemCreate.setAttribute('type', 'button');
        elemCreate.setAttribute('runat', 'server');
        elemCreate.className = 'btn btn-defualt twelveMonth';
        var elemText = document.createTextNode(monthNames[i]);
        elemCreate.setAttribute('onclick', 'javascript: specialMonth("' + monthNames[i] + '","' + btnText + '","","' + forAnyTab + '");');

        elemCreate.appendChild(elemText);
        divTenYear.appendChild(elemCreate);
    }
}

function specialYear(year, forAnyTab) {
    $('#years').text(year);
    $('#divTenYear').remove();
    create12Month('calenderContent', forAnyTab);
}

function specialMonth(month, year, _searchSpecialDay, forAnyTab) {

    $('#years').text('');

    var yearBtn = document.querySelector('#years');

    var yearOf = document.createTextNode(year);
    var monthSup = document.createElement("sup");
    monthSup.className = "superMonth";
    var monthOf = document.createTextNode(month);

    $('.yearBefore').css('display', 'none');
    $('.yearAfter').css('display', 'none');

    var parameters;
    if (month.length == 2 || month.length == 1) { // لو جايه من السيرش 
        parameters = month + "," + year + "," + _searchSpecialDay + "," + forAnyTab;
        monthOf = document.createTextNode(monthNames[month - 1]);
    }
    else {
        parameters = (monthNames.indexOf(month) + 1) + "," + year + "," + _searchSpecialDay + "," + forAnyTab;
        monthOf = document.createTextNode(month);
    }

    monthSup.appendChild(monthOf);
    yearBtn.appendChild(monthSup);
    yearBtn.appendChild(yearOf);

    __doPostBack('btnTest', parameters);
}

function searchBtn(forAnyTab) {

 var day = $('#daySearch').val();
    var month = $('#monthSearch').val();
    var year = $('#yearSearch').val();

    // if texts empty
    if (day == '' && month == '' && year == '') {
        alert('Must Fill Fields');
    }
        // for special day
    else if (day != '') {
        if ((day < 1 || day > 31) && (month < 1 || month > 12 || month == '') && (year == '' || year.length != 4)) {
            alert('Please Enter Valid Day , Month , Year');
        } else if ((day < 1 || day > 31) && (month < 1 || month > 12)) {
            alert('Please Enter Valid Day , Month');
        } else if ((day < 1 || day > 31) && (year == '' || year.length != 4)) {
            alert('Please Enter Valid Day , Year');
        } else if ((day < 1 || day > 31)) {
            alert('Please Enter Valid Day');
        } else if ((month < 1 || month > 12 || month == '') && (year == '' || year.length != 4)) {
            alert('Please Enter Valid Month , Year');
        } else if (year == '' || year.length != 4) {
            alert('Please Enter Valid Year');
        } else if (month < 1 || month > 12 || month == '') {
            alert('Please Enter Valid Month ');
        } else {

            theDate = new Date(year, month - 1);
            var daysInMonth = new Date(theDate.getFullYear(), theDate.getMonth() + 1, 0).getDate();

            if (day > daysInMonth)
                alert('This Month Have ' + daysInMonth + ' Day Not ' + day);
            else {
                $('#divDaysMonth').remove();
                $('#divWeekDays').remove();
                $('#divTwelveMonth').remove();

                specialMonth(month, year, day, forAnyTab);
            }
        }
    }
        // for special month in year
    else if (month != '') {

        if ((month < 1 || month > 12) && (year == '')) {
            alert('Must Enter Valid Month ( 1 - 12 ) And Select Your Year');
        }
        else if (month < 1 || month > 12) {
            alert('Must Enter Valid Month ( 1 - 12 )');
        }
        else if (year == '' || year.length != 4) {
            alert('Select Your Year Correct');
        } else {
            $('#divDaysMonth').remove();
            $('#divWeekDays').remove();
            $('#divTwelveMonth').remove();

            specialMonth(month, year, '', forAnyTab);
        }
    }
        // for special year
    else if (year != '') {
        $('#divDaysMonth').remove();
        $('#divWeekDays').remove();
        $('#divTwelveMonth').remove();

        $('.yearBefore').css('display', 'inline-block');
        $('.yearAfter').css('display', 'inline-block');
        specialYear($('#yearSearch').val(), forAnyTab);
    }
}