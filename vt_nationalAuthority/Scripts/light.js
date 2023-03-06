var aAlertIcons = ['success', 'error', 'warning', 'info', 'question'];
// Fetch all the forms we want to apply custom Bootstrap validation styles to
var forms = $('form');//document.getElementsByClassName('needs-validation');
// For Paging Model
var tableId = '', AlerMsg = '', URL = '', PageSize = '', TotalPages;
$(function () {
    //$('.selectpicker').selectpicker();

    $('.pagination li').removeClass().addClass("page-item");
    $('.pagination li a').removeClass().addClass("page-link hvr-bob");

    const urlParams = new URLSearchParams(window.location.search);
    const paramInPage = urlParams.get('inPage');

    if (paramInPage == null) {
        $('.pagination li:first-child').addClass("active")
        $('.pagination li.active a').removeClass('hTable');
    }
    else
        $('.pagination li a').filter(function () { return this.textContent == paramInPage }).removeClass('hTable').parent().addClass('active');

    $('#ckboxException').click(function () {
        if ($(this).prop("checked"))
            $('#divViewException').css('display', 'inline-block');
        else
            $('#divViewException').css('display', 'none');
    });

    $('#navBar > li > a').filter(function () {
        var path = location.href.split('/');
        var action = path[4].split('?');
        return this.href.indexOf(path[3] + "/" + action[0]) > -1
    }).parent().addClass('active').siblings().removeClass('active');

    $('.req').css('visibility', 'visible');

    $(document).keypress(function (e) {
        // numbers only 
        if (e.keyCode == 13)
            $('#btnSearch').click();
    });

    $('.number').keypress(function (e) {
        // numbers only
        if (e.keyCode >= 48 && e.keyCode <= 57)
            return true;
        return false;
    });

    $('.decimal').keypress(function (e) {
        // decimal only
        if ((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode == 46 && $(this).val().indexOf('.') == -1))
            return true;
        return false;
    });

    $('.english').keypress(function (e) {
        // english letters only
        if ((e.keyCode >= 65 && e.keyCode <= 90) || (e.keyCode >= 97 && e.keyCode <= 122) || (e.keyCode == 32))
            return true;
        return false;
    });
    $('.enNum').keypress(function (e) {

        // english letters only - numbers only
        if ((e.keyCode >= 65 && e.keyCode <= 90) || (e.keyCode >= 97 && e.keyCode <= 122) || (e.keyCode == 32) || (e.keyCode >= 48 && e.keyCode <= 57))
            return true;
        return false;
    });
    $('.enNumSign').keypress(function (e) {

        // english letters only - numbers only - signs
        if ((e.keyCode >= 32 && e.keyCode <= 126))
            return true;
        return false;
    });

    $('.arabic').keypress(function (e) {
        // arabic letters only
        if ((e.keyCode >= 0 && e.keyCode <= 255) && !(e.keyCode == 32))
            return false;
        return true;
    });
    $('.arNum').keypress(function (e) {

        // numbers only
        if (e.keyCode >= 48 && e.keyCode <= 57)
            return true;
        // arabic letters only
        if ((e.keyCode >= 0 && e.keyCode <= 255) && !(e.keyCode == 32))
            return false;
        return true;
    });
    $('.arNumSign').keypress(function (e) {

        // arabic letters only
        if ((e.keyCode >= 32 && e.keyCode <= 64) || (e.keyCode >= 91 && e.keyCode <= 96))
            return true;
        if ((e.keyCode >= 0 && e.keyCode <= 255) && !(e.keyCode == 32))
            return false;
        return true;
    });

    //cut copy paste
    $('.arabic,.arNum,.english,.enNum,.number,.decimal').bind('paste', function (e) {
        e.preventDefault();
    });

    $('.date').keypress(function (e) {
        if (e.keyCode >= 8 && e.keyCode <= 222)
            return false;
        else
            return false;
    });

    $('.characters').keypress(function (e) {
        // numbers only
        if ((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode == 46))
            return false;
        return true;
    });

    $('.barcode').on("click copy paste keydown keypress keyup", function (e) {
        e.preventDefault();
    });

    $(document).keypress(function (e) {
        if (e.keyCode == 43)
            $('#lbadd').trigger('click');
    });

    $(document).keypress(function (e) {
        if (e.keyCode == 27)
            $("#myModal").fadeOut().modal("hide");
    });

    $('.isMuted').on('click', function () {
        setCookieValue();
    });

    /* Start dropdown-j */
    $('.dropdown-j').click(function () {
        $('.dropdown-content-j').css('display', 'none');
        $(this).find('.dropdown-content-j').css('display', 'block');
    });

    document.onclick = function (e) {
        if ((!$(event.target).hasClass("dropdown-content-j") && !$(event.target).hasClass("dropdown-j") && !$(event.target).hasClass("fas fa-ellipsis-v") && !$(event.target).hasClass("d-sm-show101")))
            $('.dropdown-content-j,.attach').css('display', 'none');
    };

    /* End dropdown-j */
})
// Loop over them and prevent submission
var validation = Array.prototype.filter.call(forms, function (form) {

    form.addEventListener('submit', function (event) {
        if (form.checkValidity() === false) {
            event.preventDefault();
            //event.stopPropagation();
            loaderClose();
        }
        form.classList.add('was-validated');
    }, false);
});

