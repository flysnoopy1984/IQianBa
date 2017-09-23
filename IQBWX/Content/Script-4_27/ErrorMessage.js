$(document).ready(function () {
    var errorCode = $("#errorCode").val();
    var url = $("#urlToJM").val();
    if(errorCode =="1002")
    {
        $("#goAction").attr("href",url );
        $("#goAction").text("立刻加入");
    }
})