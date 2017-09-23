var page = 0;
var loadingShow = false;
var pageCount = 0;
var IsEnd = false;
var swiper;
var startScroll, touchStart, touchCurrent;

$(document).ready(function () {
    page = 0;
    RequestCommissionDetalData();

    InitPageControl();
    alert(swiper.params.height);

});

function GetCommissionDetailData()
{
    setTimeout(function () {
        loadingShow = false;
        page++;
        RequestCommissionDetalData();      
    }, 1000);
    swiper.params.onlyExternal = false;
}



function InitPageControl()
{
    swiper = new Swiper('#swiper-container', {
        scrollbar: '.swiper-scrollbar',
        direction: 'vertical',
        slidesPerView: 'auto',
        mousewheelControl: false,
        freeMode: true,
        observer: true,
        observeParents: true,
        onTouchStart: function (e) {
         
        },
        onTouchMove:function(e) {
          
        },
       
        onTouchEnd: function (swiper) {
          
            if (swiper.isEnd) {
              
                if (IsEnd) return;
                if (swiper.translate < -PageSens) {
                 
                    //完成一次loading才能开始下一次loading，防止重复多次
                    if (!loadingShow) {

                        //加载loading提示
                        $("#InitloadBar").show();
                      //  swiper.appendSlide('<div class="nrxqcon3 mgz xi21 hui cen" id="loadBar">加载数据中...</div>');
                      
                        loadingShow = true;
                        GetCommissionDetailData();
                   //     swiper.slideNext();
                    }
                    //  getAjaxNews();
                }

            }

        }
    });   
}
function RequestCommissionDetalData()
{
    var url = site + "/Html5/CommissionDetailData";
   
    $.ajax({
        type: "post",
        data: "Page=" + page,
        url: url,
        success: function (result) {
            var strCtrl = "";
            var ul = $("#TransUl");          
            var newSlideData;
            $.each(result, function (i) {
                if (page==0 && i==0)
                {                   
                    pageCount = result[i].TotalCount;
                 
                }
                strCtrl += "<li>";
                strCtrl += "<div class='yjmxl f xi20'>" + result[i].TransRemark + "<br /><span class='xi19 hui'>" + DateTimeFormate(result[i].TransDateTime) + "</span></div>";
                strCtrl += "<div class='yjmxr r xi30 lan'>+" + result[i].Amount + "</div>";
                strCtrl += "</li>";
                ul.append(strCtrl);
                newSlideData += strCtrl;
                strCtrl = "";
            });
            //swiper.appendSlide(newSlideData);
            //if(page >0)
            //swiper.slideNext();

            $("#InitloadBar").hide();

            if ((page + 1) * 10 > pageCount) {
                $("#LastRecored").show();
                IsEnd = true;
            }

         
        },
        error: function (ex) {

        }
    })

}


