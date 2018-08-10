$(function () {
    var FromClient = "";

    Init = function () {

        var openId = getUrlParam("Id");
        if (openId == "" || openId == null)
        {
            toPage("/Home/ErrorMessage?code=2000&ErrorMsg=没有获取中介身份，请联系管理员");
        }
           

        var url = "/api/pagedata/InitPaySelection";

        FromClient = IsWeixinOrAlipay();

        if (FromClient == "false") {
            $(".boxInfo").hide();
            $.alert({
                title: '注意!',
                content: '请从支付宝或微信扫码进入',
            });

            return;
        }

        ShowBlock();
        
        $.ajax({
            type: 'get',
            dataType: "json",
            data: { "openId": openId },
            url: url,
            success: function (data) {
                var result = data.resultList;
                if (data.IsSuccess) {
                    $.each(result, function (i) {
                        if(result[i].QRType == 2){
                            $("#liJY").on("click",{"Id":result[i].QRUserId,"t":result[i].QRType},toPay);
                        }

                        else if (result[i].QRType == 1 && FromClient == "Alipay") {
                            $("#liSJ").on("click", { "Id": result[i].QRUserId,"t":result[i].QRType },toPay);
                        }
                    })
                }
                else
                {
                    toPage("/Home/ErrorMessage?code=2000&ErrorMsg=" + data.ErrorMsg);
                }
                $.unblockUI();
            },
            error: function (xhr, type) {

                alert(xhr.responseText);
                $.unblockUI();
            }
        });
        
    }
    toPay = function(e)
    {
        var Id = e.data.Id;
        var type = e.data.t;
      
        if (FromClient == "WeiXin")
        {
            window.location.href = "/PP/WXPay?Id=" + Id + "&t=" + type;
        }
        else
        {
            window.location.href = "/PP/Pay?Id=" + Id + "&t=" + type;
        }
       
    }
    toPage = function (url) {
        window.location.href = url;
        return;
    }

    ShowBlock = function () {

        var msg = ' <div id="ProcessArea1" class="progress progress-striped active">';
        msg += '<div id="upload_progress1" class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 100%; height:40px;">';
        msg += '<span class="sr-only">数据加载中...</span>';
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

    Init();
})