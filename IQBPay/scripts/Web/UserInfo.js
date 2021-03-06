﻿var gStoreList;
var gParentAgentList;
var OpenId;
$(document).ready(function () {

     OpenId = GetUrlParam("OpenId");

    if (OpenId == null || OpenId == "" || OpenId == "undefined") {
        alert("无法识别OpenId");
        window.location.href = "list";
        return;
    }

    $('#StoreRate').attr("disabled", true);

   // $('#Name').attr("disabled", true);
    //$('#ParentCommissionRate').attr("disabled", true);
    Init(OpenId);

});

function AjaxInviteCode(Id)
{
    var url = "/QR/Get";
    $.ajax({
        type: 'post',
        data: "Id=" + Id + "&qrType=3",
        url: url,
        success: function (data) {

            $("#Invite_Rate").val(data.Rate);
          
            $("#Invite_ParentCommissionRate").val(data.ParentCommissionRate);

            $("#Invite_MaxInviteCount").val(data.MaxInviteCount);

            $("#Invite_ParentOpenId").empty();
            $("#Invite_ParentOpenId").append("<option value=''>无</option>");

            $("#GoQRDetail").attr("href", "/QR/ARInfo?id="+Id);

            $(data.HashUserList).each(function (i, r) {
                if (data.ParentOpenId == r.OpenId)
                    $("#Invite_ParentOpenId").append("<option value='" + r.OpenId + "' selected>" + r.Name + "</option>");
                else
                    $("#Invite_ParentOpenId").append("<option value='" + r.OpenId + "'>" + r.Name + "</option>");
            });

        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });
}

function Init(OpenId) {
   
    $("#OpenId").val(OpenId);

    $('#selStore').on('change', function () {
        var selVal = $('#selStore').val();
        if (selVal == "-1")
        {
            $('#StoreRate').val("");
        }
        else
        {
            $(gStoreList).each(function (i, r) {
                if(r.Id == selVal)
                {
                    $('#StoreRate').val(r.Rate);
                }
            });
        }
    });

    var url = "/User/Get";
    $.ajax({
        type: 'post',
        data: "OpenId=" + OpenId,
        url: url,
        success: function (data) {
            gStoreList = data.StoreList;
            gParentAgentList = data.ParentAgentList;
            InitFormData(data);

            var QRHuge = data.QRHuge;
            $("#QRHuge_Rate").val(QRHuge.Rate);
            $("#QRHuge_MarketRate").val(QRHuge.MarketRate);

            var QRCC = data.QRCC;
            $("#QRCC_Rate").val(QRCC.Rate);
            $("#QRCC_MarketRate").val(QRCC.MarketRate);

        //    AjaxInviteCode(data.QRInviteCode);

            AjaxO2OQR();

        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });
}

function CloseQRHuge()
{
    var url = "/User/CloseQRHuge";
    var OpenId = $("#OpenId").val();
}

function CreateOrUpdateQRHuge()
{
    var url = "/User/CreateOrUpdateQRHuge";
    var OpenId = $("#OpenId").val();

    var QRHuge_Rate = $("#QRHuge_Rate").val();
    var QRHuge_MarketRate = $("#QRHuge_MarketRate").val();
    if (QRHuge_Rate == 0 || QRHuge_MarketRate == 0)
    {
        alert("值不能为空或0");
        return;
    }
    $.ajax({
        type: 'post',
        dataType: "json",
        //  data: "OpenId=" + OpenId + "&Rate=" + QRHuge_Rate + "&QRHuge_MarketRate=" + QRHuge_MarketRate,
        data:{"OpenId":OpenId,"Rate":QRHuge_Rate,"marketRate":QRHuge_MarketRate},
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
                alert(data.SuccessMsg);  
            }
            else {
                alert(data.ErrorMsg);
            }
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });

}

function InitFormData(data) {
 
    $("#QrUserId").val(data.QRUserId);
    $("#Name").val(data.Name);
    $("#Rate").val(data.Rate);

    //$("#ParentAgent").val(data.ParentAgent);
    $("#ParentCommissionRate").val(data.ParentCommissionRate);

    $("#NeedFollowUp").attr("checked", data.NeedFollowUp);

    //$("#StoreId").val(data.StoreId);
    $("#RegisterDate").val(data.RegisterDate);
    //$("#StoreName").val(data.StoreName);

    $("#selStore").empty();
    $("#selStore").append("<option value='-1'>随机</option>");
    $(data.StoreList).each(function (i, r) {
        if (data.StoreId == r.Id)
            $("#selStore").append("<option value='" + r.Id + "' selected>" + r.Name + "</option>");
        else
            $("#selStore").append("<option value='" + r.Id + "'>" + r.Name + "</option>");
    });


    $("#selParentAgent").empty();
    $("#selParentAgent").append("<option value=''>无</option>");
    $(data.ParentAgentList).each(function (i, r) {
        if (data.ParentAgentOpenId == r.OpenId)
            $("#selParentAgent").append("<option value='" + r.OpenId + "' selected>" + r.Name + "</option>");
        else
            $("#selParentAgent").append("<option value='" + r.OpenId + "'>" + r.Name + "</option>");
    });

    $("#AliPayAccount").val(data.AliPayAccount);
    $("#UserStatus").val(data.UserStatus);

    $("#selUserRole").val(data.UserRole);

    $("#QRShow").attr("src", data.OrigQRFilePath);

    $("#MarketRate").val(data.MarketRate);
    //邀请码 - Begin
    $("#Invite_QRCode").val(data.QRInviteCode);  
    //邀请码 - End

    //码商
    $("#UserStore_Rate").val(data.UserStoreRate);
    $("#UserStore_FixComm").val(data.UserStoreFixComm);
    $("#UserStore_OwnRate").val(data.UserStoreOwnerRate);

    var st;
    if (data.UserStatus == 0)
        st = false;
    else
        st = true;
    
    $("#UserStatus").bootstrapSwitch({
        onText: "启用",
        state: st,
        offText: "禁用",
        onColor: "success",
        offColor: "danger",
        size: "small",
        onSwitchChange: function (event, state) {
            if (state == true) {
                $(this).val("1");
            } else {
                $(this).val("0");
            }
        }
    });
}

function CheckForm() {
    $("#EditArea").find("input[class*='NeedFill']").each(function (i) {
        if ($.trim($(this).val()) == "") {
            alert($(this).parent().prev().text() + "不能空");
            return false;
        }
    });
    return true;
}

function Save() {
    
    var OpenId = $("#OpenId").val();
    var NeedFollowUp = $("#NeedFollowUp").get(0).checked;
    var AliPayAccount = $("#AliPayAccount").val();
    var UserStatus = $("#UserStatus").val();
   
    var ParentOpenId = $("#selParentAgent").val();
    var ParentName = $("#selParentAgent").find("option:selected").text();
    var ParentCommissionRate = $("#ParentCommissionRate").val();
    var StoreId = $("#selStore").val();

    var qrUserId = $("#QrUserId").val();
    var UserRole = $("#selUserRole").val();

    var MarketRate = $("#MarketRate").val();
    var Rate = $("#Rate").val();

    var qrInfoId = $("#Invite_QRCode").val();
    var Invite_Rate = $("#Invite_Rate").val();
    var Invite_ParentCommissionRate = $("#Invite_ParentCommissionRate").val();
    var QRInfo_MaxInviteCount = $("#Invite_MaxInviteCount").val();


    if (!CheckForm()) return;

    var url = "/User/SaveUserAgent";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: {
            "OpenId": OpenId, "QRInfo_MaxInviteCount": QRInfo_MaxInviteCount,
            "NeedFollowUp": NeedFollowUp, "QRInfo_Rate": Invite_Rate,
            "QRInfo_ParentCommissionRate": Invite_ParentCommissionRate,
            "Rate": Rate, "MarketRate": MarketRate,
            "AliPayAccount": AliPayAccount, "UserRole": UserRole,
            "UserStatus": UserStatus, "ParentOpenId": ParentOpenId,
            "ParentName": ParentName, "ParentCommissionRate": ParentCommissionRate,
            "StoreId": StoreId, "qrUserId": qrUserId
        },
        url: url,
        success: function (data) {
            if (data == "OK") {

                alert("Save Done");
              //  window.location.href = "list";
            }
            else {
                alert(data);
            }
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });

}

