var pageIndex = -1;
var pageSize = 40;

var RuleHash;

$(document).ready(function () {

    InitCondition();

   
    Query(true, pageIndex + 1);
});

function InitCondition() {
    pageIndex = -1;
}


function GetCellHtml() {
    var ctrl = '<div style="width:92%;" Id="New">';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>规则ID：</span><input id="Id" type="text" class="form-control" disabled value="{0}" /></li>'; 
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>Code：</span><input id="Code" type="text" class="form-control" value="{1}" /></li>';
    ctrl += '<li><span>名字：</span><input id="Name" type="text" class="form-control" value="{2}" /></li>';
    ctrl += '</ul>';
   
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li>需要账户：<input id="NeedMallAccount" type="checkbox" class="fieldControl_checkBox" {3} /></li>';
    ctrl += '<li>需要短信验证：<input id="NeedMallSMSVerify" type="checkbox" class="fieldControl_checkBox" {4} /></li>';
    ctrl += '<li>是否支持秒到：<input id="IsMoneyImmediate" type="checkbox" class="fieldControl_checkBox" {5} /></li>';
    ctrl += '<li>生成支付QR：<input id="IsGeneralPayQR" type="checkbox" class="fieldControl_checkBox" {6} /></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal" style="float:right">';
    ctrl += '<li><button  type="button" class="btn btn-success" onclick="Save(this)">保存</button></li>';
    ctrl += '<li><button  type="button" class="btn btn-danger" onclick="Delete(this);">删除</button></li>  ';
    ctrl += '</ul>';
    ctrl += '<hr class="Hr_Tr" />';
    ctrl += '</div>';
 
    return ctrl;
}

function CreateNew() {
    var ctrl = GetCellHtml();
    ctrl = String.format(ctrl, "","", "", "", "", "", "");
    $("#DataContainer").append(ctrl);
    
 
}

function Query(NeedClearn, _PageIndex) {

    var url = "/O2O/RuleListQuery";
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
        var NeedMallAccountChecked = result[i].NeedMallAccount ? "checked" : "";
        var NeedMallSMSVerifyChecked = result[i].NeedMallSMSVerify ? "checked" : "";
        var IsMoneyImmediateChecked = result[i].IsMoneyImmediate ? "checked" : "";
        var IsGeneralPayQRChecked = result[i].IsGeneralPayQR ? "checked" : "";
        var ctrl = GetCellHtml();
        ctrl = String.format(ctrl,
            result[i].Id,
            result[i].Code,
            result[i].Name,
           NeedMallAccountChecked,
            NeedMallSMSVerifyChecked,
            IsMoneyImmediateChecked,
            IsGeneralPayQRChecked);
        $("#DataContainer").append(ctrl);
    });
}




function Save(obj)
{
    var url = "/O2O/SaveRule";

    var pObj = $(obj).closest("div");
    var Id = pObj.find("#Id").val();
    var Name = pObj.find("#Name").val();
    var Code = pObj.find("#Code").val();
    var NeedMallAccount = pObj.find("#NeedMallAccount").get(0).checked;
    var NeedMallSMSVerify = pObj.find("#NeedMallSMSVerify").get(0).checked;
    var IsMoneyImmediate = pObj.find("#IsMoneyImmediate").get(0).checked;
    var IsGeneralPayQR = pObj.find("#IsGeneralPayQR").get(0).checked;


    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "Id": Id, "Code":Code,"Name": Name, "NeedMallAccount": NeedMallAccount, "NeedMallSMSVerify": NeedMallSMSVerify, "IsMoneyImmediate": IsMoneyImmediate, "IsGeneralPayQR": IsGeneralPayQR },
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

function Delete(obj)
{
    var pObj = $(obj).closest("div");
    var Id = pObj.find("#Id").val();
  
    if (confirm("确认是否要删除？")) {
      
        var url = "/O2O/DeleteRule";
       
        if (Id > 0) {
            $.ajax({
                type: 'post',
                data: { "Id": Id },
                url: url,
                success: function (data) {
                    if (data.IsSuccess) {  
                        alert("操作成功！");
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