function play() {
    var audio = new Audio("../soundNotification/sound.mp3");
    var isMutedValue = getCookieValue("isMuted");

    // 0 => Not Muted || 1 => Muted
    if (isMutedValue == '' || isMutedValue == '0') { // 0 => Not Muted
        audio.play();

        $('.unmuted').css('display', 'block');
        $('.muted').css('display', 'none');
    }
    else { // 1 => Muted
        $('.unmuted').css('display', 'none');
        $('.muted').css('display', 'block');
    }
}

function attach(action) {
    $('.attach').css('display', 'none');
    $(action).find('.dropdown-content-j').css('display', 'block');
}

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById('blah').src = e.target.result;
        }
        reader.readAsDataURL(input.files[0]);
    }
}

function openModel(titleModel, textTitleModel, btnSubmit, txtBtnSubmit, classLi, ClassBtn, url, queryStrings, bodyModel) {
    loaderOpen();
    $(titleModel).text(textTitleModel);
    //queryStrings = queryStrings.replaceAll(' ', '%20');
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
        $(btnSubmit).removeClass().addClass(ClassBtn).append('<li class="' + classLi + '"> ' + txtBtnSubmit + ' </li>');
        loaderClose();
    });


}

function openModel(modelName, titleModel, textTitleModel, url, queryStrings, bodyModel) {
    loaderOpen();

    $(modelName).modal('show');
    $(titleModel).text(textTitleModel);
    if (queryStrings != "") {
        var qs = queryStrings.split(',');

        url = url + "?";

        for (var i = 0 ; i < qs.length ; i++) {
            if (i != 0)
                url = url + "&";

            url = url + qs[i];
        }
    }
    $(bodyModel).load(url, function () { loaderClose(); });
}

function openModel(titleModel, textTitleModel, btnSubmit, txtBtnSubmit, classLi, ClassBtn, url, queryStrings, bodyModel, btnRemoved) {
    loaderOpen();
    $(titleModel).text(textTitleModel);
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
        $(btnRemoved).remove();
        $(btnSubmit).removeClass().addClass(ClassBtn).append('<li class="' + classLi + '"> ' + txtBtnSubmit + ' </li>');
        loaderClose();
    });
}

function openModel___s(titleModel, textTitleModel, btnSubmit, txtBtnSubmit, classLi, ClassBtn, url, queryStrings, bodyModel) {
    loaderOpen();
    $(titleModel).text(textTitleModel);
    queryStrings = queryStrings.replaceAll(' ', '%20');
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
        $(btnSubmit).removeClass().addClass(ClassBtn).append('<li class="' + classLi + '"> ' + txtBtnSubmit + ' </li>');
        loaderClose();
    });

}

function openModel___ss(titleModel, textTitleModel, btnSubmit, txtBtnSubmit, classLi, ClassBtn, url, queryStrings, bodyModel) {
    loaderOpen();
    $(titleModel).text(textTitleModel);

    $(bodyModel).load(url + queryStrings, function () {
        $(btnSubmit).removeClass().addClass(ClassBtn).append('<li class="' + classLi + '"> ' + txtBtnSubmit + ' </li>');
        loaderClose();
    });

}

function openModel_(modelName, titleModel, textTitleModel, url, queryStrings, values, bodyModel) {
    loaderOpen();
    $(modelName).modal('show');
    $(titleModel).text(textTitleModel)
    var url_ = url;
    if (queryStrings != "") {

        var qs = queryStrings.split(',');
        var values_ = values.split(',');
        var value3 = values_[0].split("'");
        url_ = url_ + "?" + qs[0] + "=" + Number(values_[0]);
    }

    $(bodyModel).load(url_, function () { loaderClose(); });
}

