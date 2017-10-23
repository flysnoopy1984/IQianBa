$(document).ready(function () {

    var Id = GetUrlParam("id");

    if (Id == null || Id == "" || Id == "undefined") {
        alert("无法识别ID");
        window.location.href = "list";
        return;
    }

    $('#Rate').attr("disabled", true);
    $('#ParentAgent').attr("disabled", true);
    $('#ParentCommissionRate').attr("disabled", true);
    Init(Id);

   
});

function Init(Id) {
   
    $("#RecId").val(Id);

    var url = "/User/Get";
    $.ajax({
        type: 'post',
        data: "Id=" + Id,
        url: url,
        success: function (data) {

            InitFormData(data);
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });

}

function InitFormData(data) {
 

    $("#Name").val(data.Name);
    $("#Rate").val(data.Rate);
    $("#ParentAgent").val(data.ParentAgent);
    $("#ParentCommissionRate").val(data.ParentCommissionRate);

    $("#IsAutoTransfer").attr("checked", data.IsAutoTransfer);

    $("#AliPayAccount").val(data.AliPayAccount);
    $("#UserStatus").val(data.UserStatus);

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
    

    var UserStatus = $("#UserStatus").val();
    var name = $("#Name").val();
   
    var AliPayAccount = $("#AliPayAccount").val();
    var ID = $("#RecId").val();
   
    var IsAutoTransfer = $("#IsAutoTransfer").get(0).checked;

    if (!CheckForm()) return;

    var url = "/User/Save";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "Id": ID, "Name": name, "IsAutoTransfer":IsAutoTransfer,"AliPayAccount": AliPayAccount, "UserStatus": UserStatus},
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