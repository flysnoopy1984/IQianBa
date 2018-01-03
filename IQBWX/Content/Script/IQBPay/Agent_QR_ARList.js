var pageIndex = -1;
var url = site + "/PP/Agent_QR_ARListQuery";
var QueryData;
var slider;

$(document).ready(function () {

    Init();
});

function Init()
{
    pageIndex = -1;

    $("#btnNext").attr("disabled", false);
    $("#btnNewQR").hide();

    QueryData = [];

    $("#trContainer").empty();

    Query(pageIndex + 1);

    ToListPage();

    $range = $("#AfterMarketRate").ionRangeSlider({
        min: 8,
        max: 12,
        from: 8,
        step:0.5,
    });
    slider = $range.data("ionRangeSlider");

    //Info Page
    $(".InfoBody").Validform({
        tiptype: 2,
        postonce: false,
        ignoreHidden: true,
        btnSubmit: "#btnSave",
        datatype: {
            "empty": /^\s*$/,
            "My8-12": function (v) {
                if (v >= 8 && v <= 12)
                    return true;
                else
                    return false;
            },
        },
        callback: function (data) {
            Save();
        },
    });
}

function DeleteUserQr(i)
{
    i--;

    var obj = QueryData[i];
    if (obj.IsCurrent)
    {
        $.alert({
            theme: 'dark',
            title: '错误!',
            content: "不能删除当前收款二维码",
        });
        return;
    }
    

    var url = "/PP/Agent_QR_ARDelete";

    $.ajax({
        type: 'post',
        data: "ID=" + obj.ID + "&IsCurrent=" + obj.IsCurrent,
        url: url,
        success: function (data) {
            if (data.IsSuccess == true) {
                $.alert({
                   
                    title: '成功!',
                    content: '删除成功',
                });
                Init();
            }
            else {
                $.alert({
                    theme:'dark',
                    title: '错误!',
                    content: data.ErrorMsg+"。或请联系管理员。",
                });
            }

        },
        error: function (xhr, type) {
            alert('Ajax error!');
            

        }
    });
}


function Save()
{

    var url = "/PP/Agent_QR_ARSave";

    var ID = $("#QrUserId").val();
    var MarketRate = $("#AfterMarketRate").val();
    var IsCurrent = $("#IsCurrent").get(0).checked;
    
    var sucMsg = "新增成功";
    if (ID != "")
        sucMsg = "修改成功";

    $("#btnSave").attr("disabled", true);

    $.ajax({
        type: 'post',
        data: "ID=" + ID + "&MarketRate=" + MarketRate + "&IsCurrent=" + IsCurrent,
        url: url,
        success: function (data) {
            if(data.IsSuccess == true)
            {
                $.alert({
                    title: '成功!',
                    content: sucMsg,
                });
                Init();
            }
            else
            {
                $.alert({
                    theme: 'dark',
                    title: '错误!',
                    content: data.ErrorMsg + ".请联系管理员",
                });
            }
            $("#btnSave").attr("disabled", false);

        },
        error: function (xhr, type) {
            alert('Ajax error!');
            $("#btnSave").attr("disabled", false);

        }
    });

}

function ToListPage() {
    $("#PageList").show();
    $("#PageInfo").hide();
}

function ToInfoPage(i) {
    $("#PageList").hide();
    $("#PageInfo").show();

    i--;
    if (i<0)
    {
        $(QueryData).each(function (i) {
            
            if(QueryData[i].IsCurrent)
            {
                $("#MarketRate").val(QueryData[i].MarketRate);
              
                return false;
            }
        });
        $("#Field_AfterMarketRate").show();

        slider.update({
            from: $("#MarketRate").val(),
        });

        $("#QrUserId").val("");
        $("#IsCurrent").attr("checked", true);
        $("#btnSave").text("创建");
    }
    else
    {
        $("#QrUserId").val(QueryData[i].ID);

        $("#MarketRate").val(QueryData[i].MarketRate);
       
        $("#IsCurrent").attr("checked", QueryData[i].IsCurrent);
      
        $("#QRImg").attr("src", payUrl + QueryData[i].OrigQRFilePath);
        
        $("#Field_AfterMarketRate").hide();

        $("#btnSave").text("更新");
    }

    $("#MarketRate").attr("disabled", true);
  

}

function Query(_pageIndex) {

    var PageSize = 20;

    $.ajax({
        type: 'post',
        data: "Page=" + _pageIndex + "&PageSize=" + PageSize,
        url: url,
        success: function (data) {
            var arrLen = data.length;

            if (arrLen > 0) {

                $("#btnNewQR").show();

                generateData(data);
                pageIndex++;

            }
            else {
                pageIndex--;
                $("#btnNext").attr("disabled", true);
                //alert("没有数据了");

            }

        },
        error: function (xhr, type) {
         
            alert('Ajax error!');
            // 即使加载出错，也得重置

        }
    });
    $("#trContainer").show();
    $("#Process").hide();
}

function generateData(result) {
    var strCtrl = "";


    $.each(result, function (i) {
      
        QueryData.push(result[i]);

        var ParentName = result[i].ParentName;
        if (ParentName == "")
            ParentName = "无"

        strCtrl = "";
        strCtrl += "<tr>";
       
        strCtrl += "<td style='width:47%' ";
        if (result[i].IsCurrent)
        {
            strCtrl += "class='LineSpecial' ";
        }
        strCtrl += "onclick='ToInfoPage(" + QueryData.length + ");'>";
        strCtrl += "<ul><li style='color:brown; font-weight:bold; height:30px;'>用户手续费:" + result[i].MarketRate + "</li>";
        strCtrl += "<li>上级代理:" + ParentName + "</li>";
        strCtrl += "</ul></td>";
        strCtrl += "<td style='width:47%' onclick='ToInfoPage(" + QueryData.length + ");'><ul><li style='height:30px;'>代理返点:" + result[i].Rate + "</li>";
        strCtrl += "<li>上级代理佣金:" + result[i].ParentCommissionRate + "</li>";
        strCtrl += "</ul></td>";
        strCtrl += "<td><input type='button' class='btn-primary' value='调整' onclick='ToInfoPage(" + QueryData.length + ");' />"
        strCtrl += "<input type='button' class='btn-danger' value='删除' onclick='DeleteUserQr(" + QueryData.length + ");' />";
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);
    });


}
