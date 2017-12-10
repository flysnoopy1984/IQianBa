$(document).ready(function () {

  
    var url = "/Wap/GetJSSDK";
    var AuthUrl = window.location.href;

    $.ajax({
        type: 'post',
        data: "AuthUrl="+AuthUrl,
        url: url,
        success: function (data) {
            WXSet(data);
        },
        error: function (xhr, type) {

            alert('Ajax error!');

        }
    });

});

function UploadImage()
{
    wx.chooseImage({

        count: 3, // 默认9

        sizeType: ['original', 'compressed'], // 可以指定是原图还是压缩图，默认二者都有

        sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有

        success: function (res) {

            var localIds = res.localIds; // 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片

            if (localIds)
            {
                wx.uploadImage({

                    localId: localIds, // 需要上传的图片的本地ID，由chooseImage接口获得

                    isShowProgressTips: 1, // 默认为1，显示进度提示

                    success: function (res) {

                        var serverId = res.serverId; // 返回图片的服务器端ID

                    }

                });
            }
          

        }

    });

   
}



function WXSet(data)
{
    wx.config({

        debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。

        appId: data.AppId, // 必填，公众号的唯一标识

        timestamp: data.timestamp, // 必填，生成签名的时间戳

        nonceStr: data.nonceStr, // 必填，生成签名的随机串

        signature: data.signature,// 必填，签名，见附录1

        jsApiList: ['chooseImage','uploadImage', 'getLocation'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2

    });

    wx.error(function (res) {
        alert("error" + res.errMsg);
    });


}