Dropzone.options.dropzoneForm = {
    autoProcessQueue: false,
    addRemoveLinks: true,
    removeAllFiles: true,
    acceptedFiles: ".jpg,.png,.jpeg,.tiff,.pdf",
    parallelUploads: 100,
    thumbnailHeight: 120,
    thumbnailWidth: 120,
    autoDiscover: false,
    maxFiles: 2000,
    dictCancelUpload: 'جارى رفع الملف',
    dictCancelUploadConfirmation: "Are you sure ?",
    dictRemoveFile: 'حذف الملف',
    createImageThumbnails: true,
    showFiletypeIcon: true,
    previewsContainer: ".dropzone-previews",
    //renameFilename: function renameFilename(file) {
    //    return "YourNewfileName." + file.split('.').pop();
    //},
    //renameFile: function(file) {
    //    //file.name = new Date().getTime() + '_' + file.name; // results in Uncaught TypeError: Cannot assign to read only property 'name' of object '#<File>'
    //    // or
    //    return file.upload.filename = new Date().getTime() + '_' + file.name; // results in Uncaught TypeError: Cannot set property 'filename' of undefined
    //},
    removedfile: function (file) {
        var _ref;
        return (_ref = file.previewElement) != null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
    },
    //maxFilesize: 5,
    url: '../Attachments/apply_upload',
    data: {
        nFolder: $('#nFolder').val(),
        actionName: $('#actionName').val(),
        controllerName: $('#controllerName').val(),
        packageAttachmentsName: $('#packageAttachmentsName').text(),
        attachmentType: $('#attachmentType').val()
    },
    init: function () {

        var myDropzone = this;

        $("#save_file").click(function (e) {
            e.preventDefault();
            
            if ($('#packageAttachmentsName').val() == '') {
                swal({
                    title: ' الرجاء التأكد من ادخال اسم المرفق واختيار المرفق.',
                    type: "error",
                    showCloseButton: false,
                    showCancelButton: false,
                    showConfirmButton: false,// ok
                    timer: 2000
                });
                loaderClose();
            } else {
                myDropzone.processQueue();
                
            }
        });

        this.on('sending', function (file, xhr, formData, e) {
            
            console.log('e: ',e);
            console.log('formData: ', formData);
            // Append all form inputs to the formData Dropzone will POST
            var data = $('#dropzoneForm').serializeArray();
            loaderFilesOpen();
            console.log('data Length: ',data);
            $.each(data, function (key, el) {
               
                formData.append(el.name, el.value);
            });

        });
        this.on('success', function (file, xhr, formData) {

            //swal({
            //    title: 'تم حفظ الملفات بنجاح',
            //    type: "success",
            //    showCloseButton: false,
            //    showCancelButton: false,
            //    showConfirmButton: false,// ok
            //    timer: 2000
            //});

            //window.location.reload(true);
            //console.log('success reload');

        });
        this.on('queuecomplete', function (file, xhr, formData) {

            loaderFilesClose();

            //setTimeout(function () { alert("Hello"); }, 3000);
             sAlertBasic('تم الحفظ بنجاح.',0);
        });
        this.on('error', function (file, response) {
            loaderFilesClose();
            swal({
                title: 'هناك مشكله ببعض الملفات',
                type: "error",
                showCloseButton: false,
                showCancelButton: false,
                showConfirmButton: false,// ok
                timer: 2000
            });
        });
    }

};

function loaderFilesOpen() {
    window.parent.$("#loader101").css("display", "block");
}
function loaderFilesClose() {
    window.parent.$("#loader101").css("display", "none");
}