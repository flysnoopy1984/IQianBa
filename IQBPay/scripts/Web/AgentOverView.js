var pageIndex = -1;

function btnSearch() {
    pageIndex = -1;
    Query(true, pageIndex + 1);
}

function Next() {
    Query(false, pageIndex + 1);
}


function Query() {
    pageIndex = -1;
    Query(true, pageIndex + 1);
}

function Query(NeedClearn, _PageIndex) {
    var url = "/Report/AgentOverViewQuery";
    var AgentName = $("#cAgentName").val();
    var ParentName = $("#cParentName").val();
    var BeforeDay = $("#cBeforeDay").val();

    ShowProcess();

    if (NeedClearn) {
        $("#trContainer").empty();
    }
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "AgentName": AgentName, "ParentName": ParentName, "BeforeDay": BeforeDay, "pageIndex": pageIndex+1 ,"pageSize":60},
        url: url,
        success: function (data) {
            var arrLen = data.length;

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

function generateData(result)
{
    var strCtrl = "";
    $.each(result, function (i) {
        var thWidth;

        strCtrl = "";
        strCtrl += "<tr>";
       //ID
        tdWidth = "width:" + $("#trHeader th").eq(0).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].Id + "</td>";

        ////OpneId
        //tdWidth = "width:" + $("#trHeader th").eq(1).css("width");
        //strCtrl += "<td style='" + tdWidth + "'>" + result[i].Id + "</td>";

        //AgentName
        tdWidth = "width:" + $("#trHeader th").eq(1).css("width");
        strCtrl += "<td style='" + tdWidth + "'><a href='/Order/List'>" + result[i].AgentName + "</a></td>";

        ////ParentOpenId
        //tdWidth = "width:" + $("#trHeader th").eq(0).css("width");
        //strCtrl += "<td style='" + tdWidth + "'>" + result[i].Id + "</td>";

        //ParentName
        tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].ParentName + "</td>";

        //RegisterDate
        tdWidth = "width:" + $("#trHeader th").eq(3).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].RegisterDate + "</td>";

        //OrderComplatedNum
        tdWidth = "width:" + $("#trHeader th").eq(4).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].OrderComplatedNum + "</td>";

        //OrderTotalAmount
        tdWidth = "width:" + $("#trHeader th").eq(5).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].OrderTotalAmount + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(6).css("width");
        strCtrl += "<td style='" + tdWidth + "'>";
        strCtrl += "<input type='button' value='禁用' onclick='DisableUser('" + result[i].OpenId + "')' />";
        strCtrl += "</td>";

        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);
    });
}

function DisableUser(openId)
{
    var url = "/User/DisableUser";
    $.ajax({
        type: 'post',
        data:"OpenId="+openId,
        url: url,
        success: function (data) {
            if (data.IsSuccess)
            {
                alert("成功");
                window.location.reload();
            }
            else
            {
                alert(data.ErrorMsg);

            }
        },
        error: function (xhr, type) {

            alert('Ajax error!');
        
        

        }
    });
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