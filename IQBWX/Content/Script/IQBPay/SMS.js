$(function () {
    var InitCount = 60;
    var countDown = InitCount;
    var PhoneCtrl = null;
    var CodeCtrl = null;
    var btnGetVC = null;
    var btnSubmitVC = null;
    var sucEvent = null;
    var befEvent = null;
    var endEvent = null;
  
    settime = function () {
        if (countDown == 0) {
            countDown = InitCount;
            btnGetVC.text('获取验证码');
            btnGetVC.attr("disabled", false);
            //var html = $('.o2o_modal').html();
            //popModel.setContent(html);
            return;
        } else {
            countDown--;

            btnGetVC.attr("disabled", true);
            btnGetVC.text("重新获取(" + countDown + ')s');
            //var html = $('.o2o_modal').html();
            //popModel.setContent(html);

        }
        setTimeout(function () {
            settime();
        }, 1000)
    }

    InitSMS = function (phoneCtrlId, codeCtrlId, btnGetVCId, btnSubmitVCId, _InitCount, _sucEvent, _befEvent, _endEvent) {

        PhoneCtrl = $("#" + phoneCtrlId);
        CodeCtrl = $("#" + codeCtrlId);
        btnGetVC = $("#" + btnGetVCId);
        btnSubmitVC = $("#" + btnSubmitVCId);
        btnSubmitVC.attr("disabled", true);
       
        
        if (_InitCount != undefined && _InitCount != 0)
        {
            InitCount = _InitCount;
            countDown = InitCount;
        }
           
        if (_sucEvent != undefined)
            sucEvent = _sucEvent;
        if (_befEvent != undefined)
            befEvent = _befEvent;
        if (_endEvent != undefined)
            endEvent = _endEvent;

        $(document).on("click", "#" + btnGetVCId, RequireVerifyCode);
        $(document).on("click", "#" + btnSubmitVCId, SubmitVerifyCode);
    }

    RequireVerifyCode = function (e) {

     
        var Phone = PhoneCtrl.val();
        if (Phone == '') {
            alert("手机号不能为空!");
            PhoneCtrl.focus();
            return;
        }
        if (!isPhoneNo(Phone))
        {
            alert("手机格式不正确!");
            PhoneCtrl.focus();
            return;
        }
        btnGetVC.attr("disabled", true);

        var url = "/API/SMS/SentSMS_IQBPay_BuyerOrder";

        $.ajax({
            type: 'get',
            dataType: "json",
            data: { "mobilePhone": Phone, "IntervalSec": countDown, "SMSEvent": 200 },
            url: url,
            success: function (result) {
              
                btnSubmitVC.attr("disabled", false);
                if (result.SMSVerifyStatus == 2) {
                    settime();

                }
                else if (result.SMSVerifyStatus == 1) {

                    alert("请不要重复发送");
                    countDown = result.RemainSec;
                    settime();

                }
                else {
                    btnGetVC.attr("disabled", false);
                }

            },
            error: function (xhr, type) {
                btnGetVC.attr("disabled", false);
                alert(xhr.responseText);

            }
        });
    }
    SubmitVerifyCode = function(e){
        //   var Phone = $("#" + e.data.phoneCtrlId).text();
        var Phone = PhoneCtrl.val();
        if (Phone == '') {
            return;
        }
        var Code = CodeCtrl.val();

        var url = "/API/SMS/IQBPay_ConfirmVerifyCode";

        if (befEvent)
            befEvent();


        $.ajax({
            type: 'get',
            dataType: "json",
            data: { "mobilePhone": Phone, "Code": Code },
            url: url,
            success: function (data) {
                switch (data.SMSVerifyStatus) {
                    case -1:
                        alert("验证码未知错误，请联系管理员");
                        return data.SMSVerifyStatus;
                    case 5:
                        alert("验证码已失效，请重新获取！");
                        return data.SMSVerifyStatus;
                    case 3:
                        ////验证成功
                        if (sucEvent)
                            sucEvent();
                        else
                        {
                            alert("验证通过");
                        }
                       
                        return data.SMSVerifyStatus;
                    case 4:
                        alert("验证码不正确，请重新填写！");
                        CodeCtrl.select();
                        CodeCtrl.focus();
                        return data.SMSVerifyStatus;

                }
                if (endEvent)
                    endEvent();


            },
            error: function (xhr, type) {
                if (endEvent)
                    endEvent();
                alert(xhr.responseText);

            }
        });
    }

});




