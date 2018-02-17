$(document).ready(function () {

    $("#btnLogin").on("click", O2OLogin);

    $(document).keydown(function (event) {
        if (event.keyCode == 13) {
            $("#btnLogin").trigger("click");
        }
    });
});

function O2OLogin()
{
    var url = "/O2O/UserLogin";
    var userPhone = $("#userPhone").val();
    var Pwd = $("#Pwd").val();
    $.ajax({
        type: 'post',
        dataType: "json",
        data: {
            "userPhone": userPhone,
            "Pwd": Pwd,
        },
        url: url,
        success: function (data) {

            if (data.IsSuccess) {

                window.location.href = "/User/Profile";

            }
            else {
                alert(data.ErrorMsg);
            }
        },
        error: function (xhr, type) {
            alert("System Error!");
        }
    });

}