$(function () {
    var pageIndex = 0;
    var pageSize = 10;
    var IQBScroll = null;
    var minAmt = 10;

    Init = function () {
        if (document.referrer != "" && document.referrer.indexOf("AliPayAccount") > -1)
            window.location.reload();

        var aliPayAccount = $("#AlipayAccount").val();
        if (aliPayAccount == "")
        {
            alert("请先设置收款支付宝账户！");
            SetUserAccount();
        }

        var phone = $("#Phone").val();
        if (phone == "")
        {
            $("#Content").hide();
            $("#PhoneArea").show();
            InitSMS("phone_num", "code_num", "btn_GetVerifyCode", "btn_ConfirmVerifyCode", 90, SMSSuccess, BeforeSMS, EndSMS);

            return;
        }
       

        $("#GetMoney_Amt").val(minAmt);

        $("#DataTable").empty();
        pageIndex = 0;
        
        IQBScroll = $("#Detail").ScrollLoad({

            loadData: function (me) {

                var url = "/PP/AgentAccountTransQuery";
              
                $.ajax({
                    type: 'post',
                    data: { "pageIndex": pageIndex, "pageSize": pageSize},
                    url: url,
                    success: function (res) {
                        if (res.IsSuccess) {
                            if (res.resultList.length > 0) {
                               
                                generateData(res.resultList);
                                pageIndex++;

                            }
                            else {

                                me.noData();
                            }
                        
                        }
                        else {
                         
                           
                                alert(res.ErrorMsg);
                            
                        }
                        me.resetLoad();

                    },
                    error: function (xhr, type) {
                        alert("系统错误！");

                        me.resetLoad();
                      
                    }
                });

            }

        });


    }

    generateData = function (data) {

        var ctrl = "";
        $.each(data, function (i) {
            ctrl = "<tr>";
            ctrl += "<td>";
            ctrl += '<div class="lineRow">';
            ctrl += '<div class="AmtType">' + data[i].TransDateStr + '</div>';
            ctrl += '<div class="AmtDes">' + data[i].TransDescription + '</div>';

            if (data[i].TransactionType == 0 ||
                data[i].TransactionType == 1 ||
                data[i].TransactionType == 2 ||
                data[i].TransactionType == 10 ||
                 data[i].TransactionType == 11 ||
                 data[i].TransactionType == 12 
                )
                ctrl += '<div class="AmtValue AmtAdd">+' + data[i].Amount + '</div>';
           else
                ctrl += '<div class="AmtValue">-' + data[i].Amount + '</div>';
            ctrl += '</div>';
            ctrl += "</td>";
            ctrl += "</tr>";
            $("#DataTable").append(ctrl);
        });
    };

    SetUserAccount = function () {
        window.location.href = "/PP/AliPayAccount";
    };

    GetMoney = function () {
        var display = $('#GetMoneyArea').css('display');
        if (display == 'none') {
            $('#GetMoneyArea').show(700);
            $('#GetMoneyArea').css('display', "flex");
            var v = parseFloat($("#AgentBalance").text());
            $("#GetMoney_Amt").val(v);
        }
        else
            $('#GetMoneyArea').hide(700);
    };



    CheckAmtValue = function (obj) {
        var v = $(obj).val();
        var r = minAmt;
        if ( v!= "")
        {
            r = v % minAmt;
            if (r != 0) {
                alert("必须输入" + minAmt + "的倍数");
                r = Math.floor((v / minAmt)) * minAmt;
                if (r < minAmt)
                    r = minAmt;
            }
            else
                r = v;

        }
        $(obj).val(r);
    };

    SureGetMoney = function () {
        var v = parseFloat($("#GetMoney_Amt").val());
        var AgentBalance = parseFloat($("#AgentBalance").text());
        if (v < minAmt) {
            alert("金额不正确"); return;
        }
        if(v>AgentBalance)
        {
            alert("提现金额大于余额");
            return;
        }

        $.confirm({
            theme: 'material',
            title: '提款确认!',
            content: '您将提款【'+v+'】元金额，是否继续',
            buttons: {
                cancel: {
                    text: '重新输入',
                    btnClass: 'btn-info',

                },
                confirm: {
                    btnClass: 'btn-danger',
                    text: '没有问题',
                    action: function () {
                        var url = "/PP/TransferAmoutToAgent";
                        $.ajax({
                            type: 'post',
                            url: url,
                            data: { "Amt": v,"minAmt":minAmt },
                            success: function (res) {
                                if (res.IsSuccess) {
                                    window.location.reload();
                                    alert("提现成功;");
                                }
                                else {
                                    alert(res.ErrorMsg);
                                    if (res.IntMsg == -10)
                                        SetUserAccount();
                                }
                            },
                            error: function (xhr, type) {
                                alert("系统错误！");
                            }
                        });

                    }
                }
            }
        });

      
    }

    BeforeSMS = function () {
        StartBlockUI("信息验证中..");
    };
    EndSMS = function () {
        $.unblockUI();
    };

    SMSSuccess = function () {

        var url = "/PP/UpdateAgentPhone";
        $.ajax({
            type: 'post',
            dataType: "json",
            data: { "Phone": $("#phone_num").val() },
            url: url,
            success: function (data) {

                if (data.IsSuccess) {
                    alert("手机校验通过！");
                    window.location.reload();
                }
                else
                    alert(data.ErrorMsg);
                    window.location.reload();

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

    Init();

    //AmtAdd = function () {
    //    var v = parseFloat($("#GetMoney_Amt").val());
    //    var r = minAmt;
    //    if (v != "") {
    //        r = v % minAmt;
    //        if (r != 0) {
    //            r = Math.floor((v / minAmt)) * minAmt;
    //        }
    //        else
    //            r = v;
    //    }
    //    r += 100;
    //    $("#GetMoney_Amt").val(r);

    //};
    //AmtMin = function () {
    //    var v = parseFloat($("#GetMoney_Amt").val());
    //    var r = minAmt;
    //    if (v != "") {
    //        r = v % minAmt;
    //        if (r != 0) {
    //            r = Math.floor((v / minAmt)) * minAmt;
    //        }
    //        else
    //            r = v;
    //    }
    //    r -= minAmt;
    //    if (r < minAmt) r = minAmt;
    //    $("#GetMoney_Amt").val(r);

    //};
});