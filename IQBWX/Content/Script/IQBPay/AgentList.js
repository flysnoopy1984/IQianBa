var pageIndex = -1;
var QueryData;

$(document).ready(function () {

    Init();
});

function Init() {
   
    $("#trContainer").empty();
    pageIndex = -1;

    QueryData = [];

    //   Query(pageIndex+1);
    pageIndex = 0;
    Query2()
   
}

function Query2() {

    var PageSize = 10;
    var url = "/PP/AgentListQuery";

    var AgentName = $("#AgentName").val();

    $('#ListTableBody').dropload({
        scrollArea: window,
        loadDownFn: function (me) {

            // 拼接HTML
            var result = '';

            $.ajax({
                type: 'post',
                data: "Page=" + pageIndex + "&PageSize=" + PageSize + "&AgentName=" + AgentName,
                url: url,
                success: function (data) {
                    var arrLen = data.length;
                    if (arrLen > 0) {
                        generateData(data);
                        pageIndex++;
                        // 如果没有数据
                    } else {
                        // 锁定
                        me.lock();
                        // 无数据
                        me.noData();
                       

                    }
                    // 为了测试，延迟1秒加载
                    setTimeout(function () {
                        // 每次数据插入，必须重置
                        me.resetload();
                    }, 1000);
                },
                error: function (xhr, type) {
                 //   alert('Ajax error!');
                    // 即使加载出错，也得重置
                    me.resetload();
                }
            });


        }
    });
    $("#trContainer").show();
    //$("#Process").hide();
}

function Query(_pageIndex) {

    var PageSize = 40;
    var url = "/PP/AgentListQuery";

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
               // $("#btnNext").attr("disabled", true);
              

            }

        },
        error: function (xhr, type) {

            alert('Ajax error!');
            // 即使加载出错，也得重置

        }
    });
    $("#trContainer").show();
    //$("#Process").hide();
}

function generateData(result) {
    var strCtrl = "";


    $.each(result, function (i) {

     //   QueryData.push(result[i]);
       
        if (i == 0)
        {
            $("#TotalMember").text(result[0].TotalMember);
            $("#TotalAmount").text(result[0].TotalAmount);
        }
        var UserStatus;
        var usColor = "style='color:forestgreen'";
        if (result[i].UserStatus == 0) {
            usColor = "style='color:crimson'";
            UserStatus = "禁用";
        
            
        }
        else
            UserStatus = "正常";

        strCtrl = "";
        if (result[i].UserStatus == 0)
            strCtrl += "<tr style='background-color:#949494'>";
        else
            strCtrl += "<tr>";
        strCtrl += "<td style='width:25%;height:80px;line-height:80px;'>";
        strCtrl += "<img style='vertical-align: middle;text-align: center;' class='img-profile' src='" + result[i].HeadImgUrl + "' />";
        if (result[i].UserStatus == 0)
            strCtrl += "<span style='color:#ff0000'>" + UserStatus + "</span>";//43ce08
        else
            strCtrl += "<span style='color:#43ce08'>" + UserStatus + "</span>";
        strCtrl += "</td>";
        strCtrl += "<td style='width:75%'>";
        strCtrl += "<ul><li style='font-weight:bold; height:40px;'>代理名称:" + result[i].UserName + "</li>";       
        strCtrl += "</ul>";
        strCtrl += "<ul>";
        strCtrl += "<li style='color:cornflowerblue; width:49%;float:left;' > 费率: " + (result[i].MarketRate - result[i].Rate) + "</li>";
        strCtrl += "<li style='height:40px; color:coral;width:49%;float:left;'>上级代理佣金: " + result[i].ParentCommissionRate + "</li>";
        strCtrl += "</ul>"
        strCtrl += "</td>";

       
        //strCtrl += "<li " + usColor + ">用户手续费：" + result[i].MarketRate + "</li>";
     
        strCtrl += "</ul></td>";
        //strCtrl += "<td><input type='button' class='btn-primary' value='详情' onclick='ToInfoPage(" + (QueryData.length-1) + ");' />"
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);
    });


}

function ToInfoPage(i)
{
    var result = QueryData[i];
    window.location.href = "/PP/AgentDetail?QrUserId=" + result.qrUserId + "&Rate=" + result.Rate + "&ParentComm=" + result.ParentCommissionRate + "&MarketRate=" + result.MarketRate + "&ParentOpenId=" + result.ParentOpenId;
    //$.alert({
    //    theme: 'dark',
    //    title: 'WOW！',
    //    content: "暂未开放，敬请期待！",
    //});
}

function FilterAgent()
{
    $(".dropload-down").remove();
    Init();
   
}
