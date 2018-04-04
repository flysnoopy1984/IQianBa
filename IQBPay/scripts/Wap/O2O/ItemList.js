$(function () {
  var httpUrl = 'http://pp.iqianba.cn';
  var shopName = ''; //商城名称
  var shopId = ''; // 商城价格
  var pdts = [];
  var pdtIdx = 0; // 选中的商品Idx
  var aoId = null;

  /**
   * [返回]
   */
  backToHome = function () {
      window.location.href = "/O2OWap/Index?aoId=" + aoId;
  };

  function GetRequest(key) {
    // 获取参数
     var url = window.location.search;
     // 正则筛选地址栏
     var reg = new RegExp("(^|&)"+ key +"=([^&]*)(&|$)");
     // 匹配目标参数
     var result = url.substr(1).match(reg);
     //返回参数值
     return result ? decodeURIComponent(result[2]) : null;
  };
  // 获取价格列表
  function getPriceList(MallCode) {

      var url = "/O2OWap/QueryPriceGrouplist";
     
      $.ajax({
          type: 'post',
          data: { 'MallCode': MallCode },
          url: url,
          success: function (res) {
              var str = "";
              for (var i = 0; i < res.length; i++) {
                  str += '<div class="price_item" id="' + res[i].Id + '_' + res[i].Code + '_price_item"> ' + res[i].Code + '</div>'
              };
              $('.price_list').html(str);
              // 默认第一个价格选中
              if (res && res.length > 0) {
                 // console.log(res.length);
                  $('#' + res[0].Id + '_' + res[0].Code + '_price_item').addClass('price_active');
              }
              $('#header-title').text(res[0].Code);
              pdtList(MallCode, res[0].Id);
          },
          error: function (xhr, type) {
              alert("系统错误！");
          }
      });

  };

  // 获取商品列表
  /**
   * [pdtList 获取商品列表]
   * @param  {[type]} MallId [商城Id]
   * @param  {[type]} PGId   [价格Id]
   * @return {[type]}        [description]
   */
  function pdtList(MallCode, PGId) {
      $('.pdt_list_content').empty();
      var url = "/O2OWap/QueryItemList";
      $.ajax({
          type: 'post',
          // data: "MallId=" + MallId + "&PGId=" + PGId,
          data: { 'MallCode': MallCode, 'PGId': PGId },
          url: url,
          success: function (res) {
              pdts = res;
              var itemStr = '';
              for (var p = 0; p < res.length; p++) {
                  itemStr += '<div id="' + res[p].Id + '_item_' + res[p].Amount + '" class="pdt_list_content_item">';
                  itemStr += '<input id="OpenId" type="hidden" value="' + res[p].OpenId + '" />';
                  if (res[p].ImgUrl != "" && res[p].ImgUrl != null && res[p].ImgUrl != "null")
                    itemStr += '<img width="175" height="175" src="' + res[p].ImgUrl + '"></img>';
                  itemStr += '<div class="pdt-price"><span class="price_symbal">￥</span><span class="price">' + res[p].Amount + '</span></div>';
                  itemStr += ' <div class="pdt-label">' + res[p].Name + '</div>';
                  itemStr += '<div class="pdt-tag">';
                  // <div class="pdt-tag-label">直降</div>
                  // <div class="pdt-tag-label">包邮</div>
                  itemStr += '</div></div>'
              }
              $('.pdt_list_content').html(itemStr);
          },
          error: function (xhr, type) {
              alert("系统错误！");
          }
      });

   
  };

  // 获取商城数据
  function getShopData() {
    shopName = GetRequest('name');
    shopId = GetRequest('id');
    $('.header_title > span').text(shopName);

    aoId = GetUrlParam("aoId");
    if (aoId == "" || aoId == "null" || aoId == undefined) {
        window.location.href = "/O2OWap/ErrorPage?ec=1";
        return;
    }

    // 获取价格列表
    getPriceList(shopId);
  };
  getShopData();


  //选择价格
  $(document).on("click",".price_item",function(e){
      $('.price_item').removeClass('price_active');

    var id = e.currentTarget.id.split('_')[0];
    var code = e.currentTarget.id.split('_')[1];
    $('#' + id + '_'+ code + '_price_item').addClass('price_active');
    $('#header-title').text(code);
    pdtList(shopId, id);
  });

  // 点击商品
  $(document).on("click", ".pdt_list_content_item", function(e) {
      var idx = e.currentTarget.id.split('_')[0];
      var amt = e.currentTarget.id.split('_')[2];
      //var OpenId = e.currentTarget.id.split('_')[2];
      var OpenId = $(e.currentTarget).find("#OpenId").val();
    //pdtIdx = idx;
    //$('.o2o_modal').css("display", "flex");
    //var modalPdt = '';
    //modalPdt += '<div class="pdt_list_modal_item">';
    ////modalPdt += '<img width="175" height="175" src="'+pdts[idx].ImgUrl+'"></img>';
    //modalPdt += '<div class="pdt-price"><span class="price_symbal">￥</span><span class="price">'+pdts[idx].Amount+'</span></div>';
    //modalPdt += ' <div class="pdt-label">'+pdts[idx].Name+'</div>';
    //modalPdt += '<div class="pdt-tag">';
    //    // <div class="pdt-tag-label">直降</div>
    //    // <div class="pdt-tag-label">包邮</div>
    //modalPdt += '</div></div>';
      //$('.o2o_modal_content_container').html(modalPdt);

      confirm(idx, OpenId, amt);
  });

  confirm = function (ItemId, OpenId, amt) {
      window.location.href = '/O2OWap/AddrConfirm?ItemId=' + ItemId + '&OpenId=' + OpenId + '&shopName=' + encodeURI(shopName) + '&MallId=' + shopId + '&amt=' + amt + '&aoId=' + aoId;
  }

  /**
   * [关闭模态框]
   * @return {[type]} [description]
   */
  closeModal = function() {
    $('.o2o_modal').css("display", "none");
  };
});
