
var SelectedTr = null;

$(document).ready(function () {

    $("#tbData tr").not("#trHeader").each(function () {
        var tr = $(this);
        tr.mousedown(function () {
            SelectedTr = $(this);
            $("#tbData tr").not("#trHeader").each(function () {
                tr = $(this);
                tr.removeClass("TableRowSelected");
            });
            var b = $(this).hasClass("TableRowSelected");
            if (b == false) {
                SelectedTr.addClass("TableRowSelected");   
            } 
        });
    });
});
