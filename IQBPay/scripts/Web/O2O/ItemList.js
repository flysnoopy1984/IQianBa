var pageIndex = -1;
var pageSize = 40;
var RuleData = null;
var MallData = null;
var PayData = null;
var AddrData = null;

var MallCode = "";
var RuleId = -1;
var MallName = "";
var DataPre = "DataContainer_";
var DataCtrl = null;

$(document).ready(function () {

    InitCondition();
 
});

function InitCondition() {
    $("#btnAdd").hide();

    var url = "/O2O/InitItemListPage";
    $.ajax({
        type: 'post',
        data: "",
        url: url,
        success: function (data) {
            RuleData = data.HashO2ORule;
            MallData = data.HashO2OMall;
            AddrData = data.AddrList;

            var btnCtrl = "";

            $.each(MallData, function (i) {
                btnCtrl = '<li><button type="button" class="btn btn-default" onclick=SelectMall("' + MallData[i].Code + '")>' + MallData[i].Name + '商城</button></li>';
                $("#ulMallList").append(btnCtrl);

                //dataContainer
                $("#AllTab").append("<div id='" + DataPre + MallData[i].Code + "'></div>")
            });

            $("#btnAdd").show();

        },
        error: function (xhr, type) {
            alert("系统错误！");
            $("#btnAdd").show();
        }
    });

}

function InitData()
{

}

function SetPayMethod(mCode)
{
    PayData = new Array();
    if (mCode == "JD") {
        var obj = { "Name": "京东白条", "Value": 10 };
        PayData.push(obj);
    }
    else
    {
        var obj;
        if (mCode == "Tmall")
        {
            obj = { "Name": "花呗风控", "Value": 1 };
            PayData.push(obj);
        }
        obj = { "Name": "花呗", "Value": 0 };
        PayData.push(obj);
    }
 
}


function SelectStatus()
{
    pageIndex = -1;
    Query(true, pageIndex + 1);
}

function SelectMall(Code)
{
    MallCode = Code;

    SetPayMethod(MallCode);

    DataCtrl = $("#" + DataPre + MallCode);

    //页面重置
    pageIndex = -1;
    Query(true, pageIndex + 1);

    //隐藏其他商城div
    $("#AllTab div").hide();

    DataCtrl.show();

    //设置Title
    $.each(MallData, function (i) {
        if (MallData[i].MallCode == MallCode)
        {
            $("#Title").text(MallData[i].Name + "商城");
        }
    });
}

function MallOption(updateData) {
    var mallOp = "";
    if (updateData == null)
    {
        $.each(MallData, function (i) {
            if (MallCode == MallData[i].Code)
            {
                mallOp += "<option ruleId=" + MallData[i].O2ORuleCode + " value=" + MallData[i].Code + " selected>" + MallData[i].Name + "</option>";
                RuleId = MallData[i].O2ORuleId;
            }    
            else
                mallOp += "<option ruleId=" + MallData[i].O2ORuleCode + " value=" + MallData[i].Code + ">" + MallData[i].Name + "</option>";
        });
    }
    else
    {
        $.each(MallData, function (i) {
            if (updateData.MallCode == MallData[i].Code)
                mallOp += "<option ruleId=" + MallData[i].O2ORuleCode + " value=" + MallData[i].Code + " selected>" + MallData[i].Name + "</option>";
            else
                mallOp += "<option ruleId=" + MallData[i].O2ORuleCode + " value=" + MallData[i].Code + ">" + MallData[i].Name + "</option>";
        });
        
    }
   
    return mallOp;
}

function RuleOption(updateData)
{
    var ruleOp = "";
    if (updateData == null) {
      

        $.each(RuleData, function (i) {
            if (RuleId == RuleData[i].Id)
                ruleOp += "<option value=" + RuleData[i].Code + " selected>" + RuleData[i].Name + "</option>";
            else
                ruleOp += "<option value=" + RuleData[i].Code + ">" + RuleData[i].Name + "</option>";
        });
    }
    else {
        $.each(RuleData, function (i) {
            if (updateData.O2ORuleCode == RuleData[i].Code)
                ruleOp += "<option value=" + RuleData[i].Code + " selected>" + RuleData[i].Name + "</option>";
            else
                ruleOp += "<option value=" + RuleData[i].Code + ">" + RuleData[i].Name + "</option>";
        });

    }
    
    return ruleOp;
}

