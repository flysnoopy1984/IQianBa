var Id = GetUrlParam("id");
var OrderType = GetUrlParam("OrderType");
var type = GetUrlParam("type");
var O2ONo = GetUrlParam("O2ONo");

$(document).ready(function () {

    
    if (OrderType == null || OrderType == "" || OrderType == undefined) {
        OrderType = "N";
        if (Id == null || Id == "" || Id == undefined) {
            alert("未获取参数 Id");
            window.close();
        }
    }
    else {
        OrderType = "O2O";
        if (O2ONo == null || O2ONo == "" || O2ONo == undefined) {
            alert("未获取参数 O2ONo");
            window.close();
        }
    }
    if (OrderType == "O2O")
    {
        QueryO2O();
        type = 1;
    }
       
    else
        Query(Id,type);
});

/*O2O begin */
function  QueryO2O()
{
    var url = "/O2O/GetTransferByO2ONo";
    $.ajax({
        type: 'post',
        data: { "O2ONo": O2ONo },
        url: url,
        success: function (data) {

            if (data.TransferList.length == 0)
                alert("没有找到数据");
            else {
                generateData(data);
            }

        },
        error: function (xhr, type) {

            alert('Ajax error!');

        }
    });
}
/*O2O end */

function AdvAlert(result) {

    $.alert({
        theme: 'dark',
        title: '异常',
        content: result,
    });
}

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
        strCtrl += "<td>" + result.Order.RateAmount + "</td>";
        strCtrl += "<td>" + result.Order.TransDateStr + "</td>";
        //strCtrl += "<td>" + result.Order.TransferId + "</td>";
        strCtrl += "</tr>";
        $("#OrderContainer").append(strCtrl);

        $.each(result.TransferList, function (i) {
            var target = "";
            switch(result.TransferList[i].TransferTarget)
            {
                case 0:
                    target = "用户打款";
                    break;
                case 1:
                    target = "代理转账";
                    break;
                case 2:
                    target = "上级代理佣金";
                    break;
                case 20:
                    target = "出库商佣金";
                    break;
            }

            strCtrl = "";
            strCtrl += "<tr>";
            strCtrl += "<td>" + result.TransferList[i].TransferId + "</td>";
            strCtrl += "<td>" + target + "</td>";
            strCtrl += "<td>" + result.TransferList[i].TransferAmount + "</td>";
            strCtrl += "<td>" + result.TransferList[i].TransDateStr + "</td>";
            strCtrl += "<td>" + result.TransferList[i].TargetAccount + "</td>";
            strCtrl += "<td><a href=\"javascript:AdvAlert('" + result.TransferList[i].Log + "')\">" + result.TransferList[i].Log + "</a></td>";
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
        strCtrl += "<td>" + result.Transfer.TargetAccount + "</td>";
        strCtrl += "</tr>";
        $("#TransferContainer").append(strCtrl);

        $.each(result.OrderList, function (i) {
            strCtrl = "";
            strCtrl += "<tr>";

            strCtrl += "<td>" + result.OrderList[i].OrderNo + "</td>";
            strCtrl += "<td>" + result.OrderList[i].AgentName + "</td>";
            strCtrl += "<td>" + result.OrderList[i].RealTotalAmount + "</td>";
            strCtrl += "<td>" + result.OrderList[i].TransDateStr + "</td>";
          

          
            strCtrl += "</tr>";
            $("#OrderContainer").append(strCtrl);
        });
        return;
    }
   

}
