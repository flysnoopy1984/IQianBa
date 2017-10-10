﻿$(document).ready(function () {

    Init();
   
})

function Init() {

    var url = "/User/Get";
    $.ajax({
        type: 'post',
        //data: "Id=" + Id,
        url: url,
        success: function (data) {
            if (data.QueryResult == false)
                window.location.href = "/Main/Login";
            else
                InitFormData(data);
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });

}

function InitFormData(data)
{
    $("#UserRole").text(data.UserRoleName);
    $("#RegisterDate").text(data.CDate);
    $("#LastLoginDate").text(data.MDate);

    $("#UserId").text(data.Id);
    $("#UserPhone").text("");
    $("#UserName").text(data.Name);
    

    $("#HeaderImg").attr("src", data.Headimgurl);
    if (data.QRFilePath == "" || data.QRFilePath == null)
        $("#QRImg").attr("src", "/Content/Images/noPic.jpg");
    else
    {
        $("#Rate").text(data.Rate+"%");
        $("#QRImg").attr("src", data.QRFilePath);
    }
}