var gStoreList;
var gParentAgentList;

$(document).ready(function () {

    var Id = GetUrlParam("id");

    if (Id == null || Id == "" || Id == "undefined") {
        alert("无法识别ID");
        window.location.href = "list";
        return;
    }

    $('#StoreRate').attr("disabled", true);

    $('#Name').attr("disabled", true);
    //$('#ParentCommissionRate').attr("disabled", true);
    Init(Id);

});

function Init(Id) {
   
    $("#RecId").val(Id);

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
        data: "Id=" + Id,
        url: url,
        success: function (data) {
            gStoreList = data.StoreList;
            gParentAgentList = data.ParentAgentList;
            InitFormData(data);
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

    $("#IsAutoTransfer").attr("checked", data.IsAutoTransfer);

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
    
    var ID = $("#RecId").val();
    var IsAutoTransfer = $("#IsAutoTransfer").get(0).checked;
    var AliPayAccount = $("#AliPayAccount").val();
    var UserStatus = $("#UserStatus").val();
   
    var ParentOpenId = $("#selParentAgent").val();
    var ParentName = $("#selParentAgent").find("option:selected").text();
    var ParentCommissionRate = $("#ParentCommissionRate").val();
    var StoreId = $("#selStore").val();

    var qrUserId = $("#QrUserId").val();
    var UserRole = $("#selUserRole").val();

    if (!CheckForm()) return;

    var url = "/User/SaveUserAgent";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "Id": ID, "IsAutoTransfer": IsAutoTransfer, "AliPayAccount": AliPayAccount,"UserRole":UserRole, "UserStatus": UserStatus, "ParentOpenId": ParentOpenId, "ParentName":ParentName,"ParentCommissionRate": ParentCommissionRate, "StoreId": StoreId, "qrUserId": qrUserId },
        url: url,
        success: function (data) {
            if (data == "OK") {

                alert("Save Done");
                window.location.href = "list";
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