var pageIndex = -1;
var totalAmt = 0, agentAmt = 0, storeAmt = 0, ppAmt = 0, transferAmt = 0;
$(document).ready(function () {
  
    //Query(true,pageIndex+1);
    InitCondition();
});

function AdvAlert(result) {

    $.alert({
        theme: 'dark',
        title: '异常',
        content: result,
    });
}

function InitCondition()
{
    $.ajax({
        type: 'post',
        url: '/Store/QueryKeyValue',
        success: function (data) {
            var arrLen = data.length;
            $("#cStore").append("<option value=-1>全部</option>");
            if (arrLen > 0) {
                $(data).each(function (i) {

                    $("#cStore").append("<option value='" + data[i].Id + "'>" + data[i].Name + "</option>");
                });
            }
           
        },
        error: function (xhr, type) {
            alert('Ajax error!');
        }
    });
}

function btnSearch()
{
    pageIndex = -1;
    Query(true, pageIndex + 1);
}

function Next()
{ 
    Query(false, pageIndex + 1);
}




function Query() {
    pageIndex = -1;
    Query(true, pageIndex + 1);
}

function ShowProcess()
{
    $("#btnSearch").attr("disabled", true);
    $("#divTableBody").hide();
    $("#divProcess").show();
}
function CloseProcess()
{
    $("#btnSearch").attr("disabled", false);
    $("#divTableBody").show();
    $("#divProcess").hide();
}

