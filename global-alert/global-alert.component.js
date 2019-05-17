$(function () {

    $("#button1").on("click", function(){

        var alertContainer = $("#alert1");        
        $(alertContainer).slideDown();

        var closeBtn = $(alertContainer).find(".close");
        $(closeBtn).on("click", function(){
            $(this).parent().slideUp();
        });

    });


    
});
