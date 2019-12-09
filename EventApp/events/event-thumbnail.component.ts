import {Component, Input} from '@angular/core';

@Component({
    selector: 'app-event-thumbnail',
    templateUrl: 'event-thumbnail.component.html'
})
export class EventThumbnailComponent {
    @Input() event: any;

    getStartTimeClass() {
        let badgeColor = 'badge-default';

        if (this.event && this.event.time === '8:00 am') {
            badgeColor = 'badge-success';
        }
        return ['badge', badgeColor];
    }
}
