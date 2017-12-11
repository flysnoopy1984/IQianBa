var IdCardFile1Done = false;
var IdCardFile2Done = false;
var IdCardFile3Done = false;

$(document).ready(function () {

    var url = "/Wap/UploadVerifyFile";
    $('#IdCardFile1').hide();

    $("#progress").hide();

    $('#IdCardFile1').fileupload({
        autoUpload: true,
        url: url,
        fileName:'test',
        dataType: 'json',
        allowedTypes: /(.|\/)(jpe?g|png)$/i,
        showStop: true,
        maxFileSize: 10000000,
        maxNumberOfFiles: 1, 
        recalculateProgress:true,
        done: function (e, data) {
            $("#uploadImg").attr({ src: data.result.ImgSrc });
            $("#uploadImg").css({ width: "290px", height: "218px" });
            IdCardFile1Done = true;
        },
        add: function (e, data) {

            $("#progress").show();
            $("#lb_IdCardFile1").hide();
            //   alert(data.files[0].name);
            var jqXHR;
            data.process().done(function () {
                jqXHR = data.submit();
                
            });

            $('#cancel_IdCardFile1').click(function () {
                if (IdCardFile1Done)
                {
                    $("#progress").hide();
                    $('.ProcessBar').css("width","0%");
                    $("#lb_IdCardFile1").show();
                    $("#uploadImg").attr({ src: "" });
                    $("#uploadImg").css({ width: "0px", height: "0px" });
                }
                if (jqXHR)
                    jqXHR.abort();
              
            })
        },
        fail: function (e, data) {
            alert(data.errorThrown);
        },


        progressall: function (e, data) {

            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('.ProcessBar').css("width", progress + "%");
        //    $('.ProcessBar').text(progress + "%");

        }
    });
});
