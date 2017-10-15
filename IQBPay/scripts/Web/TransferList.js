﻿var pageIndex = -1;
$(document).ready(function () {
    Query(true, pageIndex + 1);
});

function Next() {
    Query(false, pageIndex + 1);
}

function Query(NeedClearn, _PageIndex) {

    var url = "/Transfer/Query";
    $.ajax({
        type: 'post',
        data: "pageIndex=" + _PageIndex,
        url: url,
        success: function (data) {
            var arrLen = data.length;
            if (NeedClearn)
                $("#trContainer").empty();
          

            if (arrLen > 0) {
                generateData(data);
                SetWidth();
                pageIndex++;
            }
            else
            {
                pageIndex--;
                alert("没有数据了");
            }
        },
        error: function (xhr, type) {

            alert('Ajax error!');

        }
    });
}

function generateData(result) {

    var strCtrl = "";
    $.each(result, function (i) {
        var thWidth;
        var TransDate = result[i].TransDateStr;

        strCtrl = "";
        strCtrl += "<tr>";

        strCtrl += "<td><a href=/Order/Info_Win?id=" + result[i].TransferId + "&type=2  target='_blank' class='td'>订单信息</a>";
        strCtrl += " <input type='hidden' value='" + result[i].FilePath + "'</td>";
        //汇款编号
        tdWidth = "width:" + $("#trHeader th").eq(1).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TransferId + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TransferAmount + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(3).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AgentName + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(4).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + TransDate + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(5).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].OrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(6).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].Buyer_AliPayLoginId + "</td>";

        

        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
}

function ToInfo(action) {
    window.location.href = "OrderInfo?do=" + action;
    return;
}