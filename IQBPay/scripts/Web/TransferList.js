
$(document).ready(function () {
    Query(true);
});

function OpenWin(url) {
    window.open(url, "_Blank", "");
}

function SetWidth() {

    var w = parseInt($("#TableHeader").css("width"));
    var hTable = $("#trContainer").height();
    var hDiv = $("#divTableBody").height();
    if (hTable > hDiv) {

        var scrollWidth = 17;
        w += scrollWidth;

        $("#divTableHeader").css("width", w);
        $("#divTableBody").css("width", w);
        $("#TableHeader").css("width", w - scrollWidth);
    }
    else {
        $("#divTableHeader").css("width", w);
        $("#divTableBody").css("width", w);
    }
}

function Query(NeedClearn) {

    var url = "/Transfer/Query";
    $.ajax({
        type: 'post',
        data: "pageIndex=0",
        url: url,
        success: function (data) {
            var arrLen = data.length;
            if (NeedClearn)
                $("#trContainer").empty();
            if (arrLen > 0) {
                generateData(data);
                SetWidth();
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
        

        strCtrl = "";
        strCtrl += "<tr>";

        strCtrl += "<td><a href=/Order/Info_Win?id=" + result[i].TransferId + "  target='_blank' class='td'>订单信息</a>";
        strCtrl += " <input type='hidden' value='" + result[i].FilePath + "'</td>";
        //汇款编号
        tdWidth = "width:" + $("#trHeader th").eq(1).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TransferId + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TransferAmount + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(3).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AgentName + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(4).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TransDate + "</td>";

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