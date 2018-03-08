$(function () {

    var shipAccountList = null;

    SelectShipAccount = function ()
    {
        var selval = $("#shipmentList").val();
        var a = selval.split("_")[1];

        $("#O2OShipBalance").val(a);
        $("#O2OOnOrderAmount").val(selval.split("_")[0]);
    }
    //出库商账户
    InitShipAccount = function()
    {
        var CurrentUserId = $("#CurrentUserId").val();
        if (CurrentUserId == 0)
        {
            alert("Session 失效！");
            window.location.href = "/O2O/Login";
            return;
        }
        var IsAdmin = $("#IsAdmin").val();
        if (!IsAdmin)
        {
            $("#shipmentList").attr("disabled", true);
        }
        var url = "/User/GetShipmentAccount";

        $.ajax({
            type: 'post',
            url: url,
            data: { },
            success: function (res) {
                if (res.IsSuccess) {
                    shipAccountList = res.resultList;
                    if (shipAccountList.length == 0)
                    {
                        alert("没有找到您的账户余额！");
                        return;
                    }
                    else
                    {
                        $.each(shipAccountList, function (i) {
                            $("#shipmentList").append("<option value='" + shipAccountList[i].O2OOnOrderAmount + "_" + shipAccountList[i].O2OShipBalance + "'>" + shipAccountList[i].UserName + "</option>");
                           
                        });
                        SelectShipAccount();
                    }
                 
                }
                else 
                {
                    alert(res.ErrorMsg);
                    
                }


            },
            error: function (xhr, type) {
                alert("系统错误！");
            }
        });
    }

    InitShipAccount();
});