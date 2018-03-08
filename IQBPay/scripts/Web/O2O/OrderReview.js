$(document).ready(function () {

    $("#ImgSpan").viewer({
        navbar: false,
        toolbar:false,
    });
});

//act =0 通过 act=1 驳回
function Review(act)
{
    var O2ONo = $("#O2ONo").val();
    var MallOrderNo = $("#MallOrderNo").val();
    var OrderAmount = parseFloat($("#OrderAmount").val());
    var Reason = $("#Reason").val();
    var IsApprove = true;
    if (act == 0)
    {
        if (O2ONo == "" || MallOrderNo == "" || OrderAmount == 0) {
            alert("必要字段没有填写");
            return;
        }
    }
    else
    {
        IsApprove = false;
        if (O2ONo == "" || Reason == "") {
            alert("必要字段没有填写");
            return;
        }
    }
   
    var url = "/O2O/OrderReviewResult";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "O2ONo": O2ONo, "RejectReason": Reason, "MallOrderNo": MallOrderNo, "OrderAmount": OrderAmount, "IsApprove": IsApprove },
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
                alert("操作成功！");
            }
            else {
                alert(data.ErrorMsg);
            }
        },
        error: function (xhr, type) {
            alert("System Error!");
        }
    });

}

