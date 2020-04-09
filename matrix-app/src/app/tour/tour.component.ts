import { Component, OnInit, NgZone, AfterViewInit } from '@angular/core';
import { analyzeAndValidateNgModules } from '@angular/compiler';

declare var $: any;
declare var Tour: any;


@Component({
  selector: 'app-tour',
  templateUrl: './tour.component.html',
  styleUrls: ['./tour.component.scss']
})
export class TourComponent implements OnInit, AfterViewInit {

  tour: any;

  constructor(private _ngZone: NgZone) { }

  ngOnInit() {

  }

  ngAfterViewInit(){

    this._ngZone.runOutsideAngular(() => {
      this.tour = new Tour({
          debug: true,
          storage: false,
          backdrop: true,
          template: `<div class='popover tour mtrx-tour'>
          <div class='arrow'></div>
          <div class="d-flex justify-content-between">
          <h3 class='popover-title'></h3>
          <button class='btn btn-link' data-role='end'><span class="fas fa-times"></span></button>
</div>
          <div class='popover-content'></div>
          <div class='popover-navigation d-flex justify-content-between'>
              <button class='btn btn-link' data-role='prev'><span class="fas fa-chevron-left"></span></button>
              <button class='btn btn-link' data-role='next'><span class="fas fa-chevron-right"></span></button>

          </div>
        </div>`
      });
      this.tour.addStep({
          element: "#feature1",
          title: "Featuring a Widget",
          content: "The Planner lets your clients review the real estate process and check off tasks as they go - so you’ll always know where they are in the process and where they might be stuck.",
          placement: 'right',
          backdrop: true,
      });
      this.tour.addStep({
          element: "#feature2",
          title: "Featuring a Button",
          content: "The Planner lets your clients review the real estate process and check off tasks as they go - so you’ll always know where they are in the process and where they might be stuck.",
          placement: 'bottom',
      });
      this.tour.addStep({
        element: "#feature3",
        title: "Featuring a Link",
        content: "The Planner lets your clients review the real estate process and check off tasks as they go - so you’ll always know where they are in the process and where they might be stuck.",
      placement: 'bottom',
    });
      // Initialize the tour
      this.tour.init();

      this.tour.start(true);
  });

  }

}








