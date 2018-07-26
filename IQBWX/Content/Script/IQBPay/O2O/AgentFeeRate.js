$(function () {

    NumInput = function (obj) {
        obj.value = obj.value.replace(/[^\d.]/g, "")
    }
    Init = function () {
       

        var url = "/O2OWap/AgentFeeRateQuery";
        $.ajax({
            type: 'post',
            url: url,
            data: { },
            success: function (res) {
                if (res.IsSuccess) {
                    if (res.resultList.length > 0)
                    {
                        $("#trContainer tr:gt(0)").empty();
                        generateData(res.resultList);
                    }    
                }
                else {
                    alert(res.ErrorMsg);
                }
            },
            error: function (xhr, type) {
                alert("系统错误！");
            }
        });
    }
    generateData = function (data) {

        var PayMethod = -1;
        $.each(data, function (i) {
            var ctrl = "";
            if (PayMethod != data[i].PayMethod)
            {
                ctrl += "<tr>";
                ctrl += '<th colspan="3">{3}</th>';
                ctrl += "</tr>";

                PayMethod = data[i].PayMethod;
            }
           
            //Id_ItemId_MallCode
            ctrl += '<tr class="tr_content" id="tr_{0}_{1}_{2}">';
            ctrl += '<td class="">金额：{4}</td>';
            ctrl += '<td class="">费率：{5}%</td>';
            ctrl += '<td class="">手续费：<input id="MarketRate" type="number" class="martketText" value={6} onkeyup="NumInput(this);" />%</td>';
            ctrl += "</tr>";

            ctrl += "<tr>";
            ctrl += ' <td colspan="3" class="td_item">';
            ctrl += ' <ul class="list-group">';
            ctrl += ' <li class="list-group-item-text" id="ItemName">{7}</li>'
            ctrl += ' </ul>';
            ctrl += "</td></tr>";


            

            ctrl = String.format(ctrl, data[i].Id,
                                       data[i].ItemId,
                                       data[i].MallCode,
                                        data[i].PayMethodStr,
                                       data[i].Amount,
                                        data[i].FeeRate,
                                       data[i].MarketRate,
                                       "【" + data[i].MallName + "】" + data[i].ItemName);

            $("#trContainer").append(ctrl);

        });

        $(document).on("focus", ".martketText", function (e) {
            this.select();
        });




        //$.each(data, function (i) {
        //    ctrl = "";
        //    ctrl += "<tr id='tr_{0}'>";
        //    ctrl += "<td>金额：{1}</td>";
        //    ctrl += "<td>费率：{2}%</td>";
        //    ctrl += '<td><input id="MarketRate" type="number" class="martketText" value={3} onkeyup="NumInput();" />%</td>';
        //    ctrl += "</tr>";

        //    ctrl = String.format(ctrl,data[i].Id, data[i].MallName, data[i].FeeRate, data[i].MarketRate);

        //    $("#trContainer").append(ctrl);

        //});
    }

    SaveMarket = function () {
        var rsNew = [];
        var rsUp = [];
        var CanSave = true;

        $(".tr_content").each(function (a, b) {

            var Id = $(this).attr("id").split("_")[1];
            var ItemId = $(this).attr("id").split("_")[2];
            var MarketRate = parseFloat($(this).find("#MarketRate").val());
            if (MarketRate == 0)
            {
                CanSave = false;
                var ItemName = $(this).next().find("#ItemName").text();
                alert(ItemName + " 用户手续费为0");
                $(this).find("#MarketRate").focus();
                return false;
            }

            
            var MallCode = $(this).attr("id").split("_")[3];
            var obj = { "Id": Id, "ItemId": ItemId, "MallCode": MallCode, "MarketRate": MarketRate };
            if (Id == 0)
                rsNew.push(obj);
            else
                rsUp.push(obj);
          
        });
        if (CanSave)
        {
            var url = "/O2OWap/AgentFeeRateSave";
            $.ajax({
                type: 'post',
                dataType: "json",
                data: { "newList": rsNew, "updateList": rsUp },
                url: url,
                success: function (res) {
                    if (res.IsSuccess) {
                        alert("更新成功！");
                    }
                    else {
                        alert(res.ErrorMsg);
                    }


                },
                error: function (xhr, type) {
                    alert("系统错误！");
                }
            });
        }
       
  
    }

    Init();

    $("#btnSaveMarket").on("click",SaveMarket);
});