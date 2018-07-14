
$(document).ready(function () {
    Query();
});

function Query() {

    var url = "/QR/Query";
    $.ajax({
        type: 'post',
        data: "QRType=2&pageIndex=0",
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

        var cn;
        var app;

        var channel = result[i].Channel;
        if (channel == 0)
            cn = "平台";
        else if (channel == 1)
            cn = "加盟商";

        if (result[i].APPId == "2017112100077913")
            app = "玉杰";
        else if (result[i].APPId == "2017122901328258")
            app = "寒翼";
        else if (result[i].APPId == "2018060760292909")
            app = "哲胜";
        


        strCtrl = "";
        strCtrl += "<tr>";
    
        strCtrl += "<td>" + result[i].Name + "</td>";
        strCtrl += "<td>" + result[i].Rate + "</td>";
        strCtrl += "<td>" + cn + "</td>";
        strCtrl += "<td>" + app + "</td>";
        strCtrl += "<td>" + result[i].Remark + "</td>";

        if (result[i].RecordStatus == 0)
            strCtrl += "<td style='color:#4AC4BC;'><div class='noft-green-number'></div>未使用</td>";
        else
            strCtrl += "<td><div class='noft-red-number'></div>已使用</td>";

        strCtrl += "<td><a href='/QR/AuthInfo?id=" + result[i].ID + "' class='td'>详情</a>";
        strCtrl += " <input type='hidden' value='" + result[i].FilePath + "'</td>";
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
}

function ToInfo(action,channel) {
    window.location.href = "AuthInfo?do=" + action + "&c=" + channel;
    return;
}
