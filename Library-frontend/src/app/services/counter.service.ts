import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CounterService {

  counter = signal(this.getCounterFromLocalStorage());

  increment() {
    this.counter.update(value => value + 1);
    localStorage.setItem('counter', this.counter().toString());
  }

  getCounterFromLocalStorage(): number
 {
    return Number(localStorage.getItem('counter')) || 0;
  }

}