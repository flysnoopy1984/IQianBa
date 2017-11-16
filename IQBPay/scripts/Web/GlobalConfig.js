$(document).ready(function () {

    Init();
});

function Init()
{
    var url = "/Global/GetOrCreate";

    $.ajax({
        type: 'post',
        url: url,
        success: function (data) {
            var arrLen = data.length;
          

        },
        error: function (xhr, type) {
            alert('Ajax error!');
        }
    });
}