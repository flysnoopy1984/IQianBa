$(function () {
  var httpUrl = 'http://pp.iqianba.cn';
  var isGetCode = false;
  var countDown = 10;
  var Inter = "";
  /**
   * [返回]
   */
  historyBack = function() {
    window.history.back();
  };

  //监听电话号码的输入
  $('#phone_num').keypress(function() {
    var phone = $('#phone_num').val();
    if (phone !== '') {
      $('#phone_clear').css({'display': 'inline-block'});
      $('#get_phone_check').css({'borderBottom': '1px solid #000000'});
    }
  });
  /**
   * [清空电话号码]
   * @return {[type]} [description]
   */
  clearPhone = function() {
    $('#phone_num').val('');
    $('#phone_clear').css({'display': 'none'});
    $('#get_phone_check').css({'borderBottom': '1px solid #f1f1f1'});
  };
  /**
   * [countdown 计时器倒计时]
   * @return {[type]} [description]
   */
  function countdown() {
    if (countDown == 1) {
      clearInterval(Inter);
      $('.get_phone_btn').text('获取验证码');
    } else {
      countDown--;
      $('.get_phone_btn').text(countDown + 's');
    }
  };
  /**
   * 获取验证码
   * @return {[type]} [description]
   */
  getCodeNum = function() {
    var phone = $('#phone_num').val();
    if (phone === '') {
      return;
    };
    if (Inter) {
      return;
    }
    Inter = setInterval( countdown, 1000);
  };

  //监听验证码
  $('#code_num').keypress(function() {
    var code = $('#code_num').val();
    if (code !== '') {
      $('#code_clear').css({'display': 'inline-block'});
      $('#verify_phone_check').css({'borderBottom': '1px solid #000000'});
    }
  });
  /**
   * 清空验证码
   * @return {[type]} [description]
   */
  clearCode = function() {
    $('#code_num').val('');
    $('#code_clear').css({'display': 'none'});
    $('#verify_phone_check').css({'borderBottom': '1px solid #f1f1f1'});
  };

  /**
   * 确认验证码
   * @return {[type]} [description]
   */
  confirmCode = function() {
    var code = $('#code_num').val();
    if (code === '') {
      return;
    }
    // 验证通过可以开始购物
    $('#begin_order').removeClass('begin_order_disabled');
    $('#begin_order').addClass('begin_order_abled');
  };

  /**
   * [开始购物]
   * @return {[type]} [description]
   */
  beginOrder = function() {
    console.log('开始购物');
    window.location.href = "./shopList.html";
  };
});
