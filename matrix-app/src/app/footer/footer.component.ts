import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  disclaimerText:string = `This MLS disclaimer text is independently owned and operated. Your membership to
  this example provides you with certain services and access to, among other things, this and that.
  This agreement grants you a license to do nothing and is subject to the terms and conditions`;

  matrixVersion:string = "Matrix 9.0";

  constructor() { }

  ngOnInit() {
  }

}
