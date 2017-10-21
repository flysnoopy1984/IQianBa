var pageIndex = -1;

$(document).ready(function () {

    //Query(true,pageIndex+1);
    InitCondition();
});

function InitCondition() {
    $.ajax({
        type: 'post',
        url: '/Store/QueryKeyValue',
        success: function (data) {
            var arrLen = data.length;
            $("#cStore").append("<option value=-1>全部</option>");
            if (arrLen > 0) {
                $(data).each(function (i) {

                    $("#cStore").append("<option value='" + data[i].Id + "'>" + data[i].Name + "</option>");
                });
            }

        },
        error: function (xhr, type) {
            alert('Ajax error!');
        }
    });
}

function Query(NeedClearn, _PageIndex) {
    var url = "/Store/QuerySum"
    var cStore = $.trim($("#cStore").val());
    var cDateType = $.trim($("#cDateType").val());
    $.ajax({
        type: 'post',
        data: "DataType=" + cDateType + "&StoreId=" + cStore + "&pageIndex=" + _PageIndex,
        url: url,
        success: function (data) {
            var arrLen = data.length;
            if (NeedClearn) {
                $("#AgentList li").not(".liHeader").remove();
            }

            if (arrLen > 0) {
                generateData(data);

                pageIndex++;
            }
            else {
                pageIndex--;
                alert("没有数据了");
            }
            CloseProcess();
        },
        error: function (xhr, type) {

            alert('Ajax error!');
            CloseProcess();

        }
    });
}

function generateData(result) {
    var liWidth;
  
    $.each(result, function (i) {
        strCtrl = "";
        strCtrl += "<li class='liBody'>";

        liWidth = "width:" + $(".liHeader div").eq(0).css("width");
        strCtrl += "<div class='liBodyDivLeft' style='" + liWidth + "'>" + result[i].StoreName + "</div>";

        liWidth = "width:" + $(".liHeader div").eq(1).css("width");
        strCtrl += "<div class='liBodyDivLeft' style='" + liWidth + "'>&yen " + result[i].DayIncome + "</div>";

        liWidth = "width:" + $(".liHeader div").eq(2).css("width");
        strCtrl += "<div class='liBodyDivLeft' style='" + liWidth + "'>&yen " + result[i].TotalAmount + "</div>";

        liWidth = "width:" + $(".liHeader div").eq(3).css("width");
        strCtrl += "<div class='liBodyDivLeft' style='" + liWidth + "'>&yen " + result[i].Rate + "</div>";

        liWidth = "width:" + $(".liHeader div").eq(4).css("width");
        strCtrl += "<div class='liBodyDivRight' style='" + liWidth + "'>";
        strCtrl += "<a href='#' target='_blank' class='td'>详情</a>";
        strCtrl += "</div>";
        strCtrl += "</li>";

        $("#AgentList").append(strCtrl);
    });
}

function btnSearch() {
    pageIndex = -1;
    Query(true, pageIndex + 1);
}

function Next() {
    Query(false, pageIndex + 1);
}

function ShowProcess() {
    //$("#btnSearch").attr("disabled", true);
    //$("#divTableBody").hide();
    //$("#divProcess").show();

}
function CloseProcess() {
    //$("#btnSearch").attr("disabled", false);
    //$("#divTableBody").show();
    //$("#divProcess").hide();
}

function ShowOrderDetail() {

}