var pageIndex = -1;
var url = site + "/PP/Agent_QR_ARListQuery";
var QueryData;

$(document).ready(function () {

    $("#btnNext").attr("disabled", false);

    Query(pageIndex + 1);

    ToListPage();

    //Info Page
    //Info Page
     $(".InfoBody").Validform({
        tiptype: 2,
        postonce: true,
        ignoreHidden: true,
        btnSubmit: "#btnAdjustAgentQR",
        datatype: {
            "empty": /^\s*$/,
            "My8-12": /^([8-9]|1[0-2])$/,
        },
        callback: function (data) {
           
        },
    });

    QueryData = [];
});

function ToListPage() {
    $("#PageList").show();
    $("#PageInfo").hide();
}



function ToInfoPage(i) {
    $("#PageList").hide();
    $("#PageInfo").show();

    i--;
  
    $("#MarketRate").val(QueryData[i].MarketRate);
 
   
}



function Query(_pageIndex) {

    var PageSize = 20;

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
      
        QueryData.push(result[i]);


        var ParentName = result[i].ParentName;
        if (ParentName == "")
            ParentName = "无"

        strCtrl = "";
        strCtrl += "<tr>";

       
        strCtrl += "<td style='width:47%' onclick='ToInfoPage(" + QueryData.length + ");'><ul><li style='color:brown; font-weight:bold;'>用户手续费" + result[i].MarketRate + "</li>";
        strCtrl += "<li>上级代理:" + ParentName + "</li>";
        strCtrl += "</ul></td>";
        strCtrl += "<td style='width:47%' onclick='ToInfoPage(" + QueryData.length + ");'><ul><li>代理点率" + result[i].Rate + "</li>";
        strCtrl += "<li>上级代理佣金:" + result[i].ParentCommissionRate + "</li>";
        strCtrl += "</ul></td>";
        strCtrl += "<td><input type='button' class='btn-primary' value='调整' onclick='ToInfoPage(" + QueryData.length + ");' /></td>";

     

        strCtrl += "</tr>";



        $("#trContainer").append(strCtrl);
    });


}