function Query(NeedClearn,_PageIndex) {

    var url = "/Order/Query";
    var OrderStatus = $("#cOrderStatus").val();
    var AgentName = $("#cAgentName").val();
    var DataType = $("#cDateType").val();
    var cStore = $("#cStore").val();
    var AliPayOrderNo = $("#AliPayOrderNo").val();
    var OrderNo = $("#OrderNo").val();
    var OrderType = $("#cOrderType").val();

    ShowProcess();

    if (NeedClearn) {
        $("#trContainer").empty();
        totalAmt = 0;
        agentAmt = 0;
        storeAmt = 0;
        ppAmt = 0;
        transferAmt = 0;
    }

    $.ajax({
        type: 'post',
        data: "OrderType="+OrderType+"&StoreId=" + cStore + "&OrderNo=" + OrderNo + "&AliPayOrderNo=" + AliPayOrderNo + "&DataType=" + DataType + "&OrderStatus=" + OrderStatus + "&OrderType=0&AgentName=" + AgentName + "&pageIndex=" + _PageIndex,
        url: url,
        success: function (data) {
            var arrLen = data.length;
           
            if (arrLen > 0) {   
                generateData(data);
                CloseProcess();//必须在计算宽度时关闭进度显示，不然将影响表格的呈现
                SetWidth();
                pageIndex++;
                $("#btnNext").show();
            }
            else
            {
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
        
        switch (result[i].OrderStatus)
        {
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
            case -2:
                OrderStatus = "等待用户确认";
                break;
            case -3:
                OrderStatus = "系统关闭交易";
                break;
        }

        //var Channel = result[i].SellerChannel;
        //var strCannel ;
        //if(Channel == 0)
        //    strCannel ="平台商户";
        //else
        //    strCannel ="加盟商户";

        var ppRealAmt = result[i].TotalAmount - result[i].RateAmount - result[i].ParentCommissionAmount - result[i].SellerCommission - result[i].BuyerTransferAmount;

       //var ppRealAmt = parseFloat(ppRealAmt.toFixed(2));

        var TransDate = result[i].TransDateStr;

        strCtrl = "";
        strCtrl += "<tr>";

        strCtrl += "<td><a href=/Transfer/Info_Win?id=" + result[i].OrderNo + "&type=1  target='_blank' class='td'>转账信息</a>";
        strCtrl += "</td>";
      
        //交易时间
        tdWidth = "width:" + $("#trHeader th").eq(1).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + TransDate + "</td>";

        //订单状态
        tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        if (result[i].OrderStatus == -1)
        { 
            strCtrl += "<td style='" + tdWidth + "'><a href=\"javascript:AdvAlert('" + result[i].LogRemark + "')\">" + OrderStatus + "</a>";
            //strCtrl += "<div class='DivHovering' id=divError" + i + ">" + result[i].LogRemark + "</div>";
            strCtrl +="</td>";
        }
        else
            strCtrl += "<td style='" + tdWidth + "'>" + OrderStatus + "</td>";

        //平台盈利
        tdWidth = "width:" + $("#trHeader th").eq(3).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + ppRealAmt.toFixed(2) + "</td>";
       // ppAmt += parseFloat(ppRealAmt);

        //总金额
        tdWidth = "width:" + $("#trHeader th").eq(4).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TotalAmount + "</td>";
        //totalAmt += parseFloat(result[i].TotalAmount);

        //代理金额
        tdWidth = "width:" + $("#trHeader th").eq(5).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].RateAmount + "</td>";
      //  agentAmt += parseFloat(result[i].RateAmount);

        //上级金额
        tdWidth = "width:" + $("#trHeader th").eq(6).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].ParentCommissionAmount + "</td>";

        //3级金额
        tdWidth = "width:" + $("#trHeader th").eq(7).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].L3CommissionAmount + "</td>";

        //商户佣金
        tdWidth = "width:" + $("#trHeader th").eq(8).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].SellerCommission + "</td>";
        storeAmt += parseFloat(result[i].SellerCommission);

        //买家转账
        tdWidth = "width:" + $("#trHeader th").eq(9).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].BuyerTransferAmount + "</td>";

        //代理用户
        tdWidth = "width:" + $("#trHeader th").eq(10).css("width");
        strCtrl += "<td style='" + tdWidth + "' title='" + result[i].AgentName + "'>" + result[i].AgentName + "</td>";

        //上级代理
        tdWidth = "width:" + $("#trHeader th").eq(11).css("width");
        strCtrl += "<td style='" + tdWidth + "' title='" + result[i].ParentName + "'>" + result[i].ParentName + "</td>";

        //3级代理
        tdWidth = "width:" + $("#trHeader th").eq(12).css("width");
        strCtrl += "<td style='" + tdWidth + "' title='" + result[i].L3Name + "'>" + result[i].L3Name + "</td>";

      

        ////代理扣点率
        //tdWidth = "width:" + $("#trHeader th").eq(10).css("width");
        //strCtrl += "<td style='" + tdWidth + "'>" + result[i].Rate + "</td>";

        //商户编号
        tdWidth = "width:" + $("#trHeader th").eq(13).css("width");
        strCtrl += "<td style='" + tdWidth + "' title='" + result[i].SellerStoreId + "'>" + result[i].SellerStoreId + "</td>";
   
        //商户名
        tdWidth = "width:" + $("#trHeader th").eq(14).css("width");
        strCtrl += "<td style='" + tdWidth + "' title='" + result[i].SellerName + "'>" + result[i].SellerName + "</td>";

        ////商户类型
        //tdWidth = "width:" + $("#trHeader th").eq(12).css("width");
        //strCtrl += "<td style='" + tdWidth + "'>" + strCannel + "</td>";

        ////商户佣金点率
        //tdWidth = "width:" + $("#trHeader th").eq(13).css("width");
        //strCtrl += "<td style='" + tdWidth + "'>" + result[i].SellerRate + "</td>";

      
        //买家账户
        tdWidth = "width:" + $("#trHeader th").eq(15).css("width");
        strCtrl += "<td style='" + tdWidth + "' title='" + result[i].BuyerAliPayLoginId + "'>" + result[i].BuyerAliPayLoginId + "</td>";

        //订单编号
        tdWidth = "width:" + $("#trHeader th").eq(16).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].OrderNo + "</td>";

        //支付宝订单编号
        tdWidth = "width:" + $("#trHeader th").eq(17).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);
     
        if(i==0)
        {
            $("#RecordSum").text("【总金额】：" + result[i].TotalAmountSum.toFixed(2) + "【代理金额】：" + result[i].RealTotalAmountSum.toFixed(2) + "【商户金额】：" + result[i].StoreAmountSum.toFixed(2) + "【买家打款】：" + result[i].BuyerTransferSum.toFixed(2) + "【平台收入】：" + result[i].PPIncome.toFixed(2));
        }

        ////结算编号
        //tdWidth = "width:" + $("#trHeader th").eq(15).css("width");
        //strCtrl += "<td style='" + tdWidth + "'>" + result[i].TransferId + "</td>";

        ////结算金额
        //tdWidth = "width:" + $("#trHeader th").eq(16).css("width");
        //strCtrl += "<td style='" + tdWidth + "'>" + result[i].TransferAmount + "</td>";
        //transferAmt += parseFloat(result[i].TransferAmount);

      
    });

    //汇总信息
    

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

function CleanWaitOrder()
{
    var url = "/Order/CleanWaitOrder";
    $.ajax({
        type: 'post',
        data: "",
        url: url,
        success: function (data) {
            if (data.IsSuccess == true) {
                alert("清除完成");
            }
            else
                alert(data.ErrorMsg);
        },
        error: function (xhr, type) {

            alert('Ajax error!');

        }
    });
}