$(function () {
    var httpUrl = 'http://pp.iqianba.cn';
    var noticeList = [];
    var ItemId = null;
    var AddrId = null;
    var MallId = null;
    ///出货商Id
    var UserId = null;
    var RealAddr = null;
    /**
     * [返回]
     */
    historyBack = function () {
        window.history.back();
    };

    Init = function()
    {
        ItemId = GetUrlParam("ItemId");
        MallId = GetUrlParam("MallId");
        UserId = GetUrlParam("UserId");
        if (ItemId == null || ItemId == undefined || MallId == null || MallId == undefined || UserId == null || UserId == undefined)
        {
            alert("未获取指定的商品，请选择商品");
            window.location.href = "/O2OWap/MallList";
            return;
        }
          
        var url = "/O2OWap/InitAddr";

        $.ajax({
            type: 'post',
            url: url,
            data:{"ItemId":ItemId,"MallId":MallId,"UserId":UserId},
            success: function (res) {
                if (res.IsSuccess)
                {
                    noticeList = [];
                    noticeList.push("点击【前往购物】按钮将进入商城。</br>已指定购买的商品！");
                    var p2 = "购物必须填写以下地址</br>" + res.resultObj.ReceiveAddress;
                    noticeList.push(p2);

                    AddrId = res.resultObj.Id;
                    RealAddr = res.resultObj.ItemRealAddr;
                    InitNoticeList();
                }
                else
                {
                    if(res.IntMsg == -1)
                    {
                        alert("未获取用户信息，需重新提交手机号");
                        window.location.href = "/O2OWap/Index";
                        return;
                    }
                    if (res.IntMsg == -2) {
                        alert("此商品收货地址没有设置，请联系管理员");
                        window.location.href = "/O2OWap/Index";
                        return;
                    }
                }
             

            },
            error: function (xhr, type) {
                alert("系统错误！");
            }
        });
    }
    /**
     * [获取notice列表]
     * @return {[type]} [description]
     */
    InitNoticeList = function () {
       
        //noticeList = [
        //'点击【前往购物】按钮将进入商城。</br>已指定购买的商品！',
        //'',
        //];
      
        var str = '';
        for (var i = 0; i < noticeList.length; i++) {
            str += '<div class="o2o_notice" id="o2o_notice_' + i + '">';
            str += '<div class="o2o_notice_title">' + noticeList[i] + '</div>';
            str += '<div class="o2o_notice_btn"><button class="btn btn-success" id="' + i + '_o2o_notice_btn">我知道了</button></div>';
            str += '<div class="gap_border"></div>';
            str += '</div>';
        }
        $('.o2o_notice_list').html(str);

        $('.o2o_notice_btn > button').click(function (e) {
            var idx = e.currentTarget.id.split('_')[0];
            var next = Number(idx) + 1;
            if (next >= noticeList.length) {
                $('.confirm_btn > button').css('backgroundColor', '#ff4b59');
                $('.confirm_btn > button').attr('disabled', false);
            } else {
                $("#o2o_notice_" + next).slideDown("slow");
            }
        });
    };

    Init();
    /**
     * [一步一步的我知道了]
     * @param  {[type]} e [description]
     * @return {[type]}   [description]
     */
   

    order = function () {
        if(RealAddr == null)
        {
            alert("此商品收货地址没有获取，请联系管理员或重新选择");
            window.location.href = "/O2OWap/MallList";
        }
        else
        {
            var url = "/O2OWap/CreateO2OOrder";

            $.ajax({
                type: 'post',
                url: url,
                data: { "ItemId": ItemId, "AddrId": AddrId },
                success: function (res) {
                    if (res.IsSuccess) {
                        window.open(RealAddr, "_blank", "menubar=0,scrollbars=1, resizable=1,status=1,titlebar=0,toolbar=0,location=1");
                    }
                    else {
                        switch(res.IntMsg)
                        {
                            //手机号为空，重新登陆
                            case -1:
                                alert("手机号未获取，请返回首页重新操作");
                                window.location.href = "/O2OWap/Index";
                                break;
                            case -2:
                                alert("商品未获取，请返回重新选择商品");
                                window.location.href = "/O2OWap/MallList";
                                break;
                            case -3:
                                alert("收货地址未获取，请联系管理员");
                              //  window.location.href = "O2OWap/MallList";
                                break;
                            case -4:
                                alert("中介信息未获取，可能等待时间过长，请返回首页重新操作");
                                window.location.href = "/O2OWap/Index";
                                break;
                            case -5:
                                alert("中介费率未配置，请联系您的中介");
                               // window.location.href = "O2OWap/MallList";
                                break;
                            case -6:
                                alert("出库商佣金未配置，请联系管理员");
                              //  window.location.href = "O2OWap/MallList";
                                break;
                            case -7:
                                alert("出库商余额未配置，请联系管理员");
                                //  window.location.href = "O2OWap/MallList";
                                break;
                            case -8:
                                alert(res.ErrorMsg);
                                //  window.location.href = "O2OWap/MallList";
                                break;
                            case -9:
                               // alert(res.ErrorMsg);
                                window.location.href = "/O2OWap/ErrorPage?ec=40001";
                                break;
                            case -10:
                                // alert(res.ErrorMsg);
                                window.location.href = "/O2OWap/ErrorPage?ec=1";
                                break;
                            default:
                                alert(res.ErrorMsg);
                           
                        }
                    }


                },
                error: function (xhr, type) {
                    alert("系统错误！");
                }
            });

            
        }
    };
});
