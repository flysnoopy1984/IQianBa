$(function () {

    selectItem = function (e) {
       

        var obj = $(e.currentTarget);
        var pObj = obj.parent();
       
        var selObj = pObj.children(".ItemSelected");
        selObj.removeClass("ItemSelected");
        selObj.addClass("Item");

       // obj.removeClass("Item");
        obj.addClass("ItemSelected");
    }



    Init = function () {
        $(".ItemList").children().on("click", selectItem);
    }

    Init();
})