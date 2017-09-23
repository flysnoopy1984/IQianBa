var page = 0;
var loadingShow = false;
var pageCount = 0;
var IsEnd = false;
var swiper;


$(function () {
    var url = site + "/info/SummeryData";

    // dropload
    $('#swiper-container').dropload({
        scrollArea: window,
        loadDownFn: function (me) {
            page++;
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
});


$(document).ready(function () {

   // requireData();

  //  InitSwiper();
});


function requireData() {
    var url = site + "/info/SummeryData";

    $("#InitloadBar").show();
    $.ajax({
        type: "post",
        data: "Page=" + page,
        url: url,
        success: function (result) {
            generateData(result);
        },
        error: function (ex) {
        }

    });
}
function generateData(result) {
    var strCtrl = "";
    var ul = $("#InfoUl");

    $.each(result, function (i) {
        if (page == 0 && i == 0) {
            pageCount = result[i].TotalCount;
        }
        strCtrl += "<li>";
        strCtrl += "<div class='kouzi_newl f'><img align='absmiddle' src='" + result[i].CoverImg + "' width='143' height='143' /></div>";
        strCtrl += "<div class='kouzi_newc f'><img align='absmiddle' src='" + site + "/Content/images/kouzi_04.png' /></div>";
        strCtrl += "<div class='kouzi_newr f'>";
        strCtrl += "<div class='kouzi_newrs mg2'><a href='Detail?id=" + result[i].InfoId + "'>" + result[i].Title + "</a></div>";
        strCtrl += "<div class='kouzi_newrc mg2 xi19 hui'>" + result[i].Summery + "</div>";
        strCtrl += "<div class='con2sxl f'><img align='absmiddle' src='" + site + "/Content/images/daikuankouzi_06.jpg' />" + result[i].ReadCount + "</div>";
        strCtrl += "<div class='con2sxr r'>" + result[i].PublishDate + "</div>";
        strCtrl += "</div>";
        strCtrl += "</div>";
        strCtrl += "</li>";
        ul.append(strCtrl);
        strCtrl = "";
    });

    $("#InitloadBar").hide();
    if ((page + 1) * 10 > pageCount) {
        $("#LastRecored").show();
        IsEnd = true;
    }
}

function InitSwiper() {
    var mySwiper = new Swiper('#swiper-container', {
        scrollbar: '.swiper-scrollbar',
        slidesPerView: 'auto',
        direction: 'vertical',
        freeModeMomentum: true,
        freeMode: true,
        onTouchStart: function () {
            //  holdPosition = 0;
        },
        onReachEnd: function (swiper) {

            //  alert('onReachEnd');
        },
        onReachBeginning: function (swiper) {
            //  alert('onReachBeginning');
        },

        onResistanceBefore: function (s, pos) {

        },
        onTouchEnd: function (swiper) {

            if (swiper.isEnd) {
                if (IsEnd) return;
                if (swiper.translate < -PageSens) {
                    if (!loadingShow) {
                        $("#InitloadBar").show();

                        loadingShow = true;
                        swiper.slideNext();

                        setTimeout(function () {
                            page++;
                            requireData();
                            loadingShow = false;
                        }, 1000);
                        swiper.params.onlyExternal = false;
                    }
                }
            }

        }
    });
}



