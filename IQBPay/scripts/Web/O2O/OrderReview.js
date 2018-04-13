$(document).ready(function () {

    var page = window.location.pathname.toLowerCase();
    if (page == "/O2O/OrderReview".toLowerCase()) {

        $("#ImgSpan").viewer({
            navbar: false,
            toolbar: false,
        });
    }
    var imgSrc = $("#imgOrder").attr("src");
    if(imgSrc == "" || imgSrc== null)
    {
        $("#imgOrder").hide();
    }
    else
    {
        $("#upload_OrderInfo").hide();
    }

  
});

function PaymentOrder()
{
    var O2ONo = $("#O2ONo").val();
    var url = "/O2O/OrderPaymentToUser_Agent";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "O2ONo": O2ONo},
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

function UploadFile() {
    var url = "/O2OWap/UploadOrderInfo";
    var formData = new FormData();

    formData.append("file", $("#upload_OrderInfo")[0].files[0]);
    formData.append("OrderNo", $("#O2ONo").val());


    StartBlockUI("正在上传请耐心等待...", 100);

    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        data: formData,
        processData: false,
        contentType: false,
        dataType: "json",
        success: function (data) {
            $.unblockUI();
            if (data.IsSuccess == true) {

                $("#imgOrder").show();
                $("#imgOrder").attr({ src: data.resultObj });
                $("#upload_OrderInfo").hide();
            }
            else {
                switch (data.IntMsg) {
                    case -1:
                        alert("文件过大"); break;
                    case -2:
                        alert("手机号未获取,请重新提交");
                      
                        //  window.location.href = "/O2OWap/Index?aoId=" + aoId;
                        break;
                    case -3:
                        alert("文件格式不正确"); break;
                    case -4:
                        alert("订单编号未获取");
                      
                        //   window.location.href = "/O2OWap/OrderDetail?aoId=" + aoId;
                        break;

                }
                InitOterControl();
            }
           
        },
        error: function (xhr, type) {
            $.unblockUI();
            alert("系统错误！");
        }
    });
}


//act =0 通过 act=1 驳回
function Review(act)
{
    var O2ONo = $("#O2ONo").val();
    var MallOrderNo = $("#MallOrderNo").val();
    var OrderAmount = parseFloat($("#OrderAmount").val());
    var Reason = $("#Reason").val();
    var IsApprove = true;
    var OrderImgSrc = $("#imgOrder").attr("src");
    //通过
    if (act == 0)
    {
        if (O2ONo == "" || MallOrderNo == "" || OrderAmount == 0 || OrderImgSrc == "" || OrderImgSrc==null) {
            alert("必要字段没有填写,或图片没上传");
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
        data: {
            "O2ONo": O2ONo,
            "RejectReason": Reason,
            "MallOrderNo": MallOrderNo,
            "OrderAmount": OrderAmount,
            "OrderImgSrc":OrderImgSrc,
            "IsApprove": IsApprove
        },
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
                alert("操作成功！");
            }
            else {
                alert(data.ErrorMsg);
                return;

            }
        },
        error: function (xhr, type) {
            alert("System Error!");
        }
    });

}

