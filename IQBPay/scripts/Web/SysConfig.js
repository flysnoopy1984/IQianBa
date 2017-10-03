$(document).ready(function () {

    var Id = $("#RecId").val();
    if (Id == null || Id == "")
    {
        Id = "IQBPPConfig";
    }
    Init(Id);
})

function Init(Id) {
  
    var url = "/Main/GetSysConfig";
    $.ajax({
        type: 'post',
        data: "Id=" + Id,
        url: url,
        success: function (data) {

            InitFormData(data);
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });

}

function InitFormData(data)
{
    $("#ARUrl").val(data.QR_ARUrl);
    $("#PPRate").val(data.PPRate);
    $("#RecId").val(data.ID);
}

function Save() {
    var Id = $("#RecId").val();
    if (Id == "") Id = "IQBPPConfig";

    var url = "/Main/SaveSysConfig";
    var ARUrl = $("#ARUrl").val();
    var PPRate = $("#PPRate").val();

    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "ID": Id, "QR_ARUrl": ARUrl, "PPRate": PPRate },
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

