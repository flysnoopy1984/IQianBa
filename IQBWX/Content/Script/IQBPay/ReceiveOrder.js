function StartQuery()
{
    $("#ROProcess").show();
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
            $("#ROProcess").nextAll().remove();
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
            EndQuery();
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
        strCtrl += "<div class='liBodyDivLeft'>";
        strCtrl += result[i].OrderNo;
        strCtrl += "</div>";
        strCtrl += "<div class='liBodyDivLeft'>";
        strCtrl += result[i].TransDateStr;
        strCtrl += "</div>";
        strCtrl += "<div class='liBodyDivRight'>";
        strCtrl += result[i].Amount;
        strCtrl += "</div>";
        strCtrl += "</li>";

        $("#ROList").append(strCtrl);
    });
}
   