import { Component } from '@angular/core';
import { CounterService } from '../../services/counter.service';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-counter',
  standalone: true,
  imports: [],
  templateUrl: './counter.html',
  styleUrl: './counter.css',
})
export class Counter {
  constructor(
    public counterService: CounterService,
    public authService: Auth
  ) {}

  increment() {
    this.counterService.increment();
  }
}
