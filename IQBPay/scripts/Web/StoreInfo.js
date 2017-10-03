$(document).ready(function () {
 
    $("#StoreStatus").bootstrapSwitch({
        onText: "启用",
        state: true,
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

});

function CheckForm() {
    $("#EditArea").find("input[class*='NeedFill']").each(function (i) {
        if ($.trim($(this).val()) == "") {
            alert($(this).parent().prev().text() + "不能空");
            return false;
        }
    });
    return true;
}

function Save()
{
    var StoreStatus = $("#StoreStatus").val();
    var name = $("#Name").val();
    var remake = $("#Remark").val();

    if (!CheckForm()) return;

    var url = "/Store/Add";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "Name": name, "Remark": remake, "RecordStatus": StoreStatus },
        url: url,
        success: function (data) {
            if(data == "OK")
            {
                alert("Save Done");
            }
            else
            {
                alert(data);
            }
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });
   

}