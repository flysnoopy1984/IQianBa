﻿$(document).ready(function () {

     Init();
});

function Init() {


    var url = "/QR/GetDefault";
    $.ajax({
        type: 'post',  
        url: url,
        success: function (data) {

            InitFormData(data);
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });

}

function InitFormData(data) {
    

    $(data.HashStoreList).each(function (i,r) {

        
        $("#ReceiveStore").append("<option value='" + r.Id + "'>" + r.Name + "</option>");
    });

    if (data.ID == 0) return;

    $("#RecId").val(data.ID);
    $("#Name").val(data.Name);
    $("#Rate").val(data.Rate);
    $("#Remark").val(data.Remark);
    
   // var filePath = data.FilePath;
    $("#QRImg").attr("src", data.TargetUrl);

    $("#ReceiveStore").val(data.ReceiveStoreId);


}

function CheckForm() {
    $("#EditArea").find("input[class*='NeedFill']").each(function (i) {
        if ($.trim($(this).val()) == "") {
            alert($(this).parent().prev().text() + "不能空");
            return false;
        }
    });
    return true;
}


function Save() {
    var name = $("#Name").val();
    var rate = $("#Rate").val();
    var remake = $("#Remark").val();
    var ID = $("#RecId").val();
    var storeId = $("#ReceiveStore").val();

  //  if (!CheckForm()) return;

    var url = "/QR/SaveARDefault";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: {"ID":ID, "Name": name, "Rate": rate, "Remark": remake,"ReceiveStoreId":storeId},
        url: url,
        success: function (data) {
            if (data == "OK") {
                alert("Save Done");
            }
            else {
                alert(data);
            }
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });

}