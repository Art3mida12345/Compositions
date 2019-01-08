$(function () {
    $.ajaxSetup({ cache: false });
    $(".modal-item").click(function (e) {
        e.preventDefault();
        $.get(this.href,
            function (data) {
                $('#dialogContent').html(data);
                $('#modDialog').modal('show');
            });
    });

    $("#btnHideModal").click(function () {
        $("#modDialog").modal('hide');
    });
});