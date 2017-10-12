
$(document).ready(function () {

    Query();
});

function Query() {

    var url = "/User/Query";
    $.ajax({
        type: 'post',
        data: "role=2&pageIndex=0",
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

       
        strCtrl = "";
        strCtrl += "<tr>";
        strCtrl += "<td>" + result[i].Name + "</td>";
        strCtrl += "<td>" + result[i].Rate + "</td>";
        strCtrl += "<td>" + result[i].CDate + "</td>";
        strCtrl += "<td>" + result[i].AliPayAccount + "</td>";
   

        if (result[i].UserStatus == 1)
            strCtrl += "<td style='color:#4AC4BC;'><div class='noft-green-number'></div>启用</td>";
        else
            strCtrl += "<td><div class='noft-red-number'></div>禁用</td>";

        strCtrl += "<td><a href='/User/Info?id=" + result[i].Id + "' class='td'>详情</a></td>";
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
}
