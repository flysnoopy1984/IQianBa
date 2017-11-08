function StartQuery()
{
    $("#ROProcess").show();

    $("#ROProcess").nextAll().remove();
}
function EndQuery()
{
    $("#ROProcess").hide();
}


function Query()
{
    var url = "/PP/OrderReceive";

    var receiveNo = $("#ReceiveNo").val();
   
    StartQuery();
    $.ajax({
        type: 'post',
        data: "ReceiveNo=" + receiveNo,
        url: url,
        success: function (data) {
            EndQuery();
           
            var arrLen = data.length;
            if (arrLen > 0) {
                generateData(data);
                
            }
            //else {
            //    pageIndex--;
            //    $("#btnNext").attr("disabled", true);
            //    alert("没有数据了");
            //}
            
        },
        error: function (xhr, type) {
            EndQuery();
            alert('Ajax error!');
            // 即使加载出错，也得重置
        }
    });
}

function generateData(result) {
    var strCtrl = "";
    var status;
  
    $.each(result, function (i) {

        status = result[i].OrderStatus;

        strCtrl = "";
        strCtrl += "<li>";
        strCtrl += "<div>收货订号: ";
        strCtrl += result[i].OrderNo;
        strCtrl += "</div>";
        strCtrl += "<div>交易时间: ";
        strCtrl += result[i].TransDateStr;
        strCtrl += "</div>";
        strCtrl += "<div>金额: ";
        strCtrl += result[i].Amount;
        strCtrl += "</div>";
        strCtrl += "</li>";
        if(status==-2)
            strCtrl += "<button type='button' class='btn btn-success ConfirmButton' id='btnConfrimRO' onclick=ConfirmRO(this,'" + result[i].OrderNo + "');>确认收款</button>";
        strCtrl += "<hr />";
        
        $("#ROList").append(strCtrl);
    });
}

function ConfirmRO(obj, OrderNo) {

    $(obj).attr("disabled", true);

    var url = "/PP/ConfirmRO";

    $.ajax({
        type: 'post',
        data: "OrderNo=" + OrderNo,
        url: url,
        success: function (data) {
         
            if (data.RunResult == "OK")
            {
                alert("收货确认成功");
                $(obj).hide();
            }
            else
            {
                alert(data.RunResult);
                $(obj).attr("disabled", false);
            }

        },
        error: function (xhr, type) {
            
            alert('Ajax error!');
            $(obj).attr("disabled", false);
            // 即使加载出错，也得重置
        }
    });
   
}

   