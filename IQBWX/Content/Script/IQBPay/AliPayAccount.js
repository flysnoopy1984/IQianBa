$(document).ready(function () {

    InitControl();
});

function InitControl()
{
   
    var AliPayAccount = $("#AliPayAccount").val();

    $("#AliPayAccount").attr("disabled", true);

    if (AliPayAccount.replace(/(^s*)|(s*$)/g, "").length >0)  
    {
        $("#btnModify").show();
        $("#btnSubmit").hide();
    }
    else {
        $("#AliPayAccount").attr("disabled", false);
        $("#btnModify").hide();
        $("#btnSubmit").show();
    }
}

function ClickModify()
{
    $("#AliPayAccount").attr("disabled", false);
    $("#btnModify").hide();

    $("#btnSubmit").show();
}

function UpdateAccount()
{
    var url = "/PP/UpdateAliPayAccount";
    var ID = $("#AgentUserRecId").val();
    var AliPayAccount = $("#AliPayAccount").val();
    if (AliPayAccount.replace(/(^s*)|(s*$)/g, "").length == 0)
    {
        alert("账户为空！");
        return;
    }
    $.ajax({
        type: 'post',
        data: "ID=" + ID + "&AliPayAccount=" + AliPayAccount,
        url: url,
        success: function (data) {
           
            if (data == "OK") {
                alert("修改成功！");
                if (document.referrer != "")
                    window.location.href = document.referrer;
            }
            else
                alert(data);
         
            InitControl();

        },
        error: function (xhr, type) {
            InitControl();
            alert('Ajax error!');
        }
    });
}