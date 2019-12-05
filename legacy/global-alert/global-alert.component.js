$(function () {

    $("#button1").on("click", function(){

        var alertContainer = $("#alert1");
        var timeout = alertContainer.attr("data-timeout");
        var closeBtn = alertContainer.find(".close");
        var message = $(this).attr("data-message");

        var clearMessage = function(){
            alertContainer.slideUp(400, function(){
                alertContainer.find('span').remove();
            });
        };

        // show alert
        alertContainer.append('<span>' + message + '</span>');
        alertContainer.slideDown();


        // hide alert
        setTimeout(function(){
            clearMessage();
        }, timeout);
        
        closeBtn.on("click", function(){
            clearMessage();
        });

    });
    
});
