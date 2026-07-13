import { Component, OnInit } from '@angular/core';
import { CounterBehaviorService } from '../../services/counter-behavior.service';

@Component({
  selector: 'app-counter-behavior',
  imports: [],
  templateUrl: './counter-behavior.html',
  styleUrl: './counter-behavior.css',
})
export class CounterBehavior implements OnInit {
  counter = 0;

  constructor(private counterService: CounterBehaviorService) 
  {}

  ngOnInit()
  {
    this.counterService.counter$.subscribe(
      (value) => {
        this.counter = value;
      }
    );
  }

  increment()
  {
    this.counterService.increment();
  }
}
