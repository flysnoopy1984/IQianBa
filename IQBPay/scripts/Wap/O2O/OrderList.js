$(function () {
    var aoId = null;
    var pageIndex = 0;
    var pageSize = 10;
    var IsAdmin = false;
    var IQBScroll = null;

    aoId = GetUrlParam("aoId");

    if (aoId == "" || aoId == "null" || aoId == undefined) {
        window.location.href = "/O2OWap/ErrorPage?ec=1";
        return;
    }

    /**
 * [返回]
 */
    backToHome = function () {
        window.location.href = "/O2OWap/Index?aoId=" + aoId;
    };

    ShowBlock = function () {

        var msg = ' <div id="ProcessArea1" class="progress progress-striped active">';
        msg += '<div id="upload_progress1" class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 100%; height:40px;">';
        msg += '<span class="sr-only">数据加载中...</span>';
        msg += '</div>';
        msg += '</div>';

        $.blockUI({
            message: msg,
            css: {
                border: 'none',
                width: '90%',
                height: '40px',
                left: '20px',
                'border-radius': '20px',
            }
        });
    }

    CloseBlock = function () {
        if (pageIndex == 1)
            $.unblockUI();
    }

    Init = function () {

        IsAdmin = $("#IsAdmin").val();

        $("#DataTable").empty();
        pageIndex = 0;
       
        IQBScroll =  $("#listloading").ScrollLoad({
            offSetTop:50,

            loadData: function (me) {

                var url = "/O2OWap/OrderListQuery";
                if(pageIndex ==0)
                    ShowBlock();
                var selOS = $("#selCondition").val();
                $.ajax({
                    type: 'post',
                    data: { "pageIndex": pageIndex, "pageSize": pageSize, "selOS": selOS },
                    url: url,
                    success: function (res) {
                        if (res.IsSuccess) {
                            if (res.resultList.length > 0) {
                                generateData(res.resultList);
                                pageIndex++;

                            }
                            else {
                               
                                me.noData();
                            }
                            CloseBlock();
                        }
                        else {
                            CloseBlock();
                            if (res.IntMsg == -1) {
                                alert("用户信息未获取，请重新提交手机号");
                                window.location.href = "/O2OWap/Index?aoId=" + aoId;
                            }
                            else {
                                alert(res.ErrorMsg);
                            }
                        }
                        me.resetLoad();
                        
                    },
                    error: function (xhr, type) {
                        alert("系统错误！");

                        me.resetLoad();
                        CloseBlock();
                    }
                });

            }

        });

     
    }

    generateData = function (data) {
        var DataTable = $("#DataTable");
        

        var ctrl = "";
        $.each(data, function (i) {
            var Id = data[i].O2ONo+"_"+i;

            ctrl = '<tr class="tr_OrderItem" id="' + Id + '">';
            ctrl += '<td class="td_Width_Content1">';
            ctrl += '<ul class="list-inline">';
            ctrl += '<li class="list-inline-item List_Title_Font">{0}</li>';
            ctrl += '</ul>';
            ctrl += '<ul class="list-inline">';
            ctrl += '<li class="list-inline-item List_Item_style1">金额：{1}</li>';
            //ctrl += '<li class="list-inline-item List_Item_style1"></li>';
            ctrl += '</ul>';
            ctrl += '</td>';
            ctrl += '<td class="td_Padding_4px">';
            ctrl += '<div class="ToDetail" >{3}<span class="Arror">></span></div>';
            if (IsAdmin)
            {
                ctrl += '<div id="' + data[i].O2ONo + '_' + data[i].O2OOrderStatus + '" class="ToReview"> <button type="button" class="btn btn-success">操作</button></div>';
            }
           
            ctrl += '</td>';
            ctrl += "</tr>";
            ctrl += '<tr id="' + Id + '_2">';
            ctrl += '<td colspan=3 class="td_border_bottom_ff6a00 td_time">下单时间：{2}</td>';
           
            ctrl += "</tr>";


            ctrl = String.format(ctrl, data[i].ItemName, data[i].OrderAmount, data[i].CreateDateTime, data[i].O2OOrderStatusStr);
            DataTable.append(ctrl);

          
        })
    }

    Init();
    

    //$("#listloading").bind("scroll", function () {
    //    console.log("win Height:"+ $(window).height());
    //    console.log("Doc Height:"+ $(document).height());
    //    console.log("Table offsetHeight:" + $('#DataTable').outerHeight(true));
    //    console.log("listloading Height:" + $("#listloading").outerHeight(true));
    //    console.log($("#listloading").scrollTop());
      
       
    //});

    //进入明细
    $(document).on("click", ".tr_OrderItem", function (e) {
        var O2ONo = e.currentTarget.id.split('_')[0];
        
       
        ToDetail(e,O2ONo);

    });

    $(document).on("click", ".ToReview", function (e) {
        var O2ONo = e.currentTarget.id.split('_')[0];
        var OrderStatus = e.currentTarget.id.split('_')[1];

        ToReview(e, O2ONo, OrderStatus);

    });
    $(document).on("change", "#selCondition", function () {
        Init();
    });

    ToReview = function (e, O2ONo, OrderStatus) {
        e.stopPropagation();

        window.location.href = "/O2OWap/UploadOrder?aoId=" + aoId + "&OrderNo=" + O2ONo + "&OrderStatus=" + OrderStatus;
    }


   

    ToDetail = function (e,O2ONo)
    {
     

        window.location.href = "/O2OWap/OrderDetail?aoId=" + aoId + "&O2ONo=" + O2ONo;
      
    }
});