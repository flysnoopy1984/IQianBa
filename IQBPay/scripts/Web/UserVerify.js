var IdCardFile1Done = false;
var IdCardFile2Done = false;
var IdCardFile3Done = false;

function InitUploadFile(n)
{
    $("#Progress"+n).hide();
    $("#ProcessBar"+n).css("width", "0%");
    $("#lb_IdCardFile"+n).show();
    $("#uploadImg"+n).attr({ src: "" });
    $("#uploadImg"+n).css({ width: "0px", height: "0px" });
}

function ConfigUploadFile(n)
{
    var url = "/Wap/UploadVerifyFile";
    $("#IdCardFile"+n).hide();
    $("#Progress"+n).hide();
    $("#IdCardFile"+n).fileupload({
        autoUpload: true,
        url: url,
       formdata:"",
        dataType: 'json',
        allowedTypes: /(.|\/)(jpeg|png|jpg)$/i,
        showStop: true,
        maxFileSize: 10000000,
        maxNumberOfFiles: 1,
        recalculateProgress: true,
        done: function (e, data) {
            $("#uploadImg"+n).attr({ src: data.result.ImgSrc });
            $("#uploadImg"+n).css({ width: "290px", height: "218px" });
            IdCardFile1Done = true;
        },
        add: function (e, data) {

            $("#Progress"+n).show();
            $("#lb_IdCardFile"+n).hide();
            //   alert(data.files[0].name);
            var jqXHR;
            data.process().done(function () {
                jqXHR = data.submit();

            });

            $("#cancel_IdCardFile"+n).click(function () {
                if (IdCardFile1Done) {
                    InitUploadFile(n);
                }
                if (jqXHR)
                    jqXHR.abort();

            })
        },
        fail: function (e, data) {
            if (data.errorThrown == "abort") {
                InitUploadFile(n);
            }
        },


        progressall: function (e, data) {

            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#ProcessBar'+n).css("width", progress + "%");
            //    $('.ProcessBar').text(progress + "%");

        }
    });
}

$(document).ready(function () {

    ConfigUploadFile("1");
    ConfigUploadFile("2");
    ConfigUploadFile("3");
});

function Submit()
{
    var name = $("#Name").val();
    var Phone = $("#Phone").val();
    var uploadImg1 = $("#uploadImg1").attr("src");
    alert(uploadImg1);

    var uploadImg2 = $("#uploadImg2").attr("src");
    alert(uploadImg2);


}