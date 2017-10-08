$(document).ready(function () {
    Query();
});

function Query() {

    var url = "/Order/Query";
    $.ajax({
        type: 'post',
        data: "pageIndex=0",
        url: url,
        success: function (data) {
            var arrLen = data.length;

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

        var ppRealAmt = result[i].TotalAmount - 
        strCtrl = "";
        strCtrl += "<tr>";
   
        strCtrl += "<td>" + result[i].OrderNo + "</td>";
        strCtrl += "<td>" + result[i].AliPayOrderNo + "</td>";
        strCtrl += "<td>" + result[i].TotalAmount + "</td>";
        strCtrl += "<td>" + result[i].Rate + "</td>";
        strCtrl += "<td>" + result[i].RateAmount + "</td>";
        strCtrl += "<td>" + result[i].AgentName + "</td>";
        strCtrl += "<td>" + result[i].RealTotalAmount + "</td>";
        strCtrl += "<td>" + result[i].SellerName + "</td>";
        strCtrl += "<td>" + result[i].SellerChannel + "</td>";
        strCtrl += "<td>" + result[i].SellerRate + "</td>";
        strCtrl += "<td>" + result[i].SellerCommission + "</td>";
        strCtrl += "<td>" + result[i].Remark + "</td>";
        strCtrl += "<td>" + result[i].Remark + "</td>";
        strCtrl += "<td>" + result[i].Remark + "</td>";
        strCtrl += "<td>" + result[i].Remark + "</td>";
        strCtrl += "<td>" + result[i].Remark + "</td>";
        strCtrl += "<td>" + result[i].Remark + "</td>";

      

        strCtrl += "<td><a href='/QR/ARInfo?id=" + result[i].ID + "' class='td'>详情</a>";
        strCtrl += " <input type='hidden' value='" + result[i].FilePath + "'</td>";
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
}

function ToInfo(action) {
    window.location.href = "OrderInfo?do=" + action;
    return;
}