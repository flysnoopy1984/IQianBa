﻿var Id = GetUrlParam("id");
var type = GetUrlParam("type");

$(document).ready(function () {

    if (Id == null || Id == "" || Id == undefined) {
        alert("未获取参数");
        window.close();
    }
    Query(Id,type);
});

function Query(Id,type)
{
    var url = "/Transfer/InfoWin";
    $.ajax({
        type: 'post',
        data: "Id="+Id+"&type="+type,
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
    if (type == 1)
    {
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
        return;
    }
    if(type==2)
    {
        strCtrl = "";
        strCtrl += "<tr>";

        strCtrl += "<td>" + result.Transfer.TransferId + "</td>";
        strCtrl += "<td>" + result.Transfer.AgentName + "</td>";
        strCtrl += "<td>" + result.Transfer.TransferAmount + "</td>";
        strCtrl += "<td>" + result.Transfer.TransDateStr + "</td>";
        strCtrl += "<td>" + result.Transfer.OrderNo + "</td>";

        strCtrl += "</tr>";
        $("#TransferContainer").append(strCtrl);

        $.each(result.OrderList, function (i) {
            strCtrl = "";
            strCtrl += "<tr>";

            strCtrl += "<td>" + result.OrderList[i].OrderNo + "</td>";
            strCtrl += "<td>" + result.OrderList[i].AgentName + "</td>";
            strCtrl += "<td>" + result.OrderList[i].RealTotalAmount + "</td>";
            strCtrl += "<td>" + result.OrderList[i].TransDateStr + "</td>";
            strCtrl += "<td>" + result.OrderList[i].TransferId + "</td>";

          
            strCtrl += "</tr>";
            $("#OrderContainer").append(strCtrl);
        });
        return;
    }
   

}
