$(document).ready(function () {

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
    if (data.ID == 0) return;

    $("#Name").val(data.Name);
    $("#Rate").val(data.Rate);
    $("#Remark").val(data.Remark);

    var filePath = data.FilePath;
    $("#QRImg").attr("src", filePath);


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

function New() {
    window.location = "ARInfo?do=1";
}

function Save() {

  

    var name = $("#Name").val();
    var rate = $("#Rate").val();
    var remake = $("#Remark").val();
  

  //  if (!CheckForm()) return;

    var url = "/QR/SaveARDefault";
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "Name": name, "Rate": rate, "Remark": remake},
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