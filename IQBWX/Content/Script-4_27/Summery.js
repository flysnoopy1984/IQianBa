var page = 0;
var loadingShow = false;
var pageCount = 0;
var IsEnd = false;
var swiper;

$(document).ready(function () {

    requireData();

    InitSwiper();
});

function GoDetail()
{
    
}

function requireData()
{
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
function generateData(result)
{
    var strCtrl = "";
    var ul = $("#InfoUl");

    $.each(result, function (i) {
        if (page == 0 && i == 0) {
            pageCount = result[i].TotalCount;
        }
        strCtrl += "<li>";
        strCtrl += "<div class='kouzi_newl f'><img align='absmiddle' src='" + result[i].CoverImg + "' width='143' height='143' /></div>";
        strCtrl += "<div class='kouzi_newc f'><img align='absmiddle' src='"+site+"/Content/images/kouzi_04.png' /></div>";
        strCtrl += "<div class='kouzi_newr f'>";
        strCtrl += "<div class='kouzi_newrs mg2'><a href='Detail?id="+result[i].InfoId+"'>" + result[i].Title + "</a></div>";
        strCtrl += "<div class='kouzi_newrc mg2 xi19 hui'>" + result[i].Summery + "</div>";
        strCtrl += "<div class='con2sxl f'><img align='absmiddle' src='"+site+"/Content/images/daikuankouzi_06.jpg' />" + result[i].ReadCount + "</div>";
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

function InitSwiper()
{
    var mySwiper = new Swiper('#swiper-container', {
        scrollbar: '.swiper-scrollbar',
        slidesPerView: 'auto',
        direction: 'vertical',
        freeModeMomentum:true,
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



