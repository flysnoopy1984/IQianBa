$(function () {
    var pageIndex = 0;
    var pageSize = 10;
    var IQBScroll = null;
    
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

   
    Query = function (me) {

        var phone = $("#PhoneNo").val();
        if (phone == "") {
            alert($("#PhoneNo").attr("placeholder"));
            $("#PhoneNo").focus();
            return;
        }
        var url = "/api/PPcus/QueryBuyerTrans";

        if (me != null)
            me.resetLoad();

        $.ajax({
            type: 'post',
            data: {
                "BuyerPhone": phone,
                "pageIndex": pageIndex,
                "pageSize": pageSize
            },
            url: url,
            success: function (res) {
                if (res.IsSuccess) {
                    if (res.resultList.length > 0) {
                        generateData(res.resultList);
                        pageIndex++;
                    }
                    else {
                        if (me != null)
                            me.noData();
                    }

                }
                else
                    alert(res.ErrorMsg);
                if (me != null)
                    me.resetLoad();

            },
            error: function (xhr, type) {
                alert('Ajax error!');
            }
        });

    };

    generateData = function (data) {
        var ctrl = "";
        $.each(data, function (i) {
            var status = data[i].OrderStatus; //1 Paid 2 Closed
           
            ctrl = "<tr>";
            ctrl += "<td>";
            ctrl += '<div class="lineRow">';
            ctrl += '<div class="lineRow_content">';
            ctrl += '<div class="PayValue">收款金额：<span class="AmtValue">' + data[i].Amount + '</span></div>';
            ctrl += '<div class="PayDate">支付日期：' + data[i].TransDateStr + '</div>';
            ctrl += '</div>';

            ctrl += '<div class="lineRow_op">';
            if (status == 1)
                ctrl += '<input type="button" class="btn btn-danger" onclick=DoReceiveMoney("' + data[i].OrderNo + '"); value="收款"/>';
            else if(status ==2)
                ctrl += '状态：<span class="StatusValue StatusValue_Closed" >已结算</span>';
            else
                ctrl += '状态：<span class="StatusValue">处理中</span>';

            ctrl += '</div>';
            ctrl += "</td>";
            ctrl += "</tr>";

            $("#DataTable").append(ctrl);
        });
    }

    StartQuery = function()
    {
        $("#Detail").show();
        $("#DataTable").empty();
        pageIndex = 0;

        IQBScroll = $("#Detail").ScrollLoad({

            loadData: function (me) {
                Query(me);
            }
        });
       
    }

    //确认手机号是否存在，并确认是否有对应的收款账户
    ConfirmPhone = function () {
        var phone = $("#PhoneNo").val();
        if (phone == "") {
            alert($("#PhoneNo").attr("placeholder"));
            $("#PhoneNo").focus();
            return;
        }
        $("#StartArea").hide();
        $("#Detail").hide();
        $("#BuyerAccountArea").hide();
        var url = "/api/PPcus/GetBuyerInfo?phone=" + phone;

        $.ajax({
            type: 'post',
         
            url: url,
            success: function (res) {
                if (res.IsSuccess) {
                    var data = res.resultObj;
                    $("#BuyerAccountArea").show();
                    $("#AlipayAccount").val(data.AliPayAccount);
                    $("#AlipayAccount").focus();
                   // $("#btnConfirmPhone").val("修改");

                   
                }
                else
                    alert(res.ErrorMsg);

              
             
            },
            error: function (xhr, type) {
                alert('Ajax error!');
            }
        });
    }

    ComfirmAccount = function () {
        var phone = $("#PhoneNo").val();
        if (phone == "") {
            alert($("#PhoneNo").attr("placeholder"));
            $("#PhoneNo").focus();
            return;
        }

        var Account = $("#AlipayAccount").val();
        if (Account == "") {
            alert($("#AlipayAccount").attr("placeholder"));
            $("#AlipayAccount").focus();
            return;
        }

        var url = "/api/PPcus/SetBuyerAliAccount?phone=" + phone + "&AliAccount=" + Account

        $.ajax({
            type: 'post',
            url: url,
            success: function (res) {
                if (res.IsSuccess) {
                    $("#StartArea").show();
                    $("#btnConfirmAccount").val("修改");
                }
                else
                    alert(res.ErrorMsg);

            },
            error: function (xhr, type) {
                alert('Ajax error!');
            }
        });
    }


    DoReceiveMoney = function (OrderNo) {
        var url = payUrl + "/api/Buyer/TransferAmountToBuyer";
        var AliAccount = $("#AlipayAccount").val();

        StartBlockUI("打款中，请稍等...");
        
        $.ajax({
            type: 'post',
            url: url,
            data:{
                "OrderNo": OrderNo,
                "AliAccount": AliAccount
            },
            success: function (res) {
                $.unblockUI();

                if (res.IsSuccess) {
                    $("#StartArea").show();
                    $("#btnConfirmAccount").val("修改");
                }
                else
                    alert(res.ErrorMsg);
               
            },
            error: function (xhr, type) {
                $.unblockUI();
                alert('失败，请联系客服');
               
            }
        });
        
    };

    Init = function () {
        if ($("#PhoneNo").val() != "")
        {
            $("#btnConfirmPhone").val("修改");
            $("#BuyerAccountArea").show();
        }
           
        //if ($("#AlipayAccount").val() != "")
        //    $("#btnConfirmAccount").val("修改");
        
           
    }

    Init();

});



   