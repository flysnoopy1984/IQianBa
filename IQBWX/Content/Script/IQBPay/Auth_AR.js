$(document).ready(function () {
    var act = getUrlParam("act");
    if (act == 1) {
        $("#ImgContainer").show();
        $("#btnAuth").hide();
    }
    else {
        $("#ImgContainer").hide();
        $("#btnAuth").show();
    }
});