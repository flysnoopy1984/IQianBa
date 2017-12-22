var Id="";
$(document).ready(function () {

    Id = GetUrlParam("id");

    Init(Id);
   

   
});

function Init(Id) {
    
    if (Id == "" || Id == null)
        Id = -1;

    $("#RecId").val(Id);

    var url = "/QR/Get";
    $.ajax({
        type: 'post',
        data: "Id=" + Id+"&qrType=3",
        url: url,
        success: function (data) {

            InitFormData(data);

           
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });

}

function InitFormData(data)
{
    $("#ReceiveStore").empty();
    $("#ReceiveStore").append("<option value='-1'>随机</option>");
    $(data.HashStoreList).each(function (i, r) {
        $("#ReceiveStore").append("<option value='" + r.Id + "'>" + r.Name + "</option>");
    });

    $("#ParentOpenId").empty();
    $("#ParentOpenId").append("<option value=''>无</option>");
    $(data.HashUserList).each(function (i, r) {
        $("#ParentOpenId").append("<option value='" + r.OpenId + "'>" + r.Name + "</option>");
    });

    $("#Name").val(data.Name);
    $("#Rate").val(data.Rate);
    $("#Level").val(data.Level);
    $("#Remark").val(data.Remark);

    //var filePath =  data.FilePath;
    $("#QRImg").attr("src", data.FilePath);
  
    $("#ReceiveStore").val(data.ReceiveStoreId);

    $("#ParentCommissionRate").val(data.ParentCommissionRate);

    $("#ParentOpenId").val(data.ParentOpenId);

    $("#NeedVerification").attr("checked", data.NeedVerification)

    var st;
    if (data.RecordStatus == 0)
        st = true;
    else
        st = false
    if (Id == -1) st = true;

    $("#QRStatus").bootstrapSwitch({
        onText: "启用",
        state: st,
        offText: "禁用",
        onColor: "success",
        offColor: "danger",
        size: "small",
        onSwitchChange: function (event, state) {
            if (state == true) {
                $(this).val("0");
            } else {
                $(this).val("1");
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

function New()
{
    window.location = "ARInfo?do=1";
}

function Save() {

    var QRStatus = $("#QRStatus").val();
    var name = $("#Name").val();
    var rate = $("#Rate").val();
    var remake = $("#Remark").val();
    var ID = $("#RecId").val();
    var storeId = $("#ReceiveStore").val();
   //var Level = $("#Level").val();
    var ParentCommissionRate = $("#ParentCommissionRate").val();
    var ParentOpenId = $("#ParentOpenId").val();

    var NeedVerification = $("#NeedVerification").get(0).checked;

    if (!CheckForm()) return;

    var url = "/QR/SaveAR";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "ID": ID, "NeedVerification":NeedVerification,"Name": name, "ParentOpenId":ParentOpenId,"ParentCommissionRate": ParentCommissionRate, "Rate": rate, "Remark": remake, "RecordStatus": QRStatus, "ReceiveStoreId": storeId },
        url: url,
        success: function (data) {
            if (data.RunResult == "OK") {

                alert("Save Done");
                window.location.href = "ARlist";

               // InitFormData(data);
            }
            else {
                alert(data.RunResult);
            }
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });

}