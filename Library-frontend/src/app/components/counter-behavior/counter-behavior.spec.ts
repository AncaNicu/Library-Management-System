import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CounterBehavior } from './counter-behavior';

describe('CounterBehavior', () => {
  let component: CounterBehavior;
  let fixture: ComponentFixture<CounterBehavior>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CounterBehavior],
    }).compileComponents();

    fixture = TestBed.createComponent(CounterBehavior);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
