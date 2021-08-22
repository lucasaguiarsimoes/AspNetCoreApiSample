import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvalidInputFeedbackComponent } from './invalid-input-feedback.component';

describe('InvalidInputFeedbackComponent', () => {
  let component: InvalidInputFeedbackComponent;
  let fixture: ComponentFixture<InvalidInputFeedbackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvalidInputFeedbackComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InvalidInputFeedbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
