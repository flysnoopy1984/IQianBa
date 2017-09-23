$(document).ready(function () {

    if (window.history && window.history.pushState) {
        $(window).on('popstate', function () {
            window.history.forward(1);
        });
    }
   
})