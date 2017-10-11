
$(document).ready(function () {
    var th = $("#trHeader th").eq(0);
    alert(th.text());
    alert(th.css("width"));
 //  Query(true);
});

function Query(NeedClearn) {

    var url = "/Order/Query";
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

        var ppRealAmt = result[i].TotalAmount - result[i].SellerCommission;

        strCtrl = "";
        strCtrl += "<tr>";

        strCtrl += "<td><a href='/Order/Info?id=" + result[i].OrderNo + "' class='td'>详情</a>";
        strCtrl += " <input type='hidden' value='" + result[i].FilePath + "'</td>";

        tdWidth = "width:"+ $("#trHeader th").eq(1).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].OrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(3).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(4).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(5).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(6).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(7).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(8).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(9).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(10).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(11).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(12).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(13).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(14).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(15).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(16).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].AliPayOrderNo + "</td>";
      

       
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
}

function ToInfo(action) {
    window.location.href = "OrderInfo?do=" + action;
    return;
}