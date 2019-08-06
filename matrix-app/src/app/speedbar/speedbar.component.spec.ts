import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpeedbarComponent } from './speedbar.component';

describe('SpeedbarComponent', () => {
  let component: SpeedbarComponent;
  let fixture: ComponentFixture<SpeedbarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpeedbarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpeedbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
