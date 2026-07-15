import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgeValidatorComponent } from './age-validator.component';

describe('AgeValidatorComponent', () => {
  let component: AgeValidatorComponent;
  let fixture: ComponentFixture<AgeValidatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AgeValidatorComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(AgeValidatorComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