function PayOption(updateData)
{
    var payop = "";
    if (updateData == null) {


        $.each(PayData, function (i) {
            if (PayData[i].Value == 0 )
                payop += "<option value=" + PayData[i].Value + " selected>" + PayData[i].Name + "</option>";
            else
                payop += "<option value=" + PayData[i].Value + ">" + PayData[i].Name + "</option>";
        });
    }
    else {
        $.each(PayData, function (i) {
            if (updateData.PayMethod == PayData[i].Value)
                payop += "<option value=" + PayData[i].Value + " selected>" + PayData[i].Name + "</option>";
            else
                payop += "<option value=" + PayData[i].Value + ">" + PayData[i].Name + "</option>";
        });

    }

    return payop;
}

function AddrOption(updateData) {
    var addrop = "<option value=0 selected>系统分配</option>";
    if (updateData == null) {

        $.each(AddrData, function (i) {
            addrop += "<option value=" + AddrData[i].Id + ">" + AddrData[i].Address + "</option>";
        });
    }
    else {
        $.each(AddrData, function (i) {
            if (updateData.AddrId == AddrData[i].Id)
                addrop += "<option value=" + AddrData[i].Id + " selected>" + AddrData[i].Address + "</option>";
            else
                addrop += "<option value=" + AddrData[i].Id + ">" + AddrData[i].Address + "</option>";
        });

    }

    return addrop;
}

function CreateNew(updateData)
{
    if (MallCode == "")
    {
        alert("请先选择商城");
        return;
    }
    
    var mallOp = MallOption(updateData);
    var ruleOp = RuleOption(updateData);
    var payOP = PayOption(updateData);
    var addrOp = AddrOption(updateData);
    
  
    var ctrl = GetCellHtml();
    if (updateData == null) {

        ctrl = String.format(ctrl, "New_" + MallCode, "", "", "1", "", "", "", "0", mallOp, ruleOp, "", "", payOP, addrOp);
    }
    else {
        ctrl = String.format(ctrl, "O_"+updateData.Id,
                                   updateData.Id,
                                   updateData.Amount,
                                   updateData.ShipFeeRate,
                                   updateData.Name,
                                   updateData.RealAddress,
                                   updateData.ImgUrl,
                                   updateData.RecordStatus,
                                   mallOp, ruleOp,
                                   updateData.CreateDateTimeStr,
                                   updateData.ModifyDateTimeStr,
                                   payOP,
                                   addrOp);
    }

    DataCtrl.prepend(ctrl);
    
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

        var IsLightReceive = $("#O_" + updateData.Id).find("#IsLightReceive");
        if(updateData.IsLightReceive)
        {
            IsLightReceive.attr("checked", true);
        }

        var NeedSMS = $("#O_" + updateData.Id).find("#NeedSMS");
        if (updateData.NeedSMS) {
            NeedSMS.attr("checked", true);
        }

    }
    else
    {
        var btn_Status = $("#New_" + MallCode).find("#btn_Status");
        btn_Status.hide();
        var btn_Delete = $("#New_" + MallCode).find("#btn_Delete");
        btn_Delete.on("click", { "ItemId": "0" }, DeleteItem);

        var NeedSMS = $("#New_" + updateData.Id).find("#NeedSMS");
        if(MallCode == "JD")
        {
            NeedSMS.attr("checked", true);
        }

     
    }
    

}

