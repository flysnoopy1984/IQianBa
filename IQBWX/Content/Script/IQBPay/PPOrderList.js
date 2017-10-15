var pageIndex = -1;
var loadingShow = false;
var pageCount = 0;
var IsEnd = false;
var swiper;
var url = site + "/PP/OrderQuery";
var OpenId;


function Next() {

    Query(pageIndex + 1);
}

$(document).ready(function () {
 
    $("#btnNext").attr("disabled", false);
    OpenId = $("#hOpenId").val();
    Query(pageIndex + 1);
 
});

function Query(_pageIndex) {

    var PageSize = 10;
    if (_pageIndex == 0)
        PageSize = 20;
    

    $.ajax({
        type: 'post',
        data: "Page=" + _pageIndex + "&PageSize=" + PageSize + "&OpenId=" + OpenId,
        url: url,
        success: function (data) {
            var arrLen = data.length;
           
            if (arrLen > 0) {
                generateData(data);
                pageIndex++;
              
            }
            else {
                pageIndex--;
                $("#btnNext").attr("disabled", true);
                alert("没有数据了");

            }
          
        },
        error: function (xhr, type) {
            alert('Ajax error!');
            // 即使加载出错，也得重置
           
        }
    });
}
function InitPagination() {

    $('#swiper-container').dropload({
        scrollArea: window,
        loadDownFn: function (me) {

            // 拼接HTML
            var result = '';

            $.ajax({
                type: 'post',
                data: "Page=" + page,
                url: url,
                success: function (data) {
                    var arrLen = data.length;
                    if (arrLen > 0) {
                        generateData(data);
                        // 如果没有数据
                    } else {
                        // 锁定
                        me.lock();
                        // 无数据
                        me.noData();

                    }
                    // 为了测试，延迟1秒加载
                    setTimeout(function () {
                        // 每次数据插入，必须重置
                        me.resetload();
                    }, 1000);
                },
                error: function (xhr, type) {
                    alert('Ajax error!');
                    // 即使加载出错，也得重置
                    me.resetload();
                }
            });


        }
    });
}

function generateData(result) {
    var strCtrl = "";
  

    $.each(result, function (i) {
        if (pageIndex == 0 && i == 0) {
            pageCount = result[i].TotalCount;
        }
        var thWidth;

        var orderStatus;
        switch(result[i].OrderStatus)
        {
            case 0:
                orderStatus = "等待支付宝响应";
                break;
            case 1:
                orderStatus = "已付款";
                break;
            case 2:
                orderStatus = "已结算";
                break;
            case -1:
                orderStatus = "异常";
                break;
        }
        strCtrl = "";
        strCtrl += "<tr>";

        tdWidth = "width:" + $("#trHeader th").eq(0).css("width");
        strCtrl += "<td style='" + tdWidth + "'>" + result[i].OrderNo + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(1).css("width");
        strCtrl += "<td>" + result[i].TransDateStr + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(2).css("width");
        strCtrl += "<td>" + result[i].BuyerAliPayLoginId + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(3).css("width");
        strCtrl += "<td>" + result[i].TotalAmount + "</td>";

        tdWidth = "width:" + $("#trHeader th").eq(4).css("width");
        strCtrl += "<td>" + orderStatus + "</td>";

        strCtrl += "</tr>";
      
       

        $("#trContainer").append(strCtrl);
    });
   

}