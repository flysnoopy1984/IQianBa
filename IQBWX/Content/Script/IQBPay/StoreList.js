var pageIndex = -1;
var url = site + "/PP/StoreQuery";
var QueryData;

$(document).ready(function () {

    $("#btnNext").attr("disabled", false);

    Query(pageIndex + 1);

    ToListPage();

    //Info Page
    $(".InfoBody").Validform({
        tiptype: 2,
        postonce: true,
        datatype: {
            "empty": /^\s*$/
        },
    });

    QueryData = [];
});

function ToListPage()
{
    $("#PageList").show();
    $("#PageInfo").hide();
}



function ToInfoPage(i) {
    $("#PageList").hide();
    $("#PageInfo").show();
  
    i--;
    $("#Name").val(QueryData[i].Name);
    $("#Rate").val(QueryData[i].Rate);
    $("#MaxLimitAmount").val(QueryData[i].MaxLimitAmount);
    $("#DayIncome").val(QueryData[i].DayIncome);
}



function Query(_pageIndex) {

    var PageSize = 30;
  
    $.ajax({
        type: 'post',
        data: "Page=" + _pageIndex + "&PageSize=" + PageSize,
        url: url,
        success: function (data) {
            var arrLen = data.length;

            if (arrLen > 0) {
                
                generateData(data);
                pageIndex++;

            }
            else {
                pageIndex--;
                $("#btnNext").attr("disabled", true);
                alert("没有数据了");

            }

        },
        error: function (xhr, type) {
            alert('Ajax error!');
            // 即使加载出错，也得重置

        }
    });
    $("#trContainer").show();
    $("#Process").hide();
}

function generateData(result) {
    var strCtrl = "";


    $.each(result, function (i) {
        //if (pageIndex == -1 && i == 0) {
        //    //  pageCount = result[i].TotalCount;
        //    $("#TodayCommAmt").text(result[i].TodayCommAmt);
        //    $("#TotalCommAmt").text(result[i].TotalCommAmt);
        //    if (result[i].ID == 0)
        //        return true;
        //}
        QueryData.push(result[i]);
       

        var IsAuth;
        if (result[i].IsAuth)
            IsAuth = "已授权";
        else
            IsAuth = "未授权";

        strCtrl = "";
        strCtrl += "<tr>";

        //tdWidth = "width:" + $("#trHeader th").eq(0).css("width");
        strCtrl += "<td style='width:47%' onclick='ToInfoPage(" + QueryData.length + ");'><ul><li style='color:brown'>" + result[i].Name + "</li>";
        strCtrl += "<li>返佣点率:" + result[i].Rate + "</li>";
        strCtrl += "<li style='color:gray; font-size:12px;'>是否授权:" + IsAuth + "</li></ul></td>";
        strCtrl += "<td style='width:47%' onclick='ToInfoPage("+QueryData.length+");'><ul><li style='color:firebrick; font-weight:bold;'>每日限额" + result[i].DayIncome + "</li>";
        strCtrl += "<li>分控额度:" + result[i].MaxLimitAmount + "</li>";
        strCtrl += "<li>创建时间:" + result[i].CDate + result[i].CTime + "</li></ul></td>";
        strCtrl += "<td class='GoToInfoButton' onclick='ToInfoPage(" + QueryData.length + ");'>></td>";

        //tdWidth = "width:" + $("#trHeader th").eq(1).css("width");
        //strCtrl += "<td>" + result[i].TransDateStr + "</td>";

        //tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        //strCtrl += "<td>" + result[i].BuyerAliPayLoginId + "</td>";

        // tdWidth = "width:" + $("#trHeader th").eq(1).css("width");


        //tdWidth = "width:" + $("#trHeader th").eq(4).css("width");
        //strCtrl += "<td>" + orderStatus + "</td>";

        strCtrl += "</tr>";



        $("#trContainer").append(strCtrl);
    });


}
