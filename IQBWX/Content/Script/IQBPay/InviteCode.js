$(document).ready(function () {

    var userRole = $("#hUserRole").val();
    if(userRole!=100)
    {
        $("#Rate").attr("disabled", true);
        $("#ParentCommRate").attr("disabled", true);
    }
});

function InviteCodeStatus()
{
    //$.alert({
    //    theme: 'dark',
    //    title: 'WOW',
    //    content: "暂时不开放",
    //});
    //return;
    var url = "/PP/InviteCodeUpdate_Status";
    var RecordStatus = $("#RecStatus").val();
    var ID = $("#hQRId").val();
    
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "ID": ID, "RecordStatus": RecordStatus },
        url: url,
        success: function (data) {
           
            if (data.IsSuccess == true) {
         
                alert("邀请码已更新");
                window.location.reload();
              //  window.location.reload();
            }
            else {
                $.alert({
                    theme: 'dark',
                    title: '错误!',
                    content: data.ErrorMsg + ".请联系管理员",
                });
            }
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });

}

function Update()
{
    var ID = $("#hQRId").val();
    //var Rate = $("#Rate").val();
  
    //var ParentCommissionRate = $("#ParentCommRate").val();
    var ReceiveStoreId = $("#selStoreRate").val();
    var ParentOpenId = $("#selParentAgent").val();

    var url = "/PP/InviteCodeUpdate";
    $.ajax({
        type: 'post',
        dataType: "json",
        //data: { "ID": ID, "Rate": Rate, "ParentCommissionRate": ParentCommissionRate, "ReceiveStoreId": ReceiveStoreId, "ParentOpenId": ParentOpenId },
        data: { "ID": ID, "ReceiveStoreId": -1, "ParentOpenId": ParentOpenId },
        url: url,
        success: function (data) {
            if (data.IsSuccess == true) {
                $.alert({
                    title: '成功!',
                    content: "修改成功",
                });
               
            }
            else {
                $.alert({
                    theme: 'dark',
                    title: '错误!',
                    content: data.ErrorMsg + ".请联系管理员",
                });
            }
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });
}