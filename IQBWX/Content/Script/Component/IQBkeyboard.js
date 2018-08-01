var maxMoney = 100000;

$(function () {

    FastClick.attach(document.body);

    InitKeyBoard = function () {
        var designW = 750;  //设计稿宽
        var font_rate = 100;
        //适配
        var fontSize = document.body.offsetWidth / designW * font_rate + "px";
        $(".payinfo").css("fontSize", fontSize);

        document.getElementsByTagName("html")[0].style.fontSize = fontSize  //document.body.offsetWidth / designW * font_rate + "px";
        document.getElementsByTagName("body")[0].style.fontSize = fontSize  // document.body.offsetWidth / designW * font_rate + "px";

        //监测窗口大小变化
        window.addEventListener("onorientationchange" in window ? "orientationchange" : "resize", function () {

            $(".payinfo").css("height", $("#numTable").css("height"));

            document.getElementsByTagName("html")[0].style.fontSize = document.body.offsetWidth / designW * font_rate + "px";
            document.getElementsByTagName("body")[0].style.fontSize = document.body.offsetWidth / designW * font_rate + "px";
        }, false);

        var $paymoney = $("#paymoney");
        $('.pay').addClass('pay-disabled');

       
        $("#pay-zero").click(function () {
            if (($paymoney.text()).indexOf(".") != -1 && ($paymoney.text()).substring(($paymoney.text()).indexOf(".") + 1, ($paymoney.text()).length).length == 2) {
                return;
            }
            if ($.trim($paymoney.text()) == "0") {
                return;
            }
            if (parseInt($paymoney.text()) > maxMoney && $paymoney.text().indexOf(".") == -1) {
                return;
            }
            $paymoney.text($paymoney.text() + $(this).text());
        });


        $("#pay-float").click(function () {
            if ($.trim($paymoney.text()) == "") {
                return;
            }

            if (($paymoney.text()).indexOf(".") != -1) {
                return;
            }

            $paymoney.text($paymoney.text() + $(this).text());
        });


        $("#pay-return").click(function () {
            $paymoney.text(($paymoney.text()).substring(0, ($paymoney.text()).length - 1));
            if (!$paymoney.text()) {
                $('.pay').addClass('pay-disabled').find('a').attr('href', 'javascript:return false;');
            }
        });

        $(".payinfo").css("height", $("#numTable").css("height"));

     

    };

    

    NumClick = function (e) {
        var n = $(e.currentTarget).text();
        var curval = $("#paymoney").text();

        if (curval.indexOf(".") != -1 && (curval).substring((curval).indexOf(".") + 1, (curval).length).length == 2) {
            return;
        }
        if ($.trim(curval) == "0") {
            return;
        }
        if (parseInt(curval) > maxMoney && curval.indexOf(".") == -1) {
            return;
        }
        $('.pay').removeClass('pay-disabled');

        $("#paymoney").text(curval+n);


    }


    InitKeyBoard();

    $(document).on("click", ".paynum", NumClick);

    
    //$("#paymoney").focus(function () {
    //    $(".payinfo").slideDown();
    //    document.activeElement.blur();
    //});


   

});