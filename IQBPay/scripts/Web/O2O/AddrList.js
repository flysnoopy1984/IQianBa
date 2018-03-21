var pageIndex = -1;
var pageSize = 40;
var RuleData = null;

$(document).ready(function () {

    InitCondition();


    Query(true, pageIndex + 1);
});

function InitCondition() {
    pageIndex = -1;

}

function CreateNew(updateData) {

   
    InitCellData(updateData);
    if (updateData != null) {
        if (updateData.RecordStatus == 0)
        {

        }
        else
        {
            $("#Addr_"+updateData.Id).addClass("ObjBlock");
        }
    }
         
}



function GetCellHtml() {

    var ctrl = '<div style="width:92%;" Id="{0}">';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>地址ID：</span><input id="Id" type="text" style="width:50px;" class="form-control" disabled value="{1}" /></li>';
    ctrl += '<li><span>Code：</span><input id="Code" type="text" class="form-control" value="{2}" /></li>';
    ctrl += '<li><span>简称：</span><input id="Name" type="text" class="form-control" value="{3}" /></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>发货地址：</span><textarea id="Address" class="form-control_textarea">{4}</textarea></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal" style="float:right">';
    ctrl += '<li><button  type="button" class="btn btn-success" onclick="Save(this)">保存</button></li>';
    ctrl += '<li><button  type="button" class="btn btn-danger" onclick="Block(this);">禁用</button></li>  ';
    ctrl += '</ul>';
    ctrl += '<hr class="Hr_Tr" />';
    ctrl += '</div>';
   
    return ctrl;
}

function InitCellData(data) {

    var ctrl = GetCellHtml();
    if (data == null) {
        ctrl = String.format(ctrl,"New", "", "", "", "");
    }
    else {
        ctrl = String.format(ctrl,"Addr_"+data.Id, data.Id, data.Code, data.Name, data.Address);
    }

    $("#DataContainer").prepend(ctrl);

}

function SelectStatus() {
    pageIndex = -1;
    Query(true, pageIndex + 1);
}

function Query(NeedClearn, _PageIndex) {

    var url = "/O2O/AddrListQuery";
    var RecordStatus = $("#cRecordStatus").val();
    if (NeedClearn) {
        $("#DataContainer").empty();
    }
    $.ajax({
        type: 'post',
        data: "pageIndex=" + _PageIndex + "&pageSize=" + pageSize + "&RecordStatus=" + RecordStatus,
        url: url,
        success: function (data) {
            var arrLen = data.length;

            if (arrLen > 0) {
                generateData(data);
                pageIndex++;
            }

        },
        error: function (xhr, type) {
            alert("系统错误！");
        }
    });
}
function generateData(result) {
    var strCtrl = "";

    $.each(result, function (i) {

        CreateNew(result[i]);
    });
}

function Save(obj) {
    var url = "/O2O/SaveAddr";

    var pObj = $(obj).closest("div");

    var Id = pObj.find("#Id").val();
    var Code = pObj.find("#Code").val();
    var Name = pObj.find("#Name").val();
    var City = pObj.find("#City").val();
    var Address = pObj.find("#Address").val();
    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "Id": Id, "Code": Code, "Name": Name, "City": City, "Address": Address },
        url: url,
        success: function (data) {

            if (data.IsSuccess) {

                alert("操作成功！");
            }
            else {
                if (data.IntMsg == -1)
                {
                    alert("未识别当前用户，请重新登录");
                    window.location.hre = "/O2O/Login";
                }
                else
                alert(data.ErrorMsg);
            }
        },
        error: function (xhr, type) {
            alert("System Error!");
        }
    });
}

function Block(obj) {
    var pObj = $(obj).closest("div");
    var AddrId = pObj.find("#Id").val();
    var url = "/O2O/AddrStatusChange";
    $.ajax({
        type: 'post',
        data: { "AddrId": AddrId },
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
                pObj.remove();
                alert("操作成功！");
            }
            else {
              
               alert(data.ErrorMsg);
             
            }
        },
        error: function (xhr, type) {
            alert("System Error!");
        }
    });
 
  


}