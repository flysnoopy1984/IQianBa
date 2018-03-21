//注意为了更好的兼容性，开始前有个分号
; (function ($) {//此处将$作为匿名函数的形参
   
    var IQBScrollLoad = function (element, options) {

        var me = this;

        me.pageIndex = 0;
        me.status = 0; //0 正常,-1 没有数据,-2 锁住
        me.$element = $(element);
        
        me.options = {
            offSetTop:50,
            loadData:''
        };
        me.lock = false;
       

        me.options = $.extend({}, me.options, options);

        var tableObj = $(element).children("table").get(0);

        me.init(options);

        $(document).on("scroll", function () {
           // console.log("status:"+me.status);
            if (me.status == 0)
            {
                console.log("window Height:" + $(window).outerHeight(true));
                console.log("document Height:" + $(document).outerHeight(true));
                console.log($(document).scrollTop());

                var winHeight = $(window).outerHeight(true);
                var docHeight = $(document).outerHeight(true)
                var curPos = $(document).scrollTop();
                if (curPos + winHeight >= docHeight) {
                    me.status = -2;
                    FnloadDown(me);

                }
            }
           
            //$("#Info").text("l h: " + $(element).outerHeight(true));
         //  console.log("Table offsetHeight:" + $(tableObj).outerHeight(true));
         
        });

    };

    FnloadDown = function (me) {

        me.loadBar();
        me.options.loadData(me);
    };

    IQBScrollLoad.prototype.noData = function () {

        var me = this;
        me.status = -1;
       // console.log("status:" + me.status);

        this.loadBar("没有数据了").show();
       

    }

    IQBScrollLoad.prototype.loadBar = function (BarTxt) {

        var me = this;
       
        var loadBar = '<div id="loadBar" class="LoadBar">数据加载中</div>';

        if ($("#loadBar").length > 0)
            $("#loadBar").show();
        else
            $(me.$element).append(loadBar);

        if (BarTxt != undefined) {
            $("#loadBar").text(BarTxt);
        }

        return $("#loadBar");

       
      
    };

    IQBScrollLoad.prototype.init = function () {

        var me = this;
        var docHeight = $(document).height();

       // me.$element.height(docHeight - me.options.offSetTop)
        if (me.pageIndex == 0)
        {
            me.options.loadData(me);
            me.pageIndex++;
        }
    };

    IQBScrollLoad.prototype.resetLoad = function () {

        var me = this;
        if (me.status != -1)
        {
            me.status = 0;
            $("#loadBar").hide();
        }
     
          
        
    };

    $.fn.ScrollLoad = function (options) {

        var obj = new IQBScrollLoad(this, options);

        return obj;

        //var contentHeight =0;
        //var viewHeight  =0;

        //var pageIndex = 0;

        //var defaults = {
        //    loadData: function (me) {}
        //};

        //var opt = $.extend({}, $.fn.IQBScroll.defaults, options);

        //if (pageIndex == 0)
        //    opt.loadData($(this));



        //resetLoad = function () {
        //    $("#loadBar").hide();
        //};
    }
 

})(jQuery);//这里将jQuery作为实参传递给匿名函数