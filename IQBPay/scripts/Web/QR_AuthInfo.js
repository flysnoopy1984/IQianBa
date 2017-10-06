$(document).ready(function () {

    var Id = GetUrlParam("id");
    var channel = GetUrlParam("c");

    if (Id != null && Id != "")
        Init(Id);
    else
    {
        if (channel == 0 || channel == 1)
            $("#Channel").val(channel);
    }

    $("#QRStatus").bootstrapSwitch({
        onText: "有效",
        state: true,
        offText: "失效",
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
});

function Init(Id) {
    if (Id == null || Id == "") {
        Id = -1;
    }
    $("#RecId").val(Id);

    var url = "/QR/Get";
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
    $("#Remark").val(data.Remark);
    $("#Channel").val(data.Channel);

    var filePath = data.FilePath;
    $("#QRImg").attr("src", filePath);


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

    var ID = $("#RecId").val();

    if (!CheckForm()) return;

    var url = "/QR/SaveAuth";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "ID": ID, "Name": name, "Rate": rate,"Channel":Channel, "Remark": remake, "RecordStatus": QRStatus },
        url: url,
        success: function (data) {
            if (data.RunResult == "OK") {
                alert("Save Done");
                InitFormData(data);
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