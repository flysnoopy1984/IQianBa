function Query() {

    var url = "/Main/AppQuery";
    $.ajax({
        type: 'post',
        data: "pageIndex=0",
        url: url,
        success: function (data) {
            var arrLen = data.length;

            $("#trContainer").empty();
            if (arrLen > 0) {
                generateData(data);
            }
        },
        error: function (req, textStatus, errorThrown) {
          
            alert(req.responseText);
        },
    });
}

function Init(Id) {
    if (Id == null || Id == "")
    {
        Id = -1;
    }
    $("#RecId").val(Id);

    var url = "/Main/GetApp";
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



function generateData(result) {
    var strCtrl = "";
    $.each(result, function (i) {
        strCtrl = "";
        strCtrl += "<tr>";
        strCtrl += "<td>" + result[i].AppId + "</td>";
        strCtrl += "<td>" + result[i].AppName + "</td>";
        strCtrl += "<td><a href='/Main/AppInfo?id=" + result[i].ID + "' class='td'>详情</a>";
        strCtrl += "</tr>";

        $("#trContainer").append(strCtrl);

    });
}

function ToInfo(action) {
    window.location.href = "AppInfo?do=" + action;
    return;
}

function InitFormData(result) {

    $("#AppId").val(result.AppId);
    $("#AppName").val(result.AppName);
    $("#ServerUrl").val(result.ServerUrl);

    $("#Private_Key").val(result.Merchant_Private_Key);
    $("#Public_Key").val(result.Merchant_Public_key);

    $("#Version").val(result.Version);
    $("#SignType").val(result.SignType);
    $("#Charset").val(result.Charset);
    $("#AppStatus").val(result.RecordStatus);

    $("#AuthUrl_Store").val(result.AuthUrl_Store);

    if ($("#AppStatus").val() == 0) st = true;
    else st = false;

    //Status 初始化
    $("#AppStatus").bootstrapSwitch({
        onText: "启用",
        state: st,
        offText: "禁用",
        onColor: "success",
        offColor: "danger",
        size: "small",
        onSwitchChange: function (event, state) {
            if (state == true) {
                $(this).val("0");
            } else {
                $(this).val("1");
            }
        }
    });

}

function Save() {

    var AppId = $("#AppId").val();
    var AppName = $("#AppName").val();
    var ServerUrl = $("#ServerUrl").val();
    var AuthUrl_Store = $("#AuthUrl_Store").val();
  
    var Private_Key = $("#Private_Key").val();
    var Public_Key = $("#Public_Key").val();
    var Version = $("#Version").val();
    var SignType = $("#SignType").val();
    var Charset = $("#Charset").val();
    var AppStatus = $("#AppStatus").val();

    var Id = $("#RecId").val();
    var url = "/Main/AddApp";
    if (Id > 0)
    {
        url = "/Main/UpdateApp";
    }

    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "ID":Id,"AppId": AppId, "AppName": AppName,"AuthUrl_Store":AuthUrl_Store, "ServerUrl": ServerUrl, "Merchant_Private_Key": Private_Key, "Merchant_Public_key": Public_Key, "Version": Version, "SignType": SignType, "Charset": Charset, "RecordStatus": AppStatus },
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
