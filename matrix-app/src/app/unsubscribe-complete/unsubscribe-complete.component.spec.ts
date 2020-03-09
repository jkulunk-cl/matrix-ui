import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UnsubscribeCompleteComponent } from './unsubscribe-complete.component';

describe('UnsubscribeCompleteComponent', () => {
  let component: UnsubscribeCompleteComponent;
  let fixture: ComponentFixture<UnsubscribeCompleteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UnsubscribeCompleteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UnsubscribeCompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
