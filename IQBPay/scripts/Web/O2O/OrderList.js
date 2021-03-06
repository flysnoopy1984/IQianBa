﻿var pageIndex = -1;
var pageSize = 20;

//0：OrderList//1:OrderListForSettlement
var FromPage = 0;

var $Pager = null;
var defaultOpts = {
    totalPages: 1,
};
//function CreateDemoData()
//{
//    var url = "/O2O/CreateDemoData";
//    $.ajax({
//        type: 'post',
//        url: url,
//        success: function (data) {
//            alert("OK");

//        },
//        error: function (xhr, type) {
//            alert("系统错误！");
//        }
//    });
//}


$(document).ready(function () {

    var page = window.location.pathname.toLowerCase();
    if (page == "/O2O/OrderListForSettlement".toLowerCase())
    {
        FromPage = 1;
    }
    
    InitCondition();

    $Pager = $('#Pager');

    defaultOpts = {
     
        onPageClick: function (event, page) {
            if (pageIndex != -1)
                Query(true, page-1);
        }
    };

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
 //   var IsSign = $("#IsSign").get(0).checked;
    var IsSMS = $("#IsSMS").get(0).checked;
    var IsSignCode = $("#IsSignCode").get(0).checked;

    var url = "/O2O/OrderListQuery";

    $.ajax({
        type: 'post',
        dataType: "json",
        data: {
            "pageIndex": _PageIndex,
            "pageSize": pageSize,
            "BeforeDay": BeforeDay,
            "MallOrderNo": MallOrderNo,
            "O2OOrderStatus": O2OOrderStatus,
            "IsSMS": IsSMS,
            "IsSignCode": IsSignCode,
        },
        url: url,
        success: function (data) {
            if (data.IsSuccess)
            {
                var arrLen = data.resultList.length;

                if (arrLen > 0) {
                    if (pageIndex == -1)
                    {
                        $Pager.twbsPagination('destroy');
                        $Pager.twbsPagination($.extend({}, defaultOpts, {
                            startPage: 1,
                            totalPages: Math.ceil(data.IntMsg/pageSize),
                        }));

                    }
                    generateData(data.resultList);
                    //pageIndex++;
                    pageIndex = _PageIndex;
                }
                else
                {
                    alert("没有数据");
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
            //<a href=/Transfer/Info_Win?id=" + result[i].OrderNo + "&type=1  target='_blank' class='td'>转账信息</a>
           // if (result[i].O2OOrderStatus ==6)
            op += '<li><a href=javascript:OpenWin("/O2O/OrderReview?O2ONo=' + result[i].O2ONo + '")>操作</a></li>';
            op += '<li><a href=javascript:OpenWin("/Transfer/Info_Win?O2ONo=' + result[i].O2ONo + '&OrderType=O2O")>转账记录</a></li>';
        }
        if (FromPage == 1)
        {
           // if (result[i].O2OOrderStatus == 14)
            op += '<li><a href=javascript:OpenWin("/O2O/OrderSettlement?O2ONo=' + result[i].O2ONo + '")>详情操作</a></li>';
        }
        op+='</ul>';
        //操作
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "'>" + op + "</td>";
       
        if (FromPage == 1) {
            var IsSMS = "没发送";
            if (result[i].HasSMS == true)
                IsSMS = "已发送";
            if (result[i].NeedSMS == false)
                IsSMS = "不用";
            //是否短信
            tdWidth = "width:" + $("#header th").eq(n++).css("width");
            Ctrl += "<td style='" + tdWidth + "'>" + IsSMS + "</td>";


            //提货信息
            var SignCode = "没有";
            if (result[i].HasSignCode == true)
                SignCode = '<a href=javascript:OpenWin("/O2O/OrderSettlement?O2ONo=' + result[i].O2ONo + '")>已提供</a>';

            tdWidth = "width:" + $("#header th").eq(n++).css("width");
            Ctrl += "<td style='" + tdWidth + "'>" + SignCode + "</td>";
        }


        //商城订单编号
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' title=" + result[i].MallOrderNo + ">" + result[i].MallOrderNo + "</td>";
       

        if (FromPage == 0)
        {
            //用户
            tdWidth = "width:" + $("#header th").eq(n++).css("width");
            Ctrl += "<td style='" + tdWidth + "' title=" + result[i].User + ">" + result[i].User + "</td>";
           
        }
       
        if (FromPage == 1) {
            if(result[i].O2OOrderStatus==18)
            {
                result[i].O2OOrderStatusStr = "已结算";
            }
        }
        //商品
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' title=" + result[i].ItemName + ">" + result[i].ItemName + "</td>";

        //金额
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' title=" + result[i].OrderAmount + ">" + result[i].OrderAmount + "</td>";

        //订单状态
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "'>" + result[i].O2OOrderStatusStr + "</td>";

        //创建时间
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "' title=" + result[i].CreateDateTimeStr + ">" + result[i].CreateDateTimeStr + "</td>";

        //商城
        tdWidth = "width:" + $("#header th").eq(n++).css("width");
        Ctrl += "<td style='" + tdWidth + "'>" + result[i].MallName + "</td>";

      

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