$(document).ready(function () {

    var canDo = CheckPage();
    if (canDo == -1)
    {
        alert("Session 失效，重新登陆");
        window.location.href = "/O2O/Login";
    }
    if (canDo == -2) {
        alert("订单未获取，联系管理员!");
        window.open("about:blank", "_self").close();
    }

    $("#ImgSpan").viewer({
        navbar: false,
        toolbar: false,
    });
});

function StartBlockUI(txt, w) {

    if (w == undefined)
        w = 100;
    var msg = ' <div id="ProcessArea1" class="progress progress-striped active">';
    msg += '<div id="upload_progress1" class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: ' + w + '%;">';
    msg += '<span class="sr-only">' + txt + '</span>';
    msg += '</div>';
    msg += '</div>';

    ////   alert(data.files[0].name);
    $.blockUI({
        message: msg,
        css: {
           
            border: 'none',
            width: '100%',
            height: '40px',
            left: '0px',
            'border-radius': '4px',
        }
    });
}
function CloseBlock() {
    //  if (pageIndex == 1)
    $.unblockUI();
}


function ConfirmWithPay()
{
    
    //var url = "/O2O/OrderSettlementToPP";
    var url = "/O2O/SettleToWH_Agent_User"; //改为一次性全部结算
    var O2ONo = $("#O2ONo").val();
    if (O2ONo == "" || O2ONo == "null")
    {
        alert("订单编号为空，联系管理员!");
        window.open("about:blank", "_self").close();
    }
    StartBlockUI("结算处理中,请稍等...");
  //  $("#btnConfirm").hide();
   
   
    
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "O2ONo": O2ONo },
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
                alert("结算成功！");
                self.opener.location.reload();
                window.open("about:blank", "_self").close();
                CloseBlock();
            }
            else {
                alert(data.ErrorMsg);
                if (data.IntMsg == -1) {
                    window.location.href = "/O2O/Login";
                }
                CloseBlock();
            }
           

        },
        error: function (xhr, type) {
            CloseBlock();
            alert('Ajax error!');
        }
    });
    
    
    
}