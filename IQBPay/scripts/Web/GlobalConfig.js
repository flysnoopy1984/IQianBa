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

            $("#IsWXNotice_AgentTransfer").find("option[value='" + data.IsWXNotice_AgentTransfer + "']").attr("selected", true);
          
            $("#RecId").val(data.ID);

            $("#MarketRate").val(data.MarketRate);
          
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

    var IsWXNotice_AgentTransfer = $("#IsWXNotice_AgentTransfer").val();

    var Note = $("#Note").val();

    var MarketRate = $("#MarketRate").val();
   
    $.ajax({
        type: 'post',
        url: url,
        data: "ID=" + ID + "&IsWXNotice_AgentTransfer="+IsWXNotice_AgentTransfer+"&MarketRate="+MarketRate+"&WebStatus=" + WebStatus + "&Note=" + Note + "&AllowRegister=" + AllowRegister,
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