$(function () {
    var httpUrl = 'http://pp.iqianba.cn';
    var OrderObj = null;
    var aoId = null;

    //var stepTmp = ' <div class="order_item">'
    //stepTmp += '<div class="{0}">';
    //stepTmp += '<div class="{1}">第一步</div>';
    //stepTmp +='<div class="{2}"></div>';
    //stepTmp +='</div>';

    //stepTmp +='<div class="{3}">';
    //stepTmp +='<div class="{4}">{6}</div>';
    //stepTmp +='<div class="{5}">{7}</div>';
    //stepTmp +='</div>';
    //stepTmp +='</div>';

  /**
   * [返回订单查询页]
   * @return {[type]} [description]
   */
  historyBack = function() {
    window.history.back();
  };

  InitOrder = function () {
      aoId = GetUrlParam("aoId");
      var url = "/O2OWap/OrderDetailQuery";
      $.ajax({
          type: 'post',
          url: url,
          success: function (res) {
              if (res.IsSuccess) {
                  if (res.resultList.length > 0) {
                      OrderObj = res.resultObj;
                   
                      $("#number").text(OrderObj.OrderAmount);
                      generateSteps(res.resultList);
                      switchControlByOrderStatus();
                  }
              }
              else {
                  if (res.IntMsg == -1) {
                      alert("用户信息未获取，请重新提交手机号");
                      window.location.href = "/O2OWap/Index";
                  }
                  else {
                      alert(res.ErrorMsg);
                  }
              }

          },
          error: function (xhr, type) {
              alert("系统错误！");
          }
      });
  };
   // 根据订单状态修改一些按钮或超链接状态
  switchControlByOrderStatus= function()
  {
      if (OrderObj.O2OOrderStatus == 45 || OrderObj.O2OOrderStatus == 50)
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
              //等待货物处理
              case 14:
                  if (step.Code == "StepReSelectItem" ||
                      step.Code == "StepUpload" ||
                      step.Code == "StepReviewOrder")
                      ctrl = step.EndContent;
                  else ctrl = step.BeginContent;

                  if (step.Code == "StepWaitingReceive")
                      ctrl = replaceStepToPrcess(ctrl,0);
                  break;
              //等待结算回款  
              case 18:
                  if (step.Code == "StepReSelectItem" ||
                      step.Code == "StepUpload" ||
                      step.Code == "StepReviewOrder" ||
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

  InitOrder();

   

  DoCloseOrder = function () {

      var url = "/O2OWap/OrderClose";
      $.ajax({
          type: 'post',
          data: { "O2ONo": OrderObj.O2ONo },
          url: url,
          success: function (res) {
              if (res.IsSuccess) {
                  alert("您将返回商品列表");
                  window.location.href="/O2OWap/MallList?aoId="+aoId;
                 
              }
              else {

              }

          },

          error: function (xhr, type) {
              alert("系统错误！");
          }
      });

  };

  CloseOrder = function (skipWarn) {

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
      if (OrderObj.O2OOrderStatus >= 2)
      {
          window.location.href = "/O2OWap/UploadOrder?aoId=" + aoId + "&OrderNo=" + OrderObj.O2ONo + "&OrderStatus=" + OrderObj.O2OOrderStatus;
      }
      else
      {
          alert("请完成上一步");
      }
    
  };

  reSelectItem=function()
  {
     
    //  window.open("http://m.baidu.com", "_blank");
      CloseOrder();
     // window.location.href = "/O2OWap/MallList?aoId=" + aoId;
  }

});
