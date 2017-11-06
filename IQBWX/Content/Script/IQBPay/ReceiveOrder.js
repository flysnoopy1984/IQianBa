function Query()
{
    var url = "/PP/OrderReceive";

    var receiveNo = $("#ReceiveNo").val();

    $.ajax({
        type: 'post',
        data: "ReceiveNo=" + receiveNo,
        url: url,
        success: function (data) {

            var arrLen = data.length;
            if (arrLen > 0) {
                generateData(data);
               
            }
            else {
                pageIndex--;
                $("#btnNext").attr("disabled", true);
                alert("没有数据了");
            }
        },
        error: function (xhr, type) {
            alert('Ajax error!');
            // 即使加载出错，也得重置
        }
    });
}

function generateData(result) {
    var strCtrl = "";


    $.each(result, function (i) {


        strCtrl = "";
        strCtrl += "<li>";

        //tdWidth = "width:" + $("#trHeader th").eq(0).css("width");
        strCtrl += "<td style='width:50%'><ul><li style='color:brown'>" + result[i].OrderNo + "</li>";
        strCtrl += "<li>创建时间:" + result[i].TransDateStr + "</li>";
        strCtrl += "<li style='color:gray; font-size:12px;'>订单状态:" + orderStatus + "</li></ul></td>";
        strCtrl += "<td style='width:50%'><ul><li style='color:firebrick; font-weight:bold;'>" + result[i].RealTotalAmount + "</li>";
        strCtrl += "<li>订单总额:" + result[i].TotalAmount + "</li>";
        strCtrl += "<li>付款账户:" + result[i].BuyerAliPayLoginId + "</li></ul></td>";

        //tdWidth = "width:" + $("#trHeader th").eq(1).css("width");
        //strCtrl += "<td>" + result[i].TransDateStr + "</td>";

        //tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        //strCtrl += "<td>" + result[i].BuyerAliPayLoginId + "</td>";

        // tdWidth = "width:" + $("#trHeader th").eq(1).css("width");


        //tdWidth = "width:" + $("#trHeader th").eq(4).css("width");
        //strCtrl += "<td>" + orderStatus + "</td>";

        strCtrl += "</li>";

        $("#trContainer").append(strCtrl);
    });
}
   