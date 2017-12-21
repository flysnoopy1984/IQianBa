var pageIndex = -1;
var AgentOpenId = GetUrlParam("AgentOpenId");

$(document).ready(function () {
    if (AgentOpenId == null || AgentOpenId == "" || AgentOpenId == undefined)
    {
        alert("未获取参数");
        window.close();
    }
    Query(true, pageIndex + 1);
});


function InitCondition() {

}


function Next() {
    Query(false, pageIndex + 1);
}

function ShowProcess() {
 
    $("#divTableBody").hide();
    $("#divProcess").show();
}
function CloseProcess() {
  
    $("#divTableBody").show();
    $("#divProcess").hide();
}

function Query(NeedClearn, _PageIndex) {

    var url = "/Order/QueryForDoTransferOrder";
  
    ShowProcess();
    $.ajax({
        type: 'post',
        data: "AgentOpenId=" + AgentOpenId + "&PageIndex=" + _PageIndex,
        url: url,
        success: function (data) {
            var arrLen = data.length;
            if (NeedClearn) {
                $("#trContainer").empty();
            }

            if (arrLen > 0) {
                generateData(data);
               
                CloseProcess();
                SetWidth();
                pageIndex++;
            }
            else {
                pageIndex--;
                alert("没有数据了");
                CloseProcess();
            }
           
        },
        error: function (xhr, type) {
           
            alert('Ajax error!');
            CloseProcess();

        }
    });
}

function generateData(result) {

    var strCtrl = "";
    $.each(result, function (i) {
        var thWidth;
     
        var TransDate = result[i].TransDateStr;

        strCtrl = "";
        strCtrl += "<tr>";
        tdWidth = "width:" + $("#trHeader th").eq(0).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TransDateStr + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(1).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].OrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].TotalAmount + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(3).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].RealTotalAmount + "</td>";

        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
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

function ToInfo(action) {
    window.location.href = "OrderInfo?do=" + action;
    return;
}