function openModelAsly(modelName, titleModel, textTitleModel, url, queryStrings, bodyModel) {
    loaderOpen();
    $(modelName).modal('show');
    $(titleModel).text(textTitleModel);
    if (queryStrings != "") {
        var qs = queryStrings.split(',');

        url = url + "?";
        for (var i = 0 ; i < qs.length ; i++) {
            if (i != 0)
                url = url + "&";
            url = url + qs[i];
        }
    }

    $(bodyModel).load(url, function () { loaderClose(); });

}

function loaderOpen() {
    $("#loader101").css("display", "block");
}
function loaderClose() {
    $("#loader101").css("display", "none");
}
//delete using sweetalert
function sAlertDel(btn_del, textAlert, confirmBtnText, cancelBtnText, control, action, queryStrings) {
    Swal.fire({
        text: textAlert,
        type: aAlertIcons[4],
        showCancelButton: true,
        showConfirmButton: true,
        confirmButtonClass: "btn-sweet101",
        cancelButtonClass: "btn-sweet101",
        showLoaderOnConfirm: true,
        confirmButtonText: " <i class='fas fa-trash-alt'></i> " + confirmBtnText,
        cancelButtonText: " <i class=' fas fa-times'></i> " + cancelBtnText
        //timer: 15000
    }).then((result) => {
        if (result.value) {
            window.location.href = "/" + control + "/" + action + "?" + queryStrings;
            return true;
        }
        else {
            return false;
        }
    })
}
function deleteClick(btn_del, textAlert, confirmBtnText, cancelBtnText) {

    Swal.fire({
        text: textAlert,
        type: aAlertIcons[4],
        showCancelButton: true,
        showConfirmButton: true,
        confirmButtonClass: "btn-sweet101",
        cancelButtonClass: "btn-sweet101",
        showLoaderOnConfirm: true,
        confirmButtonText: " <i class='fas fa-trash-alt'></i> " + confirmBtnText,
        cancelButtonText: " <i class=' fas fa-times'></i> " + cancelBtnText
        //timer: 15000
    }).then((result) => {
        if (result.value) {
            $(btn_del).closest('tr').remove();
            $('#tblBody tr').each(function (i, obj) {

                if (i == 0) {
                    // الرقم القومى
                    if ($(obj).find('td:eq(1)').text() == '---------')
                        $('#hNationalID').val('null');
                    else
                        $('#hNationalID').val($(obj).find('td:eq(1)').text());

                    // الرقم التأمينى
                    if ($(obj).find('td:eq(2)').text() == '---------')
                        $('#hInsuranceNumber').val('null');
                    else
                        $('#hInsuranceNumber').val($(obj).find('td:eq(2)').text());

                    $('#workerId').val($(obj).find('td:eq(5)').attr('data-val'));
                    $('#hWorkerCareer').val($(obj).find('td:eq(3)').attr('data-val'));
                    $('#hSkillDegree').val($(obj).find('td:eq(4)').attr('data-val'));

                } else {
                    // الرقم القومى
                    if ($(obj).find('td:eq(1)').text() == '---------')
                        $('#hNationalID').val($('#hNationalID').val() + ',' + 'null');
                    else
                        $('#hNationalID').val($('#hNationalID').val() + ',' + $(obj).find('td:eq(1)').text());

                    // الرقم التأمينى
                    if ($(obj).find('td:eq(2)').text() == '---------')
                        $('#hInsuranceNumber').val($('#hInsuranceNumber').val() + ',' + 'null');
                    else
                        $('#hInsuranceNumber').val($('#hInsuranceNumber').val() + ',' + $(obj).find('td:eq(2)').text());

                    $('#workerId').val($('#workerId').val() + ',' + $(obj).find('td:eq(5)').attr('data-val')); // كود العاملس
                    $('#hWorkerCareer').val($('#hWorkerCareer').val() + ',' + $(obj).find('td:eq(3)').attr('data-val')); // المهنه
                    $('#hSkillDegree').val($('#hSkillDegree').val() + ',' + $(obj).find('td:eq(4)').attr('data-val')); // مستوى المهاره
                }

            });
            return true;
        }
        else {
            return false;
        }
    })
}

//accept using sweetalert
function sAlertBasic(textAlert, index) {
    Swal.fire({
        text: textAlert,
        type: aAlertIcons[index]
    })
}

function sAlertAccept(btn_del, textAlert, confirmBtnText, cancelBtnText, control, action, queryStrings) {
    Swal.fire({
        text: textAlert,
        type: aAlertIcons[4],
        showCancelButton: true,
        showConfirmButton: true,
        cancelButtonClass: "btn-sweet101",
        showLoaderOnConfirm: true,
        confirmButtonText: " <i class='fas fa-check'></i> " + confirmBtnText,
        cancelButtonText: " <i class=' fas fa-times'></i> " + cancelBtnText
        //timer: 15000
    }).then((result) => {
        if (result.value) {
            window.location.href = "/" + control + "/" + action + "?" + queryStrings;
            return true;
        }
        else {
            return false;
        }
    })
}

