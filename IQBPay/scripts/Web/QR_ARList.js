var pageIndex = -1;

$(document).ready(function () {

    Query(true, pageIndex + 1);
});

function Next() {

    Query(false, pageIndex + 1);
}

function Prev() {

    pageIndex--;
    if (pageIndex < 0) {
        alert("已经第一页了");
        pageIndex = 0;
        return;

    }
    Query(false, pageIndex);
}
function BtnQuery() {
    pageIndex = -1;
    Query(true, pageIndex + 1);
}


function Query(NeedClearn, _PageIndex) {

    if (_PageIndex == 0)
        $("#trContainer").empty();

    var Name = $("#cAgentName").val();
    var UserRole = $("#cUserRole").val();
    var url = "/QR/Query";
    $.ajax({
        type: 'post',
        data: "QRType=3&Name=" + Name + "&UserRole="+UserRole+"&pageIndex=" + _PageIndex,
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
        //var storeName = result[i].StoreName;
        //if (storeName == null)
        //    storeName = "随机";

        var parentName = result[i].ParentName;
        if (parentName == null)
            parentName = "无";

        strCtrl = "";
        strCtrl += "<tr>";
      //  strCtrl += "<td>" + result[i].ID + "</td>";
        strCtrl += "<td>" + result[i].Name + "</td>";
        strCtrl += "<td>" + result[i].Rate + "</td>";
        strCtrl += "<td>" + result[i].ParentCommissionRate + "</td>";
        strCtrl += "<td>" + parentName + "</td>";      
        strCtrl += "<td>" + result[i].MaxInviteCount + "</td>";
        strCtrl += "<td>" + result[i].CurrentInvitedNum + "</td>";
        strCtrl += "<td>" + result[i].Remark + "</td>";

        if (result[i].RecordStatus == 0)
            strCtrl += "<td style='color:#4AC4BC;'><div class='noft-green-number'></div>启用</td>";
        else
            strCtrl += "<td><div class='noft-red-number'></div>停用</td>";

        strCtrl += "<td><a href='/QR/ARInfo?id=" + result[i].ID + "' class='td'>详情</a>";
        strCtrl += " <input type='hidden' value='" + result[i].FilePath + "'</td>";
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
}

function ToInfo(action) {
    window.location.href = "ARInfo?do=" + action;
    return;
}
