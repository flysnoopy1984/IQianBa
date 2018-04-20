var pageIndex = -1;
var pageSize = 20;

var $Pager = null;
var defaultOpts = {
    totalPages: 1,
};

function CreateDemoData() {
    var url = "/O2O/CreateTransData";
    $.ajax({
        type: 'post',
        url: url,
        success: function (data) {
            alert("OK");

        },
        error: function (xhr, type) {
            alert("系统错误！");
        }
    });
}

$(document).ready(function () {

 
    Init();

    $Pager = $('#Pager');

    defaultOpts = {

        onPageClick: function (event, page) {
            if (pageIndex != -1)
                Query(true, page - 1);
        }
    };


});

function Init() {
  
    var url = "/User/GetShipmentAccount";

    $.ajax({
        type: 'post',
        url: url,
        data: {},
        success: function (res) {
            if (res.IsSuccess) {
                shipAccountList = res.resultList;
                if (shipAccountList.length == 0) {
                    alert("没有找到您的账户余额！");
                    return;
                }
                else {
                    var obj = shipAccountList[0];
                  
                    $("#AliPayAccount").text(obj.AliPayAccount);
                    $("#O2OShipBalance").text(obj.O2OShipBalance);
                    $("#O2OShipOutCome").text(obj.O2OShipOutCome);
                    $("#O2OShipInCome").text(obj.O2OShipInCome);

                    if (obj.O2OOnOrderAmount == null)
                        obj.O2OOnOrderAmount = 0;
                    $("#O2OOnOrderAmount").text(obj.O2OOnOrderAmount);
                }

            }
            else {
                if (res.IntMsg == -1)
                {
                    window.location.href = "/O2O/Login?action=sessionlost";
                }
                else
                alert(res.ErrorMsg);

            }


        },
        error: function (xhr, type) {
            alert("系统错误！");
        }
    });

}

function btnQuery() {
    pageIndex = -1;
    Query(true, pageIndex + 1);
}

function Query(NeedClearn, _PageIndex) {
    if (NeedClearn) {
        $("#DataTable tr:gt(0)").empty();
    }
    var BeforeDay = $("#cBeforeDay").val();
    var MallOrderNo = $("#cMallOrderNo").val();
    var TransferTarget = $("#cTransferTarget").val();
   
    var url = "/O2O/TransWHQuery";

    $.ajax({
        type: 'post',
        dataType: "json",
        data: {
            "pageIndex": _PageIndex,
            "pageSize": pageSize,
            "BeforeDay": BeforeDay,
            "MallOrderNo": MallOrderNo,
            "TransferTarget": TransferTarget,
        },
        url: url,
        success: function (data) {
            var arrLen = data.resultList.length;
            if (arrLen > 0) {

                if (pageIndex == -1) {
                    $Pager.twbsPagination('destroy');
                    $Pager.twbsPagination($.extend({}, defaultOpts, {
                        startPage: 1,
                        totalPages: Math.ceil(data.IntMsg / pageSize),
                    }));

                }

                generateData(data.resultList);
                pageIndex++;
            }
        },
        error: function (xhr, type) {

            alert('Ajax error!');


        }
    });
}

function generateData(result) {
    var Ctrl = "";
    var thWidth;
    var n = 0;
    $.each(result, function (i) {
        
        
        n = 0;
        Ctrl = '<tr style="display:-webkit-flex;">';
        //类型
        var transType = result[i].TransferTarget;
        var AmountClass = "FontAmount";
        var AmountValue = "";
        var des = "";
        if (transType == 100)
        {
            des = "扣减押金";
            AmountValue ="-"+result[i].TransferAmount;
        }
        else if (transType == 20)
        {
            des = "获得佣金";
            AmountClass += " Color_AmountIn";
            AmountValue ="+"+result[i].TransferAmount;
        }
        else if (transType == 50) {
            des = "充值";
            //AmountClass += " Color_AmountIn";
            AmountValue =  result[i].TransferAmount;
        }

        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "'>" + des + "</td>";

        //金额
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' class='" + AmountClass + " '>" + AmountValue + "</td>";

        ////费率
        //tdWidth = "width:" + $("#header th").eq(n++).css("width");
        //Ctrl += "<td style='" + tdWidth + "'>" + result[i].FeeRate + "</td>";

        ////账户
        //tdWidth = "width:" + $("#header th").eq(n++).css("width");
        //Ctrl += "<td style='" + tdWidth + "'>" + result[i].ReceiveAccount + "</td>";

        //打款时间
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' title=" + result[i].TransDateTimeStr + ">" + result[i].TransDateTimeStr + "</td>";

        //商城
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "'>" + result[i].MallName + "</td>";

        //商城订单编号
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' title=" + result[i].MallOrderNo + ">" + result[i].MallOrderNo + "</td>";

        //商品
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' title=" + result[i].ItemName + ">" + result[i].ItemName + "</td>";

      

        Ctrl += "</tr>";



        $("#DataTable").append(Ctrl);
    });


}