var pageIndex = -1;
var pageSize = 20;


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

 
    InitCondition();

});

function InitCondition() {
  

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
   

    var url = "/O2O/TransWHQuery";

    $.ajax({
        type: 'post',
        dataType: "json",
        data: {
            "pageIndex": _PageIndex,
            "pageSize": pageSize,
            "BeforeDay": BeforeDay,
            "MallOrderNo": MallOrderNo,
          
        },
        url: url,
        success: function (data) {
            var arrLen = data.length;
            if (arrLen > 0) {
                generateData(data);
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

        //商城
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "'>" + result[i].MallName + "</td>";

        //商城订单编号
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' title=" + result[i].MallOrderNo + ">" + result[i].MallOrderNo + "</td>";

        //商品
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' title=" + result[i].ItemName + ">" + result[i].ItemName + "</td>";

    
        //金额
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "'>" + result[i].TransferAmount + "</td>";

        //费率
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "'>" + result[i].FeeRate + "</td>";

        //账户
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "'>" + result[i].ReceiveAccount + "</td>";

        //打款时间
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' title=" + result[i].TransDateTime + ">" + result[i].TransDateTime + "</td>";

        Ctrl += "</tr>";



        $("#DataTable").append(Ctrl);
    });


}