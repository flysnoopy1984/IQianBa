var pageIndex = 0;
var pageSize = 10;
var IQBScroll = null;

var QueryData;

$(document).ready(function () {

    $("#btnNext").attr("disabled", false);
    
    //用于重新加载，保持上次选择的下拉框
    var selAppId = getUrlParam("appId");
  //  alert(selAppId);
    ToListPage();

    APPQuery(selAppId);

  
    //Info Page
    $(".InfoBody").Validform({
        tiptype: 2,
        postonce: false,
        ignoreHidden: true,
        btnSubmit: "#btnSave",
        datatype: {
            "empty": /^\s*$/,
        },
        callback: function (data) {
            Save();
        },
    });

    QueryData = [];
});

function ShowBlock() {

    var msg = ' <div id="ProcessArea1" class="progress progress-striped active">';
    msg += '<div id="upload_progress1" class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 100%; height:40px;">';
    msg += '<span class="sr-only">数据加载中...</span>';
    msg += '</div>';
    msg += '</div>';

    $.blockUI({
        message: msg,
        css: {
            border: 'none',
            width: '90%',
            height: '40px',
            left: '20px',
            'border-radius': '20px',
        }
    });
}

function ToListPage()
{
    $("#PageList").show();
    $("#PageInfo").hide();
}

function ToInfoPage(i) {
    $("#PageList").hide();
    $("#PageInfo").show();
  
   

    //-1代表新增
    if(i == -1)
    {
        $("#Name").val("");
        $("#MinLimitAmount").val(0);
        $("#MaxLimitAmount").val(1000);
        $("#DayIncome").val(10000);
        $("#Status").text("");
        $("#r_cc").prop("checked", true);
        $("#r_sqr").prop("checked", false);
    }
    else
    {
        i--;
        var status = GetStatus(QueryData[i]);

        $("#Name").val(QueryData[i].Name);
        $("#MinLimitAmount").val(QueryData[i].MinLimitAmount);
        $("#MaxLimitAmount").val(QueryData[i].MaxLimitAmount);
        $("#DayIncome").val(QueryData[i].DayIncome);
        $("#Status").text(status);

        $("#storeId").val(QueryData[i].ID);

        var st = QueryData[i].StoreType;
       // alert($(".StoreTypeArea input[name='sType']").attr("checked"));
       // alert($("#r_sqr").attr("checked"));

        if(st == 2)
        {
            $("#r_cc").prop("checked", true);
            $("#r_sqr").prop("checked", false);
        }
        else if(st == 1)
        {
            $("#r_sqr").prop("checked", true);
            $("#r_cc").prop("checked", false);
        }
    }
   
}

function APPQuery(selAppId)
{
    var url = "/PP/APPList";
    $.ajax({
        type: 'post',
        data: "",
        url: url,
        success: function (data) {

            if (data.IsSuccess) {
                $("#AppList").empty();
                $.each(data.resultList, function (i) {
                    if (data.resultList[i].IsCurrent)
                    {
                        SelAppId = data.resultList[i].AppId;
                        $("#AppList").append("<option selected value='" + data.resultList[i].AppId + "'>" + data.resultList[i].AppName + "</option>");
                    }
                    else
                        $("#AppList").append("<option value='" + data.resultList[i].AppId + "'>" + data.resultList[i].AppName + "</option>")
                    
                });
                if (selAppId != null && selAppId != "")
                    $("#AppList").val(selAppId);
                Query();

            }
            else
                alert(data.ErrorMsg);

        },
        error: function (xhr, type) {

            alert("系统错误！");

            me.resetLoad();

        }
    });
}



function Query() {

    var PageSize = 30;
    $("#trContainer").empty();
    pageIndex = 0;
    QueryData = [];

    var SelAppId = $("#AppList").val();

    IQBScroll = $("#DataList").ScrollLoad({

        loadData: function (me) {
            var url = site + "/PP/StoreQuery";
            var openId = $("#hOpenId").val();
            $.ajax({
                type: 'post',
                data: "Page=" + pageIndex + "&PageSize=" + pageSize + "&OpenId=" + openId + "&SelAppId=" + SelAppId,
                url: url,
                success: function (data) {
                  
                    if (data.IsSuccess) {
                        var list = data.resultList;
                        if (list.length > 0)
                        {
                            generateData(list);
                            pageIndex++;
                        }
                        else
                            me.noData();
                       
                    }
                    else {
                        alert(data.ErrorMsg);
                    }

                    me.resetLoad();

                },
                error: function (xhr, type) {

                    alert("系统错误！");

                    me.resetLoad();

                }
            });

        }
    });
 
}

function GetStatus(obj)
{
    var status = "";
    if (obj.RecordStatus == 0)
        status = "已上架";
    else if (obj.RecordStatus == 1)
        status = "已下架";
    else if (obj.RecordStatus == 10)
        status = "待审核";
    else if (obj.RecordStatus == -1)
        status = "创建";

    return status;
}

