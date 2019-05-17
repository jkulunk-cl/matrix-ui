$(function () {

    $("#button1").on("click", function(){

        
        // Rework. add a data-target to the alert ID
        var alertContainer = $("#alert1");
        var timeout = alertContainer.attr("data-timeout");
        
        // Rework. problem with adding multiple on multiple clicks.
        var message = $(this).attr("data-message");

        $(alertContainer).prepend(message);
        $(alertContainer).slideDown();

        setTimeout(function(){
            $(alertContainer).slideUp();
        }, timeout);

        var closeBtn = $(alertContainer).find(".close");
        $(closeBtn).on("click", function(){
            $(this).parent().slideUp();
        });

    });
    
});
