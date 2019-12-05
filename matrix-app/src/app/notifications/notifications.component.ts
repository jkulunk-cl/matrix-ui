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

    $('[data-toggle="popover"]').popover(
      {
        content: $('#notificationItems'),
        placement: 'auto'
      }
    );
    
  }

}



