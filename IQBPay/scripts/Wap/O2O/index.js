$(function () {

    var isGetCode = false;
    const InitCount = 10;
    var countDown = InitCount;
    var IsPassVerify = false;
    var popModel = null;
    var aoId = null;
    var HasOrder = false;
    var ck_IQB_O2OBuyerPhone = "IQB_O2OBuyerPhone";

    function  InitPage()
    {
        $("#ActionArea").hide();
        $("#get_phone_check").show();

        aoId = GetUrlParam("aoId");
        if (aoId == "" || aoId == "null" || aoId == undefined)
        {
            window.location.href = "/O2OWap/ErrorPage?ec=1";
            return;
        }
        var phone = $.cookie(ck_IQB_O2OBuyerPhone);
        if (phone != undefined && phone!="")
        {
            ShowActionArea(1);
        }
    }

InitPage();
  /**
   * [跳转到我要购物页面]
   */
orderPdt = function () {
     var url = "/O2OWap/HasBuyerOrder";
     $.ajax({
         type: 'post', 
         url: url,
         success: function (data) {
             if (data == true) {
                 alert("您有未完成的订单，请先处理");
                 lookOrderList();
             }
             else
                 window.location.href = "/O2OWap/MallList?aoId=" + aoId;
         },
         error: function (xhr, type) {
             alert("error");
         }
     });
    
  };

  /**
   * [跳转到我要查看我的订单页面]
   */
  lookOrderList = function() {
      window.location.href = "/O2OWap/OrderDetail?aoId=" + aoId;
  };

  $('#phone_num').keypress(function () {
      var phone = $('#phone_num').val();
      if (phone !== '') {
          $('#phone_clear').css({ 'display': 'inline-block' });
          //   $('#get_phone_check').css({ 'borderBottom': '1px solid #000000' });
      }
      else {
          $('#phone_clear').css({ 'display': 'none' });
      }
  });
    /**
     * [清空电话号码]
     * @return {[type]} [description]
     */
  clearPhone = function () {
      $('#phone_num').val('');
      $('#phone_clear').css({ 'display': 'none' });
   //   $('#get_phone_check').css({ 'borderBottom': '1px solid #f1f1f1' });
  };

    // 验证手机号
  function isPhoneNo(phone) {
      var pattern = /^1[34578]\d{9}$/;
      return pattern.test(phone);
  }

  //0新用户
  function ShowActionArea(userStatus)
  {
      $("#get_phone_check").hide();
      $("#ActionArea").show();
      if(userStatus ==0)
      {
          $("#o2o_btn__look").attr("disabled",true);
      }
  }
  function CloseSMSModel()
  {
      popModel.close();
      if(IsPassVerify)
      {
          ShowActionArea(0);  
      }
      else
      {
       
          $("#get_phone_check").show();
          $("#ActionArea").hide();
      }
  }


  function GetPopHtml()
  {
      var h = '<div id="SMSVerify" class="o2o_modal">';
      h += '<div class="o2o_modal_content">';
      h += '<div class="o2o_modal_content_body">';
      h += '<div class="o2o_modal_content_container">';
      h += '<label id="phone_Verify">{0}</label>';
      h += '<button id="btnVerifyCode" type="button" class="btn btn-warning" onclick="RequireVerifyCode();">获取验证码</button>';
      h += ' <input id="VerifyCode" type="text" placeholder="验证码" class="form-control" />';
      h += '<button id="btnSubmitCode" type="button" class="btn btn-primary" onclick="SubmitVerifyCode();">确定</button>';
      h += '</div>';
      h += '</div>';
      h += '</div>';
      h += '</div>';
      return h;
  }

  function ShowSMSModel()
  {
      //   $('.o2o_modal').css("display", "flex");
      var phone = $("#phone_num").val();
     // $("#phone_Verify").text(phone);
      var html = GetPopHtml();// $('.o2o_modal').html();

      html = String.format(html, phone);
      
      popModel = $.confirm({
          theme: "modern",
          title: '请先验证手机',
          type: 'red',
          content: html,
          buttons: {
              cancel: {
                  btnClass: 'o2o_modal_content_btn',
                  text: "关闭",
                  action: function () {
                      CloseSMSModel();
                  },
                 
              },

          }
      });
  }

  function AddNewBuyer(Phone)
  {
      var url = "/O2OWap/NewBuyer";
      $.ajax({
          type: 'post',
          dataType: "json",
          data: { "Phone": Phone},
          url: url,
          success: function (data) {
              if (data.IsSuccess) {
                  alert("Ok");

              }
              else
                  alert(data.ErrorMsg);
          },
          error: function (xhr, type) {
              alert("error");
          }
      });
  }

    //提交验证码
  SubmitVerifyCode =function(){
      var Phone = $("#phone_Verify").text();
      if (Phone == '') {
          return;
      }
      var Code = $("#VerifyCode").val();

      var url = "/API/PPSMS/IQBPay_ConfirmVerifyCode";

      

      $.ajax({
          type: 'get',
          dataType: "json",
          data: { "mobilePhone": Phone, "Code": Code},
          url: url,
          success: function (data) {
              switch (data.SMSVerifyStatus) 
              {
                  case -1:
                      alert("验证码未知错误，请联系管理员");
                      break;
                  case 5:
                      alert("验证码已失效，请重新获取！");
                      break;
                  case 3:
                      //验证成功
                      IsPassVerify = true;
                      $.cookie('IQB_O2OBuyerPhone', Phone);
                      AddNewBuyer(Phone);
                      CloseSMSModel();
                      break;
                  case 4:
                      alert("验证码不正确，请重新填写！");
                      break;

              }
 

          },
          error: function (xhr, type) {

              alert(xhr.responseText);

          }
      });
  }
    //请求验证码
  RequireVerifyCode = function () {
    //  var bn = $("#btnVerifyCode");
      var Phone = $("#phone_Verify").text();
      if (Phone == '') {
          return;
      }


      var url = "/API/PPSMS/SentSMS_IQBPay_BuyerOrder";

      $.ajax({
          type: 'get',
          dataType: "json",
          data: { "mobilePhone": Phone, "IntervalSec": countDown,"SMSEvent":200 },
          url: url,
          success: function (data) {
              if (data.SMSVerifyStatus == 2) {
                  settime();

              }
              else if (data.SMSVerifyStatus == 1) {

                  alert("请不要重复发送");
                  countDown = data.RemainSec;
                  settime();

              }
              else
              {

              }
              
          },
          error: function (xhr, type) {

              alert(xhr.responseText);

          }
      });
  }

  LoginByPhone = function () {
      var Phone = $('#phone_num').val();
      if(isPhoneNo(Phone))
      {
          //检查是用户信息是否存在，检查是否有订单
        //  $("#btnLogin").hide();

          var url = "/O2OWap/Login";
          $.ajax({
              type: 'post',
              dataType: "json",
              data: { "Phone": Phone },
              url: url,
              success: function (data) {
                  if (data.IsSuccess) {
                      switch(data.IntMsg)
                      {
                          case 0:
                              ShowSMSModel();
                              break;
                          case 2:
                              HasOrder = true;
                          default:
                            //  $.cookie('IQB_O2OBuyerPhone', Phone); 后台已经写入
                              ShowActionArea(1);
                              
                      }
                  }
                  else {
                      alert(data.ErrorMsg);
                  }
              },
              error: function (xhr, type) {

                  alert(xhr.responseText);

              }
          });

      }
      else
      {
          alert("请正确填写手机号");
          $('#phone_num').focus();
      }
  }

    //切换用户
  switchUser = function () {
      $.cookie(ck_IQB_O2OBuyerPhone, '');
      window.location.href = "/O2OWap/Index?aoId=" + aoId+"&act=switchUser";
  }

    /**
  * [countdown 计时器倒计时]
  * @return {[type]} [description]
  */

  function settime() {
      if (countDown == 0) {
          countDown = InitCount;
          $("#btnVerifyCode").text('获取验证码');
          $("#btnVerifyCode").attr("disabled", false);
          //var html = $('.o2o_modal').html();
          //popModel.setContent(html);
          return;
      } else {
          countDown--;
        
          $("#btnVerifyCode").attr("disabled", true);
          $("#btnVerifyCode").text("重新获取("+countDown + ')s');
          //var html = $('.o2o_modal').html();
          //popModel.setContent(html);

      }
      setTimeout(function () {
          settime();
      }, 1000)
  }

    // 阻止时间冒泡
  cancelBubble = function (e) {
      e.stopPropagation();
      e.preventDefault();
  }

});
