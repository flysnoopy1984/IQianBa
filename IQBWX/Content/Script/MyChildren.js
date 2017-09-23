var url = site + "/Html5/MyChildrenDetail";
var dropload = null;
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

 
    currentMP = getCurrentMP();
    currentMP.pageCount = parseInt($("#l" + cursel).text());

   
    clickTab();

}


function GetChildSummery()
{
    var SummeryUrl = site + "/Html5/MyChildrenCount";
    var l1 = $("#l1");
    var l2 = $("#l2");
    var l3 = $("#l3");
    var l4 = $("#l4");

    $.ajax({
        type: "post",
        data: "",
        url: SummeryUrl,
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


$(document).ready(function () {

    currentMP = getCurrentMP();

    GetChildSummery();
 
    InitPageControl();  

});



function clickTab()
{
    if (currentMP.IsEnd)
    {
        dropload.lock('up');
        dropload.noData();
    }
    else
    {
        dropload.unlock();
        dropload.noData(false);
    }
  
    // 重置
    dropload.resetload();

}


function generateData(result)
{
    var table = $("#table_l" + currentLevel + " tbody");
    var strCtrl = "";

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

}

function InitPageControl()
{
    dropload = $('#content').dropload({
        scrollArea: window,
        loadDownFn: function (me) {

            $.ajax({
                type: 'Post',
                url: url,
                data: "cLevel=" + currentLevel + "&Page=" + currentMP.page,
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

                        currentMP.isEnd = true;
                    }
                    setTimeout(function () {
                        me.resetload();
                    }, 1000);
                },
                error: function (xhr, type) {
                    //alert('Ajax error!');
                    // 即使加载出错，也得重置
                    me.resetload();
                }
            });
        }
    });
}