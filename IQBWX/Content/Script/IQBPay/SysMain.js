function RefreshSession()
{
    var url = "/PP/RefreshSession";
    $.ajax({
        type: 'post',
        data: "",
        url: url,
        success: function (data) {
            alert(data);

        },
        error: function (xhr, type) {
            alert('Ajax error!');
           

        }
    });
}