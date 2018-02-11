
$(document).ready(function () {

    InitCondition();

});
function InitCondition()
{
    Query();
}

function Query()
{
    var url = "/O2O/PriceGroupListQuery";
    $.ajax({
        type: 'post',
        data: "",
        url: url,
        success: function (data) {
            var arrLen = data.length;
            if (arrLen > 0) {
                generateData(data);  
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

function GetCellHtml()
{
    var ctrl = '<div style="width:92%;" Id="{0}">';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>组ID：</span><input id="Id" type="text" style="width:50px;" class="form-control" disabled value="{1}" /></li>';
    ctrl += '<li><span>Code：</span><input id="Code" type="text" class="form-control" value="{2}" /></li>';
    ctrl += '<li><span>Name：</span><input id="Name" type="text" class="form-control" value="{3}" /></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>From Price：</span><input id="FromPrice" type="number" step="1" class="form-control" value="{4}" /></li>';
    ctrl += '<li><span>To Price：</span><input id="ToPrice" type="number" step="1" class="form-control" value="{5}" /></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li style="float:right"><button  type="button" id="btn_Delete" class="btn btn-danger" onclick="Delete(this)">删除</button></li>  ';
    ctrl += '<li style="float:right"><button  type="button" class="btn btn-info" onclick="Save(this)">保存</button></li>';
    ctrl += '</ul>';
    ctrl += '<hr class="Hr_Tr" />';
    ctrl += '</div>';

    return ctrl;
}


function CreateNew(updateData)
{
    var ctrl = GetCellHtml();
    if (updateData == null) {
        ctrl = String.format(ctrl, "New", "", "", "", "", "");
    }
    else {
        ctrl = String.format(ctrl, "PG_" + updateData.Id,
                                    updateData.Id,
                                   updateData.Code,
                                   updateData.Name,
                                   updateData.FromPrice,
                                   updateData.ToPrice
                                  );
    }

    $("#DataContainer").prepend(ctrl);
}

function Delete(obj) {

    if (confirm("确认是否要删除？")) {
     
        var pObj = $(obj).closest("div");
        var url = "/O2O/DeletePriceGroup";
        var Id = pObj.find("#Id").val();
        if (Id > 0) {
            $.ajax({
                type: 'post',
                data: { "Id": Id },
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
        else {
            pObj.remove();
        }

    }

}


function Save(obj) {
    var url = "/O2O/SavePriceGroup";

    var pObj = $(obj).closest("div");
    var Id = pObj.find("#Id").val();
    var Name = pObj.find("#Name").val();
    var Code = pObj.find("#Code").val();
    var FromPrice = pObj.find("#FromPrice").val();
    var ToPrice = pObj.find("#ToPrice").val();

    $.ajax({
        type: 'post',
        dataType: "json",
        data: {
            "Id": Id,
            "Name": Name,
            "Code": Code,
            "FromPrice": FromPrice,
            "ToPrice": ToPrice
        },
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
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
