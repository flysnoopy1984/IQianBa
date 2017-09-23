function SearchSummery(url)
{
    var st = $("#selSummeryType").val();
    var mNum = $("#mNum");
    var mTotal = $("#mTotal");
    var uNum = $("#uNum");
    var uTotal = $("#uTotal");
    var sd = $("#d12").val();
    
    $.ajax({
        type: "post",
        dataType:"json",
        data: { summeryType: st, searchDate: sd },
        url: url,
        success: function (result) {
            try
            {
                mNum.text(result.DayMemberAdded);
                mTotal.text(result.TotalMember);
                uNum.text(result.DayUserSub);
                uTotal.text(result.TotalUser);
            }
            catch(e)
            {
                alert(e);
            }
        },
        error: function () {
        }
    })
}

function CreateTableData(item)
{

}

function SearchDetail(url)
{
    var st = $("#selSummeryType").val();
    var sd = $("#d12").val();
    var ulMember = $("#ulMember");
    var ulUser = $("#ulUser");
    var ulARUserTrans = $("#ulARUserTrans");

    ulMember.hide();
    ulUser.hide();
    ulARUserTrans.hide();

    $.ajax({
        type: "post",
        dataType: "json",
        data: { summeryType: st, searchDate: sd },
        url: url,
        success: function (result) {
            try
            {
                switch(st)
                {
                    case "1":
                        ulUser.show();
                        ulUser.children("li.cd").remove();
                        $(result).each(function (i) {
                            var item = result[i];
                            ulUser.append("<li class='cd'>" + item.openId + "</li>");
                            ulUser.append("<li class='cd'>" + item.nickname + "</li>");
                            ulUser.append("<li class='cd'>" + item.SubscribeDate + "</li>");
                            ulUser.append("<li class='cd'>" + item.sex + "</li>");
                            ulUser.append("<li class='cd'>" + item.ParentOpenId + "</li>");
                        });
                        break;
                    case "2":
                        ulMember.show();
                        ulMember.children("li.cd").remove();
                        $(result).each(function (i) {
                            var item = result[i];
                            ulMember.append("<li class='cd'>" + item.openId + "</li>");
                            ulMember.append("<li class='cd'>" + item.nickname + "</li>");
                            ulMember.append("<li class='cd'>" + item.RegisterDate + "</li>");
                            ulMember.append("<li class='cd'>" + item.TotalGainAmt + "</li>");
                            ulMember.append("<li class='cd'>" + item.AvailDeposit + "</li>");
                            ulMember.append("<li class='cd'>" + item.Balance + "</li>");
                        });
                        break;
                    case "3":
                        ulARUserTrans.show();
                        ulARUserTrans.children("li.cd").remove();
                        $(result).each(function (i) {
                            var item = result[i];
                            ulARUserTrans.append("<li class='cd'>" + item.openId + "</li>");
                            ulARUserTrans.append("<li class='cd'>" + item.FromOpenId + "</li>");
                            ulARUserTrans.append("<li class='cd'>" + item.ChildLevel + "</li>");
                            ulARUserTrans.append("<li class='cd'>" + item.Amount + "</li>");
                            ulARUserTrans.append("<li class='cd'>" + item.ItemId + "</li>");
                            ulARUserTrans.append("<li class='cd'>" + item.TransDate + "</li>");
                        });

                        break;
                }
            }
            catch (e) {
                alert(e);
            }
        },
        error: function () {
        }
    })
}