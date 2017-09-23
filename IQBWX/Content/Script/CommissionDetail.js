var page = 0;
var loadingShow = false;
var pageCount = 0;
var IsEnd = false;
var url = site + "/html5/CommissionDetailData";


$(document).ready(function () {
    InitPagination();
});



function InitPagination() {

    $('#swiper-container').dropload({
        scrollArea: window,
        loadDownFn: function (me) {

            // 拼接HTML
            var result = '';
         //   if (IsEnd) return;

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
                   // alert('Ajax error!');
                    // 即使加载出错，也得重置
                    me.resetload();
                }
            });


        }
    });
}



function generateData(result)
{
    var strCtrl = "";
    var ul = $("#TransUl");

        $.each(result, function (i) {
            if (page==0 && i==0)
            {                  
                pageCount = result[i].TotalCount;                 
            }
            if (result[i].TransRemark == null)
                result[i].TransRemark = "获取佣金";
            strCtrl += "<li>";
            strCtrl += "<div class='yjmxl f xi20'>" +  result[i].TransRemark + "<br /><span class='xi19 hui'>" + DateTimeFormate(result[i].TransDateTime) + "</span></div>";
            strCtrl += "<div class='yjmxr r xi30 lan'>+" + result[i].Amount + "</div>";
            strCtrl += "</li>";
       
            ul.append(strCtrl);
        
            strCtrl = "";
        });
         
        page++;
     

}


