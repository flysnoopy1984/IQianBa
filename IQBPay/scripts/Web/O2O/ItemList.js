var pageIndex = -1;
var pageSize = 40;
var RuleData = null;
var MallData = null;

$(document).ready(function () {

    InitCondition();
 
});


function InitCondition()
{
    $("#btnAdd").hide();
   
    var url = "/O2O/InitRule_Mall";
    $.ajax({
        type: 'post',
        data: "",
        url: url,
        success: function (data) {
            RuleData = data.HashO2ORule;
           

            MallData = data.HashO2OMall;
            //$.each(RuleData, function (i) {
            //    mallOp += "<option value=" + MallData[i].ID + " selected>" + MallData[i].Name + "</option>";
            //});

            $("#btnAdd").show();
            Query(true, pageIndex + 1);
        },
        error: function (xhr, type) {
            alert("系统错误！");
            $("#btnAdd").show();
        }
    });
}

function MallOption(updateData) {
    var mallOp = "";
    if (updateData == null)
    {
        $.each(MallData, function (i) {
            mallOp += "<option ruleId=" + MallData[i].O2ORuleId + " value=" + MallData[i].Id + ">" + MallData[i].Name + "</option>";
        });
    }
    else
    {
        $.each(MallData, function (i) {
            if (updateData.MallId == MallData[i].Id)
                mallOp += "<option ruleId=" + MallData[i].O2ORuleId + " value=" + MallData[i].Id + " selected>" + MallData[i].Name + "</option>";
            else
                mallOp += "<option ruleId=" + MallData[i].O2ORuleId + " value=" + MallData[i].Id + ">" + MallData[i].Name + "</option>";
        });
        
    }
   
    return mallOp;
}

function RuleOption(updateData)
{
    var ruleOp = "";
    if (updateData == null) {
        $.each(RuleData, function (i) {
            ruleOp += "<option value=" + RuleData[i].Id + ">" + RuleData[i].Name + "</option>";
        });
    }
    else {
        $.each(RuleData, function (i) {
            if (updateData.O2ORuleId == RuleData[i].Id)
                ruleOp += "<option value=" + RuleData[i].Id + " selected>" + RuleData[i].Name + "</option>";
            else
                ruleOp += "<option value=" + RuleData[i].Id + ">" + RuleData[i].Name + "</option>";
        });

    }
    
    return ruleOp;
}

function CreateNew(updateData)
{
    var ruleOp = RuleOption(updateData);
    var mallOp = MallOption(updateData);
    var ctrl = GetCellHtml();
    if (updateData == null) {

        ctrl = String.format(ctrl,"New", "", "", "1", "", "","","0", mallOp, ruleOp,"","");
    }
    else {
        ctrl = String.format(ctrl, "O_"+updateData.Id,
                                   updateData.Id,
                                   updateData.Amount,
                                   updateData.Qty,
                                   updateData.Name,
                                   updateData.RealAddress,
                                   updateData.ImgUrl,
                                   updateData.RecordStatus,
                                   mallOp, ruleOp,
                                   updateData.CreateDateTimeStr,
                                   updateData.ModifyDateTimeStr);
    }

    $("#DataContainer").prepend(ctrl);
    
    if(updateData != null)
    {
        var btn_Status = $("#O_" + updateData.Id).find("#btn_Status");
       
        if (updateData.RecordStatus == 0)
            btn_Status.text("下架"); 
        else
        {
            var pObj = btn_Status.closest("div");
            pObj.addClass("ItemBlock");
            btn_Status.text("上架");
        }
        btn_Status.on("click", { "ItemId": updateData.Id }, ItemInOut);

        var btn_Delete = $("#O_" + updateData.Id).find("#btn_Delete");
        btn_Delete.on("click", { "ItemId": updateData.Id }, DeleteItem);
    }
    else
    {
        var btn_Status = $("#New").find("#btn_Status");
        btn_Status.hide();
        var btn_Delete = $("#New").find("#btn_Delete");
        btn_Delete.on("click", { "ItemId": "0" }, DeleteItem);    
    }
    

}

