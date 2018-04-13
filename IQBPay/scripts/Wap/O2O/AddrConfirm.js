$(function () {
    var httpUrl = 'http://pp.iqianba.cn';
    var noticeList = [];
    var ItemId = null;
    var AddrId = null;
    var MallId = null;//接收的是shopName
    var shopName = null;
    var amt = null;
    ///出货商Id
    var OpenId = null;
    var RealAddr = null;
    var ReceiveAddress = "";
    //var aoId = null;
    /**
     * [返回]
     */
    backToHome = function () {
        toPage("/O2OWap/Index");
      
    };
    backPage = function () {
        history.back();
    };

    StartBlockUI = function (txt, w) {

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
                width: '90%',
                height: '20px',
                left: '20px',
                'border-radius': '4px',
            }
        });
    }

    Init = function()
    {
        ItemId = GetUrlParam("ItemId");
        MallId = GetUrlParam("MallId");
        OpenId = GetUrlParam("OpenId");
        aoId = GetUrlParam("aoId");
        shopName =GetUrlParam("shopName",true);
        amt = GetUrlParam("amt");

       
        if (ItemId == null || ItemId == undefined || MallId == null || MallId == undefined || OpenId == null || OpenId == undefined)
        {
            alert("未获取指定的商品，请选择商品");
            toPage("/O2OWap/MallList");
           
            return;
        }
        if (OpenId == null || OpenId == undefined) {
            alert("商品供应商未获取，请联系管理员");
            toPage("/O2OWap/Index");
            
            return;
        }
          
        var url = "/O2OWap/InitAddr";

        $.ajax({
            type: 'post',
            url: url,
            data: { "ItemId": ItemId, "MallId": MallId, "OpenId": OpenId },
            success: function (res) {
                if (res.IsSuccess)
                {
                    noticeList = [];
                    var p = String.format("请确认【{0}】的订单，金额为【{1}】，若不是，请返回重新选择", shopName, amt);
                    noticeList.push(p);
                    p = "点击【前往购物】按钮,若没有跳转请复制链接到浏览器中打开";
                    noticeList.push(p);
                    p = "请务必填写以下收货地址:</br>" + res.resultObj.ReceiveAddress;
                    noticeList.push(p);
                    p = "下单完成切记上传订单信息";
                    noticeList.push(p);

                    ReceiveAddress = res.resultObj.ReceiveAddress;
                    AddrId = res.resultObj.Id;
                    RealAddr = res.resultObj.ItemRealAddr;
                    InitNoticeList();
                }
                else
                {
                    if (res.IntMsg == -1) {
                        alert("未获取用户信息，需重新提交手机号");
                        toPage("/O2OWap/Index");
                      
                        return;
                    }
                    else if (res.IntMsg == -2) {
                        alert("此商品收货地址没有设置，请联系管理员");
                        toPage("/O2OWap/Index");
                      
                        return;
                    }
                    else
                        alert(res.ErrorMsg);

                }
             

            },
            error: function (xhr, type) {
                alert("系统错误！");
            }
        });
    }

    //复制到剪贴板
    copyToClipboard = function (Id) {
        
        var clipboard = new ClipboardJS("#" + Id, {
            text: function (trigger) {
                return ReceiveAddress;
            }
        });


    };
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
            if (i == 0) //带返回按钮
            {
                str += '<div class="o2o_notice_btn">';
                str += '<button class="btn btn-success" onclick="backPage();" style="margin-right:40px;">返回</button>';
                str +='<button class="btn btn-danger" id="' + i + '_o2o_notice_btn">确定</button></div>';
            }
            else if (i == 2) //复制到剪贴板
            {
                str += '<div class="o2o_notice_btn"><button class="btn btn-danger copy" id="' + i + '_o2o_notice_btn" data-clipboard-text="' + ReceiveAddress + '">点击复制内容</button></div>';
            }
            else
                str += '<div class="o2o_notice_btn"><button class="btn btn-success" id="' + i + '_o2o_notice_btn">我知道了</button></div>';
            str += '<div class="gap_border"></div>';
            str += '</div>';
        }
        $('.o2o_notice_list').html(str);


        $('.o2o_notice_btn > button').click(function (e) {

            var idx = e.currentTarget.id.split('_')[0];
            if (idx == 2)
            {
                var clipboard = new ClipboardJS('.copy');

            }

            var next = Number(idx) + 1;
            if (next >= noticeList.length) {
                $('.confirm_btn > button').css('backgroundColor', '#ff4b59');
                $('.confirm_btn > button').attr('disabled', false);
            } else {
                $("#o2o_notice_" + next).slideDown("slow");
            }

            $(document).scrollTop($(document).outerHeight(true));   

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
            toPage("/O2OWap/MallList");
         
        }
        else
        {
            this.StartBlockUI("数据处理,正在跳转商城,请稍等...");
            var url = "/O2OWap/CreateO2OOrder";

            $.ajax({
                type: 'post',
                url: url,
                data: { "ItemId": ItemId, "AddrId": AddrId, "aoId": aoId,"un":un },
                success: function (res) {
                    $.unblockUI();
                    if (res.IsSuccess) {
                        window.location.href = RealAddr;
                       // window.open(RealAddr, "menubar=0,scrollbars=1, resizable=1,status=1,titlebar=0,toolbar=0,location=1");
                    }
                    else {
                        switch(res.IntMsg)
                        {
                            //手机号为空，重新登陆
                            case -1:
                                alert("手机号未获取，请返回首页重新操作");
                                toPage("/O2OWap/Index");
                               
                                break;
                            case -2:
                                alert("商品未获取，请返回重新选择商品");
                                toPage("/O2OWap/MallList");
                             
                                break;
                            case -3:
                                alert("收货地址未获取，请联系管理员");
                              //  window.location.href = "O2OWap/MallList";
                                break;
                            case -4:
                                alert("中介信息未获取，可能等待时间过长，请返回首页重新操作");
                                toPage("/O2OWap/ErrorPage?ec=1");
                             
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
                                toPage("/O2OWap/ErrorPage?ec=40001");
                                break;
                            case -10:
                                window.location.href = RealAddr;
                                break;
                            //Session失效
                            case -11:
                                toPage("/O2OWap/ErrorPage?ec=2");
                                break;
                          
                            default:
                                alert(res.ErrorMsg);
                           
                        }
                    }
                },
                error: function (xhr, type) {
                    $.unblockUI();
                    alert("系统错误！");
                }
            });

            
        }
    };
});
