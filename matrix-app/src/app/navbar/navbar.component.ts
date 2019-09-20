import { Component, OnInit } from '@angular/core';
import { IUser } from '../interface';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  user: IUser;

  constructor() {
    this.user = {
      userId: 1,
      username: 'jkulunk',
      firstName: 'John',
      image: '/assets/images/1.jpg'
    } as IUser;

  }

  ngOnInit() {
  }

}