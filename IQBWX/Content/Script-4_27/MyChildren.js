
(function ($) {
    $.MultPage = function () {
        // Add plugin code here
        this.page = 0;
        this.IsEnd = false;
        this.pageCount = 0;
        this.loadingShow = false;
    }  

})(jQuery);

var mp1 = new $.MultPage();
var mp2 = new $.MultPage();
var mp3 = new $.MultPage();
var mp4 = new $.MultPage();

var currentMP = null;

var currentLevel = 1;
var swiper;
//var page = 0;
//var loadingShow = false;
//var IsEnd = false;
//var pageCount = 0;



function getCurrentMP()
{
    switch (currentLevel)
    {
        case 1: return mp1;
        case 2:return mp2;
        case 3:return mp3;
        case 4:return mp4;
    }
}

function setTab(name, cursel, n) {
    currentLevel = cursel;
    
    for (i = 1; i <= n; i++) {
        var menu = document.getElementById(name + i);
        var con = document.getElementById("con_" + name + "_" + i);
        menu.className = i == cursel ? "hover" : "";
        con.style.display = i == cursel ? "block" : "none";
    }

    $("#loadBar" + currentLevel).hide();
    currentMP = getCurrentMP();
    currentMP.pageCount = parseInt($("#l" + cursel).text());

    InitPageControl("#" + "con_" + name + "_" + cursel);
    if (currentMP.page == 0)
        GetChildDetail();

}


function GetChildSummery()
{
    var url = site + "/Html5/MyChildrenCount";
    var l1 = $("#l1");
    var l2 = $("#l2");
    var l3 = $("#l3");
    var l4 = $("#l4");

    $.ajax({
        type: "post",
        data: "",
        url: url,
        success: function (result) {
            l1.text(result[0]); 
            l2.text(result[1]);
            l3.text(result[2]);
            l4.text(result[3]);
            if (currentLevel == 1)
                currentMP.pageCount = parseInt(result[0]);
        },
        error: function (ex) {
        }
    });
}

function GetChildDetail()
{
    var url = site + "/Html5/MyChildrenDetail";
    var table = $("#table_l" + currentLevel+" tbody");
    var strCtrl ="";
    $.ajax({
        type: "post",
        data: "cLevel=" + currentLevel + "&Page=" + currentMP.page,
        url: url,
        success: function (result) {
            $.each(result, function (i) {
                strCtrl = "<tr class='hui'>";
                strCtrl += "<td>" + result[i].cMemberTypeValue + "</td>";
                strCtrl += "<td>" + result[i].cNickName + "</td>";
                strCtrl += "<td>" + DateFormate(result[i].cAddDateTime) + "</td>";
                strCtrl += "</tr>";
                table.append(strCtrl);
                strCtrl = "";
            });
            currentMP.page++;
            $("#loadBar" + currentLevel).hide();
          

            if ((currentMP.page-1) * 10 > currentMP.pageCount) {
                $("#endBar" + currentLevel).show();
                currentMP.IsEnd = true;
            }
           
          
        },
        error: function (ex) {
        }

    });
}

$(document).ready(function () {

    currentMP = getCurrentMP();

    GetChildSummery();

    GetChildDetail();
  

    InitPageControl("#con_one_1");
   

});

function PageGetData()
{
    setTimeout(function () {

        currentMP.loadingShow = false;
       
        GetChildDetail();


  }, 1000);

    swiper.params.onlyExternal = false;
}

function InitPageControl(id) {
    var sb = ".swiper-scrollbar" + currentLevel;

    swiper = new Swiper(id, {
        scrollbar: sb,
        direction: 'vertical',
        slidesPerView: 'auto',
        mousewheelControl: true,
        freeMode: true,
        onTouchEnd: function (swiper) {

            if (swiper.isEnd) {

                if (currentMP.IsEnd) return;
                if (swiper.translate < PageSens) {
                    //完成一次loading才能开始下一次loading，防止重复多次
                    if (!currentMP.loadingShow) {

                        //加载loading提示
                        $("#loadBar" + currentLevel).show();
                        //  swiper.appendSlide('<div class="nrxqcon3 mgz xi21 hui cen" id="loadBar">加载数据中...</div>');

                        currentMP.loadingShow = true;
                        PageGetData();
                        swiper.slideNext();
                    }
                    //  getAjaxNews();
                }

            }

        }
    });
}