function GetCellHtml() {
  
    var ctrl = '<div style="width:92%;" Id="{0}">';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>商品ID：</span><input id="Id" type="text" style="width:50px;" class="form-control" disabled value="{1}" /></li>';
    ctrl += '<li><span>金额：</span><input id="Amount" type="text" onkeyup="OnlyNumber(this);" class="form-control" value="{2}" /></li>';
    ctrl += '<li><span>数量：</span><input id="Qty" type="number" step="1" class="form-control" value="{3}" /></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>商城：</span><select id="MallId" class="form-control" onchange="MallChanged(this);">{8}</select></li>';
    ctrl += '<li><span>规则：</span><select id="O2ORuleId" class="form-control">{9}</select></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>名称描述：</span><input id="Name" type="text" class="form-control" style="width:680px;" value="{4}" /></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>商品地址：</span><textarea id="RealAddress" class="form-control_textarea">{5}</textarea></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>图片地址：</span><input id="ImgUrl" type="text" class="form-control" style="width:680px;" value="{6}" /></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li class="CellTitle" style="width:300px;">创建时间:<span id="CreateDate" style="margin-left:10px;">{10}</span></li>';
    ctrl += '<li class="CellTitle">修改时间:<span id="ModifyDate" style="margin-left:10px;">{11}</span></li>';
    ctrl += '<li style="float:right"><button  type="button" id="btn_Delete" class="btn btn-danger">删除</button></li>  ';
    ctrl += '<li style="float:right"><button  type="button" id="btn_Status" class="btn btn-warning">下架</button></li>  ';
    ctrl += '<li style="float:right"><button  type="button" class="btn btn-info" onclick="Save(this)">保存</button></li>';
   
    ctrl += '</ul>';
    ctrl += '<input type="hidden" id="RecordStatus" value="{7}">';
    ctrl += '<hr class="Hr_Tr" />';
    ctrl += '</div>';
   
    return ctrl;
}

function OnlyNumber(obj)
{
    obj.value = obj.value.replace(/[^\d.]/g, "");
}

function Query(NeedClearn, _PageIndex) {

    var url = "/O2O/ItemListQuery";
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

//商城规则联动
function MallChanged(obj)
{
    var pObj = $(obj).closest("div");
    var selRule = pObj.find("#O2ORuleId");
    var RuleId = $(obj).find('option:selected').attr('ruleId');

    $.each(RuleData, function (i) {
        if(RuleData[i].Id == RuleId)
        {
            selRule.val(RuleId);
            return false;
        }
    });
}
//上下架
function ItemInOut(e)
{
    var obj = $(this);
    var pObj = obj.closest("div");
    var url = "/O2O/ItemStatusChange";
    var ItemId = e.data.ItemId;
    $.ajax({
        type: 'post',
   
        data: { "ItemId": ItemId },
        url: url,
        success: function (data) {

            if (data.IsSuccess) {
                alert("操作成功！");
                if(data.SuccessMsg == "0")
                {
                    obj.text("下架");
                    pObj.removeClass("ItemBlock");

                }
                else
                {
                    obj.text("上架");
                  
                    pObj.addClass("ItemBlock");
                    $("#DataContainer").append(pObj);
                }
                
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

function DeleteItem(e)
{
    if (confirm("确认是否要删除？"))
    {
        var obj = $(this);
        var pObj = obj.closest("div");
        var url = "/O2O/ItemDelete";
        var ItemId = parseInt(e.data.ItemId);
        if (ItemId >0)
        {
            $.ajax({
                type: 'post',
                data: { "ItemId": ItemId },
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
        else
        {
            pObj.remove();
        }
       
    }
   
}

function VerifyItem(pObj)
{
    var amtObj = pObj.find("#Amount");
    var amt = parseFloat(amtObj.val());
    if(amt<=0 || amt>=100000)
    {
        alert("金额必须大于0且小于100000");
        amtObj.focus();
        return false;
    }

    var qtyObj = pObj.find("#Qty");
    var qty = parseFloat(qtyObj.val());
    if (qty <= 0 || qty >= 1000) {
        alert("数量必须大于0且小于1000");
        qtyObj.focus();
        return false;
    }
    return true;
}

function Save(obj) {
    var url = "/O2O/SaveItem";

    var pObj = $(obj).closest("div");

    var Id = pObj.find("#Id").val();
    var Name = pObj.find("#Name").val();
    var Amount = pObj.find("#Amount").val();
    var Qty = pObj.find("#Qty").val();
    var RealAddress = pObj.find("#RealAddress").val();
    var ImgUrl = pObj.find("#ImgUrl").val();
    var RecordStatus = pObj.find("#RecordStatus").val();
    var O2ORuleId = pObj.find("#O2ORuleId").val();
    var MallId = pObj.find("#MallId").val();

    if (!VerifyItem(pObj)) return;
   
    $.ajax({
        type: 'post',
        dataType: "json",
        data: {
            "Id": Id,
            "Name": Name,
            "Amount": Amount,
            "ImgUrl":ImgUrl,
            "Qty": Qty,
            "RealAddress": RealAddress,
            "O2ORuleId": O2ORuleId,
            "MallId": MallId,
            "RecordStatus":RecordStatus,
        },
        url: url,
        success: function (data) {

            if (data.IsSuccess) {

                alert("操作成功！");
                window.location.reload();
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