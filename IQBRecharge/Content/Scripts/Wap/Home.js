$(function () {

    ToPage = function (e) {
        if(e.data.act == "JY")
        {
            window.location.href = "/User/SellFuelCard";
            return ;
        }
        if(e.data.act == "SJ")
        {
            window.location.href = "/User/SellPhoneCard";
            return;
        }
    }

   

    $("#liJY").on("click",{ act: "JY" },ToPage);

    $("#liSJ").on("click", { act: "SJ" }, ToPage);
});