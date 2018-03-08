var pageIndex = -1;
var pageSize =20;
//0：OrderList//1:OrderListForSettlement
var FromPage = 0;
function CreateDemoData()
{
    var url = "/O2O/CreateDemoData";
    $.ajax({
        type: 'post',
        url: url,
        success: function (data) {
            alert("OK");

        },
        error: function (xhr, type) {
            alert("系统错误！");
        }
    });
}

$(document).ready(function () {

    var page = window.location.pathname.toLowerCase();
    if (page == "/O2O/OrderListForSettlement".toLowerCase())
    {
        FromPage = 1;
    }
    
    InitCondition();

});

function InitCondition()
{
    if (FromPage == 0)
    {
        var url = "/O2O/HashOrderStatus";
        $.ajax({
            type: 'post',
            url: url,
            success: function (data) {
                var arrLen = data.length;
                var selos = $("#cO2OOrderStatus");
                var op = "";
                if (arrLen > 0) {
                    $.each(data, function (i) {
                        if (data[i].Key == 99)
                            op = '<option value="' + data[i].Key + '" selected>' + data[i].Value + '</option>';
                        else
                            op = '<option value="' + data[i].Key + '">' + data[i].Value + '</option>';
                        selos.append(op);
                    });
                }
            },
            error: function (xhr, type) {
                alert('Ajax error!');
            }
        });
    }
  
}

function btnQuery()
{
    pageIndex = -1;
    Query(true, pageIndex + 1);
}

function Query(NeedClearn,_PageIndex) 
{
    if (NeedClearn) {
        $("#DataTable tr:gt(0)").empty();
      
    }
    var BeforeDay = $("#cBeforeDay").val();
    var MallOrderNo = $("#cMallOrderNo").val();
    var O2OOrderStatus = $("#cO2OOrderStatus").val();

    var url = "/O2O/OrderListQuery";

    $.ajax({
        type: 'post',
        dataType: "json",
        data: {
            "pageIndex": _PageIndex,
            "pageSize": pageSize,
            "BeforeDay": BeforeDay,
            "MallOrderNo": MallOrderNo,
            "O2OOrderStatus": O2OOrderStatus
        },
        url: url,
        success: function (data) {
            if (data.IsSuccess)
            {
                var arrLen = data.resultList.length;
                if (arrLen > 0) {
                    generateData(data.resultList);
                    pageIndex++;
                }
            }
            else
            {
                if(data.IntMsg == -1)
                {
                    alert("Session 失效");
                    window.location.href = "/O2O/Login";
                }
            }
          
        },
        error: function (xhr, type) {

            alert('Ajax error!');
          

        }
    });
}

function OpenWin(url)
{
    window.open(url);
}

function generateData(result)
{
    var Ctrl = "";
    var thWidth;
    var n=0;
    $.each(result, function (i) {
        n = 0;
        var bkColor = "";
        switch(result[i].O2OOrderStatus)
        {
            //等待审核
            case 6:
                bkColor = "background-color:chartreuse;";
                break;
            //等待结算
            case 14:
                bkColor = "background-color:gold;";
                break;

        }

        Ctrl = '<tr style="display:-webkit-flex;' + bkColor + '">';
        var op = '<ul class="OrderAction">';
        if (FromPage == 0)
        {
           // if (result[i].O2OOrderStatus ==6)
            op += '<li><a href=javascript:OpenWin("/O2O/OrderReview?O2ONo=' + result[i].O2ONo + '")>审核</a></li>';
        }
        if (FromPage == 1)
        {
            if (result[i].O2OOrderStatus == 14)
                op += '<li><a href=javascript:OpenWin("/O2O/OrderSettlement?O2ONo=' + result[i].O2ONo + '")>结算</a></li>';
        }
        op+='</ul>';
        //操作
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "'>" + op + "</td>";
       

        //商城订单编号
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' title=" + result[i].MallOrderNo + ">" + result[i].MallOrderNo + "</td>";
       

        if (FromPage == 0)
        {
            //用户手机
            tdWidth = "width:" + $("#header th").eq(n++).css("width");
            Ctrl += "<td style='" + tdWidth + "' title=" + result[i].UserPhone + ">" + result[i].UserPhone + "</td>";
           
        }
       
        if (FromPage == 1) {
            if(result[i].O2OOrderStatus==18)
            {
                result[i].O2OOrderStatusStr = "已结算";
            }
        }
        //订单状态
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "'>" + result[i].O2OOrderStatusStr + "</td>";

        //创建时间
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' title=" + result[i].CreateDateTimeStr + ">" + result[i].CreateDateTimeStr + "</td>";

        //商城
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "'>" + result[i].MallName + "</td>";

        //商品
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' title=" + result[i].ItemName + ">" + result[i].ItemName + "</td>";

        if (FromPage == 0) {
            //商城登陆账户
            tdWidth = "width:" + $("#header th").eq(n++).css("width");
            Ctrl += "<td style='" + tdWidth + "'>" + result[i].MallAccount + "</td>";

            //商城登陆密码
            tdWidth = "width:" + $("#header th").eq(n++).css("width");
            Ctrl += "<td style='" + tdWidth + "'>" + result[i].MallPwd + "</td>";

            //O2O编号
            tdWidth = "width:" + $("#header th").eq(n++).css("width");
            Ctrl += "<td style='" + tdWidth + "' title=" + result[i].O2ONo + ">" + result[i].O2ONo + "</td>";

            //平台订单编号
            tdWidth = "width:" + $("#header th").eq(n++).css("width");
            Ctrl += "<td style='" + tdWidth + "' title=" + result[i].RefOrderNo + ">" + result[i].RefOrderNo + "</td>";
           
        }
        Ctrl += "</tr>";

        $("#DataTable").append(Ctrl);
    });
    

}