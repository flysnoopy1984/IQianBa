var gStoreList;
var gParentAgentList;

$(document).ready(function () {

    var OpenId = GetUrlParam("OpenId");

    if (OpenId == null || OpenId == "" || OpenId == "undefined") {
        alert("无法识别OpenId");
        window.location.href = "list";
        return;
    }

    $('#StoreRate').attr("disabled", true);

    $('#Name').attr("disabled", true);
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

            $("#Invite_ParentOpenId").empty();
            $("#Invite_ParentOpenId").append("<option value=''>无</option>");

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

            AjaxInviteCode(data.QRInviteCode);

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
                alert(data.SuccessMsge);  
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
    $("#StoreRate").val(data.StoreRate);
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


    if (!CheckForm()) return;

    var url = "/User/SaveUserAgent";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "OpenId": OpenId,"NeedFollowUp":NeedFollowUp,"QRInfo_Rate":Invite_Rate,"QRInfo_ParentCommissionRate":Invite_ParentCommissionRate,"Rate": Rate, "MarketRate": MarketRate, "AliPayAccount": AliPayAccount, "UserRole": UserRole, "UserStatus": UserStatus, "ParentOpenId": ParentOpenId, "ParentName": ParentName, "ParentCommissionRate": ParentCommissionRate, "StoreId": StoreId, "qrUserId": qrUserId },
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