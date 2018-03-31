$(function () {
    var httpUrl = 'http://pp.iqianba.cn';
    var OrderObj = null;
    var aoId = null;
    var O2ONo = null;
    var HasOrder = true;

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

    


  /**
   * [返回订单查询页]
   * @return {[type]} [description]
   */
  historyBack = function() {
    window.history.back();
  };

  backToHome = function () {
      window.location.href = "/O2OWap/Index?aoId=" + aoId;
  };


   // 根据订单状态修改一些按钮或超链接状态
  switchControlByOrderStatus= function()
  {
      if (OrderObj.O2OOrderStatus >10)
          $("#btnCloseOrder").hide();
  }

   //List：所有步骤
  generateSteps = function (list) {

      $.each(list, function (i) {
          var step = list[i];
          var ctrl = AnalyStepByOrderStatus(step);

          if (ctrl != null && ctrl != "null")
          {
              $("#order_list").append(ctrl);
              //如果驳回
              if (OrderObj.O2OOrderStatus == 5 && step.Code == "StepReviewOrder")
              {
                  $("#RejectArea").show();
                  var rt = $("#RejectArea").find("#Reason").text();
                  $("#RejectArea").find("#Reason").text(rt + OrderObj.RejectReason);
                  $("#PassArea").hide();
              }
              //如果用户关闭
              if (OrderObj.O2OOrderStatus == 45 && step.Code == "StepComplete") {
                 
                  $("#StepComplete").find(".step_desc_title").text("用户关闭");
                  $("#StepUpload").find("a").hide();
                 
              }

              //签收状态，需要显示签收按钮
              if (OrderObj.O2OOrderStatus == 10 && step.Code == "StepConfirmSign" ) {
                  $("#StepConfirmSign").find("#linkToSignConfirm").show();
              }
             

              //已经不能上传照片(>Settlement),不能重新选择商品
              if(OrderObj.O2OOrderStatus>=10 && (step.Code == "StepUpload" || step.Code =="StepReSelectItem"))
              {
                  $("#linkToupload").text("查看订单");
                  $("#LinkToSelItem").hide();

              }
             
          }
              
      });

  };

    //根据订单状态生成每个步骤
  AnalyStepByOrderStatus = function (step) {
      if(OrderObj!=null)
      {
          var ctrl = null;
          switch(OrderObj.O2OOrderStatus)
          {
              // 仅商品选择
              case 0:
                  ctrl = step.BeginContent;
                  if (step.Code == "StepReSelectItem")
                      ctrl = replaceStepToPrcess(ctrl,0);
                  break;
             //等待上传订单详情
              case 2:
                  if (step.Code == "StepReSelectItem") ctrl = step.EndContent;
                  else ctrl = step.BeginContent;
                  if (step.Code == "StepUpload")
                      ctrl = replaceStepToPrcess(ctrl,0);
                  break;
             //审核驳回
              case 5:
                  if (step.Code == "StepReSelectItem" || step.Code == "StepUpload" ||step.Code == "StepReviewOrder") ctrl = step.EndContent;
                  else ctrl = step.BeginContent;
                  if (step.Code == "StepReviewOrder")
                      ctrl = replaceStepToPrcess(ctrl,1);
                  break;
              //等待订单审核
              case 6:
                  if (step.Code == "StepReSelectItem" || step.Code == "StepUpload")
                      ctrl = step.EndContent;
                  else ctrl = step.BeginContent;

                  if (step.Code == "StepReviewOrder")
                      ctrl = replaceStepToPrcess(ctrl,0);

                  break;
              //签收确认
              case 10:
                  if (step.Code == "StepReSelectItem" ||
                      step.Code == "StepUpload" ||
                      step.Code == "StepReviewOrder")
                      ctrl = step.EndContent;
                  else ctrl = step.BeginContent;
                  if (step.Code == "StepConfirmSign")
                      ctrl = replaceStepToPrcess(ctrl,0);
                  break;
              //等待结算回款
              case 14:
              case 18:
                  if (step.Code == "StepReSelectItem" ||
                      step.Code == "StepUpload" ||
                      step.Code == "StepReviewOrder" ||
                      step.Code == "StepConfirmSign" ||
                      step.Code == "StepWaitingReceive")
                      ctrl = step.EndContent;
                  else ctrl = step.BeginContent;
                  if (step.Code == "StepPayment")
                      ctrl = replaceStepToPrcess(ctrl,0);
                  break;
              case 45:
                  ctrl = step.BeginContent;
                  if (step.Code == "StepComplete")
                      ctrl = step.EndContent;

                  break;

              //完成  
              case 50:
                  ctrl = step.EndContent;
                  break;
          }
          return ctrl;
          
      }
  };

  replaceStepToPrcess=function(step,bORe)
  {
      if (bORe == 0)
      {
          step = step.replace('step_time', 'step_time_current');
          step = step.replace('step_desc', 'step_desc_current');
      }
      else
      {
          step = step.replace('step_time_done', 'step_time_current');
          step = step.replace('step_desc_done', 'step_desc_current');
      }
     
      return step;
  }


  DoCloseOrder = function () {

      var url = "/O2OWap/OrderClose";
      $.ajax({
          type: 'post',
          data: { "O2ONo": OrderObj.O2ONo },
          url: url,
          success: function (res) {
              if (res.IsSuccess) {
                  alert("您将返回商品列表");
                  window.location.href = "/O2OWap/MallList?aoId=" + aoId;
              }
              else {
                  alert(res.ErrorMsg);
                  if (res.IntMsg == -1)//"订单未获取！请联系中介";
                      window.location.href = "/O2OWap/Index?aoId=" + aoId;
              }

          },

          error: function (xhr, type) {
              alert("系统错误！");
          }
      });

  };

  CloseOrder = function (skipWarn) {

      if (!HasOrder) {
          alert("没有订单！"); return;
      }
      $.confirm({
          type: "red",
          theme: 'material',
          title: '谨慎确认!',
          content: '如果您已经去商城支付，【关闭订单】/【重选商品】将无法收到回款，是否继续？',
          buttons: {

              cancel: {
                  text: '不了',
                  btnClass: 'btn-info',

              },
              confirm: {
                  btnClass: 'btn-danger',
                  text: '我很确定',
                  action: function () {
                      DoCloseOrder();
                  }
              }
          }
      });

  };

    /**
     * [按钮模拟图片上传input]
     * @return {[type]} [description]
     */
  upLoadOrder = function () {
      if (OrderObj.O2OOrderStatus >= 2) {
          window.location.href = "/O2OWap/UploadOrder?aoId=" + aoId + "&OrderNo=" + OrderObj.O2ONo + "&OrderStatus=" + OrderObj.O2OOrderStatus;
      }
      else {
          alert("请完成上一步");
      }

  };

  reSelectItem = function () {

      //  window.open("http://m.baidu.com", "_blank");
      CloseOrder();
      // window.location.href = "/O2OWap/MallList?aoId=" + aoId;
  }

  SignConfirm = function () {
      $.confirm({
          theme: "light",
          title: '签收确认',
          type: 'red',
          content: "注意请不要提前确认签收,否则将被处罚停单！",
          buttons: {
              know: {
                  text: "签收",
                  btnClass: "btn-red",
                  action: function () {
                      DoSignConfirm();
                  },
              },
              cancel: {
                  text: "再确认下",
              }

          }
      });
  }

  DoSignConfirm = function () {
      var url = "/O2OWap/OrderSignConfirm";
      $.ajax({
          type: 'post',
          data: { "O2ONo": OrderObj.O2ONo },
          url: url,
          success: function (res) {
              if (res.IsSuccess) {
                  alert("签收已确认，请等待回款结算");
                  window.location.reload();
              }
              else {
                  alert(res.ErrorMsg);
                  if (res.IntMsg == -1)//"订单未获取！请联系中介";
                      window.location.href = "/O2OWap/Index?aoId=" + aoId;
              }

          },

          error: function (xhr, type) {
              alert("系统错误！");
          }
      });
  }

    //初始化函数
  InitOrder = function () {
      aoId = GetUrlParam("aoId");
      if (aoId == "" || aoId == "null" || aoId == undefined) {
          window.location.href = "/O2OWap/ErrorPage?ec=1";
          return;
      }
      ShowBlock();

      O2ONo = GetUrlParam("O2ONo");

      var url = "/O2OWap/OrderDetailQuery";
      $.ajax({
          type: 'post',
          data: { "O2ONo": O2ONo },
          url: url,
          success: function (res) {
              if (res.IsSuccess) {
                  if (res.resultList.length > 0) {
                      OrderObj = res.resultObj;

                      $("#number").text(OrderObj.OrderAmount);
                      generateSteps(res.resultList);
                      switchControlByOrderStatus();
                  }
                  $.unblockUI();
              }
              else {
                  $.unblockUI();
                  if (res.IntMsg == -1) {
                      alert("用户信息未获取，请重新提交手机号");
                      window.location.href = "/O2OWap/Index?aoId=" + aoId;
                  }
                  else if (res.IntMsg == -2) {
                      $("#btnCloseOrder").hide();
                      HasOrder = false;
                      alert("您还没有订单，请先创建");
                      window.location.href = "/O2OWap/MallList?aoId=" + aoId;
                  }
                  else {
                      alert(res.ErrorMsg);
                  }
              }

          },
          error: function (xhr, type) {
              alert("系统错误！");
              $.unblockUI();
          }
      });
  };

  InitOrder();


});