//clear using sweetalert
function sAlertclear(btn_del, textAlert, confirmBtnText, cancelBtnText) {
    Swal.fire({
        text: textAlert,
        type: aAlertIcons[4],
        showCancelButton: true,
        showConfirmButton: true,
        confirmButtonClass: "btn-sweet101 text-primary",
        cancelButtonClass: "btn-sweet101 text-info",
        showLoaderOnConfirm: true,
        confirmButtonText: " <i class='fas fa-broom'></i> " + confirmBtnText,
        cancelButtonText: "  <i class=' fas fa-times'></i> " + cancelBtnText,
    }).then((result) => {
        if (result.value) {
            $('form input').val('');
            return true;
        }
        else {
            return false;
        }
    })
}

function displayDivsCkbox(controlDisplay, controlNotDisplay, divCkbox, displayDivCkbox, ckbox, valueCkbox) {
    $(controlDisplay).css('display', 'inline-block');
    $(controlNotDisplay).css('display', 'none');
    $(divCkbox).css('display', displayDivCkbox);
    $(ckbox).prop('checked', valueCkbox);
}

function sAlert(text, index) {
    swal({
        type: aAlertIcons[index],
        title: text,
        showCloseButton: false,
        showCancelButton: false,
        showConfirmButton: true// ok
        //timer: 2000
    })
}

function closeDiv(openDiv) {
    $(openDiv).hide("slow");
}

function openDiv(openDiv) {
    $(openDiv).show("slow");
}

function openDivDown(openDiv) {
    $(openDiv).slideUp("slow");
    $(openDiv).slideDown("slow");
}

function openDivFade(openDiv) {
    $(openDiv).fadeOut("slow");
    $(openDiv).fadeIn("slow");
}

function openDivUp(openDiv) {
    $(openDiv).slideUp("slow");
}

/* start tree*/
var toggler = document.getElementsByClassName("caret");
var i;

for (i = 0; i < toggler.length; i++) {
    toggler[i].addEventListener("click", function () {
        this.parentElement.querySelector(".nested").classList.toggle("active");
        this.classList.toggle("caret-down");
    });
}

function grid() {
    var x = document.getElementById("grid");
    var y = document.getElementById("list");
    x.style.display = "flex";
    y.style.display = "none";
}
function list() {
    var x = document.getElementById("grid");
    var y = document.getElementById("list");
    x.style.display = "none";
    y.style.display = "block";
}

/* end tree*/

/* Attachment Start */
function openAttachmentSearchOrAdd(titleModel, textTitleModel, btn, url, nestedFolder, queryString, bodyModel) {
    $(titleModel).text(textTitleModel);
    $(btn).addClass('active').siblings().removeClass('active');
    url = url + "?nFolder=" + nestedFolder;
    if (queryString != "") {
        var qs = queryString.split(',');
        url = url + "&";
        for (var i = 0 ; i < qs.length ; i++) {
            url = url + qs[i];
        }
    }
    $(bodyModel).load(url, function () { });
    $(bodyModel).css('display', 'initial');
}

function openAttachmentModel(urlAdd, urlSearch, nestedFolder, queryStringAdd, queryStringSearch) {
    $('#btnSharedAttachmentSearch,#btnSharedAttachmentAdd').removeClass('active');
    $('#attAttachmentSearchModelBody,#divDisplaySharedSearchAddAttchment,#divIFrame').css('display', 'none');
    $('#btnSharedAttachmentSearch').attr('onclick', 'openAttachmentSearchOrAdd("","",this,"' + urlSearch + '","' + nestedFolder + '","' + queryStringSearch + '","#divDisplaySharedSearchAddAttchment")');
    $('#btnSharedAttachmentAdd').attr('onclick', 'openAttachmentSearchOrAdd("","",this,"' + urlAdd + '","' + nestedFolder + '","' + queryStringAdd + '","#divDisplaySharedSearchAddAttchment")');
}

function openFile(doc_path) {
    doc_path = doc_path.replace(/,/g, '/');
    openDiv('#divIFrame');
    console.log('doc_path: ',doc_path);
    $("#attachmentFrame").attr("src", doc_path);
}

function toggleTr(btnToggle) {
    $(btnToggle).slideToggle();
}

