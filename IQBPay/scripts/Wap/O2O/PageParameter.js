var aoId = "";
var un = "";

$(function () {

  

    InitParameter = function()
    {
        un = GetUrlParam("un", true); //有这个字段传过来说明是代下单

        aoId = GetUrlParam("aoId"); //有这个字段传过来说明是代下单 aoId = GetUrlParam("aoId");

        if (aoId == "" || aoId == "null" || aoId == undefined) {
            //排除ErrorPage本身
            if (window.location.pathname.toLocaleLowerCase() != "/O2OWap/ErrorPage".toLocaleLowerCase())
            {
                window.location.href = "/O2OWap/ErrorPage?ec=1";
                return;
            }
        }
    }

    toPage =function(url)
    {
        if (url.indexOf("?") > -1)
            url += "&aoId=" + aoId;
        else
            url += "?aoId=" + aoId;

        if (un != "" && un != undefined && un != "null")
        {
            url += "&un=" + encodeURI(un);
        }
      
        window.location.href = url;
    }

    InitParameter();
    
});