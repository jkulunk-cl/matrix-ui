$(function () {

    $("#button1").on("click", function(){

        
        // Rework. add a data-target to the alert ID
        var alertContainer = $("#alert1");
        var timeout = alertContainer.attr("data-timeout");
        
        var message = $(this).attr("data-message");

        $(alertContainer).prepend('<span>' + message + '</span>');
        $(alertContainer).slideDown();

        setTimeout(function(){
            $(alertContainer).slideUp();
        }, timeout);

        var closeBtn = $(alertContainer).find(".close");
        $(closeBtn).on("click", function(){
            $(alertContainer).slideUp();
        });

    });
    
});
