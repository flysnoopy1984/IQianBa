$(function () {

    const InitCount = 300;
    var countDown = InitCount;

    RequireVerifyCode = function () {
        //  var bn = $("#btnVerifyCode");
        var Phone = $("#user_phone").val();
        if (Phone == '') {
            alert("请先输入手机号!");
            return;
        }
        if (!isPhoneNo(Phone))
        {
            alert("手机号填写不正确!");
            return;
        }

        var url = "/API/SMS/Sent";
        $("#btnVerifyCode").attr("disabled", true);

        $.ajax({
            type: 'get',
            dataType: "json",
            data: { "mobilePhone": Phone},
            url: url,
            success: function (data) {
                //已发送
                if (data.SMSVerifyStatus == 2) {
                    settime();

                }
                else if (data.SMSVerifyStatus == 1) {

                    alert("请不要重复发送");
                    countDown = data.RemainSec;
                    settime();

                }
                else {

                }
            },
            error: function (xhr, type) {

                alert(xhr.responseText);

            }
        });
    }

    settime = function () {
        if (countDown == 0) {
            countDown = InitCount;
            $("#btnVerifyCode").val('验证码');
            $("#btnVerifyCode").attr("disabled", false);
            return;
        } else {
            countDown--;

            $("#btnVerifyCode").attr("disabled", true);
            $("#btnVerifyCode").val("重新获取(" + countDown + ')s');

        }
        setTimeout(function () {
            settime();
        }, 1000)
    };

    // 验证手机号
    isPhoneNo = function (phone) {
        var pattern = /^1[34578]\d{9}$/;
        return pattern.test(phone);
    };

    Register = function () {
        var phone = $("#user_phone").val();
        var pwd = $("#user_pwd").val();
        var verifyCode = $("#txt_verifyCode").val();

        if(phone == "" || pwd == "" || verifyCode == "")
        {
            alert("手机，密码，验证码 都不能为空");
            return;
        }

        var url = "/User/DoRegister";
      

        $.ajax({
            type: 'get',
            dataType: "json",
            data: { "phone": phone, "pwd": pwd, "verifyCode": verifyCode },
            url: url,
            success: function (data) {
                if(data.IsSuccess)
                {
                    alert("注册成功");
                    window.location.href = "/User/Home";
                }
                else
                {
                    alert(data.ErrorMsg);
                }
            },
            error: function (xhr, type) {

                alert(xhr.responseText);

            }
        });

    }


    $("#div_vc").on("click", RequireVerifyCode);

    $("#btnRegister").on("click", Register);
    


})