function AjaxO2OQR() {
    var url = "/QR/GetByType";

    $.ajax({
        type: 'post',
        data: "openId=" + OpenId + "&qrType=10",
        url: url,
        success: function (data) {
            $("#QRO2O_FeeRate").val(parseFloat(data.MarketRate) - parseFloat(data.Rate));
            $("#QRO2O_MarketRate").val(data.MarketRate);
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });
}
//roleType:10 出库商  12：商户
function BecomeO2ORole(roleType)
{
    var url = "/User/O2OCreateShipment";
    var OpenId = $("#OpenId").val();

    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "OpenId": OpenId, "O2ORole": roleType },
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
                alert(data.SuccessMsg);
            }
            else {
                alert(data.ErrorMsg);
            }
        },
        error: function (xhr, type) {

            alert("系统出错");

        }
    });
}

function CreateOrUpdateQRO2O() {
    var url = "/User/O2OCreateOrUpdate";
    var OpenId = $("#OpenId").val();

    var QRO2O_FeeRate = $("#QRO2O_FeeRate").val();
    var QRO2O_MarketRate = $("#QRO2O_MarketRate").val();

    var Rate = parseFloat(QRO2O_MarketRate) - parseFloat(QRO2O_FeeRate);
    if (QRO2O_FeeRate == 0 || QRO2O_MarketRate == 0) {
        alert("值不能为空或0");
        return;
    }
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "OpenId": OpenId, "Rate": Rate, "marketRate": QRO2O_MarketRate },
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
                alert(data.SuccessMsg);
            }
            else {
                alert(data.ErrorMsg);
            }
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });
}

