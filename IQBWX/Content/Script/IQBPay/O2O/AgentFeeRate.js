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

        //var ctrl = "";
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
        var rs = [];

        $("#trContainer tr:gt(0)").each(function (a,b) {

            var Id = $(this).attr("id").split("_")[1];
            var MarketRate = $(this).find("#MarketRate").val();
            var obj = { "Id": Id, "MarketRate": MarketRate};
            rs.push(obj);
          
        });

        var url = "/O2OWap/AgentFeeRateSave";
        $.ajax({
            type: 'post',
            dataType: "json",
            data: { "list": rs },
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

//    Init();

    $("#btnSaveMarket").on("click",SaveMarket);
});