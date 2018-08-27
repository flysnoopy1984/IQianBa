$(function () {
    var dialog = null;
    var verifyDlg = null;

    PayToAli = function () {

        var amt = parseFloat($("#paymoney").text());

        if (amt == null || amt == "" || amt <20 || isNaN(amt)) {
            $.alert({
                theme: "material",
                title: "错误",
                content: "<div style='font-size:14px !important;'>金额不能为空或小于20元</div>",


            });

            return false;
        }

        var qrUserId = $("#qrUserId").val();
        if (qrUserId == null || qrUserId == "" || qrUserId<=0) {
            $.alert({
                theme: "material",
                title: "错误",
                content: "<div style='font-size:14px !important;'>未获取代理商家ID，请重新扫描后再尝试或联系代理商家</div>",

            });

            return;
        }

        var MarketRate = parseFloat($("#hMarketRate").val());
        var Rate = parseFloat($("#hRate").val());

        var AgentComm = (amt * (MarketRate / 100)).toFixed(2);
        $("#PayVal").text(amt);
        $("#AgentComm").text(AgentComm);
        $("#SrvComm").text("2.00");
        $("#RealGet").text((amt - AgentComm - 2).toFixed(2));

        ShowConfirm(qrUserId,amt);

    }

    $("#btnPay").on("click", PayToAli);


    ShowConfirm = function (qrUserId,amt) {

        var info = $("#Info");
        var buyerPhone = $("#hPhone").val()
      //  var str =
        $.confirm({
            theme: "modern",
            title: '请仔细确认',
            type: 'red',
            content: info.html(),
            columnClass: "col-md-2",
            
            buttons: {
            
                Cancel: {
                    btnClass: 'btn btn-info',
                    text: "那算了吧",
                    action: function () {
                    }
                },
                Know: {
                    btnClass: 'btn btn-danger',
                    text: "确定支付",
                    action: function () {
                        var url = payUrl + "/AliPay/F2FPay?qrUserId=" + qrUserId + "&Amount=" + amt + "&BuyerPhone=" + buyerPhone;
                        window.location.href = url;
                    }
                }
             }
        });
    }

   

    DialogHtml = function () {
        var html = '<div id="PhoneArea">';
        html += '<div id="get_phone_check" class="get_phone_check">';
        html += '<input id="phone_num" type="tel" placeholder="请输入手机号" class="form-control get_phone_check_input" />';
        //html += '<button type="button" class="btn btn-warning get_phone_btn" id="btn_GetVerifyCode">获取验证码</button>';
        html += '<input type="button" class="btn btn-warning get_phone_btn" id="btn_Confirm" onclick="ConfirmPhone();" value="确定" />';
        html += '</div>';
        //html += '<div id="verify_phone_check">';
        //html += '<input id="code_num" type="text" placeholder="请输入验证码" class="form-control get_phone_check_input" />';
        //html += '<button type="button" class="btn btn-success get_code_btn" id="btn_ConfirmVerifyCode">确认验证</button>';
        //html += '</div>';
        html += '</div>';
        return html;

    }

    VerifyDialogHtml = function (phone) {
      
        var html = '<div id="VerifyPhoneArea">';
        html += '<div id="get_phone_check" class="get_phone_check">';
        html += '<input id="phone_num" type="tel" value="' + phone + '" placeholder="请输入手机号" class="form-control get_phone_check_input" />';
        html += '<button type="button" class="btn btn-warning get_phone_btn" id="btn_GetVerifyCode">获取验证码</button>';
        html += '</div>';
        html += '<div id="verify_phone_check">';
        html += '<input id="code_num" type="tel" placeholder="请输入验证码" class="form-control get_phone_check_input" />';
        html += '<button type="button" class="btn btn-success get_code_btn" id="btn_ConfirmVerifyCode">确认验证</button>';
        html += '</div>';
        html += '</div>';
        return html;
    }

    ShowVerifyPhone = function (phone) {
        var html = VerifyDialogHtml(phone);

        verifyDlg = $.dialog({
            title: '新手机号需要先验证',
            content: html,
            type: "red",
            columnClass: 'col-md-4',
            closeOnEscape: false,
            onOpen: function () {

                InitSMS("phone_num", "code_num", "btn_GetVerifyCode", "btn_ConfirmVerifyCode", 90, SMSSuccess, BeforeSMS, EndSMS);
            },
            onClose: function () {
                if ($("#hPhone").val() == "") {
                    Init();
                }
                verifyDlg = null;

            }
        });
    }

    Init = function () {

        if ($("#hPhone").val() == "") {
         
            var html = DialogHtml();
            dialog  = $.dialog({
                title: '您的手机号',
                content: html,
                type:"red",
                columnClass: 'col-md-4',
                closeOnEscape: false,
                onClose: function () {
                    if ($("#hPhone").val() == "" && verifyDlg == null) {
                        alert("需要填写手机号！"); 
                        Init();
                    }
                    else
                        dialog = null;  
                },
                onOpen:function(){
                    $("#phone_num").focus();
                }
            });
           
        }
    };

    //提交手机确认是否存在
    ConfirmPhone = function () {

        var url = "/api/ppCus/CheckPhone";
        var phone = $("#phone_num").val();
        if (phone == "" || !isPhoneNo(phone))
        {
            alert("请正确填写手机号码！");
            $("#phone_num").focus();
            return false;
        }
        $.ajax({
            type: 'get',
            data: { "phone": phone},
            url: url,
            success: function (data) {

                if (data.IsSuccess == true) {

                    if(data.IntMsg == 0)
                    {
                        ShowVerifyPhone(phone);
                        dialog.close();
                       
                       
                    }
                    else
                    {
                        $("#hPhone").val(phone);
                        dialog.close();
                    }
                   // window.location.reload();
                    //  window.location.reload();
                }
                else {
                    $.alert({
                        theme: 'dark',
                        title: '错误!',
                        content: "<div style='font-size:12px;'>"+data.ErrorMsg + ".请联系平台"+"</div>",
                    });
                }
            },
            error: function (xhr, type) {

                alert(xhr.responseText);

            }
        });
    }

    /*SMS begin*/
    BeforeSMS = function () {
        StartBlockUI("信息验证中..");
    };
    EndSMS = function () {
        $.unblockUI();
    };

    SMSSuccess = function () {

        var phone = $("#phone_num").val();
        var url = "/api/ppcus/AddBuyerPhone?phone="+phone;
        $.ajax({
            type: 'post',

            url: url,
            success: function (data) {

                if (data.IsSuccess) {
                    $("#hPhone").val(phone);
                    verifyDlg.close();
                }
                else
                {
                    $.alert({
                        theme: 'dark',
                        title: '错误!',
                        content: "<div style='font-size:12px;'>" + data.ErrorMsg + ".或请联系平台" + "</div>",
                    });
                }

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
    /*SMS end*/

 
    Init();

   

});