function GetCellHtml() {
  
    var ctrl = '<div style="width:92%;" Id="{0}">';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>商品ID：</span><input id="Id" type="text" style="width:50px;" class="form-control" disabled value="{1}" /></li>';  
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>金额：</span><input id="Amount" type="text" onkeyup="OnlyNumber(this);" class="form-control" value="{2}" /></li>';
    ctrl += '<li><span>费率：</span><input id="FeeRate" style="width:60px;" type="number" step="1" class="form-control" value="{3}" /></li>';
    ctrl += '<li><span>商城：</span><select id="MallCode" class="form-control" onchange="MallChanged(this);">{8}</select></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li><span>规则：</span><select id="O2ORuleId" style="width:120px;" class="form-control">{9}</select></li>';
    ctrl += '<li><span>支持秒到：</span><input id="IsLightReceive"  type="checkbox" class="CheckBox_Control" /></li>';
    ctrl += '<li><span>需要SMS：</span><input id="NeedSMS"  type="checkbox" class="CheckBox_Control" /></li>';
    ctrl += '<li><span>套现方式：</span><select class="form-control" id="PayMethod">{12}</select></li>';
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
    ctrl += '<li><span>收货地址：</span><select id="selAddrList" class="form-control">{13}</select></li>';
    ctrl += '</ul>';
    ctrl += '<ul class="UlHorizontal">';
    ctrl += '<li class="CellTitle" style="width:300px;">创建时间:<span id="CreateDate" style="margin-left:10px;">{10}</span></li>';
    ctrl += '<li class="CellTitle">修改时间:<span id="ModifyDate" style="margin-left:10px;">{11}</span></li>';
  //  ctrl += '<li style="float:right"><button  type="button" id="btn_Delete" class="btn btn-danger">删除</button></li>  ';
    ctrl += '<li style="float:right"><button  type="button" id="btn_Status" class="btn btn-danger">下架</button></li>  ';
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
    var ItemStatus = $("#cItemStatus").val();

    if (NeedClearn) {
        DataCtrl.empty();
    }
    $.ajax({
        type: 'post',
        data: "MallCode=" + MallCode + "&pageIndex=" + _PageIndex + "&pageSize=" + pageSize + "&ItemStatus=" + ItemStatus,
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

    var feeObj = pObj.find("#FeeRate");
    //var fee = parseFloat(feeObj.val())
    //if (fee < 0 || fee > 5)
    //{
    //    alert("费率必须在【0-5】之间");
    //    feeObj.focus();
    //    return false;
    //}

    //var qtyObj = pObj.find("#Fee");
    //var qty = parseFloat(qtyObj.val());
    //if (qty <= 0 || qty >= 1000) {
    //    alert("数量必须大于0且小于1000");
    //    qtyObj.focus();
    //    return false;
    //}

   // var RealAddress = pObj.find("#RealAddress").val();

    return true;
}

function Save(obj) {
    var url = "/O2O/SaveItem";

    var pObj = $(obj).closest("div");

    var Id = pObj.find("#Id").val();
    var Name = pObj.find("#Name").val();
    var Amount = pObj.find("#Amount").val();
    var FeeRate = pObj.find("#FeeRate").val();
    var RealAddress = pObj.find("#RealAddress").val();
    var ImgUrl = pObj.find("#ImgUrl").val();
    var RecordStatus = pObj.find("#RecordStatus").val();
    var O2ORuleCode = pObj.find("#O2ORuleId").val();
    var mCode = pObj.find("#MallCode").val();
    var IsLightReceive = pObj.find("#IsLightReceive").get(0).checked;
    var NeedSMS = pObj.find("#NeedSMS").get(0).checked;
    var PayMethod = pObj.find("#PayMethod").val();
    var AddrId = pObj.find("#selAddrList").val();
    if (!VerifyItem(pObj)) return;
   
    $.ajax({
        type: 'post',
        dataType: "json",
        data: {
            "Id": Id,
            "Name": Name,
            "Amount": Amount,
            "ImgUrl":ImgUrl,
            "ShipFeeRate": FeeRate,
            "RealAddress": RealAddress,
            "O2ORuleCode": O2ORuleCode,
            "MallCode": mCode,
            "RecordStatus": RecordStatus,
            "IsLightReceive": IsLightReceive,
            "PayMethod": PayMethod,
            "AddrId": AddrId,
            "NeedSMS": NeedSMS,
        },
        url: url,
        success: function (data) {

            if (data.IsSuccess) {

                alert("操作成功！");
                window.location.reload();
               
            }
            else {
                if (data.IntMsg == -1) {
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

function ImportItem()
{
    alert("暂时未开通");
}