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
function BtnQuery()
{
    pageIndex = -1;
    Query(true, pageIndex + 1);
}

function Query(NeedClearn,_PageIndex) {

    if (_PageIndex == 0)
        $("#trContainer").empty();

    var cAgentName = $("#cAgentName").val();
    var cParentName = $("#cParentName").val();

    var url = "/User/Query";
    $.ajax({
        type: 'post',
        data: "role=2&AgentName=" + cAgentName + "&ParentName="+cParentName+"&pageIndex=" + _PageIndex,
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
        strCtrl += "<td title='" + result[i].Name + "'>" + result[i].Name + "</td>";
        strCtrl += "<td>" + result[i].Rate + "</td>";
        strCtrl += "<td>" + result[i].ParentCommissionRate + "</td>";
        strCtrl += "<td>" + result[i].MarketRate + "</td>";
        strCtrl += "<td title='" + result[i].ParentAgent + "'>" + result[i].ParentAgent + "</td>";
        strCtrl += "<td>" + result[i].CDate + "</td>";
        strCtrl += "<td title='" + result[i].AliPayAccount + "'>" + result[i].AliPayAccount + "</td>";
        strCtrl += "<td title='"+result[i].OpenId+"'>" + result[i].OpenId + "</td>";
       
       
        if (result[i].StoreName == "" || result[i].StoreName == null)
        {
            result[i].StoreName = "随机";
        }
       
        strCtrl += "<td title='" + result[i].StoreName + "'>" + result[i].StoreName + "</td>";

        if (result[i].UserStatus == 1)
            strCtrl += "<td style='color:#4AC4BC;'><div class='noft-green-number'></div>启用</td>";
        else
            strCtrl += "<td><div class='noft-red-number'></div>禁用</td>";

        strCtrl += "<td><a href='/User/Info?OpenId=" + result[i].OpenId + "' class='td'>详情</a></td>";
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
}
