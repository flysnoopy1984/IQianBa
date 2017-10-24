var pageIndex = -1;

function Query(NeedClearn,_PageIndex)
{
    var url = "/Order/QuerySum";
    var agentName = $.trim($("#AgentName").val());
  
    $.ajax({
        type: 'post',
        data: "AgentName=" + agentName + "&pageIndex=" + _PageIndex,
        url: url,
        success: function (data) {
            var arrLen = data.length;
            if (NeedClearn) {
                $("#AgentList li").not(".liHeader").remove();
            }

            if (arrLen > 0) {
                generateData(data);
             
                pageIndex++;
            }
            else {
                pageIndex--;
                alert("没有数据了");
            }
            CloseProcess();
        },
        error: function (xhr, type) {

            alert('Ajax error!');
            CloseProcess();

        }
    });
}

function TransferToUser(obj,orderAmt,CommAmt,account,openId)
{
 
    if (account == "" || account == null)
    {
        alert("用户没有设置提款账号，无法打款！");
        return;
    }
    if (orderAmt + CommAmt == 0)
        alert("没有可转金额");

    var url = "/API/TransferAPI/TransferToAgent";
    $.ajax({
        type: 'post',
        data: "OpenId=" + openId + "&AliPayAccount=" + account + "&OrderAmount=" + orderAmt+"&CommissionAmount="+CommAmt,
        url: url,
        success: function (data) {
            if (data.IsSuccess) {
                alert("转账成功！");
                $(obj).parent().prev().find("a").text(0);
                $(obj).parent().prev().prev().find("a").text(0);
            }
            else
                alert(data.ErrorMsg);
        },
        error: function (xhr, type) {

            alert('Ajax error!');
           

        }
    });

}

function generateData(result)
{ 
    $.each(result, function (i) {
        strCtrl = "";
        strCtrl += "<li class='liBody'>";
        strCtrl += "<div class='liBodyDivLeft'>" + result[i].AgentName + "</div>";
        strCtrl += "<div class='liBodyDivLeft'>" + result[i].AliPayAccount + "</div>";
        strCtrl += "<div class='liBodyDivLeft'>&yen <a href='#'>" + result[i].RemainAmount + "</a> </div>";
        strCtrl += "<div class='liBodyDivLeft'>&yen <a href='#'>" + result[i].CommissionAmount + "</a> </div>";
        strCtrl += "<div class='liBodyDivRight'>";
        strCtrl += "<button type='button' class='btn btn-success' onclick=TransferToUser(this,'" + result[i].RemainAmount + "','" + result[i].CommissionAmount +"','" + result[i].AliPayAccount + "','" + result[i].AgentOpenId + "');>打款</button>";
        strCtrl += "<a href=/Order/Info_DoTransferOrder?AgentOpenId=" + result[i].AgentOpenId + " target='_blank' class='td'>详情</a>";
        strCtrl +="</div>";
        strCtrl += "</li>";

        $("#AgentList").append(strCtrl);
    });
}

function btnSearch()
{
    pageIndex = -1;
    Query(true, pageIndex + 1);
}

function Next() {
    Query(false, pageIndex + 1);
}

function ShowProcess() {
    //$("#btnSearch").attr("disabled", true);
    //$("#divTableBody").hide();
    //$("#divProcess").show();

}
function CloseProcess() {
    //$("#btnSearch").attr("disabled", false);
    //$("#divTableBody").show();
    //$("#divProcess").hide();
}

function ShowOrderDetail()
{

}