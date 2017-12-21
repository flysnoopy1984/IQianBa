var pageIndex = -1;

$(document).ready(function () {

    Query(true,pageIndex + 1);
});

function Next() {

    Query(false, pageIndex + 1);
}

function Prev() {
   
    pageIndex--;
    if (pageIndex < 0)
    {
        alert("已经第一页了");
        pageIndex = 0;
        return;

    }
    Query(false, pageIndex);
}

function Query(NeedClearn,_PageIndex) {

    if (_PageIndex == 0)
        $("#trContainer").empty();

    var url = "/User/Query";
    $.ajax({
        type: 'post',
        data: "role=2&pageIndex=" + _PageIndex,
        url: url,
        success: function (data) {
            var arrLen = data.length;
          
            if (NeedClearn) {
                $("#trContainer").empty();
            }

            if (arrLen > 0) {
                pageIndex++;
                generateData(data);
            }
            else {
                pageIndex--;
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

       
        strCtrl = "";
        strCtrl += "<tr>";
        strCtrl += "<td>" + result[i].Name + "</td>";
        strCtrl += "<td>" + result[i].Rate + "</td>";
        strCtrl += "<td>" + result[i].CDate + "</td>";
        strCtrl += "<td>" + result[i].AliPayAccount + "</td>";
        strCtrl += "<td>" + result[i].IsAutoTransfer + "</td>";
        strCtrl += "<td>" + result[i].ParentAgent + "</td>";
        strCtrl += "<td>" + result[i].ParentCommissionRate + "</td>";
        if (result[i].StoreName == "" || result[i].StoreName == null)
        {
            result[i].StoreName = "随机";
        }
       
       strCtrl += "<td>" + result[i].StoreName + "</td>";

        if (result[i].UserStatus == 1)
            strCtrl += "<td style='color:#4AC4BC;'><div class='noft-green-number'></div>启用</td>";
        else
            strCtrl += "<td><div class='noft-red-number'></div>禁用</td>";

        strCtrl += "<td><a href='/User/Info?id=" + result[i].Id + "' class='td'>详情</a></td>";
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
}
