import { Component, OnInit } from '@angular/core';
declare var $:any;

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit {

  constructor() { }

  ngOnInit() {

    //
    // jQuery Popover Function

    var popoverTemplate = '<div class="popover mtrx-popover" role="tooltip"><div class="arrow mtrx-arrow"></div><h3 class="popover-header mtrx-popover-header"></h3><div class="popover-body mtrx-popover-body"></div></div>';

    $('[data-toggle="popover"]').popover(
      {
        title: 'Notifications',
        content: $('#ExampleNotificationItems'),
        template: popoverTemplate,
        placement: 'bottom',
        trigger: 'click'
      }
    );

      // TODO - If count > 0 add "active" class to alert badge

    // end Popover
    //
    
  }

}



