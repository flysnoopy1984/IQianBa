var pageIndex = -1;
var totalAmt = 0;
$(document).ready(function () {

});

function InitCondition() {

}

function btnSearch() {
    pageIndex = -1;
    Query(true, pageIndex + 1);
}

function Next() {
    Query(false, pageIndex + 1);
}

function ShowProcess() {
    $("#btnSearch").attr("disabled", true);
    $("#divTableBody").hide();
    $("#divProcess").show();
}

function CloseProcess() {
    $("#btnSearch").attr("disabled", false);
    $("#divTableBody").show();
    $("#divProcess").hide();
}

function Query(NeedClearn, _PageIndex) {

    var url = "/Transfer/Query";
 
    var AgentOpenId = $("#OpenId").val();
    var DataType = $("#cDateType").val();
    ShowProcess();

    $.ajax({
        type: 'post',
        data: "DataType=" + DataType + "&AgentOpenId=" + AgentOpenId + "&PageIndex=" + _PageIndex,
        url: url,
        success: function (data) {
            var arrLen = data.length;
            if (NeedClearn) {
                $("#trContainer").empty();
                totalAmt = 0;
            }

            if (arrLen > 0) {
                generateData(data);
                CloseProcess();//必须在计算宽度时关闭进度显示，不然将影响表格的呈现
                SetWidth();
                pageIndex++;
                $("#btnNext").show();
            }
            else {
                pageIndex--;
                alert("没有数据了");
                CloseProcess();
                $("#btnNext").hide();

            }
        },
        error: function (xhr, type) {

            alert('Ajax error!');
            CloseProcess();
            $("#btnNext").hide();

        }
    });
}

function generateData(result) {

    var strCtrl = "";

    $.each(result, function (i) {
        var thWidth;
        var TransDate = result[i].TransDateStr;

        var transferStatus = "";
        switch (result[i].TransferStatus) {
            case 0:
                transferStatus = "未汇款";
                break;
            case 1:
                transferStatus = "汇款成功";
                break;
            case -1:
                transferStatus = "汇款失败";
                break;
        }

        strCtrl = "";
        strCtrl += "<tr>";

        strCtrl += "<td><a href=/Order/Info_Win?id=" + result[i].TransferId + "&type=2  target='_blank' class='td'>订单信息</a>";
        strCtrl += " <input type='hidden' value='" + result[i].FilePath + "'</td>";
        //汇款编号
        tdWidth = "width:" + $("#trHeader th").eq(1).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TransferId + "</td>";

        //汇款状态
        tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        if (result[i].TransferStatus == -1)
        {
            strCtrl += "<td style='" + tdWidth + "'><a href='javascript:ShowError(" + i + ")'>" + transferStatus + "</a>";
            strCtrl += "<div class='DivHovering' id=divError" + i + ">" + result[i].Log + "</div>";
            strCtrl += "</td>";
        }
        else
        {
            strCtrl += "<td style='" + tdWidth + "'>" + transferStatus + "</td>";
        }
      
       
      //  totalAmt += parseFloat(result[i].TransferAmount);

        //汇款金额
        tdWidth = "width:" + $("#trHeader th").eq(3).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TransferAmount + "</td>";
        totalAmt += parseFloat(result[i].TransferAmount);

        //汇款时间
        tdWidth = "width:" + $("#trHeader th").eq(4).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + TransDate + "</td>";

        //订单
        tdWidth = "width:" + $("#trHeader th").eq(5).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].OrderNo + "</td>";

        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);
    });

  //  $("#RecordSum").text("【汇款总金额】：" + totalAmt.toFixed(2));
}

function ShowError(no) {
    var obj = $("#divError" + no);
    if (obj.is(":hidden")) {
        obj.css("position", "relative");
        obj.show();
    }
    else {
        obj.css("position", "absolute");
        obj.hide();
    }
}

