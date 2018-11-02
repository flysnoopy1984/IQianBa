$(function () {
    var browser = {
        versions: function () {
            var u = navigator.userAgent,
                app = navigator.appVersion;
            return {
                trident: u.indexOf('Trident') > -1, //IE内核
                presto: u.indexOf('Presto') > -1, //opera内核
                webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核
                gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1,//火狐内核
                mobile: !!u.match(/AppleWebKit.*Mobile.*/), //是否为移动终端
                ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
                android: u.indexOf('Android') > -1 || u.indexOf('Adr') > -1, //android终端
                iPhone: u.indexOf('iPhone') > -1, //是否为iPhone或者QQHD浏览器
                iPad: u.indexOf('iPad') > -1, //是否iPad
                webApp: u.indexOf('Safari') == -1, //是否web应该程序，没有头部与底部
                weixin: u.indexOf('MicroMessenger') > -1, //是否微信 （2015-01-22新增）
                qq: u.match(/\sQQ/i) == " qq" //是否QQ
            };
        }(),
        language: (navigator.browserLanguage || navigator.language).toLowerCase()
    }
 

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
                    if (browser.versions.mobile || browser.versions.android || browser.versions.ios)
                    webkit.messageHandlers.ModifyPwd.postMessage(1);
                }
                else {
                    alert(res.ErrorMsg);
                    if (browser.versions.mobile || browser.versions.android || browser.versions.ios)
                    webkit.messageHandlers.ModifyPwd.postMessage(-1);
                }
               
            },
            error: function (xhr, type) {
                CloseBlock();
                alert("系统错误！");
                if (browser.versions.mobile || browser.versions.android || browser.versions.ios)
                webkit.messageHandlers.ModifyPwd.postMessage(-1);
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