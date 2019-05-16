$(function () {

    // Tooltip
    $(".j-layerListItems .NavBar_menuItem.disabled").tooltip({
        "title": "Zoom-in to enable"
    });


    /*

    // Toggle Date Range Options with jQuery

     $(".mls_status_option").change(function() {
        var target = this.getAttribute('data-target');
        $('#' + target).toggle();
    });

    */ 


    // Toggle Date Range Options without jQuery
    var mlsStatusOptions = document.querySelectorAll('.mls_status_option');

    mlsStatusOptions.forEach(function (el) {
        el.addEventListener('change', function () {
            var dataTarget = this.getAttribute('data-target');
            document.getElementById(dataTarget).classList.toggle('show');
        });
    });

    
});