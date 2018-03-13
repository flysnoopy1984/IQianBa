$(function () {
  var httpUrl = 'http://pp.iqianba.cn';
  var selectedId = '';
  var selectedName = '';
  var aoId = null;

  /**
   * [返回]
   */
  backToHome = function () {
      window.location.href = "/O2OWap/Index?aoId=" + aoId;
  };

  // 获取商城列表
  function getShopList() {

      aoId = GetUrlParam("aoId");
     
      if (aoId == "" || aoId == "null" || aoId == undefined) {
          window.location.href = "/O2OWap/ErrorPage?ec=1";
          return;
      }
      var url = "/O2OWap/QueryMallList";
      $.ajax({
          type: 'post',
          url: url,
          success: function (res) {
              var src = "";
             
              $.each(res, function (i) {
                  var Id = res[i].Code + "_" + res[i].Description + "_" + res[i].Id + "_shop_list_item";
                  src += "<div class='shop_list_item' id='" + Id + "'>";
                  src += "<div class='shop_list_item_logo'> <img src='" + res[i].MallLogoPath + "' /></div>";
                  src += "<div class='shop_list_item_name'>" + res[i].Description + "</div>";
                  src += "</div>";
              });
             
              $('#shop_list').html(src);
            

          },
          error: function (xhr, type) {
              alert("系统错误！");
          }
      });

  };
  // 获取商城列表
  getShopList();

  /**
   * [选择商店]
   * @return {[type]} [description]
   */
  selectedShop = function(shop) {
    console.log(shop);
  };


  //进入商城
  $(document).on("click",".shop_list_item",function(e){
    var code = e.currentTarget.id.split('_')[0];
    var name = e.currentTarget.id.split('_')[1];
    var id = e.currentTarget.id.split('_')[2];
  //  $('.o2o_modal').css("display", "flex");
    $('#header').text(name);
    selectedName = name;
    selectedId = id;
   
    login();
   
  });

  /**
   * [商城登录]
   * @return {[type]} [description]
   */
  login = function() {
    //var name = $('#name').val();
    //var password = $('#password').val();
    //console.log('name: ' + name);
    //console.log('password: ' + password);
      window.location.href = '/O2OWap/ItemList?name=' + selectedName + '&id=' + selectedId + '&aoId=' + aoId;
  };

  /**
   * [关闭模态框]
   * @return {[type]} [description]
   */
  closeModal = function() {
    $('.o2o_modal').css("display", "none");
    $('#name').val('');
    $('#password').val('');
    selectedName = '';
    selectedId = '';
  };

  //  // 阻止时间冒泡
  //cancelBubble = function (e) {
  //    e.stopPropagation();
  //    e.preventDefault();
  //}
});
