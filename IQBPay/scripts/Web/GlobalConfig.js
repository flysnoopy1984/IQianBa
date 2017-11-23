$(document).ready(function () {

    Init();
});

function Init()
{
    var url = "/Main/GlobalGetOrCreate";

    $.ajax({
        type: 'post',
        url: url,
        success: function (data) {

            
            $("#WebStatus").find("option:selected").attr("selected", false);
           
            $("#WebStatus").find("option[value='" + data.WebStatus + "']").attr("selected", true);

            $("#AllowRegister").find("option[value='" + data.AllowRegister + "']").attr("selected", true);
          
            $("#RecId").val(data.ID);
          
            $("#Note").val(data.Note);

        },
        error: function (xhr, type) {
            alert('Ajax error!');
        }
    });
}

function Save()
{
    var url = "/Main/GlobalSave";

    var ID = $("#RecId").val();

    var WebStatus = $("#WebStatus").val();

    var AllowRegister = $("#AllowRegister").val();

    var Note = $("#Note").val();
   
    $.ajax({
        type: 'post',
        url: url,
        data: "ID=" + ID + "&WebStatus=" + WebStatus + "&Note=" + Note + "&AllowRegister=" + AllowRegister,
        success: function (data) {

            if(data == "OK")
            {
                alert("更新成功!");
            }
            else
                alert(data);


        },
        error: function (xhr, type) {
            alert('Ajax error!');
        }
    });
}