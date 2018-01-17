function QueryNoAccount()
{
    var url = "/User/UserNoAccount";
    var cDateBefore = $("#cDateBefore").val();

    $.ajax({
        type: 'post',
        data: "DateBefore=" + cDateBefore,
        url: url,
        success: function (data) {
            var arrLen = data.length;

            $("#trContainer").empty();

            if (arrLen > 0) {
             //  pageIndex++;
                generateData(data);
            }
            else {
               // pageIndex--;
                alert("没有数据了");
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
        var HasQRHuge = "没有";
        if (result[i].HasQRHuge) {
            HasQRHuge = "有";
        }


        strCtrl = "";
        strCtrl += "<tr>";
        strCtrl += "<td title='" + result[i].Name + "'>" + result[i].Name + "</td>";
        strCtrl += "<td>" + result[i].Rate + "</td>";
        strCtrl += "<td>" + result[i].ParentCommissionRate + "</td>";
        strCtrl += "<td>" + result[i].MarketRate + "</td>";
        strCtrl += "<td title='" + result[i].ParentAgent + "'>" + result[i].ParentAgent + "</td>";
        strCtrl += "<td>" + result[i].CDate + "</td>";
        if (result[i].HasQRHuge)
            strCtrl += "<td style='color:blue'>" + HasQRHuge + "</td>";
        else
            strCtrl += "<td>" + HasQRHuge + "</td>";

        //strCtrl += "<td title='" + result[i].AliPayAccount + "'>" + result[i].AliPayAccount + "</td>";
        strCtrl += "<td title='" + result[i].OpenId + "'>" + result[i].OpenId + "</td>";


        if (result[i].StoreName == "" || result[i].StoreName == null) {
            result[i].StoreName = "随机";
        }

        strCtrl += "<td title='" + result[i].StoreName + "'>" + result[i].StoreName + "</td>";

        if (result[i].UserStatus == 1)
            strCtrl += "<td style='color:#4AC4BC;'><div class='noft-green-number'></div>启用</td>";
        else
            strCtrl += "<td><div class='noft-red-number'></div>禁用</td>";

        strCtrl += "<td><a href='/User/Info?OpenId=" + result[i].OpenId + "' class='td'>详情</a><br />"
        strCtrl += "<input type=button onclick=deleteUser('" + result[i].OpenId + "') value='删除' /></td>";
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });


}