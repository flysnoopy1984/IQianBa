
$(document).ready(function () {
    // 初始化内容
    alert("in");
    var url = "/api/hjb/summerylist";
    Init(url);
});

function Init(url)
{
    var ul = $("#ulContent");
    var strLi = "<a href='content.html'><img src='{0}' alt='' /><div><p>{1}</p><span>2016-02-14</span></div></a>";
    $.ajax({
        type: "post",
        data: "",
        url: url,
        success: function (result) {
           
            ul.children().remove();
            
            $(result).each(function (i) {
                var item = result[i];
                ul.append("<li>" + item.Title + "</li>");
            });
        },
        error: function () {
            alert("error");
        }
    })
}