var StepData = null;
$(document).ready(function () {

    InitRuleCode();
    InitStep();
});

function SwitchRule()
{
    var url = "/O2O/RuleStepsQuery";
    var RuleCode = $("#RuleCode").val();
    if (RuleCode == -1) {
      
        return;
    }

    $.ajax({
        type: 'post',
        data: "RuleCode=" + RuleCode,
        url: url,
        success: function (data) {
            var arrLen = data.length;
            if (arrLen > 0)
            {
                $("#DataContainer").empty();
                $.each(data, function (i) {
                    AddStep(data[i]);
                });

            }
          
        },
        error: function (xhr, type) {
            alert("系统错误！");
        }
    });
}

function InitRuleCode()
{
    var url = "/O2O/RuleHash";

    $.ajax({
        type: 'post',
        data: "",
        url: url,
        success: function (data) {
         
            var arrLen = data.length;
            var selRuleCode = $("#RuleCode");
            selRuleCode.append('<option value=-1>请选择...</option>');
            $.each(data, function (i) {
                selRuleCode.append('<option value='+data[i].Code+'>'+data[i].Name+'</option>');
            });  
        },
        error: function (xhr, type) {
            alert("系统错误！");
        }
    });
}

function InitStep() {
    var url = "/O2O/HashStep";

    $.ajax({
        type: 'post',
        data: "",
        url: url,
        success: function (data) {

            StepData = data;
        },
        error: function (xhr, type) {
            alert("系统错误！");
        }
    });
}

function SaveSteps()
{
    var child = $("#DataContainer").children("div");
    var rs = [];
    var url = "/O2O/SaveRuleSteps";
    var RuleCode = $("#RuleCode").val();
    if (RuleCode == -1)
    {
        alert("RuleCode没有选择");
        return;
    }
    $.each(child, function (i) {

        var Id = $(child[i]).find("#Id").val();
        var Seq = $(child[i]).find("#Seq").val();
        var StepCode = $(child[i]).find("#SelCode").val();
        var obj = { "Id":Id,"RuleCode": RuleCode, "Seq": Seq, "StepCode": StepCode };
        rs.push(obj);   
    });

    $.ajax({
        type: 'post',
        dataType: "json",
        data: { "list": rs },
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

function AddStep(data)
{
    var ctrl = '<div style="width:92%;">';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><input type="hidden" Id="Id" value="{2}" /><span>Seq：</span><input id="Seq" style="width:80px;" type="number" class="form-control" value="{1}" /></li>';
    ctrl += '<li><span>Code：</span><select id="SelCode">{0}</select></li>';
    ctrl += '<li><button  type="button" class="btn btn-danger" onclick="Delete(this);">删除</button></li>';
    ctrl += "</ul>";
    ctrl += '<hr class="Hr_Tr" />';
    ctrl += "</div>";
   

    var op = "<option value=-1>请选择...</option>";
   
    if (data == null)
    {
        $.each(StepData, function (i) {
            op += "<option value=" + StepData[i].Code + ">" + StepData[i].Code + "</option>";
        });
        ctrl = String.format(ctrl, op, "", "");
    }     
    else
    {
        $.each(StepData, function (i) {
            if (StepData[i].Code == data.StepCode)
                op += "<option value=" + StepData[i].Code + " selected>" + StepData[i].Code + "</option>";
            else
                op += "<option value=" + StepData[i].Code + ">" + StepData[i].Code + "</option>";
        });
        ctrl = String.format(ctrl, op, data.Seq, data.Id);
    }
       

    $("#DataContainer").append(ctrl);
}

function Delete(obj)
{
    var pObj = $(obj).closest("div");

    var Id = pObj.find("#Id").val();

    if (confirm("确认是否要删除？")) {

        var url = "/O2O/DeleteRelRuleStep";

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