function sAlertDelFolder(btn_del, textAlert, confirmBtnText, cancelBtnText, control, action, nFolder, docName, rootFolder) {
    Swal.fire({
        text: textAlert,
        type: aAlertIcons[4],
        showCancelButton: true,
        showConfirmButton: true,
        confirmButtonClass: "btn-sweet101",
        cancelButtonClass: "btn-sweet101",
        showLoaderOnConfirm: true,
        confirmButtonText: " <i class='fas fa-trash-alt'></i> " + confirmBtnText,
        cancelButtonText: " <i class=' fas fa-times'></i> " + cancelBtnText
        //timer: 15000
    }).then((result) => {
        if (result.value) {

            // file remove
            if (rootFolder == '') {
                var badge = $(btn_del).closest('tr.collapse').prevAll('tr[role="button"]').first().find('td:first-child > .badge').first();
            }
            $.getJSON("../" + control + "/" + action, { filePath: nFolder, docName: docName },
                      function (data) {
                          if (data) {
                              sAlert('تم الحذف بنجاح', 0);
                              $(btn_del).closest('tr').remove();
                              $(rootFolder).remove();
                              closeDiv('#divIFrame');
                              $(badge).text(Number($(badge).text()) - 1);
                          }
                          else {
                              sAlert('عفوا , لم يتم الحذف بنجاح.', 1);
                          }

                      });
            return true;
        }
        else {
            return false;
        }
    })
}
/*pagging using Jq*/

function PaggingTemplate(totalPage, currentPage, Name) {
    var template = "";
    var TotalPages = totalPage;
    var CurrentPage = currentPage;
    var PageNumberArray = Array();
    var countIncr = 1;
    for (var i = currentPage; i <= totalPage; i++) {
        PageNumberArray[0] = currentPage;
        if (totalPage != currentPage && PageNumberArray[countIncr - 1] != totalPage) {
            PageNumberArray[countIncr] = i + 1;
        }
        countIncr++;
    };
    PageNumberArray = PageNumberArray.slice(0, 5);
    var FirstPage = 1;
    var LastPage = totalPage;
    if (totalPage != currentPage) {
        var ForwardOne = currentPage + 1;
    }
    var BackwardOne = 1;
    if (currentPage > 1) {
        BackwardOne = currentPage - 1;
    }
    template = template + '<ul class="pagination ml-5" >' +
                          '<li class="page-item"><a  href="#" onclick="' + Name + '(' + FirstPage + ')" class="hvr-bob page-link"> << </a></li>';  //FirstItem ;

    var numberingLoop = "";
    for (var i = 0; i < PageNumberArray.length; i++) {
        numberingLoop = numberingLoop + '<li class="page-item"> <a class="page-number active hvr-bob page-link" onclick="' + Name + '(' + PageNumberArray[i] + ')" href="#">' + PageNumberArray[i] + ' &nbsp;&nbsp;</a></li>'
    }
    template = template + numberingLoop +
                               '<li class="page-item"><a href="#" class="hvr-bob page-link" onclick="' + Name + '(' + LastPage + ')" > >> </a></li></ul>';
    $("#paged").append(template);
    $('#masterPaged').fadeIn();
}


function PaggingTemplate(totalPage, currentPage, Name, url, msg, tbID) {
    AlerMsg = msg;
    tableId = tbID;
    URL = url;
    var template = "";
    var TotalPages = totalPage;
    var CurrentPage = currentPage;
    var PageNumberArray = Array();
    var countIncr = 1;
    for (var i = currentPage; i <= totalPage; i++) {
        PageNumberArray[0] = currentPage;
        if (totalPage != currentPage && PageNumberArray[countIncr - 1] != totalPage) {
            PageNumberArray[countIncr] = i + 1;
        }
        countIncr++;
    };
    PageNumberArray = PageNumberArray.slice(0, 5);
    var FirstPage = 1;
    var LastPage = totalPage;
    if (totalPage != currentPage) {
        var ForwardOne = currentPage + 1;
    }
    var BackwardOne = 1;
    if (currentPage > 1) {
        BackwardOne = currentPage - 1;
    }
    template = template + '<ul class="pagination ml-5" >' +
                          '<li class="page-item"><a  href="#" onclick="' + Name + '(' + FirstPage + ',' + url + ',' + msg + ')" class="hvr-bob page-link"> << </a></li>';  //FirstItem ;
    var numberingLoop = "";
    for (var i = 0; i < PageNumberArray.length; i++) {
        numberingLoop = numberingLoop + '<li class="page-item"> <a class="page-number active hvr-bob page-link" onclick="' + Name + '(' + PageNumberArray[i] + ',' + url + ',' + msg + ')" href="#">' + PageNumberArray[i] + ' &nbsp;&nbsp;</a></li>'
    }
    template = template + numberingLoop +
                               '<li class="page-item"><a href="#" class="hvr-bob page-link" onclick="' + Name + '(' + LastPage + ',' + url + ',' + msg + ')" > >> </a></li></ul>';
    $("#paged").append(template);
    $('#masterPaged').fadeIn();
}
function GetPaggingJq(pageNum, url, msg) {
    console.log('page number = ', pageNum);
    console.log('url = ', url);
    $("#paged").empty();
    $.ajax({
        url: url,
        data: { 'pageIndex': pageNum },
        type: "post",
        cache: false,
        success: OnSuccess,
        error: function (response) {
            sAlert(msg);
        }
    });
}
function OnSuccess(response) {
    console.log('table ID = ', tableId);
    if (response.length > 0) {
        $(tableId + " tbody").empty();
        var model = response.LModels;
        $.each(model, function (index) {
            $(tableId).find('tbody').append('<tr">'
            + '<td class="text-center" id="ConName">' + model[index][0] + '</td>' // اسم المقاول
            + '<td class="text-center" id="ConNum">' + model[index][1] + '</td>' // اسم الرقم التأمينى للمقاول
            + '</tr>');
        });
        PaggingTemplate(response.TotalPages, response.CurrentPage, 'GetPaggingJq', URL, msg, tableId);
    }
    else {
        sAlert(AlerMsg);
        $(tableId + "tbody").empty();
    }
};

