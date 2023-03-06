function AttachmentSearchOrEditClick(btn) {
    if (btn) {
        $('#titleAttachmentSearchOrEditModal').text('تعديل اسم المرفق');
        $('#btnAttachmentSearchOrEdit > i').removeClass().addClass('far fa-edit font-weight-bold').text(' تعديل اسم المرفق');
        $('#btnAttachmentSearchOrEdit').removeClass().addClass('btn btn-outline-warning font-weight-bold');
    }
    else {
        $('#titleAttachmentSearchOrEditModal').text('بحث عن اسم المرفق');
        $('#btnAttachmentSearchOrEdit > i').removeClass().addClass('fas fa-search font-weight-bold').text(' بحث');
        $('#btnAttachmentSearchOrEdit').removeClass().addClass('btn btn-outline-primary font-weight-bold');

    }
}
//function sAlertDel(btn_del) {
//    Swal.fire({
//        text: 'هل تريد حذف هذا المرفق ؟',
//        type: aAlertIcons[4],
//        showCancelButton: true,
//        confirmButtonColor: '#2899b6',
//        cancelButtonColor: '#d33',
//        showLoaderOnConfirm: true,
//        confirmButtonText: 'حذف المرفق',
//        cancelButtonText: 'إلغاء الحذف',
//        timer: 15000
//    }).then((result) => {
//        if (result.value) {
//            $(btn_del).submit();

//            return true;
//        }
//        else {
//            return false;
//        }
//    })
//}
function closeDiv(openDiv) {
    $(openDiv).hide("slow");
}

function openDiv(openDiv) {
    $(openDiv).show("slow");
    //$(openDiv).slideDown();
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

var showFile = function () {
    openDiv('#divIFrame');
    $("#attachmentFrame").attr("src", "/img/anyThing.pdf");
    //$("#attachmentFrame").attr("src", "/img/mss.png");
    //$("#attachmentFrame").attr("src", "/img/logo.png");
}

