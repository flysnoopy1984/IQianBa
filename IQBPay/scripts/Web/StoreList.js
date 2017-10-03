
$(document).ready(function () { 
        Query();
    });

function Query() {

    var url = "/Store/Query";
    $.ajax({
        type: 'post',
        data: "pageIndex=0&pageSize=20",
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
        
        strCtrl = "";
        strCtrl += "<tr>";
        strCtrl += "<td>" + result[i].ID + "</td>";
        strCtrl += "<td>" + result[i].Name + "</td>";
        strCtrl += "<td>" + result[i].Remark + "</td>";

        if (result[i].RecordStatus == 0)
            strCtrl += "<td style='color:#4AC4BC;'><div class='noft-green-number'></div>启用</td>";
        else
            strCtrl += "<td><div class='noft-red-number'></div>停用</td>";

        strCtrl += "<td><a href='/Store/Info?id='" + result[i].ID + " class='td'>详情</a></td>";
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
}

function ToInfo(action)
{
    window.location.href = "Info?do="+action;
    return;
}
