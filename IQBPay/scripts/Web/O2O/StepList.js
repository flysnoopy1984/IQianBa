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
}



function GetCellHtml() {
    var ctrl = '<div style="width:92%;" Id="New">';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><input id="Id" type="hidden" style="width:50px;" class="form-control" value="{0}" />';
    //ctrl += '<li><span><class="LeftSpan">Seq：</span><input id="Seq" type="text" style="width:50px;" class="form-control" value="{1}" /></li>';
    ctrl += '<span>Code：</span><input id="Code" type="text" class="form-control" value="{1}" /></li>';
    ctrl += '<li><span><class="LeftSpan">LeftName：</span><input id="LeftName" type="text" class="form-control" value="{2}" /></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span class="TopSpan">BeginContent：</span><textarea id="BeginContent" style="" class="form-control_textarea_ext">{3}</textarea></li>'; 
    ctrl += '</li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span class="TopSpan">EndContent：</span><textarea id="EndContent" style="" class="form-control_textarea_ext">{4}</textarea></li>';
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

function InitCellData(data) {

    var ctrl = GetCellHtml();
    if (data == null) {
        ctrl = String.format(ctrl, "","", "", "", "");
    }
    else {
        ctrl = String.format(ctrl, data.Id, data.Code,data.LeftName, data.BeginContent, data.EndContent);
    }

    $("#DataContainer").append(ctrl);

}

function Query(NeedClearn, _PageIndex) {

    var url = "/O2O/StepListQuery";
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
    var url = "/O2O/SaveStep";
    var pObj = $(obj).closest("div");
    var Id = pObj.find("#Id").val();
    var Code = pObj.find("#Code").val();
    var LeftName = pObj.find("#LeftName").val();
    var Seq = pObj.find("#Seq").val();
    var BeginContent = pObj.find("#BeginContent").val();
    var EndContent = pObj.find("#EndContent").val();

    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "Id": Id, "Code": Code, "LeftName": LeftName, "Seq": Seq, "BeginContent": BeginContent, "EndContent": EndContent },
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

    var Id = pObj.find("#Id").val();

    if (confirm("确认是否要删除？")) {

        var url = "/O2O/DeleteStep";

        if (Id > 0) {
            $.ajax({
                type: 'post',
                data: { "Id": Id },
                url: url,
                success: function (data) {
                    if (data.IsSuccess) {
                        alert("删除成功！");
                        pObj.remove();
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