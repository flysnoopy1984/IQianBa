$(document).ready(function () {

});

$(function () {
    //$("#tb tr")--选择id为tb的table，再选择该table的行tr
    // mouseover--鼠标放上去的事件（悬停）
    //$("#tb tr").mouseover();--给tb的行tr添加鼠标悬停事件
    $("#tb tr").mouseover(function () {
        $(this).addClass("tr1");//添加样式tr1，$(this)表示当前选择的元素。
    });
    // mouseout--鼠标离开的事件
    $("#tb tr").mouseout(function () {
        $(this).removeClass("tr1");//去掉样式tr1
    });
});