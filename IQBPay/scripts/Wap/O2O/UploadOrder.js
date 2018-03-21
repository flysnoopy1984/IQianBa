

$(function () {
    var httpUrl = 'http://pp.iqianba.cn';
    var OrderNo = '';
    var aoId = null;
    var OrderStatus = null;
    var IsPassAccount = false;
    var jqXHR = null;

    $(window).bind('beforeunload', function () {
        if(jqXHR!=null)
        {
            jqXHR.abort();
        }
    });
  
  /**
   * [删除已经上传的图片]
   * @return {[type]} [description]
   */
  deleteImage = function() {
      InitOterControl();
      this.InitUploadControl();
  };
  /**
   * [按钮模拟图片上传input]
   * @return {[type]} [description]
   */
  fileSelect = function() {
      $('#upload_OrderInfo').click();
  };
  /**
   * [选择上传的图片信息]
   * @param  {[type]} e [上传的图片信息]
   * @return {[type]}   [description]
   */
  fileSelected = function(e) {
    console.log(e);
  };
  /**
   * [提交]
   * @return {[type]} [description]
   */
  submitUpload = function () {

      var ReceiveAccount = $("#ReceiveAccount").val();
      if (ReceiveAccount == null || ReceiveAccount == "" || ReceiveAccount == undefined) {
          alert("请输入收款账户");
          $("#ReceiveAccount").focus();
          return;
      }
      if (!IsPassAccount) {
          alert("请点击按钮,先验证账户");
          return;
      }
      var imgUpload1 = $("#imgUpload1").attr("src");
      if (imgUpload1 == null || imgUpload1 == "" || imgUpload1 == undefined)
      {
          alert("请先上传图片");
          return;
      }
      

      var MallOrderNo = $("#order_num").val();

      var url = "/O2OWap/SubmitUpload";
      $.ajax({
          type: 'post',
          url: url,
          data: { "imgUpload1": imgUpload1, "MallOrderNo": MallOrderNo, "OrderNo": OrderNo, "ReceiveAccount": ReceiveAccount, "OrderStatus": OrderStatus },
          success: function (res) {
              if (res.IsSuccess) {
                  alert("提交成功！");
                  window.location.href = "/O2OWap/OrderDetail?aoId=" + aoId;
              }
              else {
                  if (res.IntMsg == -1) {
                      alert("订单编号未获取!");
                      window.location.href = "/O2OWap/OrderDetail?aoId=" + aoId;
                      return;
                  }
                  if (res.IntMsg == -2) {
                      alert("上传文件未获取，请联系管理员");                    
                      return;
                  }
                  if (res.IntMsg == -3) {
                      $("#ReceiveAccount").focus();
                      alert("收款账户未填写");
                      return;
                  }
                  if (res.IntMsg == -4) {
                      alert("订单状态不正确，无法上传图片");
                      return;
                  }
              }
          },
          error: function (xhr, type) {
              alert("系统错误！");
          }
      });
  };
  /**
   * [返回订单查询页]
   * @return {[type]} [description]
   */
  BackToOrderDetail = function() {
      window.history.back();
      
  };

  VerifyAccount = function () {
      var url = "/O2OWap/VerifyAliPayAccount";
      var AliPayAccount =$.trim($("#ReceiveAccount").val());
      if(AliPayAccount =="")
      {
          alert("收款账户不能为空");
          $("#ReceiveAccount").focus();
          return;
      }
      $.ajax({
          type: 'post',
          url: url,
          data: { "AliPayAccount": AliPayAccount },
          success: function (res) {
              if (res.IsSuccess) {
                  alert("验证通过");
                  $("#ReceiveAccount").attr("disabled", true);
                  $("#btnVerifyAccount").hide();
                  IsPassAccount = true;
              }
              else {
                  alert(res.ErrorMsg);
                  if (res.IntMsg == -1) {  
                      window.location.href = "/O2OWap/Index?aoId=" + aoId;
                      return;
                  }

              }
          },
          error: function (xhr, type) {
              alert("系统错误！");
          }
      });
      
  }

  InitData = function () {
      
     
      var url = "/O2OWap/UploadOrderQuery";
      $.ajax({
          type: 'post',
          url: url,
          data: { "OrderNo": OrderNo },
          success: function (res) {
              if (res.IsSuccess) {
                  if (res.resultObj!=null)
                  {
                      $("#imgContainer1").show();
                      $("#imgUpload1").attr({ src: res.resultObj.v1 });
                      $("#ReceiveAccount").val(res.resultObj.v2);
                      $("#btnUpload1").hide();
                  }
              }
              else {
                  if (res.IntMsg == -1) {
                      alert("订单编号未获取!");
                      window.location.href = "/O2OWap/OrderDetail?aoId=" + aoId;
                      return;
                  }
                 
              }
          },
          error: function (xhr, type) {
              alert("系统错误！");
          }
      });
  };


  InitOterControl = function () {

      $("#imgUpload1").attr({ src: "" });
      $("#imgContainer1").hide();
      $("#btnUpload1").show();

      //进度条
    //  $("upload_progress1").css("width", "0%");
    //  $("#ProcessArea1").hide();
     // $("#ProcessArea1").removeClass("active");

      //Settlement 等待到货结算之前,提交按钮显示
      if (OrderStatus < 14) {
          $("#btnDelImg").show();
          $("#btnSubmit").show();
      }

      else {
          $("#btnDelImg").hide();
          $("#btnSubmit").hide();
      }
         

      
  }

  Init = function () {

      aoId = GetUrlParam("aoId");
      if (aoId == "" || aoId == undefined)
      {
          window.location.href = "/O2OWap/ErrorPage?ec=1";
          return;
      }

      OrderNo = GetUrlParam("OrderNo");
      if (OrderNo == "" || OrderNo == undefined) {
          alert("订单未获取");
          window.location.href = "/O2OWap/OrderDetail?aoId=" + aoId;
          return;
      }
      OrderStatus = GetUrlParam("OrderStatus");
      //如果是管理员打开审核面板
      var IsAdmin = $("#IsAdmin").val();
      if (IsAdmin)
      {
          $("#ReviewArea").show();
          $("#O2ONo").val(OrderNo);
          $("#OrderStatus").val(OrderStatus);
      }
     
     

      InitOterControl();

      this.InitUploadControl();

      //2:WaitingUpload
      if (OrderStatus >2)
        this.InitData();
  }

    /*
    *初始化上传控件
    *
    */
  InitUploadControl = function () {
      var url = "/O2OWap/UploadOrderInfo";

      $("#upload_OrderInfo").fileupload({
          autoUpload: true,
          url: url,
          dataType: 'json',
          formData: { "OrderNo": OrderNo },
          allowedTypes: /(.|\/)(jpeg|png|jpg|bmp)$/i,
          showStop: true,
          maxFileSize: 10480,
          maxNumberOfFiles: 1,
          recalculateProgress: true,
          forceIframeTransport:true,
          done: function (e, data) {
              $.unblockUI();
              if(data.result.IsSuccess == true)
              {
                  $("#imgContainer1").show();
                  $("#imgUpload1").attr({ src: data.result.resultObj });
              }
              else
              {
                  switch(data.result.IntMsg)
                  {
                      case -1:
                          alert("文件过大"); break;
                      case -2:
                          alert("手机号未获取,请重新提交");
                          window.location.href = "/O2OWap/Index?aoId=" + aoId;
                          break;
                      case -3:
                          alert("文件格式不正确"); break;
                      case -4:
                          alert("订单编号未获取");
                          window.location.href = "/O2OWap/OrderDetail?aoId=" + aoId;
                          break;

                  }
                  InitOterControl();
              }

              //$("#uploadImg" + n).attr({ src: data.result.ImgSrc });
              //$("#uploadImg" + n).css({ width: "290px", height: "218px" });
              //IdCardFile1Done = true;
          },
          add: function (e, data) {

              //$("#Progress" + n).show();
            //  $("#ProcessArea1").show();
             // $("#ProcessArea1").addClass("active");

              $("#btnUpload1").hide();

              var msg = ' <div id="ProcessArea1" class="progress progress-striped active">';
              msg+='<div id="upload_progress1" class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 45%;">';
              msg+='<span class="sr-only">正在上传请耐心等待...</span>';
              msg+='</div>';
              msg+='</div>';
            
              ////   alert(data.files[0].name);
              $.blockUI({
                  message: msg,
                  css: {
                      border: 'none',
                      width: '90%',
                      height:'20px',
                      left: '20px',
                      'border-radius':'4px',
                  }
              });
              
              data.process().done(function () {
                  jqXHR = data.submit();
                  
              });

              //$("#cancel_IdCardFile" + n).click(function () {
              //    if (IdCardFile1Done) {
              //        InitUploadFile(n);
              //    }
              //    if (jqXHR)
              //        jqXHR.abort();

              //})
          },
          fail: function (e, data) {
              if (data.errorThrown == "abort") {
                  InitOterControl();
                  this.InitUploadControl();
               //   InitUploadFile(n);
              }
              $("#imgContainer1").hide();
              $("#btnUpload1").show();
              $.unblockUI();
          },


          progressall: function (e, data) {

              var progress = parseInt(data.loaded / data.total * 100, 10);
              $('#upload_progress1').css("width", progress + "%");
              

          },
        
              
      });
  }

  Init();

});
