var pageIndex = -1;
var url = site + "/PP/Agent_QR_ARListQuery";
var QueryData;
var slider;
var ReqHuge

$(document).ready(function () {

    Init();

   
});


function Init()
{
   
    pageIndex = -1;

    $("#btnNext").attr("disabled", false);
    $("#btnNewQR").hide();
    QueryData = [];

    //从大额入口直接进入
    ReqHuge = getUrlParam("ReqHuge");
    if (ReqHuge == 1)
    {
        InitFromQRHuge();

    }
    else
    {
       
        $("#trContainer").empty();
        Query(pageIndex + 1);
        ToListPage();
    }

   

    //Info Page
    $(".InfoBody").Validform({
        tiptype: 2,
        postonce: false,
        ignoreHidden: true,
        btnSubmit: "#btnSave",
        datatype: {
            "empty": /^\s*$/,
            "My6-14": function (v) {
                if (v >= 6 && v <= 14)
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

function InitFromQRHuge()
{
    var url = "/PP/GetQRUser";
    $.ajax({
        type: 'post',
        data: "QRType=4",
        url: url,
        success: function (data) {
           
            if(data)
            {
                QueryData.push(data);
                ToInfoPage(1, 1);
            }

        },
        error: function (xhr, type) {
            alert('Ajax error!');
           

        }
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
    var AfterMarketRate = $("#AfterMarketRate").val();
    var MarketRate = parseFloat($("#MarketRate").val()).toFixed(2);
   // var IsCurrent = $("#IsCurrent").get(0).checked;
    var Rate = parseFloat($("#Rate").val()).toFixed(2);
    var diff = MarketRate - Rate;
    Rate = (AfterMarketRate - diff).toFixed(2);
    //var realRate = 10-diff;

   
    //if (AfterMarketRate < realRate)
    //{
    //    $.alert({
    //        theme: 'dark',
    //        title: '错误!',
    //        content: "您这样设置，费率将小于0，请重新调整！",
    //    });
    //    return;
    //}
    var sucMsg = "新增成功";
    if (ID != "")
        sucMsg = "修改成功";

    $("#btnSave").attr("disabled", true);

    $.ajax({
        type: 'post',
        data: "ID=" + ID + "&MarketRate=" + AfterMarketRate + "&Rate=" + Rate,
        url: url,
        success: function (data) {
            if(data.IsSuccess == true)
            {
                if (ReqHuge == 1)
                {
                    alert("成功，将返回大额页面");
                    window.location.href = "/PP/QRHugeEntry";
                }                   
                else
                {
                    $.alert({
                        title: '成功!',
                        content: sucMsg,
                    });
                    Init();
                }
              
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
    if (ReqHuge == 1) {
        window.location.href = "/PP/QRHugeEntry";
    }
    else {
        $("#PageList").show();
        $("#PageInfo").hide();
    }

}

//qrType =0 小 1 大
function ToInfoPage(i,qrType) {

    $("#PageList").hide();
    $("#PageInfo").show();

    if (qrType == 0)
    {
        $("#Title").text("【普通码设置】");
        $("#QRImgContainer").show();
    }
    else
        $("#Title").text("【大额码设置】");
    $("#QRImgContainer").hide();

    i--;
    //新增
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
    //更新
    else
    {
        
        var diff = (QueryData[i].MarketRate - QueryData[i].Rate).toFixed(2);
        if (qrType == 0)
        {
            $range = $("#AfterMarketRate").ionRangeSlider({
                min: 6,
                max: 14,
                from: QueryData[i].MarketRate,
                step: 0.5,
                //onChange: function (data) {
                //    $("#MarketRate").val(data.from);
                //},
            });
            $("#QRImg").attr("src", payUrl + QueryData[i].OrigQRFilePath);
        }
        else
        {
            $range = $("#AfterMarketRate").ionRangeSlider({
                min: 6,
                max: 20,
                from: QueryData[i].MarketRate,
                step: 0.5,
                //onChange: function (data) {
                //    $("#MarketRate").val(data.from);
                //},
            });
        }
      
        slider = $range.data("ionRangeSlider");

        $("#QrUserId").val(QueryData[i].ID);

        $("#MarketRate").val(QueryData[i].MarketRate);
       
    //    $("#IsCurrent").attr("checked", QueryData[i].IsCurrent);


        $("#Rate").val(QueryData[i].Rate);
     //   $("#Field_AfterMarketRate").hide();

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
      
      
        var qrType = 1;
        QueryData.push(result[i]);

        var ParentName = result[i].ParentName;
       
        if (ParentName == "")
            ParentName = "无"

        strCtrl = "";
        

        if (i == 0) {
            strCtrl += "<tr><td colspan='3' style='text-align:center'>普通码配置</td></tr>";
            qrType = 0;
        }
        if (i == 1) {
            strCtrl += "<tr><td colspan='3' style='text-align:center'>大额码配置</td></tr>";
            qrType = 1;
        }

        strCtrl += "<tr>";
        if (i == 0)
            strCtrl += "<td style='width:10%' class='td_bgIcon_sQR'></td>";
        else
            strCtrl += "<td style='width:10%' class='td_bgIcon_bQR'></td>";
        strCtrl += "<td style='width:45%' ";     
        strCtrl += "onclick='ToInfoPage(" + QueryData.length + ");'>";
        strCtrl += "<ul><li style='color:brown; font-weight:bold; height:30px;'>用户手续费:" + result[i].MarketRate + "</li>";
        //strCtrl += "<li>上级代理:" + ParentName + "</li>";
        strCtrl += "</ul></td>";
        strCtrl += "<td style='width:45%' onclick='ToInfoPage(" + QueryData.length + ");'><ul><li style='height:30px;'>代理成本:" + (result[i].MarketRate - result[i].Rate).toFixed(2) + "</li>";
        //strCtrl += "<li>上级代理佣金:" + result[i].ParentCommissionRate + "</li>";
        strCtrl += "</ul></td>";
        if (i == 0 && result.length==2)
            strCtrl += "<td style='border-bottom: 1px solid #ddd;'><input type='button' class='btn-primary' value='调整' onclick='ToInfoPage(" + QueryData.length + "," + qrType + ");' /></td>";
        else
            strCtrl += "<td><input type='button' class='btn-primary' value='调整' onclick='ToInfoPage(" + QueryData.length + "," + qrType + ");' /></td>";
        //strCtrl += "<input type='button' class='btn-danger' value='删除' onclick='DeleteUserQr(" + QueryData.length + ");' />";
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);
    });


}
