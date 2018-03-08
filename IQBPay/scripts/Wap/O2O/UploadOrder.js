$(function () {
    var httpUrl = 'http://pp.iqianba.cn';
    var OrderNo = '';
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
      var imgUpload1 = $("#imgUpload1").attr("src");
      if (imgUpload1 == null || imgUpload1 == "" || imgUpload1 == undefined)
      {
          alert("请先上传图片");
          return;
      }
      var ReceiveAccount = $("#ReceiveAccount").val();
      if (ReceiveAccount == null || ReceiveAccount == "" || ReceiveAccount == undefined) {
          alert("请输入收款账户");
          $("#ReceiveAccount").focus();
          return;
      }

      var url = "/O2OWap/SubmitUpload";
      $.ajax({
          type: 'post',
          url: url,
          data: { "imgUpload1": imgUpload1, "OrderNo": OrderNo, "ReceiveAccount": ReceiveAccount },
          success: function (res) {
              if (res.IsSuccess) {
                  alert("提交成功！");
                  window.location.href = "/O2OWap/OrderDetail";
              }
              else {
                  if (res.IntMsg == -1) {
                      alert("订单编号未获取!");
                      window.location.href = "/O2OWap/OrderDetail";
                      return;
                  }
                  if (res.IntMsg == -2) {
                      alert("上传文件未获取，请联系管理员");                    
                      return;
                  }
                  if (res.IntMsg == -3) {
                      alert("收款账户未填写");
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

  InitData = function () {
      OrderNo = GetUrlParam("OrderNo");

      if (OrderNo == "" || OrderNo == undefined) {
          alert("订单未获取");
          window.location.href = "/O2OWap/OrderDetail";
          return;
      }
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
                      window.location.href = "/O2OWap/OrderDetail";
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

      $("#imgUpload1").attr({ src:""});
      $("#imgContainer1").hide();
      $("#btnUpload1").show();
  }
  Init = function () {
      InitOterControl();

      this.InitUploadControl();

      this.InitData();
  }

    /*
    *初始化上传控件
    *
    */
  InitUploadControl = function () {
      var url = "/O2OWap/UploadOrderInfo";
      OrderNo = GetUrlParam("OrderNo");
    

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
          done: function (e, data) {
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
                          window.location.href = "/O2OWap/Index";
                          break;
                      case -3:
                          alert("文件格式不正确"); break;
                      case -4:
                          alert("订单编号未获取");
                          window.location.href = "/O2OWap/OrderDetail";
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
            
              $("#btnUpload1").hide();
              ////   alert(data.files[0].name);

              var jqXHR;
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
               //   InitUploadFile(n);
              }
              $("#imgContainer1").hide();
              $("#btnUpload1").show();
          },


          progressall: function (e, data) {

              //var progress = parseInt(data.loaded / data.total * 100, 10);
              //$('#ProcessBar' + n).css("width", progress + "%");
              //    $('.ProcessBar').text(progress + "%");

          },
        
              
      });
  }

  Init();

});
