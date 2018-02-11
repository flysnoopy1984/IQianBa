var pageIndex = -1;
var pageSize = 40;

function CreateDemoData()
{
    var url = "/O2O/CreateDemoData";
    $.ajax({
        type: 'post',
        url: url,
        success: function (data) {
            alert("OK");

        },
        error: function (xhr, type) {
            alert("系统错误！");
        }
    });
}

$(document).ready(function () {

    InitCondition();

});

function InitCondition()
{
 
}
