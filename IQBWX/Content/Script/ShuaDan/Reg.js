$(function () {
    var openId;

    SMSSuccess = function () {

        var url = "/ShuaDan/GetUserPhone?openId=" + openId;
        $.ajax({
            type: 'post',
            dataType: "json",
            data: { "Phone": $("#phone_num").val() },
            url: url,
            success: function (data) {

                if (data.IsSuccess) {
                    $("#ResultArea").show();
                    $("#main").hide();
                    //alert("手机校验通过！");
                   // window.location.href = "/O2OWap/EntryAgent";
                }
                else
                    alert(data.ErrorMsg);

            },
            error: function (xhr, type) {

                alert(xhr.responseText);

            }
        });
    };
    StartBlockUI = function (txt, w) {

        if (w == undefined)
            w = 100;
        var msg = ' <div id="ProcessArea1" class="progress progress-striped active">';
        msg += '<div id="upload_progress1" class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: ' + w + '%;">';
        msg += '<span class="sr-only">' + txt + '</span>';
        msg += '</div>';
        msg += '</div>';

        ////   alert(data.files[0].name);
        $.blockUI({
            message: msg,
            css: {
                border: 'none',
                width: '90%',
                height: '20px',
                left: '20px',
                'border-radius': '4px',
            }
        });
    };

    BeforeSMS = function () {
        StartBlockUI("信息验证中..");
    };
    EndSMS = function () {
        $.unblockUI();
    };

    Init = function () {
      
       
        openId = getUrlParam("OpenId");

        if (openId == undefined || openId == "" || openId == null)
        {
            window.location.href = "ErrorPage?errormsg=非法进入,请扫码进入";
            return;
        } 
        InitSMS("phone_num", "code_num", "btn_GetVerifyCode", "btn_ConfirmVerifyCode", 45, SMSSuccess, BeforeSMS, EndSMS);
     
    };


    Init();



});