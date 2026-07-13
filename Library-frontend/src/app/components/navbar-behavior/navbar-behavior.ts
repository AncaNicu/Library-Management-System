import { Component, OnInit } from '@angular/core';
import { CounterBehaviorService } from '../../services/counter-behavior.service';

@Component({
  selector: 'app-navbar-behavior',
  standalone: true,
  imports: [],
  templateUrl: './navbar-behavior.html',
  styleUrl: './navbar-behavior.css',
})
export class NavbarBehavior implements OnInit {
  counter = 0;

  constructor(private counterService: CounterBehaviorService) 
  {}

  ngOnInit() {
    this.counterService.counter$.subscribe(
      (value) => {
        this.counter = value;
      }
    );
  }


}
