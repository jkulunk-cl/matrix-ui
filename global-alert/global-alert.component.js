$(function () {

    $("#button1").on("click", function(){
        
        var pause = 6000;

        var alertContainer = $("#alert1");        
        $(alertContainer).slideDown();

        setTimeout(function(){
            $(alertContainer).slideUp();
        }, pause);

        var closeBtn = $(alertContainer).find(".close");
        $(closeBtn).on("click", function(){
            $(this).parent().slideUp();
        });

    });
    
});
