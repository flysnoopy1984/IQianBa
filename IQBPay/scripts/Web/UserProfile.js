$(document).ready(function () {

    var Id = $("#RecId").val();
   
})

function Init(Id) {

    var url = "/User/Get";
    $.ajax({
        type: 'post',
        //data: "Id=" + Id,
        url: url,
        success: function (data) {

            InitFormData(data);
        },
        error: function (xhr, type) {

            alert(xhr.responseText);

        }
    });

}

function InitFormData(data)
{
    $("#UserRole").text(data.)
}