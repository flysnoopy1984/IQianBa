var pageIndex = -1;

function Query(NeedClearn,_PageIndex)
{
    var url = "/Order/QuerySum"
    var agentName = $.trim($("#AgentName").val());
    if (agentName == "" || agentName == null) return;
    $.ajax({
        type: 'post',
        data: "AgentName=" + agentName + "&pageIndex=" + _PageIndex,
        url: url,
        success: function (data) {
            var arrLen = data.length;
            if (NeedClearn) {
                $("#trContainer").empty();
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

function generateData(data)
{
    strCtrl = "";
    strCtrl += "<li class='liBody'>";
    strCtrl += "<div>" + data[i].AgentName + "</div>";
    strCtrl += "<div><a href='#'>&yen " + data[i].RemainAmount + "</a> </div>";

    strCtrl += "</li>";


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