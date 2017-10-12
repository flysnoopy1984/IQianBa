
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

function SetWidth() {

    var w = parseInt($("#TableHeader").css("width"));
    var hTable = $("#trContainer").height();
    var hDiv = $("#divTableBody").height();
    if (hTable > hDiv) {

        var scrollWidth = 17;
        w += scrollWidth;

        $("#divTableHeader").css("width", w);
        $("#divTableBody").css("width", w);
        $("#TableHeader").css("width", w - scrollWidth);
    }
    else {
        $("#divTableHeader").css("width", w);
        $("#divTableBody").css("width", w);
    }
}
