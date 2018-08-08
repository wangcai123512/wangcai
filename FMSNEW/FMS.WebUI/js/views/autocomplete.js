
function log(message) {
    $("#Log").val(message);
}

$(document).ready(function () {
    var client = GetClient();

    $("#autocomplete").autocomplete({
        source: client, minLength: 0, autoFocus: false, delay: 200, max: 10,
        select: function (event, ui) {
            log(ui.item.id);
        }
    }).focus(function () {
        $(this).autocomplete("search");
    });
})

