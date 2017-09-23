$(document).ready(function () {

    InitData();
});

function queryStringByName(queryName) {
    var str = location.href; //取得整个地址栏
    if (str.indexOf("?") > -1) {
        var queryParam = str.substring(str.indexOf("?") + 1);
        //如果有多个参数
        //if (queryParam.indexOf("&") > -1)
        var param = queryParam.split("&");
        for (var a = 0; a < param.length; a++) {
            var query = param[a].split("=");
            if (query[0] == queryName) {
                return query[1];
            }
        }
    }
    return "";
}

function InitData()
{
    var id = queryStringByName("id");
   
    var url = site + "/info/DetailData";
  
    $.ajax({
        type: "post",
        data: "id=" + id,
        url: url,
        success: function (result) {
            $("#readCount").append(result.ReadCount);
            $("#Title").append(result.Title);
            $("#publishDate").text(result.PublishDate);
            $("#DetailData").html(result.ArticleContent);
        },
        error: function (ex) {
        }

    });
}