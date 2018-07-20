var Id = "";
$(document).ready(function () {

    Id = GetUrlParam("id");
    var channel = GetUrlParam("c");

    if (Id != null && Id != "")
        Init(Id);
    else
    {
        if (channel == 0 || channel == 1)
            $("#Channel").val(channel);

        $("#MaxLimitAmount").val(400);
        $("#MinLimitAmount").val(0);
        $("#DayIncome").val(10000);
        $("#RemainAmount").val(10000);

        $("#QRStatus").bootstrapSwitch({
            onText: "未使用",
            state: true,
            offText: "已使用",
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

    
});

function Init(Id) {
   
   

    var url = "/QR/GetStoreAuthQR";
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

    //if (Id == "" || Id == null)
    //    Id = -1;

    $("#RecId").val(data.ID);
    $("#Name").val(data.StoreName);
    $("#Rate").val(data.Rate);
    $("#Remark").val(data.Remark);
    $("#Channel").val(data.Channel);
    $("#QRStatus").val(data.RecordStatus);
    $("#appId").find("option[value='" + data.APPId + "']").attr("selected", true);
    $("#StoreType").val(data.StoreType);
    $("#OpenId").val(data.OwnnerOpenId);

    $("#MaxLimitAmount").val(data.MaxLimitAmount);
    $("#MinLimitAmount").val(data.MinLimitAmount);
    $("#DayIncome").val(data.DayIncome);
    $("#RemainAmount").val(data.RemainAmount);


    var filePath = data.FilePath;
    if(filePath!=null && filePath!="")
        $("#QRImg").attr("src", filePath);

    var st;
    if (data.RecordStatus == 0)
        st = true;
    else
        st = false

    $("#QRStatus").bootstrapSwitch({
        onText: "未使用",
        state: st,
        offText: "已使用",
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

function New() {
    window.location = "AuthInfo?do=1";
}

function Save() {

    var QRStatus = $("#QRStatus").val();
    var name = $("#Name").val();
    var rate = $("#Rate").val();
    var remake = $("#Remark").val();
    var Channel = $("#Channel").val();
    var level = $("#Level").val();
    var appId = $("#appId").val();
    var StoreType = $("#StoreType").val();
    var ID = $("#RecId").val();
    var MaxLimitAmount = $("#MaxLimitAmount").val();
    var MinLimitAmount = $("#MinLimitAmount").val();
    var DayIncome = $("#DayIncome").val();
    var RemainAmount = $("#RemainAmount").val();
 

    if (!CheckForm()) return;

    var url = "/QR/SaveStoreAuth";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: {
            "ID": ID, "APPId": appId,
            "StoreName": name, "StoreType": StoreType,
            "Rate": rate, "Channel": Channel,
            "Remark": remake, "RecordStatus": QRStatus,
            "MaxLimitAmount": MaxLimitAmount, "MinLimitAmount": MinLimitAmount,
            "DayIncome": DayIncome, "RemainAmount": RemainAmount
        },
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
                alert("Save Done");
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