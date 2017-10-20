var pageIndex = -1;
var totalAmt = 0, agentAmt = 0;
$(document).ready(function () {

    
});


function InitCondition() {

}

function btnSearch() {
    pageIndex = -1;
    Query(true, pageIndex + 1);
}

function Next() {
    Query(false, pageIndex + 1);
}

function ShowProcess() {
    $("#btnSearch").attr("disabled", true);
    $("#divTableBody").hide();
    $("#divProcess").show();

}
function CloseProcess() {
    $("#btnSearch").attr("disabled", false);
    $("#divTableBody").show();
    $("#divProcess").hide();
}

function Query(NeedClearn, _PageIndex) {

    var url = "/Order/Query";
    var OrderStatus = $("#cOrderStatus").val();
    var AgentOpenId = $("#OpenId").val();
    var DataType = $("#cDateType").val();
    ShowProcess();

    $.ajax({
        type: 'post',
        data: "DataType=" + DataType + "&OrderStatus=" + OrderStatus + "&OrderType=0&AgentOpenId=" + AgentOpenId + "&pageIndex=" + _PageIndex,
        url: url,
        success: function (data) {
            var arrLen = data.length;
            if (NeedClearn) {
                $("#trContainer").empty();
                totalAmt = 0;
                agentAmt = 0;
            }

            if (arrLen > 0) {
                generateData(data);
                CloseProcess();//必须在计算宽度时关闭进度显示，不然将影响表格的呈现
                SetWidth();
                pageIndex++;
                $("#btnNext").show();
            }
            else {
                pageIndex--;
                alert("没有数据了");
                CloseProcess();
                $("#btnNext").hide();

            }
        },
        error: function (xhr, type) {

            alert('Ajax error!');
            CloseProcess();
            $("#btnNext").hide();

        }
    });
}

function generateData(result) {

    var strCtrl = "";
   

    $.each(result, function (i) {
        var thWidth;
        var OrderStatus = "";
        switch (result[i].OrderStatus) {
            case 0:
                OrderStatus = "等待支付宝反馈";
                break;
            case 1:
                OrderStatus = "已支付";
                break;
            case 2:
                OrderStatus = "已提现";
                break;
            case -1:
                OrderStatus = "异常";
                break;
        }


        var TransDate = result[i].TransDateStr;

        strCtrl = "";
        strCtrl += "<tr>";

        strCtrl += "<td><a href=/Transfer/Info_Win?id=" + result[i].OrderNo + "&type=1  target='_blank' class='td'>结算信息</a>";
        strCtrl += "</td>";
        //订单编号
        tdWidth = "width:" + $("#trHeader th").eq(1).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].OrderNo + "</td>";

        //支付宝订单编号
        tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        //订单状态
        tdWidth = "width:" + $("#trHeader th").eq(3).css("width");
        if (result[i].OrderStatus == -1) {

            strCtrl += "<td style='" + tdWidth + "'><a href='javascript:ShowError(" + i + ")'>" + OrderStatus + "</a>";
            strCtrl += "<div class='DivHovering' id=divError" + i + ">" + result[i].LogRemark + "</div>";
            strCtrl += "</td>";
        }
        else
            strCtrl += "<td style='" + tdWidth + "'>" + OrderStatus + "</td>";

        //总金额
        tdWidth = "width:" + $("#trHeader th").eq(4).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TotalAmount + "</td>";
        totalAmt += parseFloat(result[i].TotalAmount);

        //交易时间
        tdWidth = "width:" + $("#trHeader th").eq(5).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + TransDate + "</td>";

     
        //代理扣点率
        tdWidth = "width:" + $("#trHeader th").eq(6).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].Rate + "</td>";

    

        //代理实际收入
        tdWidth = "width:" + $("#trHeader th").eq(7).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].RealTotalAmount + "</td>";
        agentAmt += parseFloat(result[i].RealTotalAmount);
     
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
    //汇总信息
    $("#RecordSum").text("【总金额】：" + totalAmt.toFixed(2) + "【代理金额】：" + agentAmt.toFixed(2));
}

function ShowError(no) {
    var obj = $("#divError" + no);
    if (obj.is(":hidden")) {
        obj.css("position", "relative");
        obj.show();
    }
    else {
        obj.css("position", "absolute");
        obj.hide();
    }
}

function ToInfo(action) {
    window.location.href = "OrderInfo?do=" + action;
    return;
}