function generateData(result) {
    var strCtrl = "";


    $.each(result, function (i) {
     
        QueryData.push(result[i]);
       

        var IsAuth;
        if (result[i].IsAuth)
            IsAuth = "已授权";
        else
            IsAuth = "未授权";
        var status = GetStatus(result[i]);
       
        strCtrl = "";
        strCtrl += "<tr>";

        strCtrl += '<td style="width:90%">';
        strCtrl += '<div class="StoreName" onclick="ToInfoPage(' + QueryData.length + ');">店铺名：' + result[i].Name + '</div>';
        strCtrl += '<div class="rowCon">';
        strCtrl += '<div onclick="ToInfoPage(' + QueryData.length + ');">';
        strCtrl += '<div>最小额度：' + result[i].MinLimitAmount + '</div>';
        strCtrl += '<div>最大额度：' + result[i].MaxLimitAmount + '</div>';
        strCtrl += '<div>每日额度：' + result[i].DayIncome + '</div>';
        strCtrl += '<div>剩余额度：' + result[i].RemainAmount + '</div>';
        strCtrl += '<div>创建日期：' + result[i].CDate + '</div>';
        strCtrl += '<div class="StoreStatus">状态：' + status + '</div>';
        strCtrl += '</div>';
        strCtrl += '<div class="rowBtnList">';
        if (result[i].RecordStatus != 10)
        {
            if (result[i].IsAuth) {
                if (result[i].RecordStatus == 0)
                    strCtrl += '<input type="button" id="btnOffLine" class="btn btn-danger btn_small" value="下架" sId="' + result[i].ID + '" />';
                else if (result[i].RecordStatus == 1)
                    strCtrl += '<input type="button" id="btnOnLine" class="btn btn-success btn_small" value="上架" sId="' + result[i].ID + '" />';
                
            }
            else {
                strCtrl += '<input type="button" class="btn btn-info btn_small" value="编辑" onclick="ToInfoPage(' + QueryData.length + ');" />';
                strCtrl += '<input type="button" id="btnAuthStore" class="btn btn-warning btn_small" value="授权" sId="' + result[i].ID + '" />';
            }
        }
       
        strCtrl += '</div>';
        strCtrl += '</td>';
        strCtrl += "<td class='GoToInfoButton' onclick='ToInfoPage(" + QueryData.length + ");'>></td>";
        strCtrl += "</tr>";



        $("#trContainer").append(strCtrl);
    });


    $(document).on("click", "#btnOnLine", OnlineStore);
    $(document).on("click", "#btnOffLine", OfflineStore);
    $(document).on("click", "#btnAuthStore", AuthStore);

}

function Save()
{
    var Name = $("#Name").val();
    var MinLimitAmount = $("#MinLimitAmount").val();
    var MaxLimitAmount =  $("#MaxLimitAmount").val();
    var DayIncome = $("#DayIncome").val();
    var ID = $("#storeId").val();
    var openId = $("#hOpenId").val();
    var StoreType = $(".StoreTypeArea input[name='sType']:checked").val();
    var AppId = $("#AppList").val();
 
    var url = "/api/UserStore/Save";
    ShowBlock();
    $.ajax({
        type: 'post',
        data: {
            "ID": ID,
            "OwnnerOpenId": openId,
            "MinLimitAmount": MinLimitAmount, "MaxLimitAmount": MaxLimitAmount,
            "DayIncome": DayIncome, "Name": Name,
            "StoreType": StoreType,
            "FromIQBAPP": AppId
        },
        url: url,
        success: function (data) {
            if(data.IsSuccess)
            {
                alert("保存成功！");
                window.location.href = "/PP/StoreList?appId=" + AppId;
               // window.location.reload();
            }
            else
            {
                alert("错误：" + data.ErrorMsg);
            }
            $.unblockUI();
        },
        error: function (xhr, type) {
       
            $.unblockUI();
            toErrorPage("系统错误！", "/PP/StoreList");
           
        }
    }); 
}

function Delete() {
    var ID = $("#storeId").val();
    var openId = $("#hOpenId").val();

    $.confirm({
        theme: 'material',
        title: '删除确认!',
        content: '您将删除此商户，是否继续',
        buttons: {
            cancel: {
                text: '还是算了',
                btnClass: 'btn-info',

            },
            confirm: {
                btnClass: 'btn-danger',
                text: '继续删除',
                action: function () {
                    var url = "/api/UserStore/Delete?StoreId="+ID;
                    $.ajax({
                        type: 'post',
                        url: url,
                        //data: { "StoreId": ID },
                        success: function (res) {
                            if (res.IsSuccess) {
                                alert("删除成功;");
                                window.location.href = "/PP/StoreList?appId=" + AppId;
                               
                                
                            }
                            else {
                                alert(res.ErrorMsg);
                            }
                        },
                        error: function (xhr, type) {
                            alert("系统错误！");
                        }
                    });

                }
            }
        }
    });
}

function OnlineStore(e)
{
    var Id = $(e.currentTarget).attr("sId");
    var AppId = $("#AppList").val();

    var url = "/api/UserStore/OnlineStore?StoreId=" + Id;
    $.ajax({
        type: 'post',
        url: url,
        success: function (res) {
            if (res.IsSuccess) {
                alert("已上线;");
                window.location.href = "/PP/StoreList?appId=" + AppId;
               // window.location.reload();
               
            }
            else {
                alert(res.ErrorMsg);
            }
        },
        error: function (xhr, type) {
            alert("系统错误！");
        }
    });

    return false;
}

function OfflineStore(e)
{
    var Id = $(e.currentTarget).attr("sId");
    var AppId = $("#AppList").val();

    var url = "/api/UserStore/OfflineStore?StoreId=" + Id;
    $.ajax({
        type: 'post',
        url: url,
        success: function (res) {
            if (res.IsSuccess) {
                alert("已下线;");

                window.location.reload();

            }
            else {
                alert(res.ErrorMsg);
            }
        },
        error: function (xhr, type) {
            alert("系统错误！");
        }
    });

    return false;
}

function AuthStore(e)
{
    var Id = $(e.currentTarget).attr("sId");
    window.location.href = "/PP/StoreAuth?Id=" + Id;
    return false;
}

