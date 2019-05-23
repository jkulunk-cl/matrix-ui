$(function () {

    // Tooltip
    $(".j-layerListItems .NavBar_menuItem.disabled").tooltip({
        "title": "Zoom-in to enable"
    });

    // Toggle Date Range Options with jQuery

    //  $(".mls_status_option").change(function() {
    //     var target = this.getAttribute('data-target');
    //     $('#' + target).toggle();
    // });

});


document.addEventListener("DOMContentLoaded", function (event) { // Page load event

    // Toggle Date Range Options without jQuery
    var mlsStatusOptions = document.querySelectorAll('.mls_status_option');

    for (var i = 0; i < mlsStatusOptions.length; i++) {
        mlsStatusOptions[i].addEventListener('change', function () {
            var dataTarget = this.getAttribute('data-target');
            document.getElementById(dataTarget).classList.toggle('show');
        });
    }

});