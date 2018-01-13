function PayToAli() {

    $.confirm({
        theme:"modern",
        title: '确认',
        type:'red',
        content: '若您是分控用户？此码将对您无效，是否继续？',
        buttons: {
            Know: {
                btnClass: 'btn-warning',
                text: "我要继续",
                action: function () {
                    GotoPay();
                }
            },
            cancel: {
                text: "取消",
                btnClass: 'btn-primary',
            },
        }
    });
}

function GotoPay()
{

}