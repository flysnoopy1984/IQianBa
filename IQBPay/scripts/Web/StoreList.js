var Channel
$(document).ready(function () {
    Channel = GetUrlParam("Channel");
    if (Channel == null || Channel == undefined)
        Channel = -1;
    else
    {
        if (Channel == 0)
            $("#ListTitleName").text("平台商户列表");
        else if(Channel == 1)
            $("#ListTitleName").text("加盟商户列表");
    }
        Query();
    });

function Query() {

    var url = "/Store/Query";
    $.ajax({
        type: 'post',
        data: "Channel="+Channel+"&pageIndex=0&pageSize=20",
        url: url,
        success: function (data) {
            var arrLen = data.length;
            
            $("#trContainer").empty();
            if (arrLen > 0)
            {
                generateData(data);
            }
        },
        error: function (xhr, type) {

            alert('Ajax error!');
           
        }
    });
}

function generateData(result)
{
    var strCtrl = "";
    $.each(result, function (i) {
        
        var cn;
        var channel = result[i].Channel;
        if (channel == 0)
            cn = "平台";
        else if (channel == 1)
            cn = "加盟商";

        strCtrl = "";
        strCtrl += "<tr>";
        strCtrl += "<td>" + result[i].Name + "</td>";
        strCtrl += "<td>" + cn + "</td>";
        strCtrl += "<td>" + result[i].Rate + "</td>";
        strCtrl += "<td>" + result[i].IsReceiveAccount + "</td>";
        strCtrl += "<td>" + result[i].AliPayAccount + "</td>";
        strCtrl += "<td>" + result[i].OpenTime + "</td>";
        strCtrl += "<td>" + result[i].CloseTime + "</td>";
        strCtrl += "<td>" + result[i].Remark + "</td>";

        if (result[i].RecordStatus == 0)
            strCtrl += "<td style='color:#4AC4BC;'><div class='noft-green-number'></div>启用</td>";
        else
            strCtrl += "<td><div class='noft-red-number'></div>停用</td>";

        strCtrl += "<td><a href='/Store/Info?id=" + result[i].ID + "' class='td'>详情</a></td>";
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
}

// 根据channel 去不同的页面
function ToInfo(action,channel)
{
    window.location.href = "/QR/AuthInfo?do=" + action+"&c="+channel;
    
    return;
}
