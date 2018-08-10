﻿var pageIndex = -1;
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
    var userId ="";

    $.each(result, function (i) {

        if (userId == result[i].userId)
            return true;

        QueryData.push(result[i]);
        //if (i == 0 && pageIndex ==0)
        //{
        //    $("#TotalMember").text(result[0].TotalMember + "/" + result[0].MaxInviteCount);
        //    $("#TotalAmount").text(result[0].TotalAmount);
        //}

        var UserStatus;
        //var usColor = "style='color:forestgreen'";
        if (result[i].UserStatus == 0) {
            usColor = "style='color:crimson'";
            UserStatus = "禁用";

        }
        else
            UserStatus = "正常";
      
       // userId = result[i].userId

        strCtrl = "";
        //if (result[i].UserStatus == 0)
        //    strCtrl += "<tr style='background-color:#949494'>";
        //else
       strCtrl += "<tr>";

        strCtrl += "<td style='width:25%;height:80px;line-height:80px;'>";
        strCtrl += "<img style='vertical-align: middle;text-align: center;' class='img-profile' src='" + result[i].HeadImgUrl + "' />";
        if (result[i].UserStatus == 0)
            strCtrl += "<span style='color:#ff0000'>" + UserStatus + "</span>";//43ce08
        else
            strCtrl += "<span style='color:#43ce08'>" + UserStatus + "</span>";
        strCtrl += "</td>";

        strCtrl += "<td style='width:75%'>";
        strCtrl += "<ul><li style='font-weight:bold; height:55px;'>代理名称:" + result[i].UserName + "</li>";       
        strCtrl += "</ul>";

    

        var totalAmount = 0;
        //大额费率返点
        //if (result[i].HugeQR)
        //{
        //    strCtrl += "<ul>";
        //    strCtrl += "<li style='color:cornflowerblue; width:49%;float:left;' > 大额费率: " + result[i].HugeQR.FeeRate.toFixed(2) + "</li>";
        //    strCtrl += "<li style='height:32px; color:#EBC952;width:49%;float:left;'>大额返点: " + result[i].HugeQR.ParentCommissionRate + "</li>";
        //    strCtrl += "</ul>";

        //    totalAmount = result[i].MemberTotalAmount + result[i].HugeQR.MemberTotalAmount;
        //    strCtrl += "<ul>";
        //    strCtrl += "<li style='color:#8E210B; width:99%;height:32px;' >业绩: " + result[i].MemberTotalAmount.toFixed(2) + "(小)" + "+" + result[i].HugeQR.MemberTotalAmount.toFixed(2) + "(大)" + "=" + totalAmount.toFixed(2) + " &yen</li>";
        //    strCtrl += "</ul>";
        //}
        //else
        //{
           
        //}
        strCtrl += "<ul>";
        strCtrl += "<li style='color:#8E210B; width:99%;height:32px;' >业绩: " + result[i].MemberTotalAmount.toFixed(2) + " &yen</li>";
        strCtrl += "</ul>";

        strCtrl += "<ul>";
        strCtrl += "<li style='width:99%;height:32px;' >注册日期: " + result[i].RegisterDate + "</li>";
        strCtrl += "</ul>";
        //小额费率 返点
        if (userId != result[i].userId)
        {
            strCtrl += "<ul>";
            strCtrl += "<li style='color:cornflowerblue; width:49%;float:left;' > 小额费率: " + result[i].FeeRate.toFixed(2) + "</li>";
            strCtrl += "<li style='height:32px; color:#EBC952;width:49%;float:left;'>返点: 0.1</li>";
            strCtrl += "</ul>";

            strCtrl += "<ul>";
            strCtrl += "<li style='color:cornflowerblue; width:49%;float:left;' > 信用卡费率: " + result[i+1].FeeRate.toFixed(2) + "</li>";
            strCtrl += "<li style='height:32px; color:#EBC952;width:49%;float:left;'>返点: 0.02</li>";
            strCtrl += "</ul>";
        }
       
           
        
       

        strCtrl += "</td>";

        strCtrl += "</ul></td>";
        strCtrl += "<td><input type='button' class='btn btn-primary' style='width:80px;height:40px;line-height:20px;' value='删除' onclick='BlockUser(" + (QueryData.length-1) + ");' />"
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

        if (userId != result[i].userId) {
            userId = result[i].userId;
        }
    });


}

function BlockUser(i)
{
    var result = QueryData[i];
    $.confirm({
        theme: "modern",
        title: '注意',
        type: 'red',
        content: "<div style='font-size:22px; color:black;'>你是否确定更要删除成员：<span style='color:blue;font-size:26px;'>" + result.UserName + "</span>?</div>",
        buttons: {
            confirm: {
                btnClass: 'btn btn-danger',
                text: "确定",
                action: function () {
                    DoBlockUser(result.userId);
                }
            },
            cancel: {
                text:"算了吧",
            }

        }
    });
}

function DoBlockUser(ID)
{
    var url = "/PP/DoBlockUser";

    $.ajax({
        type: 'post',
        data:'ID='+ID,
        url: url,
        success: function (data) {
            if(data.IsSuccess)
            {
                alert("删除成功");
                window.location.reload();
            }
            else
            {
                $.alert({
                    theme:'dark',
                    title: '删除失败!',
                    content: '请联系管理员!',
                   
                });
            }

        },
        error: function (xhr, type) {
            alert('Ajax error!');
        }
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