function CreateOrUpdateQRCC() {
    var url = "/User/CreateOrUpdateQRCC";
    var OpenId = $("#OpenId").val();

    var QRCC_FeeRate = $("#QRCC_Rate").val();
    var QRCC_MarketRate = $("#QRCC_MarketRate").val();

  //  var Rate = parseFloat(QRCC_MarketRate) - parseFloat(QRCC_FeeRate);
    if (QRCC_FeeRate == 0 || QRCC_MarketRate == 0) {
        alert("值不能为空或0");
        return;
    }
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "OpenId": OpenId, "Rate": QRCC_FeeRate, "marketRate": QRCC_MarketRate },
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
                alert(data.SuccessMsg);
            }
            else {
                alert(data.ErrorMsg);
            }
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });
}

function CreateUserStore() {
    var url = "/User/CreateOrUpdateUserStore";
    var OpenId = $("#OpenId").val();

    var UserStoreRate = $("#UserStore_Rate").val();
    var UserStoreFixComm = $("#UserStore_FixComm").val();
    var UserStoreOwnRate = $("#UserStore_OwnRate").val();

    if (UserStoreRate == 0) {
        alert("值不能为空或0");
        return;
    }
    $.ajax({
        type: 'post',
        dataType: "json",
        data: {
            "OpenId": OpenId,
            "UserStoreRate": UserStoreRate,
            "UserStoreFixComm": UserStoreFixComm,
            "UserStoreOwnRate":UserStoreOwnRate,
        },
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
                alert(data.SuccessMsg);
            }
            else {
                alert(data.ErrorMsg);
            }
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });
}
