//Ce code empêche de refaire un submit après le premier clique
//Solution trouver sur http://www.hackered.co.uk/articles/asp-net-mvc-query-preventing-multiple-clicks-by-disabling-submit-buttons

//$(document).on('submit', 'form', function () {
//    var buttons = $(this).find('[type="submit"]').not('#searchBtn');
//    if ($(this).valid()) {
//        buttons.each(function (btn) {
//            $(buttons[btn]).prop('disabled', true);
//        });
//    } else {
//        buttons.each(function (btn) {
//            $(buttons[btn]).prop('disabled', false);
//        });
//    }
//});
