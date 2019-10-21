var filterMenu = (function(){
    // Toggle filter menu
    var filterBtn = document.querySelectorAll('.filter-btn');

    for (var i = 0; i < filterBtn.length; i++) {
        filterBtn[i].addEventListener('click', function () {
            var dataTarget = this.getAttribute('data-target');
            document.getElementById(dataTarget).classList.toggle('show');
        });
    }

    // Close menu
    var handleClose = function(){
        var filterMenu = document.querySelectorAll('.filter-menu');
        for (var i = 0; i < filterMenu.length; i++){
            filterMenu[i].classList.remove('show');
        }
    };

    // Selected item
    var filterItem = document.querySelectorAll('.filter-menu a');
    
    for (var i = 0; i < filterItem.length; i++) {
        filterItem[i].addEventListener('click', function () {
            var selected = this.innerText;
            document.getElementById("filter-dropdown").innerText = selected;
                    //show reset. to redo
                    // if not top level
                    //document.getElementById('filter-reset').classList.add('show');

            handleClose();
        });
    }




}());

filterMenu();