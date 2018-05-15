$(function () {

    Init = function () {
        $("#btnLeftMainMenu").hide();
        $("#o2o_header").append('<div class="LeftMenu"><a href="#">教程</a></div>');
       
    };
    checkSepcValue = function(obj)
    {
        $(obj).val($(obj).val().replace(/[~'!<>@#$%^&*()-+_=:]/g, ""));
    }

    ToOrder = function()
    {
        var un = $("#UserName").val().trim();
        
        if (un == "")
        {
            alert("用户名不能为空");
            return;
        }
        window.location.href = "/O2OWap/Index?un=" + encodeURI(un)+ "&aoId=" + aoId;
    }

    Init(); 
});