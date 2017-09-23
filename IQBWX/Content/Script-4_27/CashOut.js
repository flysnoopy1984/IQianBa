var page = 0;
var loadingShow = false;
var pageCount = 0;
var IsEnd = false;
var swiper;


$(document).ready(function () {
    page = 0;
    RequestCashOutData();

    InitPageControl();
});

function GetCashOutData() {
    setTimeout(function () {
        //
        // $("#loadBar").hide();   

        loadingShow = false;
        page++;
        RequestCashOutData();

    }, 1000);

    swiper.params.onlyExternal = false;
}


function InitPageControl() {
    
    swiper = new Swiper('#swiper-container', {
        scrollbar: '.swiper-scrollbar',
        direction: 'vertical',
        slidesPerView: 'auto',
        mousewheelControl: true,
        watchActiveIndex: true,
        freeMode: true,
        observer: true,
        observeParents: true,
        onTouchEnd: function (swiper) {

            if (swiper.isEnd) {

                if (IsEnd) return;
                if (swiper.translate < PageSens) {
                    //完成一次loading才能开始下一次loading，防止重复多次
                    if (!loadingShow) {

                        //加载loading提示
                        $("#InitloadBar").show();
                        //  swiper.appendSlide('<div class="nrxqcon3 mgz xi21 hui cen" id="loadBar">加载数据中...</div>');

                        loadingShow = true;
                        GetCashOutData();
                        swiper.slideNext();
                    }
                    //  getAjaxNews();
                }

            }

        }
    });
}
function RequestCashOutData() {
    var url = site + "/Html5/CashOutData";
   

    $.ajax({
        type: "post",
        data: "Page=" + page,
        url: url,
        success: function (result) {
            var strCtrl = "";
            var ul = $("#TransUl");

            $.each(result, function (i) {
                if (page == 0 && i == 0) {
                    pageCount = result[i].TotalCount;
                }
                strCtrl += "<li>";
                strCtrl += "<div class='yjmxl f xi24'>提取余额" + "<br /><span class='xi19 hui'>" + DateTimeFormate(result[i].TransDateTime) + "</span></div>";
                strCtrl += "<div class='yjmxr r xi40 lan'>-"+ result[i].Amount + "</div>";
                strCtrl += "</li>";
                ul.append(strCtrl);
                strCtrl = "";
            });

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

