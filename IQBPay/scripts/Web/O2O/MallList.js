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

    var op = "";
    if (RuleData == null) {
        var url = "/O2O/RuleHash";
        $.ajax({
            type: 'post',
            data: "",
            url: url,
            success: function (data) {
                RuleData = data;
                var arrLen = RuleData.length;

                $.each(RuleData, function (i) {
                    if (updateData != null)
                    {
                        if (updateData.O2ORuleId == RuleData[i].Id)
                            op += "<option value=" + RuleData[i].Id + " selected>" + RuleData[i].Name + "</option>";
                        else
                            op += "<option value=" + RuleData[i].Id + ">" + RuleData[i].Name + "</option>";
                    }
                    else
                        op += "<option value=" + RuleData[i].Id + ">" + RuleData[i].Name + "</option>";
                });
                InitCellData(op, updateData);
            },
            error: function (xhr, type) {
                alert("系统错误！");
            }
        });
    }
    else {
        $.each(RuleData, function (i) {
            if (updateData != null) {
                if (updateData.O2ORuleId == RuleData[i].Id)
                    op += "<option value=" + RuleData[i].Id + " selected>" + RuleData[i].Name + "</option>";
                else
                    op += "<option value=" + RuleData[i].Id + " selected>" + RuleData[i].Name + "</option>";
            }
            else
                op += "<option value=" + RuleData[i].Id + ">" + RuleData[i].Name + "</option>";
        });
        InitCellData(op, updateData);
    }

    
    return op;
}



function GetCellHtml() {

    var ctrl = '<div style="width:92%;" Id="New">';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>商城ID：</span><input id="Id" type="text" style="width:50px;" class="form-control" disabled value="{0}" /></li>';
    ctrl += '<li><span>名字：</span><input id="Name" type="text" class="form-control" value="{1}" /></li>';
    ctrl += '<li><span>Code：</span><input id="Code" type="text" class="form-control" value="{2}" /></li>';
    ctrl += '<li><span>描述：</span><input id="Description" type="text" class="form-control" value="{3}" /></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>规则选择：</span><select id="O2ORuleId" class="form-control">{4}</select>';
    ctrl += '</li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal" style="float:right">';
    ctrl += '<li><button  type="button" class="btn btn-success" onclick="Save(this)">保存</button></li>';
    ctrl += '<li><button  type="button" class="btn btn-danger" onclick="Delete(this);">删除</button></li>  ';
    ctrl += '</ul>';
    ctrl += '</div>';
    ctrl += '<hr class="Hr_Tr" />';
    return ctrl;
}

function InitCellData(op, data) {
 
    var ctrl = GetCellHtml();
    if (data == null)
    {
        ctrl = String.format(ctrl, "","", "", "", op);
    }
    else
    {
        ctrl = String.format(ctrl, data.Id, data.Name,data.Code,data.Description, op);
    }
  
    $("#DataContainer").append(ctrl);
 
}

function Query(NeedClearn, _PageIndex) {

    var url = "/O2O/MallListQuery";
    if (NeedClearn) {
        $("#DataContainer").empty();
    }
    $.ajax({
        type: 'post',
        data: "pageIndex=" + _PageIndex + "&pageSize=" + pageSize,
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
    var url = "/O2O/SaveMall";

    var pObj = $(obj).closest("div");

    var Id = pObj.find("#Id").val();
    var Code = pObj.find("#Code").val();
    var Name = pObj.find("#Name").val();
    var Description = pObj.find("#Description").val();
    var O2ORuleId = pObj.find("#O2ORuleId").val();


    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "Id": Id,"Code":Code, "Name": Name, "Description": Description, "O2ORuleId": O2ORuleId},
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

function Delete(obj) {
    var pObj = $(obj).closest("div");

    var hrObj = $(pObj).next();
    $(pObj).remove();
    $(hrObj).remove();


}