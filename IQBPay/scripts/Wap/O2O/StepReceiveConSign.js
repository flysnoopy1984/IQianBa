$(function () {

    var O2ONo;
    var HasDone = false;

    Init = function () {
        O2ONo = GetUrlParam("O2ONo");
        if (O2ONo == null || O2ONo == "") {
            alert("订单编号没有获取，请联系管理员");
            toPage("/O2OWap/OrderList");
        }
     
        var url = "/O2OWap/Order_SignCodeInfo";
        $.ajax({
            type: 'post',
            data: { 'O2ONo': O2ONo },
            url: url,
            success: function (res) {
                if (res.IsSuccess) {
                    if (res.resultObj.SignCode != "" && res.resultObj.SignCode != "null") {
                        $("#ConInfo").val(res.resultObj.SignCode);
                    }

                    if (res.resultObj.HasSignCode) {
                        HasDone = true;
                        $("#opStatus").text("(已操作)");
                        $("#btnSubmit").text("重新提交");
                      

                    }
                    else
                    {
                      
                        $("#btnBack").text("没提货码");
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

    },

    toDetailPage = function () {
        if (HasDone)
            toPage("/O2OWap/OrderDetail?O2ONo=" + O2ONo);
        else
        {
            var d = { data: { HasCode: false } };

            DoSignCode(d);
            
        }
    }


    DoSignCode = function (e) {
        var url = "/O2OWap/DoSignCode";
        var HasCode = e.data.HasCode;
        if (HasCode == true)
        {
            var Code = $("#ConInfo").val().trim();
            if(Code == "" || Code ==null)
            {
                alert("提货信息不能为空，请起码输入提货柜号和密码");
                return;
            }
        }

        $.ajax({
            type: 'post',
            data: { 'O2ONo': O2ONo, 'HasCode': HasCode, 'Code': Code },
            url: url,
            success: function (res) {

                if (res.IsSuccess) {
                    if (HasCode == true)
                    {
                        alert("操作成功");
                        window.location.reload();
                    }
                    else
                    {
                        toPage("/O2OWap/OrderDetail?O2ONo=" + O2ONo);
                        return;
                    }
                  
                }
                else {
                    alert(res.ErrorMsg);
                    if (res.IntMsg == -1)
                        toPage("/O2OWap/OrderList");
                }
            },
            error: function (xhr, type) {
                alert("系统错误！");
            }
        });
    };

  

    Init();

   

    $(document).on("click", "#btnSubmit",{HasCode:true}, DoSignCode);
    $(document).on("click", "#btnBack", toDetailPage);




});