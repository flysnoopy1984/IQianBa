$(function () {

    modifyPwd = function () {

        var url = apiUrl + "/api/User/ModifyPwd";
        var phone = $("#txtPhone").val();
        var txtOldPwd = $("#txtOldPwd").val();
        var txtNewPwd = $("#txtNewPwd").val();
        ShowBlock("处理中，请稍等...");
        $.ajax({
            type: 'post',
            data: {
                "Phone": phone,
                "newPwd": txtNewPwd,
                "oldPwd": txtOldPwd
            },

            url: url,
            success: function (res) {
                CloseBlock();
                if (res.IsSuccess == true)
                {
                    alert("修改成功！");
                }
                else {
                    alert(res.ErrorMsg);
                }
               
            },
            error: function (xhr, type) {
                CloseBlock();
                alert("系统错误！");
            }
        });
    }
    ShowBlock = function (info) {

        if (info == "")
            info = "数据加载中...";

        var msg = ' <div id="ProcessArea1" class="progress progress-striped active">';
        msg += '<div id="upload_progress1" class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100">';
        msg += '<span class="sr-only">' + info + '</span>';
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

    CloseBlock = function () {
        //  if (pageIndex == 1)
        $.unblockUI();
    }

    $("#btn_Sure").on("click", modifyPwd);
})