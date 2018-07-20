var Channel;
$(document).ready(function () {
    Channel = GetUrlParam("Channel");
    if (Channel == null || Channel == undefined)
        Channel = -1;
    else
    {
        if (Channel == 0)
            $("#ListTitleName").text("平台商户列表");
        else if(Channel == 1)
            $("#ListTitleName").text("加盟商户列表");
    }
        Query();
    });
function CheckStoreAuth()
{
    var url = "/Store/CheckStoreAuth";
    $.ajax({
        type: 'post',
        data: "",
        url: url,
        success: function (data) {
            if(data.IsSuccess)
            {
                alert("检查完毕");
                window.location.reload();
                
            }
        },
        error: function (xhr, type) {

            alert('Ajax error!');

        }
    });
}

function ReSet()
{
    var url = "/Store/ResetAmount";
    $.ajax({
        type: 'post',
        data: "",
        url: url,
        success: function (data) {
            if(data.IsSuccess)
            {
                alert("重置完成");
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

function Query() {

    var url = "/Store/Query";
    var StoreName = $("#cStoreName").val();
    var cRecordStatus = $("#cRecordStatus").val();
    var cStoreType = $("#cStoreType").val();

    $.ajax({
        type: 'post',
        data: "Name=" + StoreName + "&StoreType=" + cStoreType + "&RecordStatus=" + cRecordStatus + "&Channel=" + Channel + "&pageIndex=0&pageSize=80",
        url: url,
        success: function (data) {
            var arrLen = data.length;
            
            $("#trContainer").empty();
            if (arrLen > 0)
            {
                generateData(data);
            }
        },
        error: function (xhr, type) {

            alert('Ajax error!');
           
        }
    });
}

function generateData(result)
{
    var strCtrl = "";
    $.each(result, function (i) {
        
        var cn;
        var channel = result[i].Channel;
        if (channel == 0)
            cn = "平台";
        else if (channel == 1)
            cn = "加盟商";

        var appName = "";
        if (result[i].FromIQBAPP == "2017112100077913")
            appName = "玉杰";
        else if (result[i].FromIQBAPP == "2017122901328258")
            appName = "寒翼";

        var storeType = "未知??";
        if (result[i].StoreType == 1)
            storeType = "小码";
        else if(result[i].StoreType == 2)
            storeType = "信用卡";
        else if (result[i].StoreType == 4)
            storeType = "大额";
        

        strCtrl = "";
       
        if (result[i].RecordStatus == 10)
        {
            strCtrl += "<tr style='background-color:#A5E068'>";
        }
        else
        {
            strCtrl += "<tr>";
        }
        strCtrl += "<td>" + result[i].ID + "</td>";
        strCtrl += "<td title='" + result[i].Provider + "'>" + result[i].Provider + "</td>";
        strCtrl += "<td>" + result[i].Name + "</td>";
        strCtrl += "<td>" + cn + "</td>";
        strCtrl += "<td>" + result[i].Rate + "</td>";
        //strCtrl += "<td>" + result[i].IsReceiveAccount + "</td>";
        strCtrl += "<td>" + result[i].DayIncome + "</td>";
        strCtrl += "<td>" + result[i].RemainAmount + "</td>";     
        strCtrl += "<td>" + result[i].MinLimitAmount + "</td>";
        strCtrl += "<td>" + result[i].MaxLimitAmount + "</td>";
        strCtrl += "<td>" +storeType + "</td>";
        strCtrl += "<td title='" + result[i].AliPayAccount + "'>" + result[i].AliPayAccount + "</td>";
        strCtrl += "<td title='" + result[i].MidCommAccount + "'>" + result[i].MidCommAccount + "</td>";
        strCtrl += "<td title='" + result[i].MidCommRate + "'>" + result[i].MidCommRate + "</td>";
        //strCtrl += "<td title='" + appName + "'>" + appName + "</td>";
    
        strCtrl += "<td title='" + result[i].Remark + "'>" + result[i].Remark + "</td>";

        if (result[i].RecordStatus == 0)
            strCtrl += "<td style='color:#4AC4BC;'>启用</td>";
        else if (result[i].RecordStatus == 10)
            strCtrl += "<td style='color:#3A97E5'>等待审核</td>";
        else
            strCtrl += "<td style='color:#DD4E42;'>停用</td>";

        strCtrl += "<td>";
        strCtrl += "<a href='/Store/Info?id=" + result[i].ID + "' class='td'>详情</a>";
        strCtrl += "<input style='float:right' type=button onclick=deleteStore('" + result[i].ID + "') value='删除' /></td>";
        strCtrl += "</td></tr>";

        $("#trContainer").append(strCtrl);

    });
}

function deleteStore(ID)
{
    if (confirm("商户被删除,是否继续?")) {
        var url = "/Store/DeleteStore";
        $.ajax({
            type: 'post',
            data: "ID=" + ID,
            url: url,
            success: function (data) {

                if (data.IsSuccess) {
                    window.location.reload();
                    alert("删除完成");
                }
                else
                    alert(data.ErrorMsg);
            },
            error: function (xhr, type) {

                alert('Ajax error!');

            }
        });
    }
    else
        alert("取消删除");
}

// 根据channel 去不同的页面
function ToInfo(action,channel)
{
    window.location.href = "/QR/AuthInfo?do=" + action+"&c="+channel;
    
    return;
}
