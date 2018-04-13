$(function () {
    toRateList = function () {
        window.location.href = "/O2OWap/AgentFeeRate";
    }

    ShowBlock = function () {

        var msg = ' <div id="ProcessArea1" class="progress progress-striped active">';
        msg += '<div id="upload_progress1" class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 100%; height:40px;">';
        msg += '<span class="sr-only">正在赶往订单列表中...</span>';
        msg += '</div>';
        msg += '</div>';

        $.blockUI({
            message: msg,
            css: {
                border: 'none',
                width: '90%',
                height: '40px',
                left: '20px',
                'border-radius': '20px',
            }
        });
    }

    ShowAllOrderList = function (phone,openId) {
        if (phone == "" || phone == "null")
        {
            alert("代理手机没有获取，请联系平台！");
            return;
        }
       // ShowBlock();
        var url = payUrl + "/O2OWap/LoginFormPP?aoId=" + openId + "&UserNum=" + phone+"&goto=OrderList";
        window.location.href = url;
        //$.ajax({
        //    type: 'post',
        //    data: { 'UserNum': phone },
        //    url: url,
        //    success: function (res) {
        //        $.unblockUI();
        //        if (res.IsSuccess) {
        //            window.location.href=payUrl +"/O2OWap/OrderList?aoId="+phone;
        //        }
        //        else {
        //            alert(res.ErrorMsg);
        //        }
        //    },
        //    error: function (xhr, type) {
        //        $.unblockUI();
        //        alert("系统错误！");
        //    }
        //});
    }
  
})