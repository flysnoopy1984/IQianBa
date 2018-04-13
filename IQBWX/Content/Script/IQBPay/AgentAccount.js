$(function () {
    var pageIndex = 0;
    var pageSize = 10;
    var IQBScroll = null;
    var minAmt = 100;

    Init = function () {
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
                data[i].TransactionType == 2)
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
        }
        else
            $('#GetMoneyArea').hide(700);
    };

    AmtAdd = function () {
        var v = parseFloat($("#GetMoney_Amt").val());
        var r = minAmt;
        if (v != "") {
            r = v % minAmt;
            if (r != 0) {
                r = Math.floor((v / minAmt)) * minAmt;
            }
            else
                r = v;
        }
        r += 100;
        $("#GetMoney_Amt").val(r);

    };
    AmtMin = function () {
        var v = parseFloat($("#GetMoney_Amt").val());
        var r = minAmt;
        if (v != "") {
            r = v % minAmt;
            if (r != 0) {
                r = Math.floor((v / minAmt)) * minAmt;
            }
            else
                r = v;
        }
        r -= minAmt;
        if (r < minAmt) r = minAmt;
        $("#GetMoney_Amt").val(r);

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

    
    Init();

    $(document).on("click", "#btnAmtAdd", AmtAdd);
    $(document).on("click", "#btnAmtMin", AmtMin);
});