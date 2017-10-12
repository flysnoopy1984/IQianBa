
$(document).ready(function () {

    var Id = GetUrlParam("id");
    Query(Id);
});

function Query(Id)
{
    var url = "/Transfer/InfoWin";
    $.ajax({
        type: 'post',
        data: "Id="+Id+"&type=1",
        url: url,
        success: function (data) {
          
            if (data.Result == -2 || data.Result == -1)
                alert("没有找到数据")
            else
            {
                generateData(data);
            }
           
        },
        error: function (xhr, type) {

            alert('Ajax error!');

        }
    });
}

function generateData(result) {
    //Order
    strCtrl = "";
    strCtrl += "<tr>";
  
    strCtrl += "<td>" + result.Order.OrderNo + "</td>";
    strCtrl += "<td>" + result.Order.AgentName + "</td>";
    strCtrl += "<td>" + result.Order.RealTotalAmount + "</td>";
    strCtrl += "<td>" + result.Order.TransDateStr + "</td>";
    strCtrl += "<td>" + result.Order.TransferId + "</td>";
    strCtrl += "</tr>";
    $("#OrderContainer").append(strCtrl);

    $.each(result.TransferList, function (i) {
        strCtrl = "";
        strCtrl += "<tr>";

        strCtrl += "<td>" + result.TransferList[i].TransferId + "</td>";
        strCtrl += "<td>" + result.TransferList[i].AgentName + "</td>";
        strCtrl += "<td>" + result.TransferList[i].TransferAmount + "</td>";
        strCtrl += "<td>" + result.TransferList[i].TransDateStr + "</td>";
        strCtrl += "<td>" + result.TransferList[i].OrderNo + "</td>";
        strCtrl += "</tr>";
        $("#TransferContainer").append(strCtrl);
    });

}
