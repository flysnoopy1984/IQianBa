$(function () {
 
    var OrderNo = '';
    //var aoId = null;
    var OrderStatus = null;
    var osDes = null;
    var IsPassAccount = false;
    var jqXHR = null;
    var MallCode;

    $(window).bind('beforeunload', function () {
        if(jqXHR!=null)
        {
            jqXHR.abort();
        }
    });

    Init = function () {


        OrderNo = GetUrlParam("OrderNo");
        if (OrderNo == "" || OrderNo == undefined) {
            alert("订单未获取");

            toPage("/O2OWap/OrderDetail");

            return;
        }

   
        this.InitData();

        InitNewUpload();

       
    }


  
  /**
   * [删除已经上传的图片]
   * @return {[type]} [description]
   */
    deleteImage = function () {

        //$("#imgUpload1").attr({ src: "" });
        //$("#imgContainer1").hide();
        //$("#btnUpload1").show();


        //Settlement 等待到货结算之前,提交按钮显示
        //if (OrderStatus < 8) {
        //    $("#btnDelImg").show();
        //    $("#btnSubmit").show();
        //}

        //else {
        //    $("#btnDelImg").hide();
        //    $("#btnSubmit").hide();
        //}
     
     // this.InitUploadControl();
  };
  /**
   * [按钮模拟图片上传input]
   * @return {[type]} [description]
   */
  fileSelect = function() {
      $('#upload_OrderInfo').click();
  };
 
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
      var MallAccount = $("#MallLoginName").val();
      if (MallAccount == null || MallAccount == "" || MallAccount == undefined) {
          alert("请输入"+$("#lb_MallLoginName").text());
          $("#MallAccount").focus();
          return;
      }
      var MallPwd = $("#MallLoginPwd").val();
      if (MallPwd == null || MallPwd == "" || MallPwd == undefined) {
          alert("请输入" + $("#lb_MallLoginPwd ").text());
          $("#MallLoginPwd").focus();
          return;
      }

      var UserPhone = $("#UserPhone").val();
      if (UserPhone == null || UserPhone == "" || UserPhone == undefined) {
          alert("请输入" + $("#lb_UserPhone").text());
          $("#UserPhone").focus();
          return;
      }
   

      //改为让审核的人上传订单
      //var imgUpload1 = $("#imgUpload1").attr("src");
      //if (imgUpload1 == null || imgUpload1 == "" || imgUpload1 == undefined)
      //{
      //    alert("请先上传图片");
      //    return;
      //}
      StartBlockUI("数据处理中...",100);

    //  var MallOrderNo = $("#MallOrderNo").val();

      var url = "/O2OWap/SubmitUpload";
      $.ajax({
          type: 'post',
          url: url,
          data: {
              "imgUpload1": "",
              "OrderNo": OrderNo,
              "ReceiveAccount": ReceiveAccount,
              "OrderStatus": OrderStatus,
              "MallAccount": MallAccount,
              "MallPwd": MallPwd,
              "UserPhone": UserPhone,
          },
          success: function (res) {
              $.unblockUI();
              if (res.IsSuccess) {
                  alert("提交成功！");

                  toPage("/O2OWap/OrderDetail?O2ONo=" + OrderNo);
               //   window.location.href = "?aoId=" + aoId + "&O2ONo=" + OrderNo;
              }
              else {
                  if (res.IntMsg == -1) {
                      alert("订单编号未获取!");
                      toPage("/O2OWap/OrderDetail?O2ONo=" + OrderNo);
                     // window.location.href = "/O2OWap/OrderDetail?aoId=" + aoId + "&O2ONo=" + OrderNo;
                      return;
                  }
                  else if (res.IntMsg == -2) {
                      alert("上传文件未获取，请联系管理员");                    
                      return;
                  }
                  else if (res.IntMsg == -3) {
                      $("#ReceiveAccount").focus();
                      alert("收款账户未填写");
                      return;
                  }
                  else if (res.IntMsg == -4) {
                      alert("订单状态不正确，无法上传图片");
                      return;
                  }
                  else {
                      alert(res.ErrorMsg);
                  }
              }
          },
          error: function (xhr, type) {
              $.unblockUI();
              alert("系统错误！");
          }
      });
  };
 
  BackToOrderDetail = function() {
      window.history.back();
      
  };
  StartBlockUI = function (txt, w) {

      if (w == undefined)
          w = 45;
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

  VerifyAccount = function () {
      var url = "/O2OWap/VerifyAliPayAccount";
      var AliPayAccount =$.trim($("#ReceiveAccount").val());
      if(AliPayAccount =="")
      {
          alert("收款账户不能为空");
          $("#ReceiveAccount").focus();
          return;
      }
      StartBlockUI("验证账户中，请等待...", 100);
      $.ajax({
          type: 'post',
          url: url,
          data: { "AliPayAccount": AliPayAccount },
          success: function (res) {
              $.unblockUI();

              if (res.IsSuccess) {
                  alert("验证通过");
                  $("#ReceiveAccount").attr("disabled", true);
                  $("#btnVerifyAccount").hide();
                  IsPassAccount = true;
              }
              else {
                  alert(res.ErrorMsg);
                  if (res.IntMsg == -1) {
                      toPage("/O2OWap/Index");
                   //   window.location.href = "/O2OWap/Index?aoId=" + aoId;
                      return;
                  }

              }
          },
          error: function (xhr, type) {
              $.unblockUI();

              alert("系统错误！");
          }
      });
      
  }

  InitData = function () {
      
      StartBlockUI("数据加载中，请等待...", 100);
      var url = "/O2OWap/UploadOrderQuery";
      $.ajax({
          type: 'post',
          url: url,
          data: { "OrderNo": OrderNo },
          success: function (res) {
              $.unblockUI();
              if (res.IsSuccess) {
                  if (res.resultObj!=null)
                  {
                     
                      if (res.resultObj.OrderImgUrl != null && res.resultObj.OrderImgUrl != "")
                      {
                          $("#imgContainer1").show();
                          $("#imgUpload1").attr({ src: res.resultObj.OrderImgUrl });
                          $("#imgUpload1").show();
                          $("#btnUpload1").hide();
                         
                      }
                      else
                      {
                          $("#btnDelImg").hide();
                      }
                    
                      if (res.resultObj.UserAliPayAccount != "" && res.resultObj.UserAliPayAccount != "null" && res.resultObj.UserAliPayAccount != null)
                      {
                          $("#ReceiveAccount").val(res.resultObj.UserAliPayAccount);
                          IsPassAccount = true;
                      }
                     
                      $("#MallOrderNo").val(res.resultObj.MallOrderNo);
                      $("#OrderAmount").val(res.resultObj.OrderAmount);

                      $("#MallLoginName").val(res.resultObj.MallAccount);
                      $("#MallLoginPwd").val(res.resultObj.MallPwd);

                      $("#UserPhone").val(res.resultObj.UserPhone);
                      $("#aUserPhone").attr("href", "tel:" + res.resultObj.UserPhone);

                      $("#AgentPhone").val(res.resultObj.AgentPhone);
                      $("#aAgentPhone").attr("href", "tel:" + res.resultObj.AgentPhone);
                      
                      OrderStatus = res.resultObj.O2OOrderStatus;
                      osDes = res.resultObj.O2OOrderStatusStr;
                      MallCode = res.resultObj.MallCode;

                      InitOterControl();
                  }
                  else
                  {
                      $("#btnDelImg").hide();
                  }
              }
              else {
                  if (res.IntMsg == -1) {
                      alert("订单编号未获取!");
                      toPage("/O2OWap/OrderDetail");
                    //  window.location.href = "/O2OWap/OrderDetail?aoId=" + aoId;
                      return;
                  }
                  else
                  {
                      alert(res.ErrorMsg);
                  }
                 
              }
          },
          error: function (xhr, type) {
              $.unblockUI();
              alert("系统错误！");
          }
      });
  };


  InitOterControl = function () {

      //$("#imgUpload1").attr({ src: "" });
      //$("#imgContainer1").hide();
      //$("#btnUpload1").show();

    
      //Settlement 等待到货结算之前,提交按钮显示
      if (OrderStatus < 8) {
        //  $("#btnDelImg").show();
          $("#btnSubmit").show();
      }

      else {
       //   $("#btnDelImg").hide();
          $("#btnSubmit").hide();
      }
      var Name = "商城";

      //商城名动态显示
      switch (MallCode) {
          case "Tmall":
              Name = "天猫";
              break;
          case "JD":
              Name = "京东";
              break;
          case "GM":
              Name = "国美";
              break;
          case "HW":
              Name = "华为";
              break;
          case "Sun":
              Name = "苏宁";
              break;
          default:
              Name = "商城";
      }
      $("#lb_MallLoginName").text(Name + "登陆用户名");
      $("#lb_MallLoginPwd").text(Name + "登陆密码");

      var IsAdmin = $("#IsAdmin").val();

      if (IsAdmin) {
          $("#ReviewArea").show();
          $("#O2ONo").val(OrderNo);
          $("#OrderStatus").val(osDes);
      }

  }



  InitNewUpload = function () {
      var uploading = false;
      var url = "/O2OWap/UploadOrderInfo";

     
      $("#upload_OrderInfo").on("change", function () {
          
          var formData = new FormData();
        
          formData.append("file", $("#upload_OrderInfo")[0].files[0]);
          formData.append("OrderNo", OrderNo);

          //if (uploading) {
          //    alert("文件正在上传中，请稍候");
          //    return false;
          //}

          StartBlockUI("正在上传请耐心等待...",100);

          $.ajax({
              url: url,
              type: 'POST',
              cache: false, 
              data: formData,
              processData: false,
              contentType: false,
              dataType: "json",
              beforeSend: function () {
                  uploading = true;
              },
              success: function (data) {
                  $.unblockUI();
                  if (data.IsSuccess == true) {
                      $("#imgContainer1").show();
                      $("#imgUpload1").attr({ src: data.resultObj });
                      $("#imgUpload1").show();
                      $("#btnUpload1").hide();
                      $("#btnDelImg").show();
                  }
                  else {
                      switch (data.IntMsg) {
                          case -1:
                              alert("文件过大"); break;
                          case -2:
                              alert("手机号未获取,请重新提交");
                              toPage("/O2OWap/Index");
                            //  window.location.href = "/O2OWap/Index?aoId=" + aoId;
                              break;
                          case -3:
                              alert("文件格式不正确"); break;
                          case -4:
                              alert("订单编号未获取");
                              toPage("/O2OWap/OrderDetail");
                           //   window.location.href = "/O2OWap/OrderDetail?aoId=" + aoId;
                              break;

                      }
                      InitOterControl();
                  }
                
              },
              error: function (xhr, type) {
                  $.unblockUI();
              alert("系统错误！");
          }
          });
      });
  }


  Init();

});
