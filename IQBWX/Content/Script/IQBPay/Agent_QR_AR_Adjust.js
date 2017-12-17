var form;
$(document).ready(function () {


    //Info Page
    form = $(".InfoBody").Validform({
        tiptype: 2,
        postonce: true,
        ignoreHidden: true,
        btnSubmit:"#btnAdjustAgentQR",
        datatype: {
            "empty": /^\s*$/
        },
        callback: function (data) {
            alert(1);
        },
    });
  
   


});

function AdjustAgentQR()
{
    
}