/*pagging in modal at runTime */
function PaggingTemplate(totalPage, currentPage, Name) {
    var template = "";
    var TotalPages = totalPage;
    var CurrentPage = currentPage;
    var PageNumberArray = Array();
    var countIncr = 1;
    for (var i = currentPage; i <= totalPage; i++) {
        PageNumberArray[0] = currentPage;
        if (totalPage != currentPage && PageNumberArray[countIncr - 1] != totalPage) {
            PageNumberArray[countIncr] = i + 1;
        }
        countIncr++;
    };
    PageNumberArray = PageNumberArray.slice(0, 5);
    var FirstPage = 1;
    var LastPage = totalPage;
    if (totalPage != currentPage) {
        var ForwardOne = currentPage + 1;
    }
    var BackwardOne = 1;
    if (currentPage > 1) {
        BackwardOne = currentPage - 1;
    }
    template = template + '<ul class="pagination ml-5" >' +
                          '<li class="page-item"><a  href="#" onclick="' + Name + '(' + FirstPage + ')" class="hvr-bob page-link"> << </a></li>';  //FirstItem ;
    var numberingLoop = "";
    for (var i = 0; i < PageNumberArray.length; i++) {
        numberingLoop = numberingLoop + '<li class="page-item"> <a class="page-number active hvr-bob page-link" onclick="' + Name + '(' + PageNumberArray[i] + ')" href="#">' + PageNumberArray[i] + ' &nbsp;&nbsp;</a></li>'
    }
    template = template + numberingLoop +
                               '<li class="page-item"><a href="#" class="hvr-bob page-link" onclick="' + Name + '(' + LastPage + ')" > >> </a></li></ul>';
    $("#paged").append(template);
    $('#masterPaged').fadeIn();
}
/* Start Pagging Using Jq*/
function PaggingTemplate(totalPage, currentPage, Name, url, msg, tbID, pageSize) {
    AlerMsg = msg;
    tableId = tbID;
    URL = url;
    PageSize = pageSize;
    TotalPages = totalPage;
    var template = "";
    var TotalPages = totalPage;
    var CurrentPage = currentPage;
    var PageNumberArray = Array();
    var countIncr = 1;
    for (var i = currentPage; i <= totalPage; i++) {
        PageNumberArray[0] = currentPage;
        if (totalPage != currentPage && PageNumberArray[countIncr - 1] != totalPage) {
            PageNumberArray[countIncr] = i + 1;
        }
        countIncr++;
    };
    PageNumberArray = PageNumberArray.slice(0, 5);
    var FirstPage = 1;
    var LastPage = totalPage;
    if (totalPage != currentPage) {
        var ForwardOne = currentPage + 1;
    }
    var BackwardOne = 1;
    if (currentPage > 1) {
        BackwardOne = currentPage - 1;
    }
    template = template + '<ul class="pagination ml-5" >' +
                          '<li class="page-item"><a  href="#" onclick="' + Name + '(' + FirstPage + ',' + url + ',' + msg + ')" class="hvr-bob page-link"> << </a></li>';  //FirstItem ;
    var numberingLoop = "";
    for (var i = 0; i < PageNumberArray.length; i++) {
        numberingLoop = numberingLoop + '<li class="page-item"> <a class="page-number active hvr-bob page-link" onclick="' + Name + '(' + PageNumberArray[i] + ',' + url + ',' + msg + ')" href="#">' + PageNumberArray[i] + ' &nbsp;&nbsp;</a></li>'
    }
    template = template + numberingLoop +
                               '<li class="page-item"><a href="#" class="hvr-bob page-link" onclick="' + Name + '(' + LastPage + ',' + url + ',' + msg + ')" > >> </a></li></ul>';
    $("#paged").append(template);
    $('#masterPaged').fadeIn();
}
function GetPageData(pageNum, url, msg) {
    $("#paged").empty();
    $.ajax({
        url: url,
        data: { 'pageIndex': pageNum },
        type: "post",
        cache: false,
        success: function (response) {
            if (url == 'contractor') {
                contractors(response);
            }
            else if (url == '../Workers/GetWorkers') {
                workers(response);
            }
            else if (url == '../Contractor/GetContractorData') {
                SubContractors(response);
            }
            else if (url == '../Workers/GetWorkersProcess') {
                ProcessWorkers(response);
            }
        },
        error: function (response) {
            sAlert(msg);
        }
    });
}
// contractors المقاولين
function contractors(response) {
    console.log('response.lrefConModel :', response.lrefConModel);
    console.log('response.lrefConModel.length :', response.lrefConModel.length);
    if (response.lrefConModel.length > 0) {
        document.getElementById("divContractor").style.display = "block";
        $("#tbContractors tbody").empty();
        var model = response.lrefConModel;
        $.each(model, function (index) {
            $('#tbContractors').find('tbody').append('<tr  onclick="GetDetails(this);">'
            + '<td class="text-center" id="ConName">' + model[index].sReferenceSideContractorName + '</td>' // اسم المقاول
            + '<td class="text-center" id="ConNum">' + model[index].sReferanceSideContractorNum + '</td>' // اسم الرقم التأمينى للمقاول
            + '</tr>');
        });
        PaggingTemplate(response.TotalPages, response.CurrentPage, 'GetPageData');
    }
    else if (response.lrefConModel.length == 0) {
        sAlert('عفوا ,هذا المقاول تم أدخاله سابقا.');
        $('#contractName').val('');
        document.getElementById("divContractor").style.display = "none";
        $("#tbContractors tbody").empty();
    }
    else {
        sAlert('عفوا ,هذا الاسم غير صحيح من فضلك تأكد من أسم المقاول.');
        $('#contractName').val('');
        document.getElementById("divContractor").style.display = "none";
        $("#tbContractors tbody").empty();
    }
}
// workers العمال
function workers(response) {
    if (response.LModels.length > 0) {
        $(tableId + " tbody").empty();
        var model = response.LModels;
        $.each(model, function (index) {
            $(tableId).find('tbody').append('<tr>'
             + '<td class="text-center font-weight-bold color_main102">' + Number((Number(response.CurrentPage) * Number(PageSize)) - (Number(PageSize) - Number(index + 1))) + '</td>'
             + '<td class="text-center">' + model[index].sWorkerName + '</td>' // اسم العامل
             + '<td class="text-center">' + model[index].sWorkerInsuranceNum + '</td>' //  الرقم التأمينى للعامل
             + '<td class="text-center">' + model[index].sWorkerNationalID + '</td>' //  الرقم القومى للعامل
             + '<td class="text-center">' + model[index].sLastAttendance + '</td>' //  تاريخ اخر حضور  
             + '<td class="text-center">' + model[index].sCareerName + '</td>' //  مهنه  للعامل
             + '</tr>');
        });
        PaggingTemplate(response.TotalPages, response.CurrentPage, 'GetPageData', URL, AlerMsg, tableId, PageSize);
    }
    else {
        sAlert(AlerMsg);
        $(tableId + "tbody").empty();
    }
};
/* Start Pagging Using Jq*/
//// المقاولين من الباطن في تفاصيل العمليه للمقاول
function SubContractors(response) {
    if (response.LoProcessSubContractorMode.length > 0) {
        $(tableId + " tbody").empty();
        var model = response.LoProcessSubContractorMode;
        $.each(model, function (index) {
            var processNumber = '';
            if (model[index].sContractorProcessNumber == null)
                processNumber = 'لا يوجد';
            else
                processNumber = model[index].sContractorProcessNumber;
            $(tableId).find('tbody').append('<tr>'
             + '<td class="text-center font-weight-bold color_main102">' + Number((Number(response.CurrentPage) * Number(PageSize)) - (Number(PageSize) - Number(index + 1))) + '</td>'
             + '<td class="text-center">' + model[index].sContractorName + '</td>' // اسم المقاول
             + '<td class="text-center">' + model[index].iContractorNationalNumber + '</td>' //  الرقم التأمينى للمقاول
               + '<td class="text-center">' + processNumber + '</td>' // بالعمليه الرقم  للمقاول
             + '</tr>');
        });
        PaggingTemplate(response.TotalPages, response.CurrentPage, 'GetPageData', URL, AlerMsg, tableId, PageSize);
    }
    else {
        if (AlerMsg == null) {
            sAlert(AlerMsg);
        }
        $(tableId + "tbody").empty();
    }
}
//// العمليات في العمال في موظف التأمينات
function ProcessWorkers(response) {
    if (response.LprocessModel.length > 0) {
        $(tableId + " tbody").empty();
        var model = response.LprocessModel;
        $.each(model, function (index) {
            $(tableId).find('tbody').append('<tr>'
             + '<td class="text-center font-weight-bold color_main102">' + Number((Number(response.CurrentPage) * Number(PageSize)) - (Number(PageSize) - Number(index + 1))) + '</td>'
             + '<td class="text-center">' + (model[index].sProcessName == null ? "----" : model[index].sProcessName) + '</td>'
             + '<td class="text-center">' + model[index].sProcessTypeName + '</td>'
             + '<td class="text-center">' + (model[index].sProcessSite == null ? "----" : model[index].sProcessSite) + '</td>'
             + '<td class="text-center">' + model[index].sDateStartProcess + '</td>'
             + '<td class="text-center">' + (model[index].sDateEndProcess == null ? "لم تنتهى بعد" : model[index].sDateEndProcess) + '</td>'
             + '<td class="text-center">' + model[index].sReferneceSideName + '</td>'
             + '<td class="text-center">' + model[index].sContractorName + '</td>'
             + '</tr>');
        });
        PaggingTemplate(response.TotalPages, response.CurrentPage, 'GetPageData', URL, AlerMsg, tableId, PageSize);
    }
    else {
        if (AlerMsg == null) {
            sAlert(AlerMsg);
        }
        $(tableId + "tbody").empty();
    }
}
/* Attachment End */

