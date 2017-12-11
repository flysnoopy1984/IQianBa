$(document).ready(function () {

    var url = "/Wap/UploadVerifyFile";
    $('#fileupload').hide();

    $('#fileupload').fileupload({
        autoUpload: true,
     
        url: url,
        fileName:'test',
        dataType: 'json',
        allowedTypes: /(.|\/)(jpe?g|png)$/i,
        showDelete: true,
        done: function (e, data) {
            $("#uploadImg").attr({ src: data.result.ImgSrc });
            $("#uploadImg").css({ width: "290px", height: "218px" });
        },
        add: function (e, data) {
            alert(data.files[0].name);
            data.process().done(function () {
                data.submit();
            });
        },

     
        progressall: function (e, data) {

            var progress = parseInt(data.loaded / data.total * 100, 10);

            $('.ProcessBar').text(progress + "%");

        }
    });
});
