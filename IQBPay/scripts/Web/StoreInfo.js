$(document).ready(function () {
 
    var Id = GetUrlParam("id");
    if (Id == null || Id == undefined)
    {
        alert("参数出错!")
        window.location.href = "/Store/List";
        return;
    }

    Init(Id);

    

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

function Init(Id) {
  
  

    var url = "/Store/Get";
    $.ajax({
        type: 'post',
        data: "Id=" + Id,
        url: url,
        success: function (data) {

            if (data.RunResult == "OK")
            {
                InitFormData(data);
                var st = true;
                if (data.RecordStatus == 1)
                    st = false;
                $("#StoreStatus").bootstrapSwitch({
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
            else
            {
                alert(data.RunResult);
            }
           

        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });
}

function InitFormData(data)
{
    $("#RecId").val(data.ID);
    $("#Name").val(data.Name);
    $("#Rate").val(data.Rate);
    //$("#OpenTime").val(data.OpenTime);
    //$("#CloseTime").val(data.CloseTime);
    $("#Remark").val(data.Remark);
    $("#DayIncome").val(data.DayIncome);
    $("#RemainAmount").val(data.RemainAmount);
    $("#MinLimitAmount").val(data.MinLimitAmount);
    $("#MaxLimitAmount").val(data.MaxLimitAmount);
    $("#StoreStatus").val(data.RecordStatus);
    $("#IsReceiveAccount").attr("checked", data.IsReceiveAccount);
    $("#Channel").val(data.Channel);

    var channel = data.Channel;
    if(channel == 0)
        $("#gpIsReceiveAccount").show();
    else
        $("#gpIsReceiveAccount").hide();

}


function Save()
{
    
    var StoreStatus = $("#StoreStatus").val();
    var name = $("#Name").val();
    var remake = $("#Remark").val();

    var ID = $("#RecId").val();
    var Rate = $("#Rate").val();
    //var OpenTime = $("#OpenTime").val();
    //var CloseTime = $("#CloseTime").val();
    var DayIncome = $("#DayIncome").val();
    var RemainAmount = $("#RemainAmount").val();
    var MinLimitAmount = $("#MinLimitAmount").val();
    var MaxLimitAmount = $("#MaxLimitAmount").val();

    var Channel = $("#Channel").val();
    //$("#MinLimitAmount").val(data.MinLimitAmount);
    //$("#MaxLimitAmount").val(data.MaxLimitAmount);


    var IsReceiveAccount = $("#IsReceiveAccount").get(0).checked;
 

    if (!CheckForm()) return;

    var url = "/Store/Save";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "ID": ID,"Channel":Channel,"RemainAmount":RemainAmount,"DayIncome":DayIncome,"MinLimitAmount":MinLimitAmount,"MaxLimitAmount":MaxLimitAmount, "Name": name, "Rate": Rate,  "Remark": remake, "RecordStatus": StoreStatus, "IsReceiveAccount": IsReceiveAccount },
        url: url,
        success: function (data) {
            if(data == "OK")
            {
                alert("Save Done");
             //   window.location = "list";
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