$(function () {
    $('.selectpicker').selectpicker();

    //$('.date').datepicker({ startView: "days", format: "yyyy-mm-dd", autoclose: true, todayHighlight: true, showWeekDays: false, clearBtn: true, todayBtn: true }).bind('cut copy paste', function (e) {
    //    e.preventDefault();
    //});
});



/* Start Cookies*/

function getCookieValue(a) {
    var b = document.cookie.match('(^|;)\\s*' + a + '\\s*=\\s*([^;]+)');
    return b ? b.pop() : '';
}

function setCookieValue() {
    $.getJSON("../jsonFunctions/setIsMutedCookie", {}, function (data) {
        // console.log('data: ', data);
        play();
    })
}
/* End Cookies*/

/* Start Reference Side - Contractors */
function getRefSideContByInsurNumLegalEntity(legalEntity, insNum, txtInsName, insNumCode) {

    if ($(insNum).val() != '' && $(legalEntity).val() != '') {

        $.getJSON("../Process/getRefSideContByInsurNumLegalEntity", { legalEntity: $(legalEntity).val(), insNum: $(insNum).val() },
       function (data) {
           if (data.length == 0) {
               sAlert('عفوا ,هذا الرقم غير صحيح .. الرجاء التأكد من الرقم ثم إعاده كتابته بشكل صحيح.');
               $(insNum).val('');
           }
           else {
               // غير مفعل
               if (data.oRefSideCont.sIsActive == 'غير مفعل') {
                   sAlert('عفوا ,هذا الرقم غير مفعل...', 1);
                   $(insNum).val('');
                   $(txtInsName).val('');
               }
               else if (data.oRefSideCont.iReferenceSideContractorCode == '0') {
                   sAlert('عفوا ,هذا الرقم غير مسجل على هذا الكيان القانونى...', 1);
                   $(insNum).val('');
                   $(txtInsName).val('');
               }
               else {
                   $(txtInsName).val(data.oRefSideCont.sReferenceSideContractorName);
                   $(insNumCode).val(data.oRefSideCont.iReferenceSideContractorCode);
               }
           }
       });
    }
    else if ($(legalEntity).val() == '') {
        sAlert('الرجاء اختيار الكيان القانونى اولا.', 2);

        $(insNum).val('');
        $(txtInsName).val('');
    }
}
/* End Reference Side - Contractors */