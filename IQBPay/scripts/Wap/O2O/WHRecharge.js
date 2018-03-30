$(function () {

    Query = function () {
        var qWHName = $("#qWHName").val();

        var url = "/O2OWap/WHReChargeQuery";
        $.ajax({
            type: 'post',
            data: { "WHName": qWHName },
            url: url,
            success: function (data) {
                if (data.IsSuccess) {
                    if (data.resultList != null) {
                        $("#trContainer").empty();

                        generateData(data.resultList);
                    }
                }
                else
                    alert(data.ErrorMsg);
              
              
            },
            error: function (xhr, type) {
                alert("系统错误！");
            }
        });
    }

    generateData = function (data) {

        var ctrl = "";
        $.each(data, function (i) {
            ctrl = "";
            ctrl += "<tr>";
            ctrl += '<td colspan="4" class="td_StoreName">商户/出货商: {0}</td>';
            ctrl += "</tr>";

            ctrl += '<tr id={2}>';
            ctrl += '<td class= "nbt td_log"><button type="button" class="btn btn-info" id="btnLog">日志</button>  </td>';
            ctrl += '<td class="td_Balance IQBFont1 nbt">';
            ctrl += '<ul class="list-inline">';
            ctrl += '<li class="BaLabel">余额</li>';
            ctrl += '<li class="FontAmount BaVal">{1}</li>';
            ctrl += '</ul></td>';
            ctrl += '<td class="td_recharge nbt"><input id="rechargeVal" type="number" class="form-control in_recharge" /></td>';
            ctrl += '<td class="nbt">';
            ctrl += '<button type="button" class="btn btn-danger" id="btnReCharge">充值</button>';
            ctrl += '</td>';
            ctrl += "</tr>";

            ctrl += '<tr class="tr_Log" id="LogContainer">';
            ctrl += '<td class="nbt" colspan="4" id="logDetail">';
            ctrl += '<ul class="list-inline">';
            ctrl += '<li>充值时间</li>';
            ctrl += '<li>充值金额</li>';         
            ctrl += '</ul>';
            ctrl += '</td>';
            ctrl += '</tr>';

            ctrl = String.format(ctrl, data[i].UserName, data[i].O2OShipBalance, data[i].OpenId);

            $("#trContainer").append(ctrl);
        });

        $(document).on("click", "#btnReCharge", reCharge);
        $(document).on("click", "#btnLog", showLog);
    }

    reCharge = function (e) {
        var url = "/O2OWap/DoReCharge";
        var obj = $(e.currentTarget).closest("tr");
        var WHOpenId = obj.attr("id");
        var amt = parseFloat(obj.find("#rechargeVal").val());
        $.ajax({
            type: 'post',
            data: { "WHOpenId": WHOpenId,"amt":amt },
            url: url,
            success: function (data) {
                if (data.IsSuccess) {
                    alert("充值成功");
                    $("#btnQuery").click();
                }
                else
                    alert(data.ErrorMsg);

            },
            error: function (xhr, type) {
                alert("系统错误！");
            }
        });
    };

    showLog = function (e) {

        var obj = $(e.currentTarget).closest("tr");
        var WHOpenId = obj.attr("id");
        
        var container = obj.next();
        var url = "/O2OWap/ReChargeLog";

        if (container.is(':hidden')) {
           
            $.ajax({
                type: 'post',
                data: { "WHOpenId": WHOpenId },
                url: url,
                success: function (data) {
                    if (data.IsSuccess) {
                        if (data.resultList != null) {

                            container.find("#logDetail ul:gt(0)").empty();

                            generateLog(data.resultList, container.find("#logDetail"));

                            container.show();
                        }
                    }
                    else
                        alert(data.ErrorMsg);

                },
                error: function (xhr, type) {
                    alert("系统错误！");
                }
            });
        }
        else
            container.hide();

      

    }

    generateLog = function (data, container)
    {
        var ctrl = "";

        
        $.each(data, function (i) {
            ctrl = "";
         
            ctrl += '<ul class="list-inline">';
            ctrl += '<li>{0}</li>';
            ctrl += '<li>{1}</li>';
            ctrl += '</ul>';
          
            ctrl = String.format(ctrl, data[i].TransDateTimeStr, data[i].TransferAmount);
            container.append(ctrl);
        });
    }
});