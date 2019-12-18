        //
        // jQuery Popover Function

        var popoverTemplate = '<div class="popover mtx-popover" role="tooltip"><div class="arrow mtx-arrow"></div><h3 class="popover-title mtx-popover-title"></h3><div class="popover-content mtx-popover-content"></div></div>';
        var popoverContent = $('#ExampleNotificationItems').html();

        $("#ExampleNotificationAlert").popover(
            {
                html: true,
                title: 'Notifications',
                content: popoverContent,
                template: popoverTemplate,
                placement: 'bottom',
                trigger: 'click'
            }
        );

      // TODO - If count > 0 add "active" class to alert badge

    // end Popover
    //