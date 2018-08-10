$(function () {

    Init = function () {
        var OrderNo = $("#OrderNo").text();
        if (OrderNo == "") {
            alert("订单没有获取");
            $("#btnAgree").hide();
        }
        var status = $("#hOrderStatus").val();
        if (status != 10) {
            $("#btnAgree").hide();
        }
    }

 
    Agree = function () {
      
        var OrderNo = $("#OrderNo").text();
        if (OrderNo == "") {
            alert("订单没有获取");
        }

        var url = "/api/PPAdmin/AgreePay?OrderNo="+OrderNo;

        $.ajax({
            type: 'post',
            url: url,
            success: function (res) {
                if (res.IsSuccess) {
                    alert("OK");
                    window.location.reload();
                }
                else {
                    alert(res.ErrorMsg);

                }
            },
            error: function (xhr, type) {
                alert("系统错误！请联系平台！");
            }
        });
    };


  

    Init();

});