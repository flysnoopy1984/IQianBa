
$(document).ready(function () {
    Query();
});

function Query() {

    var url = "/QR/Query";
    $.ajax({
        type: 'post',
        data: "QRType=3&pageIndex=0",
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
        var storeName = result[i].StoreName;
        if (storeName == null)
            storeName = "随机";

        var parentName = result[i].ParentName;
        if (parentName == null)
            parentName = "无";

        strCtrl = "";
        strCtrl += "<tr>";
      //  strCtrl += "<td>" + result[i].ID + "</td>";
        strCtrl += "<td>" + result[i].Name + "</td>";
        strCtrl += "<td>" + result[i].Rate + "</td>";
        strCtrl += "<td>" + parentName + "</td>";
        strCtrl += "<td>" + result[i].ParentCommissionRate + "</td>";
        strCtrl += "<td>" + storeName + "</td>";
        strCtrl += "<td>" + result[i].Remark + "</td>";

        if (result[i].RecordStatus == 0)
            strCtrl += "<td style='color:#4AC4BC;'><div class='noft-green-number'></div>启用</td>";
        else
            strCtrl += "<td><div class='noft-red-number'></div>停用</td>";

        strCtrl += "<td><a href='/QR/ARInfo?id=" + result[i].ID + "' class='td'>详情</a>";
        strCtrl += " <input type='hidden' value='" + result[i].FilePath + "'</td>";
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
}

function ToInfo(action) {
    window.location.href = "ARInfo?do=" + action;
    return;
}
