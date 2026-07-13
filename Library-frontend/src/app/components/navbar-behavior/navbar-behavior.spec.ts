import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarBehavior } from './navbar-behavior';

describe('NavbarBehavior', () => {
  let component: NavbarBehavior;
  let fixture: ComponentFixture<NavbarBehavior>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavbarBehavior],
    }).compileComponents();

    fixture = TestBed.createComponent(NavbarBehavior);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
